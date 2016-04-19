using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ContrastCompare
{
    public class Log
    {
        public static void Add(String text)
        {
            StreamWriter sw = new StreamWriter("log.txt");
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd") + "--" + text.Replace("\r","").Replace("\n",""));
            sw.Close();
        }
    }
}
