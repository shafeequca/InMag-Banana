using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using InMag_V._16.DataSet;

namespace InMag_V._16
{
    public partial class frmOrderedItems : Form
    {
        DataSet1 ds;
        public frmOrderedItems()
        {
            InitializeComponent();
        }

        private void frmOrderedItems_Load(object sender, EventArgs e)
        {
            ds = new DataSet1();
            comboLoad();
        }
        private void comboLoad()
        {
            string query = "select areaId,Area from tblArea order By Area";
            cboArea.DataSource = Connections.Instance.ShowDataInGridView(query);
            cboArea.DisplayMember = "Area";
            cboArea.ValueMember = "areaId";
            cboArea.SelectedIndex = -1;
            cboArea.Text = "";

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "select i.Item_Name,i.inMalayalam,sum(s.qty) as Qty from tblSaleTrans s,tblItem i where s.itemId=i.itemId and s.saleId IN(Select saleId from tblSales where areaId='" + cboArea.SelectedValue + "' and  BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "') group by i.inMalayalam,i.Item_Name";
                System.Data.DataColumn area = new System.Data.DataColumn("Area", typeof(System.String));
                area.DefaultValue = cboArea.Text;
                System.Data.DataColumn DateFrom = new System.Data.DataColumn("DateFrom", typeof(System.String));
                DateFrom.DefaultValue = DtFrom.Value.ToString("dd-MMM-yyyy");
                System.Data.DataColumn DateTo = new System.Data.DataColumn("DateTo", typeof(System.String));
                DateTo.DefaultValue = DtTo.Value.ToString("dd-MMM-yyyy");
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                dt.Columns.Add(area);
                dt.Columns.Add(DateFrom);
                dt.Columns.Add(DateTo);
                ds.Tables["ItemOrder"].Clear();
                ds.Tables["ItemOrder"].Merge(dt);
                ItemGrid.Columns.Clear();
                ItemGrid.DataSource = null;
                ItemGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                ItemGrid.Columns[1].Visible = false;
                ItemGrid.Columns[2].Width = 150;
            }
            catch { }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\Reports\rptOrderSheet.rpt");
            cryRpt.SetDataSource(ds);
            cryRpt.Refresh();
            cryRpt.PrintToPrinter(1, true, 0, 0);
        }
    }
}
