using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Security.Forms.Test
{
    public partial class Form2 : SecureBaseForm
    {
        public Form2(IPrincipal userPrincipal) : base(new string[] { "UserRole1" }, userPrincipal)
        {
            InitializeComponent();
        }

        private void Form2_UserIsDenied(object sender, EventArgs e)
        {
            this.BackColor = Color.Red;
        }

        private void Form2_UserIsAllowed(object sender, EventArgs e)
        {
            this.BackColor = Color.Green;
        }
    }
}
