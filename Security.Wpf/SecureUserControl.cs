using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Security.Wpf
{
    public class SecureUserControl : UserControl
    {
		private enum CalledShowMethod
		{
			Show,
			ShowWithOwner
		}

		public event EventHandler UserIsAllowed;
		public event EventHandler UserIsDenied;

		// Variable to capture the roles allowed for this Window
		private List<string> _WindowRoles;
		// Variable to capture the users Principal
		private IPrincipal _WindowPrincipal;

		public List<string> ValidatedUserRoles { get; private set; }

		public bool IsMainWindow { get; set; }

        /// <summary>
        /// Common constructor needed for Windows Designer, When instantiated throug this
        /// method, the window should be secured and disallow access.
        /// </summary>
        public SecureUserControl()
        {
			this.IsMainWindow = false;
			this.ValidatedUserRoles = new List<string>();
			this.UserCanOpenWindow = false;
		}

		/// <summary>
		/// Constructor allowing the user/developer to assign roles for the specific Window
		/// and parse the user principal.
		/// </summary>
		/// <param name="roles">A collection of roles, when empty access will be denied.</param>
		/// <param name="userPrincipal">The user principal</param>
		public SecureUserControl(string[] roles, IPrincipal userPrincipal)
		{
			this.IsMainWindow = false;
			this.ValidatedUserRoles = new List<string>();
			this._WindowRoles = new List<string>();
			this._WindowRoles.AddRange(roles);

			this._WindowPrincipal = userPrincipal;

			ValidateUserRoles();
		}

		/// <summary>
		/// Validates the user roles.
		/// </summary>
		private void ValidateUserRoles()
		{
			foreach (string role in _WindowRoles)
				if (_WindowPrincipal.IsInRole(role))
					this.ValidatedUserRoles.Add(role);

			this.UserCanOpenWindow = this.ValidatedUserRoles.Count > 0;
		}

		/// <summary>
		/// Gets or sets a value indicating whether [user can open Window].
		/// </summary>
		/// <value><c>true</c> if [user can open Window]; otherwise, <c>false</c>.</value>
		public bool UserCanOpenWindow { get; private set; }

		/// <summary>
		/// Displays the control to the user.
		/// </summary>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
		/// 	<IPermission class="System.Diagnostics.PerWindowanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// </PermissionSet>
		public new virtual void Show()
		{
			Show(CalledShowMethod.Show, null);
		}

		/// <summary>
		/// Shows the Window with the specified owner to the user.
		/// </summary>
		/// <param name="owner">Any object that implements <see cref="T:System.Windows.Windows.IWin32Window"/> and represents the top-level window that will own this Window.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The Window specified in the <paramref name="owner"/> parameter is the same as the Window being shown.
		/// </exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
		/// 	<IPermission class="System.Diagnostics.PerWindowanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
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
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				if (UserCanOpenWindow)
				{
					ShowWindow(calledShowMethod, owner);
					if (UserIsAllowed != null)
						UserIsAllowed(this, new EventArgs());
				}
				else
				{
					if (UserIsDenied != null)
					{
						ShowWindow(calledShowMethod, owner);
						UserIsDenied(this, new EventArgs());
					}
					else
						if (this.IsMainWindow)
						Application.Current.Shutdown();
				}
			}
			else
				ShowWindow(calledShowMethod, owner);
		}

		/// <summary>
		/// Shows the Window.
		/// </summary>
		/// <param name="calledShowMethod">The called show method.</param>
		/// <param name="owner">The owner.</param>
		private void ShowWindow(CalledShowMethod calledShowMethod, IWin32Window owner)
		{
			if (calledShowMethod == CalledShowMethod.Show)
				Show();
			else
				Show();
		}

		public new virtual bool? ShowDialog()
		{
			return ShowDialog(CalledShowMethod.ShowWithOwner, null);
		}

		public new virtual bool? ShowDialog(IWin32Window owner)
		{
			return ShowDialog(CalledShowMethod.ShowWithOwner, owner);
		}

		/// <summary>
		/// Shows the dialog.
		/// </summary>
		/// <param name="calledShowMethod">The called show method.</param>
		/// <param name="owner">The owner.</param>
		/// <returns></returns>
		private bool? ShowDialog(CalledShowMethod calledShowMethod, IWin32Window owner)
		{
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				bool? result = false;
				if (UserCanOpenWindow)
				{
					result = ShowDialogWindow(calledShowMethod, owner);
					if (UserIsAllowed != null)
						UserIsAllowed(this, new EventArgs());
				}
				else
				{
					if (UserIsDenied != null)
					{
						result = ShowDialogWindow(calledShowMethod, owner);
						UserIsDenied(this, new EventArgs());
					}
					else
						if (IsMainWindow)
						Application.Current.Shutdown();
				}

				return result;
			}
			else
				return ShowDialogWindow(calledShowMethod, owner);
		}

		/// <summary>
		/// Shows the dialog Window.
		/// </summary>
		/// <param name="calledShowMethod">The called show method.</param>
		/// <param name="owner">The owner.</param>
		/// <returns></returns>
		private bool? ShowDialogWindow(CalledShowMethod calledShowMethod, IWin32Window owner)
		{
			if (calledShowMethod == CalledShowMethod.Show)
				return ShowDialog();
			else
				return ShowDialog(owner);
		}

		private void SecureBaseWindow_WindowClosed(object sender, CancelEventArgs e)
		{
			if (this.IsMainWindow)
				Application.Current.Shutdown();
		}
	}
}
