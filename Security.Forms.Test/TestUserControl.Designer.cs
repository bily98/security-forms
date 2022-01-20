namespace Security.Forms.Test
{
    partial class TestUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LblRol = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // LblRol
            // 
            this.LblRol.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblRol.Appearance.Options.UseFont = true;
            this.LblRol.Location = new System.Drawing.Point(114, 103);
            this.LblRol.Name = "LblRol";
            this.LblRol.Size = new System.Drawing.Size(184, 39);
            this.LblRol.TabIndex = 0;
            this.LblRol.Text = "labelControl1";
            // 
            // TestUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LblRol);
            this.Name = "TestUserControl";
            this.Size = new System.Drawing.Size(433, 271);
            this.UserIsAllowed += new System.EventHandler(this.TestUserControl_UserIsAllowed);
            this.UserIsDenied += new System.EventHandler(this.TestUserControl_UserIsDenied);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl LblRol;
    }
}
