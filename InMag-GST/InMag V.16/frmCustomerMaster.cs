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
    public partial class frmCustomerMaster : Form
    {
        public frmCustomerMaster()
        {
            InitializeComponent();
            this.txtCustomer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyDown);
            this.txtGSTIN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTin_KeyDown);
            this.txtAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCst_KeyDown);
            this.txtState.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtState_KeyDown);
            this.txtPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPhone_KeyDown);
            this.cboArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboArea_KeyDown);
            this.txtBalance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBalance_KeyDown);
            this.txtStateCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStateCode_KeyDown);
            

            this.txtBalance.KeyPress += new KeyPressEventHandler(this.NumberOnly_KeyPress);

        }
        private void NumberOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 8 || e.KeyChar == 46 || e.KeyChar == 13 || (e.KeyChar >= 48 && e.KeyChar <= 57)))
                e.Handled = true;
        }
        private void frmCustomerMaster_Load(object sender, EventArgs e)
        {
            comboLoad();
            GridShow();
            txtSearch_TextChanged(null, null);
        }
        private void comboLoad()
        {
            string query = "select areaId,Area from tblArea order By Area";
            cboArea.DataSource = Connections.Instance.ShowDataInGridView(query);
            cboArea.DisplayMember = "Area";
            cboArea.ValueMember = "areaId";

        }
        private void GridShow()
        {
            try
            {
                string query = "select cus.*,ar.Area from tblCustomer cus, tblArea ar where cus.areaId=ar.areaId and cus.Customer like '" + txtSearch.Text.Trim() + "%'  order by cus.areaId,cus.Customer";
                dataGridView1.DataSource = Connections.Instance.ShowDataInGridView(query);
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Width = 60;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[15].Visible = false;
                dataGridView1.Columns[16].Width = 80;

            }
            catch { }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            lblID.Text = "";
            txtBalance.Text = "";
            txtAddress.Text = "";
            txtCustomer.Text = "";
            txtStateCode.Text = "";
            txtPhone.Text = "";
            txtState.Text = "";
            txtSearch.Text = "";
            txtGSTIN.Text = "";
            cboArea.SelectedIndex = 0;
            txtCustomer.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblID.Text.Trim() == "")
                MessageBox.Show("No item selected to delete");
            else
            {
                DialogResult dialogResult = MessageBox.Show("All data under this customer would be deleted. Do you want to delete the customer", "Customer Master", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string query = "Delete from tblCustomer where custId='" + lblID.Text.Trim() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    GridShow();
                    btnClear_Click(null, null);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ctype = "";
            if (Party.Checked)
                ctype = "Party";
            else if (Both.Checked)
                ctype = "Both";
            else
                ctype = "Customer";
            if (lblID.Text.Trim() == "")
            {
                
                //Insert
                if (txtCustomer.Text.Trim() == "" || txtState.Text == "" || txtBalance.Text == "")
                    MessageBox.Show("Please enter the data");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save?", "Customer Master", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        
                        string query = "insert into tblCustomer("+
                                                    "Customer"+
                                                    ",Phone"+
                                                    ",areaId"+
                                                    ",creditBal"+
                                                    ",GSTIN"+
                                                    ",Address"+
                                                    ",State"+
                                                    ",State_code"+
                                                    ",CType)"+
                                        " values('" + txtCustomer.Text.Trim() + "','" + txtPhone.Text.Trim() + "','" + cboArea.SelectedValue + "','" + Convert.ToDouble(txtBalance.Text) + "','" + txtGSTIN.Text.Trim() + "','" + txtAddress.Text.Trim() + "','" + txtState.Text.Trim() + "','" + txtStateCode.Text.Trim() + "','" + ctype + "')";
                        Connections.Instance.ExecuteQueries(query);
                        GridShow();
                        btnClear_Click(null, null);
                    }
                }
            }
            else
            {
                //Update
                if (txtCustomer.Text.Trim() == "")
                    MessageBox.Show("Please enter the data");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save?", "Customer Master", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "update  tblCustomer set "+
                                               "Customer='" + txtCustomer.Text.Trim() + "'"+
                                              ",Phone='" + txtPhone.Text.Trim() + "'"+
                                              ",areaId='" + cboArea.SelectedValue + "'"+
                                              ",creditBal='" + Convert.ToDouble(txtBalance.Text) + "'"+
                                              ",GSTIN='" + txtGSTIN.Text.Trim() + "'"+
                                              ",Address='" + txtAddress.Text.Trim() + "'"+
                                              ",State='" + txtState.Text.Trim() + "'"+
                                              ",State_code='" + txtStateCode.Text.Trim() + "'"+
                                              ",CType='" + ctype + "'" +
                                              " where custId='" + lblID.Text.Trim() + "'";
                        Connections.Instance.ExecuteQueries(query);
                        GridShow();
                        btnClear_Click(null, null);
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                lblID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtCustomer.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtGSTIN.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
                txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString();
                txtState.Text = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
                txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                cboArea.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(); 
                txtBalance.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                txtStateCode.Text = dataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString();
                switch (dataGridView1.Rows[e.RowIndex].Cells[15].Value.ToString())
                    {
                        case "Customer": Customer.Checked = true;
                            break;
                        case "Party": Party.Checked = true;
                            break;
                        case "Both": Both.Checked = true;
                            break;

                    }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GridShow();
        }
        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtGSTIN.Focus();
            }
        }
        private void txtTin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtAddress.Focus();
            }
        }
        private void txtCst_KeyDown(object sender, KeyEventArgs e)
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
                txtBalance.Focus();
            }
        }
        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtState.Focus();
            }
        }
        private void cboArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnSave_Click(null, null);
            }
        }
        private void txtBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cboArea.Focus();
            }
        }
        
        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtTin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void cboArea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtPlace_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtCST_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblID_Click(object sender, EventArgs e)
        {

        }

        private void txtStateCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBalance_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
