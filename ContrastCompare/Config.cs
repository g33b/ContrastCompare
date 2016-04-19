using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace ContrastCompare
{
    public class Config
    {
        public List<String> compareColumns { get; set; }
        public List<String> outColumns { get; set; }
        public DataTable pruneItems { get; set; }
        public bool caseSensitive = false;


        public Config(String fileName)
        {
            compareColumns = new List<string>();
            outColumns = new List<string>();
            pruneItems = new DataTable();
            pruneItems.Columns.Add("column");
            pruneItems.Columns.Add("value");

            Load(fileName);
        }

        /// <summary>
        /// Loads the configuration File
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(String fileName)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fileName);
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    processLine(line);
                }
            } catch(Exception wtf)
            {
                Log.Add(wtf.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }


        /// <summary>
        /// Processes lines in the config file and sets up variables
        /// </summary>
        /// <param name="line"></param>
        private void processLine(String line)
        {
            try {
                if (!line.Trim().StartsWith("#")) // Ignore commented lines
                {
                    String l = line.Trim();
                    String[] split = l.Split('=');

                    if (split.Length > 1)
                    {
                        String left = split[0];
                        String right = split[1];

                        // Now setup the variables

                        if (left.ToLower().Equals("include_columns"))
                        {
                            String[] incCols = right.Split(',');
                            foreach (String s in incCols)
                            {
                                compareColumns.Add(s);
                            }
                        }

                        if (left.ToLower().Equals("output_columns"))
                        {
                            String[] outCols = right.Split(',');
                            foreach (String s in outCols)
                            {
                                outColumns.Add(s);
                            }
                        }

                        if (left.ToLower().Equals("case_sensitive"))
                        {
                            if (right.ToLower().Trim().Equals("true"))
                            {
                                caseSensitive = true;
                            }
                        }


                        if (left.ToLower().Equals("prune_rows"))
                        {
                            String[] outCols = right.Split(',');
                            foreach (String s in outCols)
                            {
                                String[] sep = s.Split('|');
                                if (sep.Length > 1)
                                {
                                    DataRow dr = pruneItems.NewRow();
                                    dr["column"] = sep[0];
                                    dr["value"] = sep[1];
                                    pruneItems.Rows.Add(dr);
                                }
                            }
                        }

                    }
                }
            } catch(Exception wtf)
            {
                // Failed to load config
                Log.Add("Failed to load config:"+wtf.Message);
            }
        }

    }
}
