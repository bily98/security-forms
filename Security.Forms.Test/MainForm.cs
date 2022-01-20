using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace Security.Forms.Test
{
    public partial class MainForm : SecureFluentDesignForm
    {
        private IPrincipal _nextFormPrincipal;

        public MainForm(IPrincipal userPrincipal) : base(new string[] { "Administrador", "Digitalizador" }, userPrincipal)
        {
            _nextFormPrincipal = userPrincipal;
            InitializeComponent();
        }

        private void AcValidRole_Click(object sender, EventArgs e)
        {

        }

        private void AcShowUser_Click(object sender, EventArgs e)
        {
            TestUserControl.Principal = _nextFormPrincipal;
            if (!fluentDesignFormContainer1.Controls.Contains(TestUserControl.Instance))
            {
                fluentDesignFormContainer1.Controls.Add(TestUserControl.Instance);
                TestUserControl.Instance.Dock = DockStyle.Fill;
            }

            TestUserControl.Instance.Show();
            TestUserControl.Instance.BringToFront();
        }

        private void FluentDesignForm1_UserIsAllowed(object sender, EventArgs e)
        {
            AcValidRole.Enabled = ValidatedUserRoles.Contains("UserRole1");
            AcShowUser.Enabled = ValidatedUserRoles.Contains("UserRole3");
        }
    }
}
