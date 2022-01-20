using System;
using System.Drawing;
using System.Security.Principal;

namespace Security.Forms.Test
{
    public partial class TestUserControl : SecureUserControl
    {
        public TestUserControl(IPrincipal userPrincipal) : base(new string[] { "UserRole2" }, userPrincipal)
        {
            InitializeComponent();
        }

        private static IPrincipal _principal;
        public static IPrincipal Principal
        {
            get { return _principal; }
            set { _principal = value; }
        }

        private static TestUserControl _instance;
        public static TestUserControl Instance
        {
            get { return _instance ?? (_instance = new TestUserControl(Principal)); }
        }

        private void TestUserControl_UserIsAllowed(object sender, EventArgs e)
        {
            LblRol.Text = "Tiene Permiso";
            LblRol.ForeColor = Color.Green;
        }

        private void TestUserControl_UserIsDenied(object sender, EventArgs e)
        {
            LblRol.Text = "No Tiene Permiso";
            LblRol.ForeColor = Color.Red;
        }
    }
}
