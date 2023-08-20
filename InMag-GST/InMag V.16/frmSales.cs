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
    public partial class frmSalesGST : Form
    {
        DataSet1 ds;
        public frmSalesGST()
        {
            InitializeComponent();
            this.cboAreaSearch.SelectionChangeCommitted += new System.EventHandler(this.cboAreaSearch_SelectionChangeCommitted);

            this.ItemDisplayGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ItemDisplayGrid_KeyDown);

            this.CustomerGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomerGrid_KeyDown);

            this.cboArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboArea_KeyDown);
            this.cboAreaSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboAreaSearch_KeyDown);
            this.txtCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyDown);
            this.txtPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPhone_KeyDown);
            this.txtState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtState_KeyDown);
            this.txtStateCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStateCode_KeyDown);
            this.txtVehicle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtVehicle_KeyDown);
            this.txtGSTIN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGSTIN_KeyDown);

            this.txtNos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNos_KeyDown);
            this.txtKg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKg_KeyDown);
            this.txtLess.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLess_KeyDown);
            this.txtQuantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuantity_KeyDown);
            this.txtItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtItems_KeyDown);
            this.txtRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRate_KeyDown);
            this.txtRate.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtItems.KeyPress += new KeyPressEventHandler(txtItems_KeyPress);
            this.txtCash.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtQuantity.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtDiscount.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.txtCBalance.KeyPress += new KeyPressEventHandler(NumberOnly_KeyPress);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Edit_Click);

            this.cboAreaSearch.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.txtBillNoSearch.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.cboCustomerSearch.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.DtFrom.KeyPress += new KeyPressEventHandler(Search_KeyPress);
            this.DtTo.KeyPress += new KeyPressEventHandler(Search_KeyPress);

        }

        private void Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchGridLoad();
            }
        }
        private void NumberOnly_KeyPress(object sender, KeyPressEventArgs e)
        
        {
            TextBox tb = sender as TextBox;
            if (!(e.KeyChar == 8 || e.KeyChar == 46 || e.KeyChar == 13 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
                e.Handled = true;
            else
            {
                if (e.KeyChar == 46 && tb.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            if (e.KeyChar == 13)
            {
                if (tb.Name == "txtCash")
                {
                    btnSave_Click(null, null);
                }
                else if (tb.Name == "txtDiscount" && cboType.Text.ToUpper()=="CASH")
                    btnSave_Click(null, null);
                else if (tb.Name == "txtDiscount" && cboType.Text.ToUpper() == "CREDIT")
                {
                    txtCash.SelectionStart = 0;
                    txtCash.SelectionLength = txtCash.Text.Length;
                    txtCash.Focus();
                }
                else if (tb.Name == "txtItems")
                {
                    txtQuantity.SelectionStart = 0;
                    txtQuantity.SelectionLength = txtQuantity.Text.Length;
                    txtQuantity.Focus();
                }
            }
            else if (e.KeyChar == 27)
            {
                if (tb.Name == "txtDiscount")
                {
                    txtCash.SelectionStart = 0;
                    txtCash.SelectionLength = txtCash.Text.Length;
                }
            }
        }
        private void frmSales_Load(object sender, EventArgs e)
        {
            ds = new DataSet1();
            string creditType = System.Configuration.ConfigurationSettings.AppSettings["DefaultCreditType"];
            if (creditType.ToUpper() == "CASH")
                cboType.SelectedIndex = 0;
            else
                cboType.SelectedIndex = 1;
            comboLoad();
            chkWholeSale.Checked = false;
            txtItemcode.Tag = null;
            SetBillNo();
            SearchGridLoad();
            btnClear_Click(null, null);
            if (cboArea.Items.Count > 0)
            {
                cboArea.SelectedValue = 1;
                txtCustomer.Tag = System.Configuration.ConfigurationSettings.AppSettings["default_user_id"].ToString();
                txtCustomer.Text = System.Configuration.ConfigurationSettings.AppSettings["default_user"].ToString();
                CustomerGrid.Visible = false;
                //cboAreaSearch.Text = "General";
            }
            

        }
        private void SetBillNo()
        {
            string query = "Truncate table tblTemp";
            Connections.Instance.ExecuteQueries(query);
            query = "select billno from tblSettings";
            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
            if (dt.Rows.Count > 0)
                txtBillno.Text = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString();
            else
            {
                txtBillno.Text = "1";
                query = "INSERT INTO tblSettings(billno) VALUES('1')";
                Connections.Instance.ExecuteQueries(query);

            }
        
        }
        private void SearchGridLoad()
        {
            try
            {
                string chCondition="";
                if (chkCashEntry.Checked)
                    chCondition = "and s.Cash=0 and c.CType IN('Customer','Both') order by c.Customer";
                else
                    chCondition = " and c.CType IN('Customer','Both') order by s.BillNo DESC";

                string condition = "(s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "') " + chCondition;
                if (txtBillNoSearch.Text.Trim() != "")
                    condition = "s.BillNo='" + txtBillNoSearch.Text + "'";
                else if (cboAreaSearch.SelectedIndex != -1 && cboAreaSearch.Text != "" && cboCustomerSearch.SelectedIndex != -1 && cboCustomerSearch.Text !="")
                    condition = "(s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboAreaSearch.SelectedValue + "' and s.custId='" + cboCustomerSearch.SelectedValue + "') " + chCondition;
                else if (cboAreaSearch.SelectedIndex != -1 && cboAreaSearch.Text != "")
                    condition = "(s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.areaId='" + cboAreaSearch.SelectedValue + "') " + chCondition;
                else if (cboCustomerSearch.SelectedIndex!=-1 && cboCustomerSearch.Text !="")
                    condition = "(s.BillDate>='" + DtFrom.Value.ToString("dd-MMM-yyyy") + "' and s.BillDate<='" + DtTo.Value.ToString("dd-MMM-yyyy") + "' and s.custId='" + cboCustomerSearch.SelectedValue + "') " + chCondition;

                string query = "select s.saleId,s.BillNo as Bill_No,CONVERT(VARCHAR(5),s.BillDate,3) as Bill_Date,c.Customer,s.areaId,s.custId,s.CBalance,s.GrandTotal as Bill_Amount,s.Cash,s.Discount,s.Balance,s.VehicleNo,s.BillType,s.InterStateBill,CONVERT(VARCHAR(11),s.BillDate,106),RIGHT(CONVERT(VARCHAR(20), s.CreatedOn, 22),11) AS Bill_Time from tblSales s,tblCustomer c where s.custId=c.Custid  and " + condition;
                SearchGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                SearchGrid.Columns[0].Visible = false;
                SearchGrid.Columns[1].Width = 90;
                SearchGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                SearchGrid.Columns[2].Width = 60;
                SearchGrid.Columns[3].Width = 180;
                SearchGrid.Columns[4].Visible = false;
                SearchGrid.Columns[5].Visible = false;
                SearchGrid.Columns[6].Visible = false;
                SearchGrid.Columns[7].Width = 88;
                SearchGrid.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                SearchGrid.Columns[8].Visible = false;
                SearchGrid.Columns[9].Visible = false;
                SearchGrid.Columns[10].Visible = false;
                SearchGrid.Columns[11].Visible = false;
                SearchGrid.Columns[12].Visible = false;
                SearchGrid.Columns[13].Visible = false;
                SearchGrid.Columns[14].Visible = false;
                SearchGrid.Columns[15].Width = 110;
            }
            catch { }
        }
        private void comboLoad()
        {
            try
            {
                itemView.Visible = false;
                string query = "select areaId,Area from tblArea order By Area";
                cboArea.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboArea.DisplayMember = "Area";
                cboArea.ValueMember = "areaId";
                cboArea.SelectedIndex = -1;
                cboArea.Text = "";

                cboAreaSearch.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboAreaSearch.DisplayMember = "Area";
                cboAreaSearch.ValueMember = "areaId";
                cboAreaSearch.SelectedIndex = -1;
                cboAreaSearch.Text = "";

                query = "select custId,Customer from tblCustomer order by Customer";
                cboCustomerSearch.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboCustomerSearch.DisplayMember = "Customer";
                cboCustomerSearch.ValueMember = "custId";
                cboCustomerSearch.Text = "";
                cboCustomerSearch.SelectedIndex = -1;
                

                //query = "select itemId,Item_Name from tblItem order By Item_Name";
                //cboItems.DataSource = Connections.Instance.ShowDataInGridView(query);
                //cboItems.DisplayMember = "Item_Name";
                //cboItems.ValueMember = "itemId";
                //cboItems.SelectedIndex = -1;
                //cboItems.Text = "";
            }
            catch { }
        }

        private void cboAreaSearch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboAreaSearch.SelectedValue != null && cboAreaSearch.Text !="")
            {
                System.Data.DataRowView DR = (System.Data.DataRowView)cboArea.Items[cboAreaSearch.SelectedIndex];
                string query = "select custId,Customer from tblCustomer where areaId='" + DR.Row[0].ToString() + "' order by Customer";
                cboCustomerSearch.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboCustomerSearch.DisplayMember = "Customer";
                cboCustomerSearch.ValueMember = "custId";
                cboCustomerSearch.Text = "";
                cboCustomerSearch.SelectedIndex = -1;
               
            }
        }
        
        
        
        private void cboAreaSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cboAreaSearch_SelectionChangeCommitted(null, null);
            }
        }
        
        
        private void cboArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            
            {
                txtCustomer.Focus();
                txtCustomer_TextChanged(null, null);
                CustomerGrid.Visible = true;

            }
        }

        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                txtCustomer_TextChanged(null, null);
                CustomerGrid.Focus();
            }
            else if (e.KeyData == Keys.Enter)
            {
                CustomerGrid.Visible = false;
                txtAddress.Focus();
            }

        }
        private void txtNos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtKg.SelectionStart = 0;
                txtKg.SelectionLength = txtKg.Text.Length;
                txtKg.Focus();
            }
        }
        private void txtKg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtLess.SelectionStart = 0;
                txtLess.SelectionLength = txtLess.Text.Length;
                txtLess.Focus();
            }
        }
        private void txtLess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtRate.SelectionStart = 0;
                txtRate.SelectionLength = txtRate.Text.Length;
                txtRate.Focus();
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTotal.Text = (Convert.ToDouble(txtQuantity.Text == "" ? "0" : txtQuantity.Text) * Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text)).ToString();
            }

            catch
            {
                txtTotal.Text = "0";
            }
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTotal.Text = (Convert.ToDouble(txtQuantity.Text == "" ? "0" : txtQuantity.Text) * Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text)).ToString();
            }
            catch
            { }
        }
        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtRate.SelectionStart = 0;
                txtRate.SelectionLength = txtRate.Text.Length;
                txtRate.Focus();
            }
        }
        private void GridShow()
        {
            try
            {
                Type t = typeof(System.Windows.Forms.SystemInformation);

                string query = "select ROW_NUMBER() OVER(ORDER BY id) AS Row,itemcode,itemid,itemname,Nos,Kg,Less,TotLess,qty,rate,total from tblTemp;";
                ItemGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                ItemGrid.Columns[0].Width = 35;
                ItemGrid.Columns[1].Width = 0;// txtItemcode.Width;//86;
                ItemGrid.Columns[2].Visible = false;
                ItemGrid.Columns[3].Width = txtItems.Width - 0;//203;
                ItemGrid.Columns[4].Width = txtNos.Width - 0;
                ItemGrid.Columns[5].Width = txtKg.Width - 0;
                ItemGrid.Columns[6].Width = txtLess.Width - 0;
                ItemGrid.Columns[7].Width = txtTotLess.Width - 0;

                ItemGrid.Columns[8].Width = txtQuantity.Width;//128;
                ItemGrid.Columns[9].Width = txtRate.Width-0;//124;
                ItemGrid.Columns[10].Width = txtTotal.Width -5-  (ItemGrid.Controls.OfType<VScrollBar>().First().Visible? SystemInformation.VerticalScrollBarWidth:0); //145;
                if (ItemGrid.Rows.Count > 0)
                {
                    ItemGrid.FirstDisplayedScrollingRowIndex = ItemGrid.RowCount - 1;
                    ItemGrid.Rows[ItemGrid.RowCount - 1].Selected = true;
                }
                query = "select sum(total) from tblTemp";
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                if (dt.Rows.Count > 0)
                    txtBillTotal.Text = dt.Rows[0][0].ToString();
                taxCalc();
                CustomerGrid.Visible = false;
                itemView.Visible = false;
            }
            catch { }
        }
        private void taxCalc()
        {
            try
            {
                double discount = 0;
                if (txtDiscount.Text == "." || txtDiscount.Text.Trim() == "")
                    discount = 0;
                else
                    discount = Convert.ToDouble(txtDiscount.Text);
                double cash = 0;
                
                double disc = discount / ItemGrid.RowCount;

                if (ItemGrid.RowCount == 0)
                {
                    disc = discount;
                    txtCGST.Text = "0";
                    txtSGST.Text = "0";
                    txtIGST.Text = "0";
                    txtGST.Text = "0";

                }

                string query = "UPDATE t "+
                                    "SET CGSTPER=tt.CGSTPER"+
	                                ",CGST=tt.CGST"+
	                                ",SGSTPER=tt.SGSTPER"+
	                                ",SGST=tt.SGST"+
	                                ",IGSTPER=tt.IGSTPER"+
	                                ",IGST=tt.IGST "+
                                    ",disc="+disc+" "+
                                "FROM tblTemp t "+
                                "INNER JOIN "+
                                "(SELECT t.itemId,i.CGSTPER,CONVERT(DECIMAL(18,2),SUM((t.Total-" + disc + ")*(ISNULL(i.CGSTPER,0)/100))) AS CGST,i.SGSTPER,CONVERT(DECIMAL(18,2),SUM((t.Total-" + disc + ")*(ISNULL(i.SGSTPER,0)/100))) AS SGST,i.IGSTPER,CONVERT(DECIMAL(18,2),SUM((t.Total-" + disc + ")*(ISNULL(i.IGSTPER,0)/100))) AS IGST from tblTemp t INNER JOIN tblItem i ON i.itemId=t.itemId " +
                                "group by i.CGSTPER,i.SGSTPER,i.IGSTPER,t.itemId) tt "+
                                "ON t.itemId=tt.itemId ";
                if (chkInterState.Checked == true)
                {
                    query = "UPDATE t " +
                                    "SET CGSTPER=tt.ISCGSTPER" +
                                    ",CGST=tt.CGST" +
                                    ",SGSTPER=tt.ISSGSTPER" +
                                    ",SGST=tt.SGST" +
                                    ",IGSTPER=tt.ISIGSTPER" +
                                    ",IGST=tt.IGST " +
                                    ",disc=" + disc + " " +
                                "FROM tblTemp t " +
                                "INNER JOIN " +
                                "(SELECT t.itemId,i.ISCGSTPER,CONVERT(DECIMAL(18,2),SUM((t.Total-" + disc + ")*(ISNULL(i.ISCGSTPER,0)/100))) AS CGST,i.ISSGSTPER,CONVERT(DECIMAL(18,2),SUM((t.Total-" + disc + ")*(ISNULL(i.ISSGSTPER,0)/100))) AS SGST,i.ISIGSTPER,CONVERT(DECIMAL(18,2),SUM((t.Total-" + disc + ")*(ISNULL(i.ISIGSTPER,0)/100))) AS IGST from tblTemp t INNER JOIN tblItem i ON i.itemId=t.itemId " +
                                "group by i.ISCGSTPER,i.ISSGSTPER,i.ISIGSTPER,t.itemId) tt " +
                                "ON t.itemId=tt.itemId ";
                }
                Connections.Instance.ExecuteQueries(query);
                query = "UPDATE tblTemp SET GST=CGST+SGST+IGST";
                Connections.Instance.ExecuteQueries(query);
                query = "select CONVERT(DECIMAL(18,2),SUM(t.CGST)),CONVERT(DECIMAL(18,2),SUM(t.SGST)),CONVERT(DECIMAL(18,2),SUM(t.IGST)) from tblTemp t";
                
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                if (dt.Rows.Count > 0)
                {
                    txtCGST.Text = dt.Rows[0][0].ToString();
                    txtSGST.Text = dt.Rows[0][1].ToString();
                    txtIGST.Text = dt.Rows[0][2].ToString();

                }
                else
                {
                    txtCGST.Text = "0";
                    txtSGST.Text = "0";
                    txtIGST.Text = "0";
                    txtGST.Text = "0";

                }
               
                txtGST.Text = (Convert.ToDouble(txtCGST.Text == "" ? "0" : txtCGST.Text) + Convert.ToDouble(txtSGST.Text == "" ? "0" : txtSGST.Text) + Convert.ToDouble(txtIGST.Text == "" ? "0" : txtIGST.Text)).ToString();
                txtGrand.Text = ((Convert.ToDouble(txtBillTotal.Text == "" ? "0" : txtBillTotal.Text) + Convert.ToDouble(txtGST.Text == "" ? "0" : txtGST.Text)) - discount).ToString();
                if (txtCash.Text == "." || txtCash.Text.Trim() == "")
                    cash = 0;
                else
                    cash = Convert.ToDouble(txtCash.Text);
                txtBalance.Text = ((Convert.ToDouble(txtGrand.Text == "" ? "0" : txtGrand.Text) + Convert.ToDouble(txtCBalance.Text == "" ? "0" : txtCBalance.Text)) - cash).ToString();
            }
            catch {
                txtGST.Text = "";
                txtGrand.Text = "";
                txtBalance.Text = "";
            }
        }
        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtItems.Text.Trim() == "" || txtItems.Tag == null || txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == "0" || txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
                {
                    if (txtItems.Text.Trim() == "" && txtItems.Tag == null && (txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == "0") && (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0"))
                    {
                        txtDiscount.SelectionStart = 0;
                        txtDiscount.SelectionLength = txtDiscount.Text.Length;
                        txtDiscount.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Please enter the details properly");
                        txtItems.Focus();
                    }

                    
                }
                else
                {
                    string query = "select Current_Stock,minRate from tblItem where itemId='" + txtItems.Tag.ToString() + "'";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    query = "select qty from tblTemp where itemId='" + txtItems.Tag.ToString() + "'";
                    DataTable dt1 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    if (Convert.ToDouble(dt.Rows[0][1].ToString()) > Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text))
                    {
                        MessageBox.Show("Minimum rate level reached" + Environment.NewLine + "Please enter the rate >= " + dt.Rows[0][1].ToString());
                        txtRate.Text = dt.Rows[0][1].ToString();
                        txtRate.SelectionStart = 0;
                        txtRate.SelectionLength = txtRate.Text.Length;
                        txtRate.Focus();
                        return;
                    
                    }
                    
                    if (dt1.Rows.Count > 0)
                    {
                        if (txtBillno.Tag == null)
                        {
                            if (txtItemcode.Tag == null)
                            {
                                double newQty = Convert.ToDouble(txtQuantity.Text == "" ? "0" : txtQuantity.Text) + Convert.ToDouble(dt1.Rows[0][0].ToString());
                                double newTotal = newQty * Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < newQty)
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                            }
                            else
                            {
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < Convert.ToDouble(txtQuantity.Text == "" ? "0" : txtQuantity.Text))
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim() == "" ? "0" : txtQuantity.Text) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim() == "" ? "0" : txtTotal.Text) + "',Nos='" + Convert.ToInt32(txtNos.Text.Trim() == "" ? "0" : txtNos.Text) + "',Kg='" + Convert.ToDouble(txtKg.Text.Trim() == "" ? "0" : txtKg.Text) + "',Less='" + Convert.ToDouble(txtLess.Text.Trim() == "" ? "0" : txtLess.Text) + "',TotLess='" + Convert.ToDouble(txtTotLess.Text.Trim() == "" ? "0" : txtTotLess.Text) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim() == "" ? "0" : txtQuantity.Text) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim() == "" ? "0" : txtTotal.Text) + "',Nos='" + Convert.ToInt32(txtNos.Text.Trim() == "" ? "0" : txtNos.Text) + "',Kg='" + Convert.ToDouble(txtKg.Text.Trim() == "" ? "0" : txtKg.Text) + "',Less='" + Convert.ToDouble(txtLess.Text.Trim() == "" ? "0" : txtLess.Text) + "',TotLess='" + Convert.ToDouble(txtTotLess.Text.Trim() == "" ? "0" : txtTotLess.Text) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                            }
                        }
                        else
                        {
                            if (txtItemcode.Tag == null)
                            {
                                double newQty = Convert.ToDouble(txtQuantity.Text == "" ? "0" : txtQuantity.Text) + Convert.ToDouble(dt1.Rows[0][0].ToString());
                                double newTotal = newQty * Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
                                query = "select qty from tblSaleTrans where itemId='" + txtItems.Tag.ToString() + "' and saleId='" + txtBillno.Tag.ToString() + "'";
                                DataTable dt2 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < (newQty-Convert.ToDouble(dt2.Rows[0][0].ToString())))
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + newQty + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + newTotal + "' where itemId='" + txtItems.Tag.ToString() + "'";
                            }
                            else
                            {
                                query = "select qty from tblSaleTrans where itemId='" + txtItems.Tag.ToString() + "' and saleId='" + txtBillno.Tag.ToString() + "'";
                                DataTable dt2 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                                if (Convert.ToDouble(dt.Rows[0][0].ToString()) < (Convert.ToDouble(txtQuantity.Text == "" ? "0" : txtQuantity.Text) - Convert.ToDouble(dt2.Rows[0][0].ToString())))
                                {
                                    DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                        query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim() == "" ? "0" : txtQuantity.Text) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim() == "" ? "0" : txtTotal.Text) + "',Nos='" + Convert.ToInt32(txtNos.Text.Trim() == "" ? "0" : txtNos.Text) + "',Kg='" + Convert.ToDouble(txtKg.Text.Trim() == "" ? "0" : txtKg.Text) + "',Less='" + Convert.ToDouble(txtLess.Text.Trim() == "" ? "0" : txtLess.Text) + "',TotLess='" + Convert.ToDouble(txtTotLess.Text.Trim() == "" ? "0" : txtTotLess.Text) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                                }
                                else
                                    query = "update tblTemp set Qty='" + Convert.ToDouble(txtQuantity.Text.Trim() == "" ? "0" : txtQuantity.Text) + "',rate='" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "',total='" + Convert.ToDouble(txtTotal.Text.Trim() == "" ? "0" : txtTotal.Text) + "',Nos='" + Convert.ToInt32(txtNos.Text.Trim() == "" ? "0" : txtNos.Text) + "',Kg='" + Convert.ToDouble(txtKg.Text.Trim() == "" ? "0" : txtKg.Text) + "',Less='" + Convert.ToDouble(txtLess.Text.Trim() == "" ? "0" : txtLess.Text) + "',TotLess='" + Convert.ToDouble(txtTotLess.Text.Trim() == "" ? "0" : txtTotLess.Text) + "' where itemId='" + Convert.ToDouble(txtItemcode.Tag.ToString()) + "'";
                            }
                        }
                    }
                    else
                    {
                        double exQty = 0;
                        if (txtBillno.Tag != null)
                        {
                            query = "select qty from tblSaleTrans where itemId='" + txtItems.Tag.ToString() + "' and saleId='" + txtBillno.Tag.ToString() + "'";
                            DataTable dt3 = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            exQty = Convert.ToDouble(dt.Rows[0][0].ToString());
                        }
                        if (Convert.ToDouble(dt.Rows[0][0].ToString()) < (Convert.ToDouble(txtQuantity.Text == "" ? "0" : txtQuantity.Text)-exQty))
                        {
                            DialogResult dialogResult = MessageBox.Show("No sufficient stock." + Environment.NewLine + "Do you want to continue with negative stock?", "Sale Voucher", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                                query = "insert into tblTemp(itemCode,ItemId,ItemName,Nos,Kg,Less,TotLess,Qty,Rate,Total) values('" + txtItemcode.Text.Trim() + "','" + txtItems.Tag.ToString() + "','" + txtItems.Text.Trim() + "','" + Convert.ToDouble(txtNos.Text.Trim() == "" ? "0" : txtNos.Text) + "','" + Convert.ToDouble(txtKg.Text.Trim() == "" ? "0" : txtKg.Text) + "','" + Convert.ToDouble(txtLess.Text.Trim() == "" ? "0" : txtLess.Text) + "','" + Convert.ToDouble(txtTotLess.Text.Trim() == "" ? "0" : txtTotLess.Text) + "','" + Convert.ToDouble(txtQuantity.Text.Trim() == "" ? "0" : txtQuantity.Text) + "','" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "','" + Convert.ToDouble(txtTotal.Text.Trim() == "" ? "0" : txtTotal.Text) + "')";
                        }
                        else
                            query = "insert into tblTemp(itemCode,ItemId,ItemName,Nos,Kg,Less,TotLess,Qty,Rate,Total) values('" + txtItemcode.Text.Trim() + "','" + txtItems.Tag.ToString() + "','" + txtItems.Text.Trim() + "','" + Convert.ToDouble(txtNos.Text.Trim() == "" ? "0" : txtNos.Text) + "','" + Convert.ToDouble(txtKg.Text.Trim() == "" ? "0" : txtKg.Text) + "','" + Convert.ToDouble(txtLess.Text.Trim() == "" ? "0" : txtLess.Text) + "','" + Convert.ToDouble(txtTotLess.Text.Trim() == "" ? "0" : txtTotLess.Text) + "','" + Convert.ToDouble(txtQuantity.Text.Trim() == "" ? "0" : txtQuantity.Text) + "','" + Convert.ToDouble(txtRate.Text.Trim() == "" ? "0" : txtRate.Text) + "','" + Convert.ToDouble(txtTotal.Text.Trim() == "" ? "0" : txtTotal.Text) + "')";
                    }
                    Connections.Instance.ExecuteQueries(query);
                    dt.Dispose();
                    dt1.Dispose();
                    GridShow();
                    cmdItemClear_Click(null, null);
                }
            }
        }
        private void cmdItemClear_Click(object sender, EventArgs e)
        {
            txtItemcode.Tag = null;
            txtItemcode.Enabled = true;
            txtItemcode.Text ="";
            txtItems.Enabled = true;
            txtItems.Tag = null;
            txtItems.Text = "";
            txtNos.Text = "0";
            txtKg.Text = "0";
            txtLess.Text = "0";
            txtQuantity.Text = "0";
            txtRate.Text = "0";
            lblPRate.Text = "0";
            txtItems.Focus();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        private void Edit_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ItemGrid.SelectedRows.Count > 0)
            {
                int rowno = ItemGrid.SelectedRows[0].Index;
                ToolStripItem item = e.ClickedItem;
                if (item.Name == "Edit")
                {
                    txtItemcode.Enabled = false;
                    txtItems.Enabled = false;
                    txtItems.Text = ItemGrid.Rows[rowno].Cells[3].Value.ToString();
                    txtItemcode.Text = ItemGrid.Rows[rowno].Cells[1].Value.ToString();
                    txtItems.Tag = ItemGrid.Rows[rowno].Cells[2].Value.ToString();
                    txtNos.Text = ItemGrid.Rows[rowno].Cells[4].Value.ToString();
                    txtKg.Text = ItemGrid.Rows[rowno].Cells[5].Value.ToString();
                    txtLess.Text = ItemGrid.Rows[rowno].Cells[6].Value.ToString();
                    txtTotLess.Text = ItemGrid.Rows[rowno].Cells[7].Value.ToString();
                    txtQuantity.Text  = ItemGrid.Rows[rowno].Cells[8].Value.ToString();
                    txtRate.Text = ItemGrid.Rows[rowno].Cells[9].Value.ToString();
                    txtItemcode.Tag = ItemGrid.Rows[rowno].Cells[2].Value.ToString();
                    itemView.Visible = false;
                    txtNos.SelectionStart = 0;
                    txtNos.SelectionLength = txtNos.Text.Length;
                    txtNos.Focus();
                }
                else if (item.Name == "Delete")
                {
                    string query = "delete from tblTemp where itemId='" + ItemGrid.Rows[rowno].Cells[2].Value.ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    GridShow();
                }
            }
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }
        private void Calculation()
        {
            double disc = 0;
            if (txtDiscount.Text == "." || txtDiscount.Text.Trim() == "")
                disc = 0;
            else
                disc = Convert.ToDouble(txtDiscount.Text);
            double cash = 0;
            if (txtCash.Text == "." || txtCash.Text.Trim() == "")
                cash = 0;
            else
                cash = Convert.ToDouble(txtCash.Text);

            txtBalance.Text = ((Convert.ToDouble(txtCBalance.Text == "" ? "0" : txtCBalance.Text) + Convert.ToDouble(txtGrand.Text == "" ? "0" : txtGrand.Text)) - cash).ToString();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            taxCalc();

            Calculation();

        }

        private void txtCBalance_TextChanged(object sender, EventArgs e)
        {
            Calculation();
        }

        private void txtGrand_TextChanged(object sender, EventArgs e)
        {
            Calculation();
            if (cboType.Text.ToUpper()=="CASH")
                txtCash.Text = txtGrand.Text;
        }

        private void cboRategroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemGrid.Rows.Count > 0)
            {
                for (int i = 0; i < ItemGrid.Rows.Count; i++)
                {
                    string query = "";
                    if (!chkWholeSale.Checked)
                        query = "select rate from tblItem where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                    else if (chkWholeSale.Checked)
                        query = "select wrate from tblItem where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    query = "update tblTemp set rate='" + Convert.ToDouble(dt.Rows[0][0].ToString()) + "',Total='" + Convert.ToDouble(dt.Rows[0][0].ToString()) * Convert.ToDouble(ItemGrid.Rows[i].Cells[4].Value) + "' where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                    Connections.Instance.ExecuteQueries(query);
                }
                GridShow();
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBillno.Tag = null;
            chkInterState.Checked = false;
            string creditType = System.Configuration.ConfigurationSettings.AppSettings["DefaultCreditType"];
            if (creditType.ToUpper() == "CASH")
                cboType.SelectedIndex = 0;
            else
                cboType.SelectedIndex = 1;
            txtState.Text = "";
            txtSGST.Text = "";
            txtCGST.Text = "";
            txtIGST.Text = "";
            txtStateCode.Text = "";
            txtPhone.Text = "";
            txtGSTIN.Text = "";
            txtVehicle.Text = "";
            cboArea.Enabled = true;
            txtCustomer.Tag = null;
            txtAddress.Text = "";
            txtCBalance.Text = "";
            cmdItemClear_Click(null, null);
            ItemGrid.DataSource = null;
            txtGrand.Text = "";
            txtCash.Text = "";
            txtDiscount.Text ="";
            txtBalance.Text = "0";
            SetBillNo();
            txtCustomer.Text = "";
            txtBillTotal.Text = "";
            CustomerGrid.Visible = false;

            //SearchGridLoad();
            //if(!(cboAreaSearch.SelectedIndex>=0 || txtBillNoSearch.Text.Trim()!="" || cboCustomerSearch.SelectedIndex>=0))
            //    btnReset_Click(null, null);
            //else if(cboAreaSearch.SelectedIndex>=0 && cboAreaSearch.SelectedIndex==cboArea.SelectedIndex)
                btnReset_Click(null, null);
                txtGST.Text = "";
                txtGrand.Text = "";
                if (cboArea.Items.Count > 0 && cboArea.SelectedValue!=null)
                {
                    if (cboArea.SelectedValue.ToString() == "1")
                    {
                        txtCustomer.Tag = System.Configuration.ConfigurationSettings.AppSettings["default_user_id"].ToString();
                        txtCustomer.Text = System.Configuration.ConfigurationSettings.AppSettings["default_user"].ToString();
                        CustomerGrid.Visible = false;
                        txtItems.Focus();

                    }
                    else
                    {

                        txtCustomer.Focus();
                    }
                }
                else
                {

                    txtCustomer.Focus();
                }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchGridLoad();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string query = "select areaId,Area from tblArea order By Area";            
            cboAreaSearch.DataSource = Connections.Instance.ShowDataInGridView(query);
            cboAreaSearch.DisplayMember = "Area";
            cboAreaSearch.ValueMember = "areaId";
            cboAreaSearch.SelectedIndex = -1;
            cboAreaSearch.Text = "";

            query = "select custId,Customer from tblCustomer order by Customer";
            cboCustomerSearch.DataSource = Connections.Instance.ShowDataInGridView(query);
            cboCustomerSearch.DisplayMember = "Customer";
            cboCustomerSearch.ValueMember = "custId";
            cboCustomerSearch.Text = "";
            cboCustomerSearch.SelectedIndex = -1;

            DtFrom.Value = DateTime.Today;
            DtTo.Value = DateTime.Today;
            txtBillNoSearch.Text = "";
            SearchGridLoad();
            cboAreaSearch.Focus();
        }

        private void SearchGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            { 
                txtBillno.Tag = SearchGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtBillno.Text = SearchGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                DatePicker.Value = Convert.ToDateTime(SearchGrid.Rows[e.RowIndex].Cells[14].Value);
                cboArea.SelectedValue = SearchGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtCustomer.Tag = SearchGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtCustomer.Text = SearchGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
                string query = "select * from tblCustomer where custId='" + txtCustomer.Tag + "'";
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                if (dt.Rows.Count > 0)
                {
                    txtAddress.Text = dt.Rows[0][12].ToString();
                    txtPhone.Text = dt.Rows[0][5].ToString();
                    txtGSTIN.Text = dt.Rows[0][11].ToString();
                    txtState.Text = dt.Rows[0][13].ToString();
                    txtStateCode.Text = dt.Rows[0][14].ToString();
                    
                }
                cboType.Text = SearchGrid.Rows[e.RowIndex].Cells[12].Value.ToString();
                txtCBalance.Text = SearchGrid.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtGrand.Text = SearchGrid.Rows[e.RowIndex].Cells[7].Value.ToString();
                txtCash.Text = SearchGrid.Rows[e.RowIndex].Cells[8].Value.ToString();
                txtDiscount.Text = SearchGrid.Rows[e.RowIndex].Cells[9].Value.ToString();
                txtBalance.Text = SearchGrid.Rows[e.RowIndex].Cells[10].Value.ToString();
                txtVehicle.Text = SearchGrid.Rows[e.RowIndex].Cells[11].Value.ToString();
                chkInterState.Checked = Convert.ToBoolean(SearchGrid.Rows[e.RowIndex].Cells[13].Value);

                 query = "truncate table tblTemp";
                Connections.Instance.ExecuteQueries(query);
                query = "insert into tblTemp(itemCode,ItemId,ItemName,Qty,Rate,Total,Nos,Kg,Less,TotLess) select i.Item_Code,i.itemId,i.Item_Name,s.qty,s.rate,s.total,s.Nos,s.Kg,s.Less,s.TotLess from tblSaleTrans s,tblItem i where s.itemId=i.itemId and saleId='" + txtBillno.Tag.ToString() + "'";
                Connections.Instance.ExecuteQueries(query);
                GridShow();
                
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
                 //if (ItemGrid.Rows.Count == 0)
                //    MessageBox.Show("Please add items");
                //else
                //{
            if (cboArea.Text == "")
            {
                MessageBox.Show("Please select an area");
                cboArea.Focus();
                return;
            }
            if (txtCustomer.Text == "")
            {
                MessageBox.Show("Please enter the customer name");
                txtCustomer.Focus();
                return;
            }
            if (ItemGrid.Rows.Count == 0)
            {
                //MessageBox.Show("Please add items");
                //txtItems.Focus();
                //return;
                txtCGST.Text = "0";
                txtSGST.Text = "0";
                txtIGST.Text = "0";
                txtGST.Text = "0";
            }
                    DialogResult dialogResult = MessageBox.Show("Do you want to save this bill?", "Sale Voucher", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "";
                        int custId;

                        if (txtCustomer.Tag == null || txtCustomer.Tag == "")
                        {
                            query = "INSERT INTO tblCustomer(Customer,Phone,areaId,creditBal,GSTIN,Address,State,State_code,CType) output INSERTED.custId " +
                                "VALUES ('" + txtCustomer.Text + "','" + txtPhone.Text + "','" + ((cboArea.SelectedValue == null) ? 1 : cboArea.SelectedValue).ToString() + "','" + ((cboType.Text.ToUpper() == "CASH") ? "0" : txtBalance.Text).ToString() + "','" + txtGSTIN.Text + "','" + txtAddress.Text + "','" + txtState.Text + "','" + txtStateCode.Text + "','Customer')";
                            custId = Connections.Instance.ExuecuteQueryWithReturn(query);
                        }
                        else
                        {
                            custId = Convert.ToInt32(txtCustomer.Tag.ToString());
                            query = "UPDATE tblCustomer SET Customer='" + txtCustomer.Text + "',Phone='" + txtPhone.Text + "',GSTIN='" + txtGSTIN.Text + "',Address='" + txtAddress.Text + "',State='" + txtState.Text + "',State_code='" + txtStateCode.Text + "' WHERE custId='" + txtCustomer.Tag + "'";
                            Connections.Instance.ExecuteQueries(query);
                        }

                        if (txtBillno.Tag == null)
                        {

                            query = "insert into tblSales(BillNo,BillDate,areaId,custId,CBalance,GrandTotal,Cash,Discount,Balance,updated" +
                                    ",VehicleNo,CGST,SGST,IGST,BillType,InterStateBill)" +
                                    "values('" + txtBillno.Text + "','" + DatePicker.Value.ToString("dd-MMM-yyyy") + "','" + cboArea.SelectedValue + "','" + custId + "','" + Convert.ToDouble(txtCBalance.Text == "" ? "0" : txtCBalance.Text) + "','" + Convert.ToDouble(txtGrand.Text == "" ? "0" : txtGrand.Text) + "','" + Convert.ToDouble(txtCash.Text == ""||txtCash.Text == "." ? "0" : txtCash.Text) + "','" + Convert.ToDouble(txtDiscount.Text == ""||txtDiscount.Text == "." ? "0" : txtDiscount.Text) + "','" + Convert.ToDouble(txtBalance.Text == "" ? "0" : txtBalance.Text) + "','false'," +
                                    "'" + txtVehicle.Text + "','" + txtCGST.Text + "','" + txtSGST.Text + "','" + txtIGST.Text + "','"+ cboType.Text +"','"+ chkInterState.Checked +"')";
                            Connections.Instance.ExecuteQueries(query);
                            query = "select ident_current('tblSales')";
                            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            int id = Convert.ToInt32(dt.Rows[0][0].ToString());
                            if (cboType.Text.ToUpper() != "CASH")
                            {
                                query = "update tblCustomer set creditBal='" + Convert.ToDouble(txtBalance.Text == "" ? "0" : txtBalance.Text) + "' where custId='" + custId + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }
                            query = "insert into tblSaleTrans([saleId],[itemid],[qty],[rate],[total],[Updated]," +
                                "[cgstper],[cgst],[sgstper],[sgst],[igstper],[igst],[gst],[disc],[Nos],[Less],[TotLess],[Kg]) " +
                                "SELECT '" + id + "',ItemId,Qty,Rate,Total,'False',"+
                                "cgstper,cgst,sgstper,sgst,igstper,igst,gst,disc,[Nos],[Less],[TotLess],[Kg] FROM tblTemp";
                            Connections.Instance.ExecuteQueries(query);
                            for (int i = 0; i < ItemGrid.Rows.Count; i++)
                            {
                                
                                query = "update tblItem set Current_Stock=Current_Stock-'" + ItemGrid.Rows[i].Cells[4].Value + "' where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }
                            query = "update tblSettings set BillNo=BillNo+1";
                            Connections.Instance.ExecuteQueries(query);

                        }
                        else
                        {
                            //update

                            
                            query = "select custId,GrandTotal,Cash,CBalance,BillType from tblSales where saleId='" + txtBillno.Tag.ToString() + "'";
                            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            int custId_Old = Convert.ToInt32(dt.Rows[0][0].ToString());
                            double Cash_Old = Convert.ToDouble(dt.Rows[0][2].ToString());
                            double GT_Old = Convert.ToDouble(dt.Rows[0][1].ToString());
                            double pbaldiff = Convert.ToDouble(txtCBalance.Text == "" || txtCBalance.Text == "." ? "0" : txtCBalance.Text) - Convert.ToDouble(dt.Rows[0][3].ToString());
                            double billDiff = Convert.ToDouble(dt.Rows[0][1].ToString())-Convert.ToDouble(txtGrand.Text == "" || txtGrand.Text == "." ? "0" : txtGrand.Text);
                            double cashDiff = Convert.ToDouble(txtCash.Text == "" || txtCash.Text == "." ? "0" : txtCash.Text) - Convert.ToDouble(dt.Rows[0][2].ToString());
                            double diffBal = billDiff + cashDiff;                          
                            //if (Convert.ToDouble(dt.Rows[0][3].ToString()) == 0)
                            //    diffBal = (Convert.ToDouble(txtGrand.Text)-Convert.ToDouble(txtCash.Text == "" || txtCash.Text == "." ? "0" : txtCash.Text)) * -1;
                            //if (dt.Rows[0][4].ToString().ToUpper() == "CREDIT" && cboType.Text.ToUpper() == "CASH")
                            //    diffBal = Convert.ToDouble(txtCash.Text == "" || txtCash.Text == "." ? "0" : txtCash.Text);
                            if (custId_Old != custId)
                            {
                                query = "update tblCustomer set creditBal=creditBal+'" + (Cash_Old - GT_Old) + "' where custId='" + custId_Old + "'";
                                Connections.Instance.ExecuteQueries(query);
                                query = "update tblCustomer set creditBal='" + (txtBalance.Text) + "' where custId='" + custId + "'";
                                Connections.Instance.ExecuteQueries(query);

                            }
                            else
                            {

                                query = "update tblCustomer set creditBal=creditBal-'" + diffBal + "' where custId='" + custId + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }
                            query = "update tblSales set custid='" + custId + "',BillDate='" + DatePicker.Value.ToString("dd-MMM-yyyy") + "',CBalance='" + Convert.ToDouble(txtCBalance.Text == "" || txtCBalance.Text == "." ? "0" : txtCBalance.Text) + "', GrandTotal='" + txtGrand.Text + "',Cash='" + Convert.ToDouble(txtCash.Text == "" || txtCash.Text == "." ? "0" : txtCash.Text) + "',Discount='" + Convert.ToDouble(txtDiscount.Text == "" || txtDiscount.Text == "." ? "0" : txtDiscount.Text) + "',Balance='" + Convert.ToDouble(txtBalance.Text == "" ? "0" : txtBalance.Text) + "',VehicleNo='" + txtVehicle.Text + "',CGST='" + txtCGST.Text + "',SGST='" + txtSGST.Text + "',IGST='" + txtIGST.Text + "',BillType='" + cboType.Text + "',InterStateBill='" + chkInterState.Checked + "' where saleId='" + txtBillno.Tag.ToString() + "'";
                            Connections.Instance.ExecuteQueries(query);
                            query = "select itemId,qty from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                            dt.Rows.Clear();
                            dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                query = "update tblItem set Current_Stock=Current_Stock+'" + Convert.ToDouble(dt.Rows[i][1].ToString()) + "' where itemId='" + dt.Rows[i][0].ToString() + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }
                            query = "delete from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                            Connections.Instance.ExecuteQueries(query);
                            query = "insert into tblSaleTrans([saleId],[itemid],[qty],[rate],[total],[Updated]," +
                               "[cgstper],[cgst],[sgstper],[sgst],[igstper],[igst],[gst],[disc],[Nos],[Less],[TotLess],[Kg]) " +
                               "SELECT '" + txtBillno.Tag.ToString() + "',ItemId,Qty,Rate,Total,'False'," +
                               "cgstper,cgst,sgstper,sgst,igstper,igst,gst,disc,[Nos],[Less],[TotLess],[Kg] FROM tblTemp";
                            Connections.Instance.ExecuteQueries(query);
                            for (int i = 0; i < ItemGrid.Rows.Count; i++)
                            {
                                query = "update tblItem set Current_Stock=Current_Stock-'" + ItemGrid.Rows[i].Cells[4].Value + "' where itemId='" + ItemGrid.Rows[i].Cells[2].Value + "'";
                                Connections.Instance.ExecuteQueries(query);
                            }

                        }
                        dialogResult = MessageBox.Show("Do you want to print this bill?", "Sale Voucher", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //string query1 = "select i.inMalayalam as ItemsInMalayalam,t.ItemName as Items,t.Qty as Qty,t.Rate as Rate,t.Total as Total from tblTemp t,tblItem i where t.ItemId=i.ItemId";
                            string reportFileName= System.Configuration.ConfigurationSettings.AppSettings["SalesReport"];
                            System.Data.DataColumn BillNo = new System.Data.DataColumn("BillNo", typeof(System.String));
                            BillNo.DefaultValue = txtBillno.Text;
                            System.Data.DataColumn BillDate = new System.Data.DataColumn("BillDate", typeof(System.String));
                            BillDate.DefaultValue = DatePicker.Value.ToString("dd-MMM-yyyy");
                            System.Data.DataColumn Customer = new System.Data.DataColumn("Customer", typeof(System.String));
                            Customer.DefaultValue = txtCustomer.Text.ToString();
                            System.Data.DataColumn GrandTotal = new System.Data.DataColumn("GrandTotal", typeof(System.Decimal));
                            GrandTotal.DefaultValue = txtGrand.Text == "" ? "0" : txtGrand.Text;
                            System.Data.DataColumn Cash = new System.Data.DataColumn("Cash", typeof(System.Decimal));
                            Cash.DefaultValue = txtCash.Text == "" ? "0" : txtCash.Text;
                            System.Data.DataColumn Discount = new System.Data.DataColumn("Discount", typeof(System.Decimal));
                            Discount.DefaultValue = txtDiscount.Text == "" ? "0" : txtDiscount.Text;
                            System.Data.DataColumn Balance = new System.Data.DataColumn("Balance", typeof(System.Decimal));
                            Balance.DefaultValue = txtBalance.Text == "" ? "0" : txtBalance.Text;
                            System.Data.DataColumn PrevBalance = new System.Data.DataColumn("PrevBalance", typeof(System.Decimal));
                            PrevBalance.DefaultValue = txtCBalance.Text == "" ? "0" : txtCBalance.Text;

                            

                            string query1 = "select i.inMalayalam as ItemsInMalayalam,t.ItemName as Items,t.Qty as Qty,t.Rate as Rate,t.Total as Total,t.cgstper as CGSTPER,t.cgst AS CGST,t.sgstper as SGSTPER,t.sgst AS SGST,t.igstper as IGSTPER,t.igst AS IGST, t.gst AS GSTTotal,disc,t.Nos,t.Kg,t.less,t.TotLess from tblTemp t,tblItem i where t.ItemId=i.ItemId";                            
                            DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query1);
                            dt.Columns.Add(PrevBalance);
                            dt.Columns.Add(Balance);
                            dt.Columns.Add(Discount);

                            dt.Columns.Add(Cash);
                            dt.Columns.Add(GrandTotal);
                            dt.Columns.Add(Customer);
                            dt.Columns.Add(BillDate);
                            dt.Columns.Add(BillNo);
                            DataTable dtCloned = dt.Clone();
                            dtCloned.Columns[4].DataType = typeof(Decimal);
                            dtCloned.Columns[5].DataType = typeof(Decimal);
                            dtCloned.Columns[6].DataType = typeof(Decimal);
                            dtCloned.Columns[7].DataType = typeof(Decimal);
                            dtCloned.Columns[8].DataType = typeof(Decimal);
                            dtCloned.Columns[9].DataType = typeof(Decimal);
                            dtCloned.Columns[10].DataType = typeof(Decimal);
                            dtCloned.Columns[11].DataType = typeof(Decimal);
                            dtCloned.Columns[12].DataType = typeof(Decimal);
                            
                            foreach (DataRow row in dt.Rows)
                            {
                                dtCloned.ImportRow(row);
                            }

                         

                            ds.Tables["Bill"].Clear();
                            ds.Tables["Bill"].Merge(dtCloned);

                            ReportDocument cryRpt = new ReportDocument();
                            cryRpt.Load(System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString() + reportFileName);

                            //if (cryRpt.DataDefinition.FormulaFields.Count > 0)
                            //{
                                //cryRpt.DataDefinition.FormulaFields[1].Text = "'" + txtBillno.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[2].Text = "'" + DatePicker.Value.ToString("dd-MM-yyyy") + "'";
                                //cryRpt.DataDefinition.FormulaFields[3].Text = "'" + txtState.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[4].Text = "'" + txtStateCode.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[5].Text = "'" + txtCustomer.Text + "'";
                                //string addd = txtAddress.Text.Replace("\r", string.Empty).Replace("\n", "^");
                                //cryRpt.DataDefinition.FormulaFields[6].Text = "'" + addd + "'";
                                //cryRpt.DataDefinition.FormulaFields[7].Text = "'" + txtGST.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[8].Text = "'" + txtVehicle.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[9].Text = "'" + txtBillTotal.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[10].Text = "'" + txtSGST.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[11].Text = "'" + txtIGST.Text + "'";
                                //double gst = Convert.ToDouble(txtCGST.Text) + Convert.ToDouble(txtSGST.Text) + Convert.ToDouble(txtIGST.Text);
                                //cryRpt.DataDefinition.FormulaFields[12].Text = "'" + gst + "'";
                                //cryRpt.DataDefinition.FormulaFields[13].Text = "'" + txtGrand.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[14].Text = "'" + txtCGST.Text + "'";
                                //cryRpt.DataDefinition.FormulaFields[18].Text = "'" + Convert.ToDouble(txtDiscount.Text).ToString() + "'";

                            //}
                            cryRpt.SetDataSource(ds);
                            cryRpt.Refresh();
                            cryRpt.PrintToPrinter(1, true, 0, 0);
                        }

                            query = "truncate table tblTemp";
                            Connections.Instance.ExecuteQueries(query);
                        
                        btnClear_Click(null, null);
                    //}
                
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtBillno.Tag == null)
                MessageBox.Show("Please select a bill to delete");
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to delete the bill?", "Sale Voucher", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string query = "select custId,CBalance,Balance from tblSales where saleId='" + txtBillno.Tag.ToString() + "'";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    double newBal = Convert.ToDouble(dt.Rows[0][1].ToString()) - Convert.ToDouble(dt.Rows[0][2].ToString());//creditbalance-balance
                    query = "update tblCustomer set creditBal=creditBal+'" + newBal + "' where custId='" + dt.Rows[0][0].ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    query = "delete from tblSales where saleId='" + txtBillno.Tag.ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    query = "select itemid,qty from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                    dt.Rows.Clear();
                    dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        query = "update tblItem set Current_Stock=Current_Stock+'" + Convert.ToDouble(dt.Rows[i][1].ToString()) + "' where itemId='" + dt.Rows[i][0].ToString() + "'";
                        Connections.Instance.ExecuteQueries(query);
                    }
                    query = "delete from tblSaletrans where saleId='" + txtBillno.Tag.ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    SearchGridLoad();
                    btnClear_Click(null, null);
                }
            }
               
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
        }

        private void cboArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCustomer.Text = "";
            txtCustomer.Tag = null;
        }

        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
             

        private void cboAreaSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboAreaSearch_SelectionChangeCommitted(null, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void txtItemcode_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == 13)
            {
                txtNos.SelectionStart = 0;
                txtNos.SelectionLength = txtNos.Text.Length;
                txtNos.Focus();
            }
        }
        private void txtItems_TextChanged(object sender, EventArgs e)
        {
            string query = "select itemId,Item_Code,Item_Name,Rate,WRate,PRate from tblitem where Item_Name like '" + txtItems.Text + "%';";
            ItemDisplayGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
            ItemDisplayGrid.Columns[2].Width = 275;

            if (txtItems.Text.Trim() != "")
            {
                itemView.Visible = false;
                if (ItemDisplayGrid.Rows.Count == 1 && txtItems.Text != ItemDisplayGrid.Rows[0].Cells[2].Value.ToString())
                {

                    txtItems.Tag = ItemDisplayGrid.Rows[0].Cells[0].Value.ToString();
                    txtItemcode.Text = ItemDisplayGrid.Rows[0].Cells[1].Value.ToString();
                    txtRate.Text = ItemDisplayGrid.Rows[0].Cells[3].Value.ToString();
                    lblPRate.Text = ItemDisplayGrid.Rows[0].Cells[5].Value.ToString();

                    if (chkWholeSale.Checked == true)
                        txtRate.Text = ItemDisplayGrid.Rows[0].Cells[4].Value.ToString();
                    itemView.Visible = false;
                    txtNos.SelectionStart = 0;
                    txtNos.SelectionLength = txtNos.Text.Length;
                    txtNos.Focus();
                    txtItems.Text = ItemDisplayGrid.Rows[0].Cells[2].Value.ToString();
                }
                else if (ItemDisplayGrid.Rows.Count == 1 && txtItems.Text == ItemDisplayGrid.Rows[0].Cells[2].Value.ToString())
                {
                    itemView.Visible = false;
                }
                else
                {
                    itemView.Visible = true;
                    ItemDisplayGrid.ColumnHeadersVisible = true;
                    ItemDisplayGrid.Columns[0].Visible = false;
                    ItemDisplayGrid.Columns[1].Visible = false;
                    //ItemDisplayGrid.Columns[3].Visible = false;
                    //ItemDisplayGrid.Columns[4].Visible = false;
                    //ItemDisplayGrid.Columns[5].Visible = false;

                    ItemDisplayGrid.ClearSelection();
                }
            }
        }

        private void txtItems_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (ItemDisplayGrid.Rows.Count > 1 && e.KeyCode == Keys.Down)
            {
                if (txtItems.Text.Trim() == "")
                {
                    string query = "select itemId,Item_Code,Item_Name,Rate,WRate,PRate from tblitem order by Item_Name;";
                    ItemDisplayGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                    ItemDisplayGrid.Columns[2].Width = 275;
                }
                itemView.Visible = true;
                ItemDisplayGrid.Focus();
                ItemDisplayGrid.Rows[0].Selected = true;

            }
            else if (e.KeyCode == Keys.Escape)
            {
                itemView.Visible = false;
            }
            else if(e.KeyCode == Keys.Down)
            {
                itemView.Visible = true;

                if (txtItems.Text.Trim() == "")
                {
                    itemView.Visible = false;
                    string query = "select itemId,Item_Code,Item_Name,Rate,WRate,PRate from tblitem order by Item_Name;";
                    ItemDisplayGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
                    ItemDisplayGrid.Columns[2].Width = 275;
                    itemView.Visible = true;
                    ItemDisplayGrid.ColumnHeadersVisible = true;
                    ItemDisplayGrid.Columns[0].Visible = false;
                    ItemDisplayGrid.Columns[1].Visible = false;
                    //ItemDisplayGrid.Columns[3].Visible = false;
                    //ItemDisplayGrid.Columns[4].Visible = false;
                    //ItemDisplayGrid.Columns[5].Visible = false;

                    ItemDisplayGrid.ClearSelection();
                    ItemDisplayGrid.Focus();
                    ItemDisplayGrid.Rows[0].Selected = true;
                }
            }
        }
        private void ItemDisplayGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && ItemDisplayGrid.Rows.Count > 0)
            {
                int r = ItemDisplayGrid.CurrentRow.Index;
                txtItems.Tag = ItemDisplayGrid.Rows[r].Cells[0].Value.ToString();
                txtItemcode.Text = ItemDisplayGrid.Rows[r].Cells[1].Value.ToString();
                txtRate.Text = ItemDisplayGrid.Rows[r].Cells[3].Value.ToString();
                if (chkWholeSale.Checked == true)
                    txtRate.Text = ItemDisplayGrid.Rows[r].Cells[4].Value.ToString();
                txtNos.SelectionStart = 0;
                txtNos.SelectionLength = txtQuantity.Text.Length;
                txtNos.Focus();
                lblPRate.Text = ItemDisplayGrid.Rows[r].Cells[5].Value.ToString();
                txtItems.Text = ItemDisplayGrid.Rows[r].Cells[2].Value.ToString();
                

                itemView.Visible = false;
            }
            else if (e.KeyData == Keys.Escape)
            {
                itemView.Visible = false;
                txtItems.Focus();
            }
        }
        private void ItemDisplayGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int r = ItemDisplayGrid.CurrentRow.Index;
                txtItems.Tag = ItemDisplayGrid.Rows[r].Cells[0].Value.ToString();
                txtItemcode.Text = ItemDisplayGrid.Rows[r].Cells[1].Value.ToString();
                txtRate.Text = ItemDisplayGrid.Rows[r].Cells[3].Value.ToString();
                if (chkWholeSale.Checked)
                    txtRate.Text = ItemDisplayGrid.Rows[r].Cells[4].Value.ToString();
                txtNos.SelectionStart = 0;
                txtNos.SelectionLength = txtQuantity.Text.Length;
                txtNos.Focus();
                lblPRate.Text = ItemDisplayGrid.Rows[r].Cells[5].Value.ToString();
                txtItems.Text = ItemDisplayGrid.Rows[r].Cells[2].Value.ToString();
                try
                {
                    if (ItemDisplayGrid.Rows.Count > 0)
                        ItemDisplayGrid.Rows.Clear();
                }
                catch (Exception ex)
                { }
                itemView.Visible = false;
            }
        }

        private void chkWholeSale_CheckedChanged(object sender, EventArgs e)
        {

            if (txtItems.Tag != "" && txtItems.Tag != null)
            {
                string query = "";
                if (!chkWholeSale.Checked)
                    query = "select rate from tblItem where itemId='" + txtItems.Tag + "'";
                else if (chkWholeSale.Checked)
                    query = "select wrate from tblItem where itemId='" + txtItems.Tag + "'";
                DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                txtRate.Text = dt.Rows[0][0].ToString();
            }
        }

        private void SearchGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Edit_Click(object sender, EventArgs e)
        {

        }

        private void ItemDisplayGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            string Query="";
            if (cboArea.SelectedIndex == -1)
            {
                CustomerGrid.Visible = false;            
            }
            else
            {
                CustomerGrid.Visible = true;
                Query = "select * from tblCustomer where Customer like '" + txtCustomer.Text + "%' and areaid='" + cboArea.SelectedValue + "'  and CType IN('Customer','Both')";
                CustomerGrid.DataSource = Connections.Instance.ShowDataInGridView(Query);
                CustomerGrid.ColumnHeadersVisible = false;
                CustomerGrid.Columns[0].Visible = false;
                CustomerGrid.Columns[2].Visible = false;
                CustomerGrid.Columns[3].Visible = false;
                CustomerGrid.Columns[4].Visible = false;
                CustomerGrid.Columns[5].Visible = false;
                CustomerGrid.Columns[6].Visible = false;
                CustomerGrid.Columns[7].Visible = false;
                CustomerGrid.Columns[8].Visible = false;
                CustomerGrid.Columns[9].Visible = false;
                CustomerGrid.Columns[10].Visible = false;
                CustomerGrid.Columns[11].Visible = false;
                CustomerGrid.Columns[12].Visible = false;
                CustomerGrid.Columns[13].Visible = false;
                CustomerGrid.Columns[14].Visible = false;
                CustomerGrid.Columns[15].Visible = false;

                
            }
        }

        private void CustomerGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int r = CustomerGrid.CurrentRow.Index;
                txtCustomer.Tag = CustomerGrid.Rows[r].Cells[0].Value.ToString();
                txtAddress.Text = CustomerGrid.Rows[r].Cells[12].Value.ToString();
                txtPhone.Text = CustomerGrid.Rows[r].Cells[5].Value.ToString();
                if (cboType.Text.ToUpper() != "CASH")
                    txtCBalance.Text = CustomerGrid.Rows[r].Cells[7].Value.ToString();
                else
                    txtCBalance.Text = "0";
                txtGSTIN.Text = CustomerGrid.Rows[r].Cells[11].Value.ToString();
                txtState.Text = CustomerGrid.Rows[r].Cells[13].Value.ToString();
                txtStateCode.Text = CustomerGrid.Rows[r].Cells[14].Value.ToString();
                txtCustomer.Text = CustomerGrid.Rows[r].Cells[1].Value.ToString();
                CustomerGrid.Visible = false;
                txtItems.Focus();
            }
        }
        private void CustomerGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && CustomerGrid.Rows.Count > 0)
            {
                int r = CustomerGrid.CurrentRow.Index;
                txtCustomer.Tag = CustomerGrid.Rows[r].Cells[0].Value.ToString();
                txtAddress.Text = CustomerGrid.Rows[r].Cells[12].Value.ToString();
                txtPhone.Text = CustomerGrid.Rows[r].Cells[5].Value.ToString();
                if (cboType.Text.ToUpper() != "CASH")
                    txtCBalance.Text = CustomerGrid.Rows[r].Cells[7].Value.ToString();
                else
                    txtCBalance.Text = "0";
                txtGSTIN.Text = CustomerGrid.Rows[r].Cells[11].Value.ToString();
                txtState.Text = CustomerGrid.Rows[r].Cells[13].Value.ToString();
                txtStateCode.Text = CustomerGrid.Rows[r].Cells[14].Value.ToString();
                txtCustomer.Text = CustomerGrid.Rows[r].Cells[1].Value.ToString();
                CustomerGrid.Visible = false;
                txtItems.Focus();
            }
            else if (e.KeyData == Keys.Escape)
            {
                itemView.Visible = false;
                txtItems.Focus();
            }
        }
        private void CustomerGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtState.Focus();
            }

        }
        private void txtState_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtStateCode.Focus();
            }

        }
        private void txtStateCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtGSTIN.Focus();
            }

        }
        private void txtGSTIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtVehicle.Focus();
            }

        }
        private void txtVehicle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtItems.Focus();
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtGSTIN_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtState_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtStateCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.Text.ToUpper() == "CASH")
            {
                txtCBalance.Text = "0";
                txtCash.Enabled = false;
                txtCash.Text = txtGrand.Text;
            }
            else
            {
                txtCash.Enabled = true;
                if (txtCustomer.Tag != null && txtCustomer.Tag.ToString()!="")
                {
                    txtCash.Enabled = true;
                    txtCash.Text = "0";

                    string query = "select creditBal FROM tblCustomer WHERE custId='"+ txtCustomer.Tag +"'";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                    if (dt.Rows.Count>0)
                    txtCBalance.Text  = dt.Rows[0][0].ToString();
                }
            }
        }

        private void chkInterState_CheckedChanged(object sender, EventArgs e)
        {
            taxCalc();
        }

        private void btnCustomerClear_Click(object sender, EventArgs e)
        {
            txtCustomer.Tag="";
            txtCustomer.Text = "";
            txtAddress.Text = "";
            txtState.Text = "";
            txtStateCode.Text = "";
            txtPhone.Text = "";
            txtGSTIN.Text = "";
            txtVehicle.Text = "";
            CustomerGrid.Visible = false;
            txtCustomer.Focus();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void txtTotLess_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtQuantity.Text = (Convert.ToDouble(txtKg.Text == "" ? "0" : txtKg.Text) - Convert.ToDouble(txtTotLess.Text == "" ? "0" : txtTotLess.Text)).ToString();
            }

            catch
            {
                txtQuantity.Text = "0";
            }
        }

        private void txtNos_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTotLess.Text = (Convert.ToDouble(txtLess.Text == "" ? "0" : txtLess.Text) * Convert.ToDouble(txtNos.Text == "" ? "0" : txtNos.Text)).ToString();
            }

            catch
            {
                txtTotLess.Text = "0";
            }
        }

        private void txtLess_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTotLess.Text = (Convert.ToDouble(txtLess.Text == "" ? "0" : txtLess.Text) * Convert.ToDouble(txtNos.Text == "" ? "0" : txtNos.Text)).ToString();
            }

            catch
            {
                txtTotLess.Text = "0";
            }
        }

        private void txtKg_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtQuantity.Text = (Convert.ToDouble(txtKg.Text == "" ? "0" : txtKg.Text) - Convert.ToDouble(txtTotLess.Text == "" ? "0" : txtTotLess.Text)).ToString();
            }

            catch
            {
                txtQuantity.Text = "0";
            }
        }

    }
}
