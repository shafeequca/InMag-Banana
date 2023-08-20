using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using InMag_V._16.DataSet;
using CrystalDecisions.CrystalReports.Engine;

namespace InMag_V._16
{
    public partial class frmMenu : Form
    {
        public string user;
        DataSet1 ds;
        public frmMenu()
        {
            InitializeComponent();
        }

        private void areaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmArea")
                {
                    f.Activate();
                    return;
                }
            }
            frmArea frm = new frmArea();
            frm.Show(this);
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            ds = new DataSet1();
            frmLogin frm  = new frmLogin();
            frm.ShowDialog();
        }

        private void staffRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmStaffRegistration")
                {
                    f.Activate();
                    return;
                }
            }
            frmStaffRegistration frm = new frmStaffRegistration();
            frm.Show(this);
        }

        private void customerMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmCustomerMaster")
                {
                    f.Activate();
                    return;
                }
            }
            frmCustomerMaster frm = new frmCustomerMaster();
            frm.Show(this);
        }

        private void itemMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmItemMaster")
                {
                    f.Activate();
                    return;
                }
            }
            frmItemMaster frm = new frmItemMaster();
            frm.Show(this);
        }

        private void salesVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSales")
                {
                    f.Activate();
                    return;
                }
            }
            frmSalesGST frm = new frmSalesGST();
            frm.Show(this);
        }

        private void saleSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSaleSheet")
                {
                    f.Activate();
                    return;
                }
            }
            frmSaleSheet frm = new frmSaleSheet();
            frm.Show(this);
        }

        private void addStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmAddStock")
                {
                    f.Activate();
                    return;
                }
            }
            frmAddStock frm = new frmAddStock();
            frm.Show(this);
        }

        private void orderedItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmOrderedItems")
                {
                    f.Activate();
                    return;
                }
            }
            frmOrderedItems frm = new frmOrderedItems();
            frm.Show(this);
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name != "frmMenu")
                    f.Close();
            }
            frmLogin frm = new frmLogin();
            frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSales")
                {
                    f.Activate();
                    return;
                }
            }
            frmSalesGST frm = new frmSalesGST();
            frm.Show(this);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmAddStock")
                {
                    f.Activate();
                    return;
                }
            }
            frmAddStock frm = new frmAddStock();
            frm.Show(this);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmSaleSheet")
                {
                    f.Activate();
                    return;
                }
            }
            frmSaleSheet frm = new frmSaleSheet();
            frm.Show(this);

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmOrderedItems")
                {
                    f.Activate();
                    return;
                }
            }
            frmOrderedItems frm = new frmOrderedItems();
            frm.Show(this);
        }

        private void stockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmStockView")
                {
                    f.Activate();
                    return;
                }
            }
            frmStockView frm = new frmStockView();
            frm.Show(this);
        }

        private void cashEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmCashEntry")
                {
                    f.Activate();
                    return;
                }
            }
            frmCashEntry frm = new frmCashEntry();
            frm.Show(this);
        }

        private void partyWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "frmPartyReport")
                {
                    f.Activate();
                    return;
                }
            }
            frmPartyReport frm = new frmPartyReport();
            frm.Show(this);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    string App_Type = System.Configuration.ConfigurationSettings.AppSettings["App_Type"];

            //    Backup bkpDatabase = new Backup();
            //    bkpDatabase.Action = BackupActionType.Database;
            //    bkpDatabase.Database = System.Configuration.ConfigurationSettings.AppSettings["DB"].ToString();
            //    ServerConnection srvConn = new ServerConnection(System.Configuration.ConfigurationSettings.AppSettings["Server"].ToString());
            //    srvConn.LoginSecure = true;
            //    string Location = System.Configuration.ConfigurationSettings.AppSettings["Backup_Location"] + @"\Backup_" + DateTime.Today.ToString("ddMMyyyyHHmmss") + ".bak";
            //    if (File.Exists(Location)==true)
            //    {
            //    File.Delete(Location);
            //    }
            //    //BackupDeviceItem bkpDevice = new BackupDeviceItem(Location + @"\Backup_" + DateTime.Now + ".bak", DeviceType.File);
            //    BackupDeviceItem bkpDevice = new BackupDeviceItem(Location, DeviceType.File);

            //    bkpDatabase.Devices.Add(bkpDevice);

            //    Server srvr;
            //    if (App_Type == "Client")
            //    {
            //        // Give the login username
            //        srvConn.Login = "sa";
            //        // Give the login password
            //        srvConn.Password = System.Configuration.ConfigurationSettings.AppSettings["psd"].ToString();
            //    }
            //    srvr = new Server(srvConn);
            //    bkpDatabase.SqlBackup(srvr);

            //    MessageBox.Show("Backup succeeded");
            //}
            //catch (Exception x)
            //{
            //    MessageBox.Show("ERROR: An error ocurred while backing up DataBase" + x, "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            
        }

        private void partyCreditToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                if (f.Name == "FrmCreditReport")
                {
                    f.Activate();
                    return;
                }
            }
            FrmCreditReport frm = new FrmCreditReport();
            frm.Show(this);
        }
    }
}
