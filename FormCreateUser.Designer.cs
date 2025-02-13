namespace BaseDeDatosSQL
{
    partial class FormCreateUser
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbNombre = new System.Windows.Forms.TextBox();
            this.tbContrasena = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbMustChangePass = new System.Windows.Forms.CheckBox();
            this.btnCrearUsuario = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbBasesDeDatos = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.clbRoles = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbNombre
            // 
            this.tbNombre.Location = new System.Drawing.Point(15, 41);
            this.tbNombre.Name = "tbNombre";
            this.tbNombre.Size = new System.Drawing.Size(194, 20);
            this.tbNombre.TabIndex = 0;
            this.tbNombre.TextChanged += new System.EventHandler(this.tbNombre_TextChanged);
            // 
            // tbContrasena
            // 
            this.tbContrasena.Location = new System.Drawing.Point(15, 92);
            this.tbContrasena.Name = "tbContrasena";
            this.tbContrasena.Size = new System.Drawing.Size(194, 20);
            this.tbContrasena.TabIndex = 1;
            this.tbContrasena.UseSystemPasswordChar = true;
            this.tbContrasena.TextChanged += new System.EventHandler(this.tbContrasena_TextChanged);
            this.tbContrasena.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbContrasena_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ingresa tu nombre:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ingresa tu Contraseña:";
            // 
            // cbMustChangePass
            // 
            this.cbMustChangePass.AutoSize = true;
            this.cbMustChangePass.Location = new System.Drawing.Point(12, 335);
            this.cbMustChangePass.Name = "cbMustChangePass";
            this.cbMustChangePass.Size = new System.Drawing.Size(221, 17);
            this.cbMustChangePass.TabIndex = 4;
            this.cbMustChangePass.Text = "User must change Password at new login";
            this.cbMustChangePass.UseVisualStyleBackColor = true;
            // 
            // btnCrearUsuario
            // 
            this.btnCrearUsuario.Enabled = false;
            this.btnCrearUsuario.Location = new System.Drawing.Point(56, 358);
            this.btnCrearUsuario.Name = "btnCrearUsuario";
            this.btnCrearUsuario.Size = new System.Drawing.Size(91, 36);
            this.btnCrearUsuario.TabIndex = 5;
            this.btnCrearUsuario.Text = "Crear nuevo usuario";
            this.btnCrearUsuario.UseVisualStyleBackColor = true;
            this.btnCrearUsuario.Click += new System.EventHandler(this.btnCrearUsuario_Click);
            // 
            // cbBasesDeDatos
            // 
            this.cbBasesDeDatos.FormattingEnabled = true;
            this.cbBasesDeDatos.Location = new System.Drawing.Point(15, 149);
            this.cbBasesDeDatos.Name = "cbBasesDeDatos";
            this.cbBasesDeDatos.Size = new System.Drawing.Size(194, 21);
            this.cbBasesDeDatos.TabIndex = 6;
            this.cbBasesDeDatos.SelectedValueChanged += new System.EventHandler(this.cbBasesDeDatos_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Base de datos del usuario:";
            // 
            // clbRoles
            // 
            this.clbRoles.FormattingEnabled = true;
            this.clbRoles.Items.AddRange(new object[] {
            "db_owner",
            "db_securityadmin",
            "db_accessadmin",
            "db_backupoperator",
            "db_ddladmin",
            "db_datawriter",
            "db_datareader",
            "db_denydatawriter",
            "db_denydatareader"});
            this.clbRoles.Location = new System.Drawing.Point(15, 202);
            this.clbRoles.Name = "clbRoles";
            this.clbRoles.Size = new System.Drawing.Size(194, 124);
            this.clbRoles.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Selecciona el rol:";
            // 
            // FormCreateUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 406);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.clbRoles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbBasesDeDatos);
            this.Controls.Add(this.btnCrearUsuario);
            this.Controls.Add(this.cbMustChangePass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbContrasena);
            this.Controls.Add(this.tbNombre);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCreateUser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Crear nuevo usuario...";
            this.Load += new System.EventHandler(this.FormCreateUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbNombre;
        private System.Windows.Forms.TextBox tbContrasena;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbMustChangePass;
        private System.Windows.Forms.Button btnCrearUsuario;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cbBasesDeDatos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox clbRoles;
        private System.Windows.Forms.Label label4;
    }
}