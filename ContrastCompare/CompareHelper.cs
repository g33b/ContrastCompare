using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Data;
using System.IO;


namespace ContrastCompare
{
    

    public class CompareHelper
    {
        Config config;
        DataTable OldFile;
        DataTable NewFile;

        DataTable Out_Matches;
        DataTable Out_NewItems;
        DataTable Out_RemovedItems;

        public CompareHelper()
        {
            config = new Config("config.ini");
            OldFile = new DataTable("old");
            NewFile = new DataTable("new");
            

        }


        public void Compare(String oldFile,String newFile,String outFile)
        {
            List<String> compOld = CSVTools.LoadCSV(oldFile);
            List<String> compNew = CSVTools.LoadCSV(newFile);

            DataTable tableOld = CSVTools.ConvertToDataTable(compOld);
            DataTable tableNew = CSVTools.ConvertToDataTable(compNew);

            try {
                // remove unnessesary columns here
                for (int i = tableOld.Rows.Count - 1; i >= 0; i--)
                {
                    foreach (DataRow dr in config.pruneItems.Rows)
                    {
                        if (tableOld.Rows[i][dr["column"].ToString()].Equals(dr["value"].ToString()))
                        {
                            tableOld.Rows.RemoveAt(i);
                        }
                    }
                }
                for (int i = tableNew.Rows.Count - 1; i >= 0; i--)
                {
                    foreach (DataRow dr in config.pruneItems.Rows)
                    {
                        if (tableNew.Rows[i][dr["column"].ToString()].Equals(dr["value"].ToString()))
                        {
                            tableNew.Rows.RemoveAt(i);
                        }
                    }
                }
            } catch(Exception wtf)
            {
                Log.Add("Error pruning rows. Check config.ini all columns should match input:" + wtf.StackTrace);
            }


            DoCompare(tableOld, tableNew,outFile);
        }


        
        private void DoCompare(DataTable oldTable, DataTable newTable,String outFileName)
        {


            #region Matches in both tables
            // Find rows that exist in both Tables
            Out_Matches = new DataTable("Out_Matches");
            foreach(String c in config.outColumns)
            {
                Out_Matches.Columns.Add(c);
            }
            foreach (DataRow o in oldTable.Rows)
            {
                int matchCount = 0;
                DataRow matchRow = oldTable.NewRow();

                foreach (DataRow n in newTable.Rows)
                {
                    matchCount = 0;
                    // Look for a match (two of the same datarows)
                    for (int i = 0; i < o.ItemArray.Length; i++)
                    {
                        // Cell case sensitivity should be handled here
                        if (config.caseSensitive)
                        {
                            if (o.ItemArray[i].ToString().Trim().Equals(n.ItemArray[i].ToString().Trim()))
                            {
                                matchCount++; // there was a match. All cells should match for a full match
                            }
                        }
                        else
                        {
                            if (o.ItemArray[i].ToString().Trim().ToLower().Equals(n.ItemArray[i].ToString().Trim().ToLower()))
                            {
                                matchCount++; // there was a match. All cells should match for a full match
                            }
                        }
                    }
                    if (matchCount == oldTable.Columns.Count)
                    {
                        matchRow.ItemArray = o.ItemArray;
                        break;
                    }
                } // Inside Loop
                if (matchCount == oldTable.Columns.Count)
                {
                    DataRow newRow = Out_Matches.NewRow();
                    newRow.ItemArray = matchRow.ItemArray;
                    Out_Matches.Rows.Add(newRow);
                }
            }
            #endregion



            // Find new rows
            Out_NewItems = new DataTable("Out_NewItems");
            foreach (String c in config.outColumns)
            {
                Out_NewItems.Columns.Add(c);
            }
            foreach (DataRow n in newTable.Rows)
            {
                int matchCount = 0;
                DataRow matchRow = oldTable.NewRow();
                bool hasMatch = false;

                foreach (DataRow o in oldTable.Rows)
                {
                    matchCount = 0;
                    // Look for a match (two of the same datarows)
                    for (int i = 0; i < o.ItemArray.Length; i++)
                    {
                        // Cell case sensitivity should be handled here
                        if (config.caseSensitive)
                        {
                            if (o.ItemArray[i].ToString().Trim().Equals(n.ItemArray[i].ToString().Trim()))
                            {
                                matchCount++; // there was a match. All cells should match for a full match
                            }
                        }
                        else
                        {
                            if (o.ItemArray[i].ToString().Trim().ToLower().Equals(n.ItemArray[i].ToString().Trim().ToLower()))
                            {
                                matchCount++; // there was a match. All cells should match for a full match
                            }
                        }
                    }
                    if (matchCount == oldTable.Columns.Count) // This row was found in the old table... so it's not new
                    {
                        hasMatch = true;
                        break;
                    }
                    else
                    {
                        matchRow.ItemArray = n.ItemArray;
                    }
                } // Inside Loop
                if (!hasMatch)
                {
                    DataRow newRow = Out_NewItems.NewRow();
                    newRow.ItemArray = matchRow.ItemArray;
                    Out_NewItems.Rows.Add(newRow);
                }
            }



            // Find removed items
            Out_RemovedItems = new DataTable("Out_RemovedItems");
            foreach (String c in config.outColumns)
            {
                Out_RemovedItems.Columns.Add(c);
            }
            foreach (DataRow o in oldTable.Rows)
            {
                int matchCount = 0;
                DataRow matchRow = oldTable.NewRow();
                bool hasMatch = false;

                foreach (DataRow n in newTable.Rows)
                {
                    matchCount = 0;
                    // Look for a match (two of the same datarows)
                    for (int i = 0; i < o.ItemArray.Length; i++)
                    {
                        // Cell case sensitivity should be handled here
                        if (config.caseSensitive)
                        {
                            if (o.ItemArray[i].ToString().Trim().Equals(n.ItemArray[i].ToString().Trim()))
                            {
                                matchCount++; // there was a match. All cells should match for a full match
                            }
                        }
                        else
                        {
                            if (o.ItemArray[i].ToString().Trim().ToLower().Equals(n.ItemArray[i].ToString().Trim().ToLower()))
                            {
                                matchCount++; // there was a match. All cells should match for a full match
                            }
                        }
                    }
                    if (matchCount == oldTable.Columns.Count) // This row was found in the new table... so it's not new
                    {
                        hasMatch = true;
                        break;
                    }
                    else
                    {
                        matchRow.ItemArray = o.ItemArray;
                    }
                } // Inside Loop
                if (!hasMatch)
                {
                    DataRow newRow = Out_RemovedItems.NewRow();
                    newRow.ItemArray = matchRow.ItemArray;
                    Out_RemovedItems.Rows.Add(newRow);
                }
            }

            if (File.Exists(outFileName))
            {
                File.Delete(outFileName);
            }
            System.Threading.Thread.Sleep(20);
            CSVTools.AppendDataTableToFile(Out_Matches, outFileName, "Matches: Found in both CSVs");
            CSVTools.AppendDataTableToFile(Out_NewItems, outFileName, "New Items!");
            CSVTools.AppendDataTableToFile(Out_RemovedItems, outFileName, "Removed/Resolved Items");
        }

    }
}
