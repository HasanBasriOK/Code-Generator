using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMaker2
{
    static class Program
    {
        public static string Server = string.Empty;
        public static string Database=string.Empty;
        public static string Username = string.Empty;
        public static string Password = string.Empty;
        public static string ProjectName = string.Empty;

        public static string ConnectionString = string.Empty;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //if (Server == string.Empty)
            //{
            //    Application.Run(new frmServer());

            //}
            //else
            //{
            //    Application.Run(new Form1());
            //}
            Application.Run(new Form1());
            
        }
    }
}
