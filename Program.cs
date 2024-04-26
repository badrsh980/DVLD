using DVLD.Applications;
using DVLD.Classes;
using DVLD.Login;
using DVLD.Tests;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string UserName = "", Password = "";
            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                clsGlobal.CurrentUser = clsUser.FindByUsernameAndPassword(UserName.Trim(), Password.Trim());
                if (clsGlobal.CurrentUser != null && clsGlobal.CurrentUser.IsActive == true)
                {
                    Application.Run(new frmMain());
                }
                else
                {
                    Application.Run(new frmLogin());
                }
            }
        }
    }
}
