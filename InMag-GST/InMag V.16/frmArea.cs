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
    public partial class frmArea : Form
    {
        public frmArea()
        {
            InitializeComponent();
            this.txtArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArea_KeyDown);
            this.txtManager.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtManager_KeyDown);
        }

        private void frmArea_Load(object sender, EventArgs e)
        {
            GridShow();
        }
        private void GridShow()
        {
            try
            {
                string query = "select * from tblArea where Area like '" + txtSearch.Text.Trim() + "%'";
                dataGridView1.DataSource = Connections.Instance.ShowDataInGridView(query);
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
            }
            catch { }
        }

        private void dataGridView1_CellClick(object sender,DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                lblID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtArea.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtManager.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtNotes.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblID.Text = "";
            txtArea.Text = "";
            txtManager.Text = "";
            txtNotes.Text = "";
            txtSearch.Text = "";
            txtArea.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblID.Text.Trim() == "")
                MessageBox.Show("No item selected to delete");
            else
            { 
                DialogResult dialogResult = MessageBox.Show("All data under this area would be deleted. Do you want to delete the area", "Area Master", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    string query = "Delete from tblArea where areaId='" + lblID.Text.Trim() + "'";
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
                if (txtArea.Text.Trim() == "")
                    MessageBox.Show("Please enter the area");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save the area?", "Area Master", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "insert into tblArea values('" + txtArea.Text.Trim() + "','" + txtManager.Text.Trim() + "','" + txtNotes.Text.Trim() + "','True')";
                        Connections.Instance.ExecuteQueries(query);
                        GridShow();
                        btnClear_Click(null, null);
                    }
                }
                
            }
            else
            {
                //Update
                if (txtArea.Text.Trim() == "")
                    MessageBox.Show("Please enter the area");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save the area", "Area Master", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "update tblArea set Area='" + txtArea.Text.Trim() + "',Managed='" + txtManager.Text.Trim() + "',Notes='" + txtNotes.Text.Trim() + "' where areaId='" + lblID.Text.Trim() + "'";
                        Connections.Instance.ExecuteQueries(query);
                        GridShow();
                        btnClear_Click(null, null);
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GridShow();
        }
        private void txtArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtManager.Focus();
            }

        }
        private void txtManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtNotes.Focus();
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }




    }
}
