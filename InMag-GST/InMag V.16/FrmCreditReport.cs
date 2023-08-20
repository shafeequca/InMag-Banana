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
    public partial class FrmCreditReport : Form
    {
        DataSet1 ds;
        public FrmCreditReport()
        {
            InitializeComponent();
        }

        private void FrmCreditReport_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet1();
                string query = "select Customer AS Party,Convert(Decimal(18,2),creditBal) CreditAmount,Address AS Test from tblCustomer where Customer<>'General' and creditBal<>0 order by 1";
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                ds.Tables["PartyCredit"].Clear();
                ds.Tables["PartyCredit"].Merge(dt);
                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\Reports\rptPartyCredit.rpt");
                cryRpt.SetDataSource(ds);
                cryRpt.Refresh();
                crystalReportViewer1.ReportSource = cryRpt;
                crystalReportViewer1.Refresh();

            }
            catch (Exception ex)
            { }

        }
    }
}
