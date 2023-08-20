using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InMag_V._16
{
    public partial class frmPurchase : Form
    {
        public frmPurchase()
        {
            InitializeComponent();
            this.txtCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyDown);
            this.ItemDisplayGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ItemDisplayGrid_KeyDown);
            this.CustomerGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomerGrid_KeyDown);
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
        private void CustomerGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ItemDisplayGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && ItemDisplayGrid.Rows.Count > 0)
            {
                int r = ItemDisplayGrid.CurrentRow.Index;
                txtItems.Tag = ItemDisplayGrid.Rows[r].Cells[0].Value.ToString();
                txtItemcode.Text = ItemDisplayGrid.Rows[r].Cells[1].Value.ToString();
                    txtRate.Text = ItemDisplayGrid.Rows[r].Cells[4].Value.ToString();
                txtQuantity.SelectionStart = 0;
                txtQuantity.SelectionLength = txtQuantity.Text.Length;
                txtQuantity.Focus();
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
                    txtRate.Text = ItemDisplayGrid.Rows[r].Cells[4].Value.ToString();
                txtQuantity.SelectionStart = 0;
                txtQuantity.SelectionLength = txtQuantity.Text.Length;
                txtQuantity.Focus();
                txtItems.Text = ItemDisplayGrid.Rows[r].Cells[2].Value.ToString();
                if (ItemDisplayGrid.Rows.Count > 0)
                    ItemDisplayGrid.Rows.Clear();
                itemView.Visible = false;
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

        private void frmPurchase_Load(object sender, EventArgs e)
        {
            cboType.SelectedIndex = 0;
            
            txtItemcode.Tag = null;
         
            btnClear_Click(null, null);
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            string Query = "";
            
                CustomerGrid.Visible = true;
                Query = "select * from tblCustomer where Customer like '" + txtCustomer.Text + "%' and CType IN('Party','Both')";
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
                    txtRate.Text = ItemDisplayGrid.Rows[0].Cells[5].Value.ToString();
                    itemView.Visible = false;
                    txtQuantity.SelectionStart = 0;
                    txtQuantity.SelectionLength = txtQuantity.Text.Length;
                    txtQuantity.Focus();
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
        
    }
}
