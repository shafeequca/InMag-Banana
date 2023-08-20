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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            this.txtUserName.KeyPress += new KeyPressEventHandler(txtUsername_KeyPress);
            this.txtPassword.KeyPress += new KeyPressEventHandler(txtPassword_KeyPress);
        }
        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txtPassword.Focus();
        }
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnLogin_Click(null,null);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.ToLower() == "nes" && txtPassword.Text.ToLower() == "nes")
            {
                this.Close();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            DateTime dt;
            dt=Convert.ToDateTime("01-01-2030");
            if (DateTime.Today.Date>=dt)
            {
                MessageBox.Show("Your trial period expired. Please contact your administrator");
                btnClear_Click(null, null);
            }
        }
    }
}
