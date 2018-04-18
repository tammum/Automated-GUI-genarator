using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using CrystalDecisions.CrystalReports.Design;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
namespace oracle_ui_final_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OracleConnection con = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=userid;Password=yourPass");
        int a = 1, b = 1, i = 0;
        public string[] data = new string[100];


        private void button1_Click(object sender, EventArgs e)//button for showing details
        {
            con.Open();
            foreach (Control c in this.Controls)//fetching the text from dynamic textbox
            {
                if (c is TextBox && c.Name.Contains("txt"))
                {
                    data[++i] = ((TextBox)c).Text;
                }
            }
            //creating an oracle command
            OracleCommand oc = new OracleCommand("threetwo.load_information", con);
            oc.CommandType = CommandType.StoredProcedure;
           
            //parameters
            //parameter refcursor
            OracleParameter ref_cursor = new OracleParameter();
            ref_cursor.OracleDbType = OracleDbType.RefCursor;
            ref_cursor.Direction = ParameterDirection.Output;
            //parameter stu_name
            OracleParameter stu_name = new OracleParameter();
            stu_name.OracleDbType = OracleDbType.Varchar2;
            stu_name.Value = data[1];
            stu_name.Direction = ParameterDirection.Input;
            //parameter stu_id
            OracleParameter stu_id = new OracleParameter();
            stu_id.OracleDbType = OracleDbType.Varchar2;
            stu_id.Value = data[2];
            stu_id.Direction = ParameterDirection.Input;
            //adding the parameters
            oc.Parameters.Add(stu_name);
            oc.Parameters.Add(stu_id);
            oc.Parameters.Add(ref_cursor);
            oc.ExecuteNonQuery();

            //printing the report
            OracleDataAdapter oda = new OracleDataAdapter();
            oda.SelectCommand = oc;
            DataSet ds = new DataSet();
            oda.Fill(ds, "LOAD_INFORMATION");
            ReportDocument rd = new ReportDocument();
            rd.Load(@"F:\projects\oracle_ui_final_2\oracle_ui_final_2\CrystalReport1.rpt");
            rd.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rd;
            i = 0;//important initialization for update report and data 
            con.Close();
        }

       
        private void Form1_Load(object sender, EventArgs e) //this is the code which will run when the form loads
        {
            con.Open();
            OracleCommand oc = new OracleCommand("threetwo.load_ui2", con);
            oc.CommandType = CommandType.StoredProcedure;
            OracleCommandBuilder.DeriveParameters(oc);
            foreach (OracleParameter p in oc.Parameters)
            {
                System.Windows.Forms.Label temp = new System.Windows.Forms.Label();
                this.Controls.Add(temp);
                temp.Top = a * 30;
                temp.Left = 10;
                temp.AutoSize = true;
                temp.Text += p.ParameterName.ToString();
                temp.Name = "lab" + a.ToString();
                ++a;
                //for textbox
                System.Windows.Forms.TextBox temp2 = new System.Windows.Forms.TextBox();
                this.Controls.Add(temp2);
                temp2.Top = b * 30;
                temp2.Left = 100;
                temp2.Name = "txt" + b.ToString();
                ++b;
            }
            con.Close();
        }
    }
}
