using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InMag_V._16
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Connections.Instance.OpenConection();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMenu());
            //test
        }
    }
}
