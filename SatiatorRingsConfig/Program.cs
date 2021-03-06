using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    static class Program
    {
        public static frmMain form;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(form == null)
            {
                form = new frmMain();
                Application.Run(form);
            }
        }
    }
}
