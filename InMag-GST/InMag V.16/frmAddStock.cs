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
    public partial class frmAddStock : Form
    {
        public frmAddStock()
        {
            InitializeComponent();
            this.txtQuantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuantity_KeyDown);
            this.txtQuantity.KeyPress += new KeyPressEventHandler(this.NumberOnly_KeyPress);
            this.cboItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboItems_KeyDown);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Edit_Click);
        }
        private void NumberOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 8 || e.KeyChar == 46 || e.KeyChar == 13 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
                e.Handled = true;
        }
        private void frmAddStock_Load(object sender, EventArgs e)
        {
            comboLoad();
        }
        private void comboLoad()
        {
            string query = "select staffId,StaffName from tblStaff order By StaffName";
            cboStaff.DataSource = Connections.Instance.ShowDataInGridView(query);
            cboStaff.DisplayMember = "StaffName";
            cboStaff.ValueMember = "staffId";
            cboStaff.SelectedIndex = -1;
            cboStaff.Text = "";

            query = "select itemId,Item_Name from tblItem order By Item_Name";
            cboItems.DataSource = Connections.Instance.ShowDataInGridView(query);
            cboItems.DisplayMember = "Item_Name";
            cboItems.ValueMember = "itemId";
            cboItems.SelectedIndex = -1;
            cboItems.Text = "";
            query = "truncate table tblTempStock";
            Connections.Instance.ExecuteQueries(query);

        }
        private void cboItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && cboItems.SelectedValue !=null )
                txtQuantity.Focus();
        }
        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (cboItems.Text.Trim() == "" || txtQuantity.Text.Trim() == "" || Convert.ToDouble(txtQuantity.Text.Trim()) <= 0 )
                {
                    MessageBox.Show("Please enter the data");
                    cboItems.Focus();
                }
                else
                {
                    string query = "";
                    if (cboItems.Tag != null)
                        query = "update tblTempStock set Qty='" + Convert.ToDouble(txtQuantity.Text) + "' where itemId='" + cboItems.SelectedValue + "'";
                    else
                    {
                        query = "select qty from tblTempStock where itemId='" + cboItems.SelectedValue + "'";
                        DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);
                        if (dt.Rows.Count > 0)
                        {
                            double newQty = Convert.ToDouble(txtQuantity.Text) + Convert.ToDouble(dt.Rows[0][0].ToString());
                            query = "update tblTempStock set Qty='" + newQty + "' where itemId='" + cboItems.SelectedValue + "'";
                        }
                        else
                            query = "insert into tblTempStock values('" + cboItems.SelectedValue + "','" + cboItems.Text.Trim() + "','" + Convert.ToDouble(txtQuantity.Text.Trim()) + "')";
                    }
                        Connections.Instance.ExecuteQueries(query);
                    GridShow();
                    cmdItemClear_Click(null, null);
                }
            }
        }
        private void GridShow()
        {
            string query = "select ROW_NUMBER() OVER(ORDER BY id) AS Row,itemid,itemname,qty from tblTempStock;";
            ItemGrid.DataSource = Connections.Instance.ShowDataInGridView(query);
            ItemGrid.Columns[0].Width = 40;
            ItemGrid.Columns[1].Visible = false;
            ItemGrid.Columns[2].Width = 285;
            ItemGrid.Columns[3].Width = 95;
            if (ItemGrid.Rows.Count > 0)
            {
                ItemGrid.FirstDisplayedScrollingRowIndex = ItemGrid.RowCount - 1;
                ItemGrid.Rows[ItemGrid.RowCount - 1].Selected = true;
            }
           
        }
        private void cmdItemClear_Click(object sender, EventArgs e)
        {
            cboItems.Tag = null;
            txtQuantity.Text = "";
            cboItems.Enabled = true;
            cboItems.SelectedIndex = -1;
            cboItems.Text = "";
            cboItems.Focus();

        }
        private void Edit_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ItemGrid.SelectedRows.Count > 0)
            {
                int rowno = ItemGrid.SelectedRows[0].Index;
                ToolStripItem item = e.ClickedItem;
                if (item.Name == "Edit")
                {
                    cboItems.Enabled = false;
                    cboItems.Tag = ItemGrid.Rows[rowno].Cells[1].Value.ToString();
                    cboItems.SelectedValue = ItemGrid.Rows[rowno].Cells[1].Value.ToString();
                    txtQuantity.Text = ItemGrid.Rows[rowno].Cells[3].Value.ToString();
                    txtQuantity.Focus();
                }
                else if (item.Name == "Delete")
                {
                    string query = "delete from tblTempStock where itemId='" + ItemGrid.Rows[rowno].Cells[1].Value.ToString() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    GridShow();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmdItemClear_Click(null, null);
            cboStaff.SelectedIndex = -1;
            cboStaff.Text = "";
            ItemGrid.DataSource = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cboStaff.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter the data");
                cboStaff.Focus();
            }
            else
            {
                if (ItemGrid.Rows.Count == 0)
                    MessageBox.Show("Please add items");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to add stock?", "Stock Voucher", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "";
                        for (int i = 0; i < ItemGrid.Rows.Count; i++)
                        {
                            query = "insert into tblStock values('" + cboStaff.SelectedValue + "','" + DatePicker.Value.ToString("dd-MMM-yyyy") + "','" + ItemGrid.Rows[i].Cells[1].Value + "','" + ItemGrid.Rows[i].Cells[3].Value + "')";
                            Connections.Instance.ExecuteQueries(query);
                            query = "update tblItem set Current_Stock=Current_Stock+'" + ItemGrid.Rows[i].Cells[3].Value + "' where itemId='" + ItemGrid.Rows[i].Cells[1].Value + "'";
                            Connections.Instance.ExecuteQueries(query);
                        }
                        btnClear_Click(null, null);

                    }
                }
            }
        }
    }
}
