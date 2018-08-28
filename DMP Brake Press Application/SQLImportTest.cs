using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using ExcelDataReader;

namespace DMP_Brake_Press_Application
{
    public partial class SQLImportTest : Form
    {
        public SQLImportTest()
        {
            InitializeComponent();
        }

        // Excel File Creation
        private static Excel._Workbook ReportWB;
        private static Excel.Application ReportApp;
        private static Excel._Worksheet ReportWS;
        private static Excel.Range ReportRange;
        private static string ExcelFileLocation;

        DataSet result;

        public void excel()
        {
            System.Data.DataTable table1 = new System.Data.DataTable();
            OleDbConnection dbConnection1 = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;"+ @"Data Source=\\insidedmp.com\Corporate\OH\OH Common\Engineering\Job List\SQL Data Tables\Paccar_Spot_Weld_SQL.xlsx;" + @"Extended Properties=Excel 8.0;HDR=Yes;");
            dbConnection1.Open();
            try
            {
                OleDbDataAdapter dbAdapter1 = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", dbConnection1);
                dbAdapter1.Fill(table1);
                dataGridView1.DataSource = table1;
            }
            finally
            {
                dbConnection1.Close();
            }
        }

        private void SQLImportTest_Load(object sender, EventArgs e)
        {
            excel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IExcelDataReader excelReader;
            OpenFileDialog open = new OpenFileDialog();
            if(open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //this.textBox1.Text = open.FileName;
                FileStream fs = File.Open(open.FileName, FileMode.Open, FileAccess.Read);
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                        ReadHeaderRow = (rowReader) => {
                        }
                    }
                });
                //result = excelReader.AsDataSet();
                excelReader.Close();
                dataGridView1.DataSource = result.Tables["Sheet1"];


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pathName = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source=" + textBox1.Text + ";Extended Properties=\"Excel 8.0;HDR = Yes;\";";
            OleDbConnection conn = new OleDbConnection(pathName);

            OleDbDataAdapter dataAd = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", conn);
            DataTable dt = new DataTable();

            dataAd.Fill(dt);
            dataGridView1.DataSource = dt;
            
        }
    }
}
