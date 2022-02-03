using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows.Forms;

namespace Security.Forms
{
    public class SecureXtraForm : XtraForm
    {
		private enum CalledShowMethod
		{
			Show,
			ShowWithOwner
		}

		public event EventHandler UserIsAllowed;
		public event EventHandler UserIsDenied;

		// Variable to capture the roles allowed for this form
		private List<string> _formRoles;
		// Variable to capture the users Principal
		private IPrincipal _formPrincipal;

		public List<string> ValidatedUserRoles { get; private set; }

		public bool IsMainWindow { get; set; }

		/// <summary>
		/// Common constructor needed for Forms Designer, When instantiated throug this
		/// method, the window should be secured and disallow access.
		/// </summary>
		public SecureXtraForm()
        {
			this.IsMainWindow = false;
			this.ValidatedUserRoles = new List<string>();
			this.UserCanOpenForm = false;
		}

		/// <summary>
		/// Constructor allowing the user/developer to assign roles for the specific form
		/// and parse the user principal.
		/// </summary>
		/// <param name="roles">A collection of roles, when empty access will be denied.</param>
		/// <param name="userPrincipal">The user principal</param>
		public SecureXtraForm(string[] roles, IPrincipal userPrincipal)
        {
			this.IsMainWindow = false;
			this.ValidatedUserRoles = new List<string>();
			this._formRoles = new List<string>();
			this._formRoles.AddRange(roles);

			this._formPrincipal = userPrincipal;

			ValidateUserRoles();
		}

		/// <summary>
		/// Validates the user roles.
		/// </summary>
		private void ValidateUserRoles()
		{
			foreach (string role in _formRoles)
				if (_formPrincipal.IsInRole(role))
					this.ValidatedUserRoles.Add(role);

			this.UserCanOpenForm = this.ValidatedUserRoles.Count > 0;
		}

		/// <summary>
		/// Gets or sets a value indicating whether [user can open form].
		/// </summary>
		/// <value><c>true</c> if [user can open form]; otherwise, <c>false</c>.</value>
		public bool UserCanOpenForm { get; private set; }

		/// <summary>
		/// Displays the control to the user.
		/// </summary>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
		/// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		public new virtual void Show()
		{
			Show(CalledShowMethod.Show, null);
		}

		/// <summary>
		/// Shows the form with the specified owner to the user.
		/// </summary>
		/// <param name="owner">Any object that implements <see cref="T:System.Windows.Forms.IWin32Window"/> and represents the top-level window that will own this form.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The form specified in the <paramref name="owner"/> parameter is the same as the form being shown.
		/// </exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
		/// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		public new virtual void Show(IWin32Window owner)
		{
			Show(CalledShowMethod.ShowWithOwner, owner);
		}

		/// <summary>
		/// Shows the specified called show method.
		/// </summary>
		/// <param name="calledShowMethod">The called show method.</param>
		/// <param name="owner">The owner or null otherwise.</param>
		private void Show(CalledShowMethod calledShowMethod, IWin32Window owner)
		{
			if (!DesignMode)
			{
				if (UserCanOpenForm)
				{
					ShowForm(calledShowMethod, owner);
					if (UserIsAllowed != null)
						UserIsAllowed(this, new EventArgs());
				}
				else
				{
					if (UserIsDenied != null)
					{
						ShowForm(calledShowMethod, owner);
						UserIsDenied(this, new EventArgs());
					}
					else
						if (this.IsMainWindow)
						Application.Exit();
				}
			}
			else
				ShowForm(calledShowMethod, owner);
		}

		/// <summary>
		/// Shows the form.
		/// </summary>
		/// <param name="calledShowMethod">The called show method.</param>
		/// <param name="owner">The owner.</param>
		private void ShowForm(CalledShowMethod calledShowMethod, IWin32Window owner)
		{
			if (calledShowMethod == CalledShowMethod.Show)
				base.Show();
			else
				base.Show(owner);
		}

		/// <summary>
		/// Shows the form as a modal dialog box with the currently active window set as its owner.
		/// </summary>
		/// <returns>
		/// One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		/// The form specified in the <paramref name="owner"/> parameter is the same as the form being shown.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The form being shown is already visible.
		/// -or-
		/// The form being shown is disabled.
		/// -or-
		/// The form being shown is not a top-level window.
		/// -or-
		/// The form being shown as a dialog box is already a modal form.
		/// -or-
		/// The current process is not running in user interactive mode (for more information, see <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive"/>).
		/// </exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
		/// 	<IPermission class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		public new virtual DialogResult ShowDialog()
		{
			return ShowDialog(CalledShowMethod.ShowWithOwner, null);
		}

		/// <summary>
		/// Shows the form as a modal dialog box with the specified owner.
		/// </summary>
		/// <param name="owner">Any object that implements <see cref="T:System.Windows.Forms.IWin32Window"/> that represents the top-level window that will own the modal dialog box.</param>
		/// <returns>
		/// One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		/// The form specified in the <paramref name="owner"/> parameter is the same as the form being shown.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// The form being shown is already visible.
		/// -or-
		/// The form being shown is disabled.
		/// -or-
		/// The form being shown is not a top-level window.
		/// -or-
		/// The form being shown as a dialog box is already a modal form.
		/// -or-
		/// The current process is not running in user interactive mode (for more information, see <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive"/>).
		/// </exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
		/// 	<IPermission class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		public new virtual DialogResult ShowDialog(IWin32Window owner)
		{
			return ShowDialog(CalledShowMethod.ShowWithOwner, owner);
		}

		/// <summary>
		/// Shows the dialog.
		/// </summary>
		/// <param name="calledShowMethod">The called show method.</param>
		/// <param name="owner">The owner.</param>
		/// <returns></returns>
		private DialogResult ShowDialog(CalledShowMethod calledShowMethod, IWin32Window owner)
		{
			if (!DesignMode)
			{
				DialogResult result = DialogResult.None;
				if (UserCanOpenForm)
				{
					result = ShowDialogForm(calledShowMethod, owner);
					if (UserIsAllowed != null)
						UserIsAllowed(this, new EventArgs());
				}
				else
				{
					if (UserIsDenied != null)
					{
						result = ShowDialogForm(calledShowMethod, owner);
						UserIsDenied(this, new EventArgs());
					}
					else
						if (IsMainWindow)
						Application.Exit();
				}

				return result;
			}
			else
				return ShowDialogForm(calledShowMethod, owner);
		}

		/// <summary>
		/// Shows the dialog form.
		/// </summary>
		/// <param name="calledShowMethod">The called show method.</param>
		/// <param name="owner">The owner.</param>
		/// <returns></returns>
		private DialogResult ShowDialogForm(CalledShowMethod calledShowMethod, IWin32Window owner)
		{
			if (calledShowMethod == CalledShowMethod.Show)
				return base.ShowDialog();
			else
				return base.ShowDialog(owner);
		}

		private void SecureBaseForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (this.IsMainWindow)
				Application.Exit();
		}
	}
}
