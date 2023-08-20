using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InMag_V._16.DataSet;

namespace InMag_V._16
{
    public partial class frmCashEntry : Form
    {
        DataSet1 ds;
        public frmCashEntry()
        {
            InitializeComponent();
            //ItemGrid.KeyDown += new KeyEventHandler(ItemGrid_KeyDown);
            //ItemGrid.EditingControl.KeyDown += new KeyEventHandler(ItemGrid_KeyDown);
            ItemGrid.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(ItemGrid_EditingControls);
        }
            void ItemGrid_EditingControls(object sender,DataGridViewEditingControlShowingEventArgs e)
            {
                e.Control.KeyDown += new KeyEventHandler(ItemGrid_KeyDown); 
                e.Control.Leave+=new EventHandler(Control_Leave);
            }
            private void Control_Leave(object sender, EventArgs e)
            {
              
                int iColumn = ItemGrid.CurrentCell.ColumnIndex;
                int iRow = ItemGrid.CurrentCell.RowIndex;
                if (iColumn == ItemGrid.Columns.Count - 1)
                    ItemGrid.CurrentCell = ItemGrid[0, iRow + 1];
                else
                    ItemGrid.CurrentCell = ItemGrid[iColumn + 1, iRow];
                ItemGrid.BeginEdit(true); 
            }
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                //string query = "select s.saleId,s.BillNo as Bill_No,CONVERT(VARCHAR(11),s.BillDate,106) as Bill_Date,c.Customer,s.CBalance,s.GrandTotal as Bill_Amount,s.Balance from tblSales s,tblCustomer c where s.custId=c.Custid  and s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "'";
                string query = "select s.saleId,c.Customer,s.BillNo as Bill_No,s.GrandTotal as Bill_Amount,CASE WHEN s.BillNo IS NULL THEN c.creditBal ELSE s.CBalance END AS Prev_Balance,s.Balance from tblCustomer  c left join tblSales  s on c.custId = s.Custid and c.areaId ='" + cboArea.SelectedValue + "'  and  s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboArea.SelectedValue + "'  where c.areaid='" + cboArea.SelectedValue + "' order by c.Customer";
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
                ItemGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                ItemGrid.Columns[0].Visible = false;
                ItemGrid.Columns[1].MinimumWidth = 350;
                ItemGrid.Columns[5].HeaderText = "Total";
                ItemGrid.Columns.Add("Cash", "Cash");
                ItemGrid.Columns.Add("Discounts", "Discounts");
                ItemGrid.Columns.Add("Bal", "Balance");
                ItemGrid.CurrentCell = ItemGrid.Rows[0].Cells[6];
                ItemGrid.BeginEdit(true); 
            }
            catch { }
        }
        private void ItemGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                int iColumn = ItemGrid.CurrentCell.ColumnIndex;
                int iRow = ItemGrid.CurrentCell.RowIndex;
                if (iColumn == ItemGrid.Columns.Count - 1)
                    ItemGrid.CurrentCell = ItemGrid[0, iRow + 1];
                else
                    ItemGrid.CurrentCell = ItemGrid[iColumn + 1, iRow];

            }
        }
        private void frmCashEntry_Load(object sender, EventArgs e)
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
    }
}
