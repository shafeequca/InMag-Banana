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
    public partial class frmPartyMaster : Form
    {
        public frmPartyMaster()
        {
            InitializeComponent();
        }
        private void frmPartyMaster_Load(object sender, EventArgs e)
        {
            GridShow();
        }

        private void GridShow()
        {
            string query = "select * from tblParty  where party like '%" + txtSearch.Text.Trim() + "%'  order by party";
            dataGridView1.DataSource = Connections.Instance.ShowDataInGridView(query);
            dataGridView1.DataSource = Connections.Instance.ShowDataInGridView(query);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtParty.Text = "";
            txtCST.Text = "";
            txtTin.Text = "";
            txtNotes.Text = "";
            txtPhone.Text = "";
            txtPlace.Text = "";
            txtBalance.Text = "";
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblID.Text.Trim() == "")
                MessageBox.Show("No item selected to delete");
            else
            {
                DialogResult dialogResult = MessageBox.Show("All data under this party would be deleted. Do you want to delete the party", "party Master", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string query = "Delete from tblParty where partyId='" + lblID.Text.Trim() + "'";
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
                if (txtParty.Text.Trim() == "" || txtPlace.Text.Trim() == "" || txtBalance.Text.Trim() == "")
                    MessageBox.Show("Please enter the data");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save?", "partyt Master", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "insert into tblParty values('" + txtParty.Text.Trim() + "','" + txtTin.Text.Trim() + "','" + txtCST.Text.Trim() + "','" + txtPlace.Text.Trim() + "','" + txtPhone.Text.Trim() + "','" + Convert.ToDouble(txtBalance.Text.Trim()) + "','" + txtNotes.Text.Trim() + "','false')";
                        Connections.Instance.ExecuteQueries(query);
                        GridShow();
                        btnClear_Click(null, null);
                    }
                }

            }
            else
            {
                //Update
                if (txtParty.Text.Trim() == "")
                    MessageBox.Show("Please enter the data");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save", "party Master", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string query = "update tblParty set Party='" + txtParty.Text.Trim() + "',Tin='" + txtTin.Text.Trim() + "',CST='" + txtCST.Text.Trim() + "',Place='" + txtPlace.Text.Trim() + "',Phone='" + txtPhone.Text.Trim() + "',creditBal='" + Convert.ToDouble(txtBalance.Text) + "',Notes='" + txtNotes.Text.Trim() + "' where partyId='" + lblID.Text.Trim() + "'";
                        Connections.Instance.ExecuteQueries(query);
                        GridShow();
                        btnClear_Click(null, null);
                    }
                }
            }
        }



        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                lblID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtParty.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtTin.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtCST.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtPlace.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtBalance.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtNotes.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            }
        }

       

        
    }
}

        


