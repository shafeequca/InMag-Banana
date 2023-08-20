using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InMag_V._16.DataSet;
using CrystalDecisions.CrystalReports.Engine;


namespace InMag_V._16
{
    public partial class frmStockView : Form
    {
        DataSet1 ds;
        public frmStockView()
        {
            InitializeComponent();
        }

        private void frmStockView_Load(object sender, EventArgs e)
        {
            ds = new DataSet1();
            DataShow();
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            DataShow();
        }
        private void DataShow()
        {
            string query = "select Item_Name as ItemName,inMalayalam,current_stock as Stock from tblItem";
            if (rbAll.Checked)
                query = "select Item_Name as ItemName,InMalayalam,current_stock as Stock from tblItem";
            else if (rbReorderLevel.Checked)
                query = "select Item_Name as ItemName,inMalayalam,current_stock as Stock from tblItem where current_stock<=ReOrder";
            else if (rbNegative.Checked)
                query = "select Item_Name as ItemName,inMalayalam,current_stock as Stock from tblItem where current_stock<0";
                 
            try
                {
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    ds.Tables["Stock"].Clear();
                    ds.Tables["Stock"].Merge(dt);
                    ItemGrid.Columns.Clear();
                    ItemGrid.DataSource = null;
                    ItemGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                    ItemGrid.Columns[1].HeaderText = "Item Name";
                    ItemGrid.Columns[1].Visible = false;
                    ItemGrid.Columns[2].Width = 150;
                }
                catch (Exception ex){ }
            

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ItemGrid.Rows.Count > 0)
            {
                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\Reports\rptStock.rpt");
                cryRpt.SetDataSource(ds);
                cryRpt.Refresh();
                cryRpt.PrintToPrinter(1, true, 0, 0);
            }
        }

        private void rbReorderLevel_CheckedChanged(object sender, EventArgs e)
        {
            DataShow();
        }

        private void rbNegative_CheckedChanged(object sender, EventArgs e)
        {
            DataShow();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (ItemGrid.Rows.Count > 0)
            {
                //Microsoft.Office.Interop.Excel.ApplicationClass XcelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                //XcelApp.Application.Workbooks.Add(Type.Missing);

                //for (int i = 1; i < ItemGrid.Columns.Count + 1; i++)
                //{
                //    XcelApp.Cells[1, i] = ItemGrid.Columns[i - 1].HeaderText;
                //}

                //for (int i = 0; i < ItemGrid.Rows.Count; i++)
                //{
                //    for (int j = 0; j < ItemGrid.Columns.Count; j++)
                //    {
                //        XcelApp.Cells[i + 2, j + 1] = ItemGrid.Rows[i].Cells[j].Value.ToString();
                //    }
                //}
                //XcelApp.Columns.AutoFit();
                //XcelApp.Visible = true;
            }
        }
    }
}
