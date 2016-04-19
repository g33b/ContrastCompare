using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RestSharp;
namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ContrastCompare.CompareHelper compare = new ContrastCompare.CompareHelper();

            compare.Compare("oldfile.txt","newfile.txt","outFIle.txt");
        }
    }
}
