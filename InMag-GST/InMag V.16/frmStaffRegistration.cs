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
    public partial class frmStaffRegistration : Form
    {
        public frmStaffRegistration()
        {
            InitializeComponent();
        }

        private void frmStaffRegistration_Load(object sender, EventArgs e)
        {
            GridShow();
        }
        private void GridShow()
        {
            try
            {
                string query = "select * from tblStaff  where StaffName like '" + txtSearch.Text.Trim() + "%'  order by StaffName";
                dataGridView1.DataSource = Connections.Instance.ShowDataInGridView(query);
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                //dataGridView1.Columns[3].Visible = false;
            }
            catch { }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblID.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblID.Text.Trim() == "")
                MessageBox.Show("No item selected to delete");
            else
            {
                DialogResult dialogResult = MessageBox.Show("All data would be deleted. Do you want to delete the details of staffs", "Staff Registration", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string query = "Delete from tblStaff where StaffId='" + lblID.Text.Trim() + "'";
                    Connections.Instance.ExecuteQueries(query);
                    GridShow();
                    btnClear_Click(null, null);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lblID.Text.Trim() == "")
            {
                //Insert
                if (txtName.Text.Trim() == "" || txtAddress.Text.Trim() == "" || txtPhone.Text.Trim() == "")
                    MessageBox.Show("Please enter the data");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save?", "Staff Registration", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "insert into tblStaff values('" + txtName.Text.Trim() + "','" + txtAddress.Text.Trim() + "','" + txtPhone.Text.Trim() + "')";
                        Connections.Instance.ExecuteQueries(query);
                        GridShow();
                        btnClear_Click(null, null);
                    }
                }

            }
            else
            {
                //Update
                if (txtName.Text.Trim() == "")
                    MessageBox.Show("Please enter the data");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save", "Staff Registration", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "update tblStaff set StaffName='" + txtName.Text.Trim() + "',Address='" + txtAddress.Text.Trim() + "',Phone='" + txtPhone.Text.Trim() + "'  where staffId='" + lblID.Text.Trim() + "'";
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
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GridShow();
        }
    }


}

