namespace BaseDeDatosSQL
{
    partial class Sistema
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeViewAsistente = new System.Windows.Forms.TreeView();
            this.btnAddBD = new System.Windows.Forms.Button();
            this.btnSearchDB = new System.Windows.Forms.Button();
            this.btnRefreshDB = new System.Windows.Forms.Button();
            this.llCreateLogin = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeViewAsistente
            // 
            this.treeViewAsistente.Location = new System.Drawing.Point(12, 32);
            this.treeViewAsistente.Name = "treeViewAsistente";
            this.treeViewAsistente.Size = new System.Drawing.Size(271, 368);
            this.treeViewAsistente.TabIndex = 0;
            this.treeViewAsistente.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewAsistente_BeforeExpand);
            this.treeViewAsistente.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewAsistente_NodeMouseClick);
            this.treeViewAsistente.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewAsistente_MouseClick);
            // 
            // btnAddBD
            // 
            this.btnAddBD.Enabled = false;
            this.btnAddBD.Location = new System.Drawing.Point(13, 3);
            this.btnAddBD.Name = "btnAddBD";
            this.btnAddBD.Size = new System.Drawing.Size(128, 23);
            this.btnAddBD.TabIndex = 4;
            this.btnAddBD.Text = "Agregar base de datos";
            this.btnAddBD.UseVisualStyleBackColor = true;
            this.btnAddBD.Click += new System.EventHandler(this.btnAddBD_Click);
            // 
            // btnSearchDB
            // 
            this.btnSearchDB.Enabled = false;
            this.btnSearchDB.Location = new System.Drawing.Point(147, 3);
            this.btnSearchDB.Name = "btnSearchDB";
            this.btnSearchDB.Size = new System.Drawing.Size(136, 23);
            this.btnSearchDB.TabIndex = 5;
            this.btnSearchDB.Text = "Cargar base de datos...";
            this.btnSearchDB.UseVisualStyleBackColor = true;
            this.btnSearchDB.Click += new System.EventHandler(this.btnSearchDB_Click);
            // 
            // btnRefreshDB
            // 
            this.btnRefreshDB.Enabled = false;
            this.btnRefreshDB.Location = new System.Drawing.Point(292, 41);
            this.btnRefreshDB.Name = "btnRefreshDB";
            this.btnRefreshDB.Size = new System.Drawing.Size(88, 53);
            this.btnRefreshDB.TabIndex = 6;
            this.btnRefreshDB.Text = "Actualizar BD";
            this.btnRefreshDB.UseVisualStyleBackColor = true;
            this.btnRefreshDB.Click += new System.EventHandler(this.btnRefreshDB_Click);
            // 
            // llCreateLogin
            // 
            this.llCreateLogin.AutoSize = true;
            this.llCreateLogin.Enabled = false;
            this.llCreateLogin.Location = new System.Drawing.Point(458, 387);
            this.llCreateLogin.Name = "llCreateLogin";
            this.llCreateLogin.Size = new System.Drawing.Size(48, 13);
            this.llCreateLogin.TabIndex = 7;
            this.llCreateLogin.TabStop = true;
            this.llCreateLogin.Text = "Clic Aqui";
            this.llCreateLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCreateLogin_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(349, 387);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "¿No tienes Usuario?";
            // 
            // Sistema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 412);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.llCreateLogin);
            this.Controls.Add(this.btnRefreshDB);
            this.Controls.Add(this.btnSearchDB);
            this.Controls.Add(this.btnAddBD);
            this.Controls.Add(this.treeViewAsistente);
            this.MaximizeBox = false;
            this.Name = "Sistema";
            this.Text = "Sistema";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Sistema_FormClosed);
            this.Load += new System.EventHandler(this.Sistema_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewAsistente;
        private System.Windows.Forms.Button btnAddBD;
        private System.Windows.Forms.Button btnSearchDB;
        private System.Windows.Forms.Button btnRefreshDB;
        private System.Windows.Forms.LinkLabel llCreateLogin;
        private System.Windows.Forms.Label label1;
    }
}

