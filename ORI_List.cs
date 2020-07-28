using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;




namespace ORI_List
{
    public partial class ORI_List : Form
    {
        DataTable dt = new DataTable();
        public ORI_List()
        {
            InitializeComponent();
        }

        private void ORI_List_Load(object sender, EventArgs e)
        {
            
            BindData("ORI_Numbers.csv");
            dataGridView1.Columns[0].Width = 370;
            dataGridView1.Columns[1].Width = 150;
            textBox1.Focus();
            
        }
        private void ORI_List_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                textBox1.Clear();
                e.Cancel = true;
                Hide();
            }
        }
        private void ORI_List_Resize(object sender, EventArgs e)
        { 
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            textBox1.Focus();
        }

        private void BindData(string filePath)
        {
            
            string[] lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                //first line to create header
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }
                //For Data
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable dtfiltered = new DataTable();
            dtfiltered.Columns.Add("Agency Name");
            dtfiltered.Columns.Add("ORI Number");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.Field<String>("Agency Name").Contains(textBox1.Text.ToUpper()) || dr.Field<String>("ORI Number").Contains(textBox1.Text.ToUpper()))
                {
                    dtfiltered.Rows.Add(dr.ItemArray);
                }
            }
            dataGridView1.DataSource = dtfiltered;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FormClosing -= ORI_List_FormClosing;
            this.Close();
        }
    }
}
