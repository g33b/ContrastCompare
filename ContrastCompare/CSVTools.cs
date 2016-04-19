using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace ContrastCompare
{
    public static class CSVTools
    {
        public static List<String> LoadCSV(String filename)
        {
            List<String> theCSV = new List<String>();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filename);
                while (!sr.EndOfStream)
                {
                    theCSV.Add(sr.ReadLine());
                }
                
            } catch(Exception wtf)
            {
                Log.Add("CSV Tools LoadCSV:" + wtf.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return theCSV;
        }


        public static List<String> LoadCSVString(String thestring)
        {
            List<String> theCSV = new List<String>();
            String[] split = thestring.Split('\n');
            foreach(String s in split)
            {
                theCSV.Add(s);
            }
            return theCSV;
        }



        public static DataTable ConvertToDataTable(List<String> csv)
        {
            DataTable rtnTable = new DataTable();
            int lineCount = 0;
            foreach(String line in csv)
            {
                if(lineCount == 0) // First line contains column names
                {
                    String[] colSplit = line.Split(',');
                    foreach(String s in colSplit)
                    {
                        rtnTable.Columns.Add(s);
                    }
                }
                else
                {
                    // The rest is data
                    String[] rowSplit = line.Split(',');
                    DataRow dr = rtnTable.NewRow();
                    dr.ItemArray = rowSplit;
                    rtnTable.Rows.Add(dr);
                }
                lineCount++;
            }
            return rtnTable;
        }


        public static void AppendDataTableToFile(DataTable dt,String fileName,String headerText)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(fileName,true);
                if (!headerText.Equals(String.Empty))
                {
                    sw.WriteLine(headerText);
                }
                int columnCount = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (columnCount == dt.Columns.Count - 1)
                    { 
                        sw.Write(dc.ColumnName);
                    } else
                    {
                        sw.Write(dc.ColumnName + ",");
                    }
                    columnCount++;
                }
                sw.WriteLine();
                int rowCount = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    columnCount = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (columnCount == dt.Columns.Count - 1)
                        {
                            sw.Write(dr[dc.ColumnName].ToString());
                        }
                        else
                        {
                            sw.Write(dr[dc.ColumnName].ToString() + ",");
                        }
                        columnCount++;
                    }
                    rowCount++;
                    sw.WriteLine();
                }

                if (!headerText.Equals(String.Empty))
                {
                    sw.WriteLine();
                    sw.WriteLine();
                }

            }catch(Exception wtf)
            {
                Log.Add("Write to CSV error:" + wtf.Message);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }


        }





    }
}
