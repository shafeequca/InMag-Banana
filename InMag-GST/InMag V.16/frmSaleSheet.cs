using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InMag_V._16.DataSet;
using InMag_V._16.Reports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;

namespace InMag_V._16
{
    public partial class frmSaleSheet : Form
    {
        DataSet1 ds;
        public frmSaleSheet()
        {
            InitializeComponent();
        }

        private void frmSaleSheet_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.Visible = false;
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
                //string query = "select s.saleId,s.BillNo as Bill_No,CONVERT(VARCHAR(11),s.BillDate,106) as Bill_Date,c.Customer,s.CBalance,s.GrandTotal as Bill_Amount,s.Balance from tblSales s,tblCustomer c where s.custId=c.Custid  and s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "'";

                crystalReportViewer1.Visible = false;
                string query;
                if (chkNonBills.Checked)
                {
                    if (cboArea.Text.Trim() == "")
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c left join tblSales  s on c.custId = s.Custid and s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "'  order by c.Customer";
                    }
                    else
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c left join tblSales  s on c.custId = s.Custid and c.areaId ='" + cboArea.SelectedValue + "' and s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboArea.SelectedValue + "'  where c.areaid='" + cboArea.SelectedValue + "' order by c.Customer";

                    }

                }
                else
                {
                    if (cboArea.Text.Trim() == "")
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c join tblSales  s on c.custId = s.Custid and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' order by c.Customer";
                    }
                    else
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c join tblSales  s on c.custId = s.Custid and c.areaId ='" + cboArea.SelectedValue + "' and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboArea.SelectedValue + "'  where c.areaid='" + cboArea.SelectedValue + "' order by c.Customer";
                    }
                }
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
                ds.Tables["SaleSheet"].Clear();
                ds.Tables["SaleSheet"].Merge(dt);
                ItemGrid.Columns.Clear();
                ItemGrid.DataSource = null;
                if (cboArea.Text.Trim() == "")
                {
                    query = "select CONVERT(DECIMAL(18,2),SUM(s.GrandTotal)) as Bill_Amount,CONVERT(DECIMAL(18,2),SUM(S.Cash)) CASH from tblSales  s where s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "'";
                }
                else
                {
                    query = "select CONVERT(DECIMAL(18,2),SUM(s.GrandTotal)) as Bill_Amount,CONVERT(DECIMAL(18,2),SUM(S.Cash)) CASH from tblSales  s where s.areaId ='" + cboArea.SelectedValue + "' and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "'";
                }
                    
                dt.Clear();
                dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                double Tot = Convert.ToDouble(dt.Rows[0][0].ToString());
                double ch = Convert.ToDouble(dt.Rows[0][1].ToString());

                if (chkNonBills.Checked)
                {
                    if (cboArea.Text.Trim() == "")
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c left join tblSales  s on c.custId = s.Custid and s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' order by c.Customer ";
                    }
                    else
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c left join tblSales  s on c.custId = s.Custid and c.areaId ='" + cboArea.SelectedValue + "' and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboArea.SelectedValue + "'  where c.areaid='" + cboArea.SelectedValue + "' order by c.Customer ";
                    }
                }
                else
                {
                    if (cboArea.Text.Trim() == "")
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c join tblSales  s on c.custId = s.Custid  and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "'  order by c.Customer ";
                    }
                    else
                    {
                        query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance,S.Cash from tblCustomer  c join tblSales  s on c.custId = s.Custid and c.areaId ='" + cboArea.SelectedValue + "' and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboArea.SelectedValue + "'  where c.areaid='" + cboArea.SelectedValue + "' order by c.Customer ";
                    }
                }
                    dt.Clear();
                dt=(DataTable)Connections.Instance.ShowDataInGridView(query);
                DataRow dr = dt.NewRow();
                dr[1] = "Total";
                dr[3] = Tot;//Tot;
                dr[6] = ch;// ch;
                dt.Rows.Add(dr);
               
                ItemGrid.DataSource = dt;
                ItemGrid.Columns[0].Visible = false;
                ItemGrid.Columns[1].MinimumWidth = 350;
                ItemGrid.Columns[5].HeaderText = "Total";
                //ItemGrid.Columns.Add("Cash", "Cash");
                ItemGrid.Columns.Add("Discounts", "Discounts");
                ItemGrid.Columns.Add("Bal", "Balance");
                DataGridViewRow row = ItemGrid.Rows[ItemGrid.Rows.Count - 1];
                row.Height = 50;
                row.DefaultCellStyle.Font = new Font("Arial", 14.0F, FontStyle.Bold);
                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                row.DefaultCellStyle.BackColor = Color.LightBlue;
                //query = "select CONVERT(DECIMAL(18,2),SUM(s.GrandTotal)) as Bill_Amount,CONVERT(DECIMAL(18,2),SUM(S.Cash)) from tblSales  s where s.areaId ='" + cboArea.SelectedValue + "' and s.GrandTotal>0  and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "'";
                //dt.Clear();                
                //dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                //DataRow toInsert = dt.NewRow();
                
                // insert in the desired place
                //dt.Rows.InsertAt(toInsert,);
                //DataGridViewRow row = (DataGridViewRow)ItemGrid.Rows[0].Clone();
                //row.Cells[0].Value = "Total";
                //row.Cells[2].Value = dt.Rows[0][0].ToString();
                //row.Cells[5].Value = dt.Rows[0][1].ToString();
                //ItemGrid.Rows.Add("five", "six", "seven", "eight");
                //ItemGrid.Rows.Add(row);
                
                
            }
            catch (Exception ex)
            { }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.Visible = false;
            if (ItemGrid.Rows.Count == 0)
                btnShow_Click(null, null);
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\Reports\rptSaleSheet.rpt");
            cryRpt.SetDataSource(ds);
            cryRpt.Refresh();
            cryRpt.PrintToPrinter(1, true, 0, 0);
        }

        private void cboArea_Click(object sender, EventArgs e)
        {
            //if (cboArea.SelectedIndex != -1)
            //    btnShow_Click(null, null);
        }

        private void cboArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cboArea.SelectedIndex>=0)
                    btnShow_Click(null, null);
            }
            catch
            { }
        }

        private void DtFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboArea.SelectedIndex >= 0)
                    btnShow_Click(null, null);
            }
            catch
            { }
        }

        private void chkNonBills_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboArea.SelectedIndex >= 0)
                    btnShow_Click(null, null);
            }
            catch
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.Visible = true;
            crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            crystalReportViewer1.Width = ItemGrid.Width;
            crystalReportViewer1.Height = ItemGrid.Height;
            crystalReportViewer1.Top = ItemGrid.Top;
            crystalReportViewer1.Left = ItemGrid.Left;

            if (ItemGrid.Rows.Count == 0)
                btnShow_Click(null, null);
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString() + @"\Reports\rptSaleSheet.rpt");
            cryRpt.SetDataSource(ds);
            cryRpt.Refresh();
            //cryRpt.PrintToPrinter(1, true, 0, 0);
            

            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();
        }

  
    }
}
