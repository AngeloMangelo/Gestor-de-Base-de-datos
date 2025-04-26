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
            this.rtbQuery = new System.Windows.Forms.RichTextBox();
            this.btnEjecutarQuery = new System.Windows.Forms.Button();
            this.cbBaseDeDatos = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNuevaConexion = new System.Windows.Forms.Button();
            this.labelbdSelected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeViewAsistente
            // 
            this.treeViewAsistente.Location = new System.Drawing.Point(12, 51);
            this.treeViewAsistente.Name = "treeViewAsistente";
            this.treeViewAsistente.Size = new System.Drawing.Size(480, 349);
            this.treeViewAsistente.TabIndex = 0;
            this.treeViewAsistente.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewAsistente_BeforeExpand);
            this.treeViewAsistente.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewAsistente_AfterSelect);
            this.treeViewAsistente.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewAsistente_NodeMouseClick);
            this.treeViewAsistente.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewAsistente_MouseClick);
            // 
            // btnAddBD
            // 
            this.btnAddBD.Enabled = false;
            this.btnAddBD.Location = new System.Drawing.Point(502, 338);
            this.btnAddBD.Name = "btnAddBD";
            this.btnAddBD.Size = new System.Drawing.Size(128, 23);
            this.btnAddBD.TabIndex = 4;
            this.btnAddBD.Text = "Agregar base de datos";
            this.btnAddBD.UseVisualStyleBackColor = true;
            this.btnAddBD.Visible = false;
            this.btnAddBD.Click += new System.EventHandler(this.btnAddBD_Click);
            // 
            // btnSearchDB
            // 
            this.btnSearchDB.Enabled = false;
            this.btnSearchDB.Location = new System.Drawing.Point(502, 367);
            this.btnSearchDB.Name = "btnSearchDB";
            this.btnSearchDB.Size = new System.Drawing.Size(136, 23);
            this.btnSearchDB.TabIndex = 5;
            this.btnSearchDB.Text = "Cargar base de datos...";
            this.btnSearchDB.UseVisualStyleBackColor = true;
            this.btnSearchDB.Visible = false;
            this.btnSearchDB.Click += new System.EventHandler(this.btnSearchDB_Click);
            // 
            // btnRefreshDB
            // 
            this.btnRefreshDB.Location = new System.Drawing.Point(404, 18);
            this.btnRefreshDB.Name = "btnRefreshDB";
            this.btnRefreshDB.Size = new System.Drawing.Size(88, 23);
            this.btnRefreshDB.TabIndex = 6;
            this.btnRefreshDB.Text = "Actualizar BD";
            this.btnRefreshDB.UseVisualStyleBackColor = true;
            this.btnRefreshDB.Click += new System.EventHandler(this.btnRefreshDB_Click);
            // 
            // llCreateLogin
            // 
            this.llCreateLogin.AutoSize = true;
            this.llCreateLogin.Enabled = false;
            this.llCreateLogin.Location = new System.Drawing.Point(789, 386);
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
            this.label1.Location = new System.Drawing.Point(680, 386);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "¿No tienes Usuario?";
            // 
            // rtbQuery
            // 
            this.rtbQuery.Location = new System.Drawing.Point(498, 67);
            this.rtbQuery.Name = "rtbQuery";
            this.rtbQuery.Size = new System.Drawing.Size(351, 243);
            this.rtbQuery.TabIndex = 9;
            this.rtbQuery.Text = "";
            // 
            // btnEjecutarQuery
            // 
            this.btnEjecutarQuery.Location = new System.Drawing.Point(774, 316);
            this.btnEjecutarQuery.Name = "btnEjecutarQuery";
            this.btnEjecutarQuery.Size = new System.Drawing.Size(75, 23);
            this.btnEjecutarQuery.TabIndex = 10;
            this.btnEjecutarQuery.Text = "Ejecutar Query";
            this.btnEjecutarQuery.UseVisualStyleBackColor = true;
            this.btnEjecutarQuery.Click += new System.EventHandler(this.btnEjecutarQuery_Click);
            // 
            // cbBaseDeDatos
            // 
            this.cbBaseDeDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaseDeDatos.FormattingEnabled = true;
            this.cbBaseDeDatos.Location = new System.Drawing.Point(683, 21);
            this.cbBaseDeDatos.Name = "cbBaseDeDatos";
            this.cbBaseDeDatos.Size = new System.Drawing.Size(166, 21);
            this.cbBaseDeDatos.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(498, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Query:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Servidores:";
            // 
            // btnNuevaConexion
            // 
            this.btnNuevaConexion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevaConexion.Location = new System.Drawing.Point(135, 12);
            this.btnNuevaConexion.Name = "btnNuevaConexion";
            this.btnNuevaConexion.Size = new System.Drawing.Size(148, 30);
            this.btnNuevaConexion.TabIndex = 14;
            this.btnNuevaConexion.Text = "(+) Nueva Conexion";
            this.btnNuevaConexion.UseVisualStyleBackColor = true;
            this.btnNuevaConexion.Click += new System.EventHandler(this.btnNuevaConexion_Click);
            // 
            // labelbdSelected
            // 
            this.labelbdSelected.AutoSize = true;
            this.labelbdSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelbdSelected.Location = new System.Drawing.Point(680, 4);
            this.labelbdSelected.Name = "labelbdSelected";
            this.labelbdSelected.Size = new System.Drawing.Size(130, 16);
            this.labelbdSelected.TabIndex = 15;
            this.labelbdSelected.Text = "Bd Seleccionada:";
            // 
            // Sistema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(861, 412);
            this.Controls.Add(this.labelbdSelected);
            this.Controls.Add(this.btnNuevaConexion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbBaseDeDatos);
            this.Controls.Add(this.btnEjecutarQuery);
            this.Controls.Add(this.rtbQuery);
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
        private System.Windows.Forms.RichTextBox rtbQuery;
        private System.Windows.Forms.Button btnEjecutarQuery;
        private System.Windows.Forms.ComboBox cbBaseDeDatos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNuevaConexion;
        private System.Windows.Forms.Label labelbdSelected;
    }
}

