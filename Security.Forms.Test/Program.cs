using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Security.Forms.Test
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize a test Principal 
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                new string[] { "UserRole1", "UserRole3" });

            Form1 mainForm = new Form1(userPrincipal);

            // Set form to be main window in order to Exit the application.
            mainForm.IsMainWindow = true;
            mainForm.Show();

            if (mainForm.Created)
                Application.Run();
        }
    }
}
