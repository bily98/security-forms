using System;
using System.Security.Principal;
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

            MainForm mainForm = new MainForm(userPrincipal);

            // Set form to be main window in order to Exit the application.
            mainForm.IsMainWindow = true;
            mainForm.Show();

            if (mainForm.Created)
                Application.Run();
        }
    }
}
