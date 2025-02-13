namespace BaseDeDatosSQL
{
    partial class FormCreateTable
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
            this.dataGridViewCampos = new System.Windows.Forms.DataGridView();
            this.btnCrearTabla = new System.Windows.Forms.Button();
            this.btnAgregarCampo = new System.Windows.Forms.Button();
            this.tbNombreTabla = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbDatosColumna = new System.Windows.Forms.GroupBox();
            this.cbIdentity = new System.Windows.Forms.CheckBox();
            this.cbPk = new System.Windows.Forms.CheckBox();
            this.cbNotNull = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbLongitud = new System.Windows.Forms.TextBox();
            this.cbTipodedato = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCampo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCampos)).BeginInit();
            this.gbDatosColumna.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewCampos
            // 
            this.dataGridViewCampos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCampos.Location = new System.Drawing.Point(25, 213);
            this.dataGridViewCampos.Name = "dataGridViewCampos";
            this.dataGridViewCampos.ReadOnly = true;
            this.dataGridViewCampos.Size = new System.Drawing.Size(644, 166);
            this.dataGridViewCampos.TabIndex = 0;
            // 
            // btnCrearTabla
            // 
            this.btnCrearTabla.Location = new System.Drawing.Point(25, 396);
            this.btnCrearTabla.Name = "btnCrearTabla";
            this.btnCrearTabla.Size = new System.Drawing.Size(86, 32);
            this.btnCrearTabla.TabIndex = 13;
            this.btnCrearTabla.Text = "Crear Tabla";
            this.btnCrearTabla.UseVisualStyleBackColor = true;
            this.btnCrearTabla.Click += new System.EventHandler(this.btnCrearTabla_Click);
            // 
            // btnAgregarCampo
            // 
            this.btnAgregarCampo.Location = new System.Drawing.Point(383, 96);
            this.btnAgregarCampo.Name = "btnAgregarCampo";
            this.btnAgregarCampo.Size = new System.Drawing.Size(105, 32);
            this.btnAgregarCampo.TabIndex = 14;
            this.btnAgregarCampo.Text = "(+) Agregar campo";
            this.btnAgregarCampo.UseVisualStyleBackColor = true;
            this.btnAgregarCampo.Click += new System.EventHandler(this.btnAgregarCampo_Click);
            // 
            // tbNombreTabla
            // 
            this.tbNombreTabla.Location = new System.Drawing.Point(130, 17);
            this.tbNombreTabla.Name = "tbNombreTabla";
            this.tbNombreTabla.Size = new System.Drawing.Size(171, 20);
            this.tbNombreTabla.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Nombre de la tabla:";
            // 
            // gbDatosColumna
            // 
            this.gbDatosColumna.Controls.Add(this.cbIdentity);
            this.gbDatosColumna.Controls.Add(this.cbPk);
            this.gbDatosColumna.Controls.Add(this.cbNotNull);
            this.gbDatosColumna.Controls.Add(this.label4);
            this.gbDatosColumna.Controls.Add(this.tbLongitud);
            this.gbDatosColumna.Controls.Add(this.cbTipodedato);
            this.gbDatosColumna.Controls.Add(this.label3);
            this.gbDatosColumna.Controls.Add(this.label2);
            this.gbDatosColumna.Controls.Add(this.tbCampo);
            this.gbDatosColumna.Controls.Add(this.btnAgregarCampo);
            this.gbDatosColumna.Location = new System.Drawing.Point(28, 73);
            this.gbDatosColumna.Name = "gbDatosColumna";
            this.gbDatosColumna.Size = new System.Drawing.Size(494, 134);
            this.gbDatosColumna.TabIndex = 17;
            this.gbDatosColumna.TabStop = false;
            this.gbDatosColumna.Text = "Datos de la columna";
            // 
            // cbIdentity
            // 
            this.cbIdentity.AutoSize = true;
            this.cbIdentity.Location = new System.Drawing.Point(292, 84);
            this.cbIdentity.Name = "cbIdentity";
            this.cbIdentity.Size = new System.Drawing.Size(60, 17);
            this.cbIdentity.TabIndex = 26;
            this.cbIdentity.Text = "Identity";
            this.cbIdentity.UseVisualStyleBackColor = true;
            // 
            // cbPk
            // 
            this.cbPk.AutoSize = true;
            this.cbPk.Location = new System.Drawing.Point(292, 58);
            this.cbPk.Name = "cbPk";
            this.cbPk.Size = new System.Drawing.Size(81, 17);
            this.cbPk.TabIndex = 25;
            this.cbPk.Text = "Primary Key";
            this.cbPk.UseVisualStyleBackColor = true;
            // 
            // cbNotNull
            // 
            this.cbNotNull.AutoSize = true;
            this.cbNotNull.Location = new System.Drawing.Point(292, 31);
            this.cbNotNull.Name = "cbNotNull";
            this.cbNotNull.Size = new System.Drawing.Size(64, 17);
            this.cbNotNull.TabIndex = 24;
            this.cbNotNull.Text = "Not Null";
            this.cbNotNull.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Longitud:";
            // 
            // tbLongitud
            // 
            this.tbLongitud.Location = new System.Drawing.Point(140, 83);
            this.tbLongitud.Name = "tbLongitud";
            this.tbLongitud.Size = new System.Drawing.Size(134, 20);
            this.tbLongitud.TabIndex = 22;
            this.tbLongitud.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLongitud_KeyPress);
            // 
            // cbTipodedato
            // 
            this.cbTipodedato.FormattingEnabled = true;
            this.cbTipodedato.Location = new System.Drawing.Point(140, 56);
            this.cbTipodedato.Name = "cbTipodedato";
            this.cbTipodedato.Size = new System.Drawing.Size(133, 21);
            this.cbTipodedato.TabIndex = 21;
            this.cbTipodedato.SelectedIndexChanged += new System.EventHandler(this.cbTipodedato_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Selecciona tipo de dato:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Nombre del campo:";
            // 
            // tbCampo
            // 
            this.tbCampo.Location = new System.Drawing.Point(139, 30);
            this.tbCampo.Name = "tbCampo";
            this.tbCampo.Size = new System.Drawing.Size(134, 20);
            this.tbCampo.TabIndex = 18;
            // 
            // FormCreateTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 441);
            this.Controls.Add(this.gbDatosColumna);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbNombreTabla);
            this.Controls.Add(this.btnCrearTabla);
            this.Controls.Add(this.dataGridViewCampos);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCreateTable";
            this.ShowIcon = false;
            this.Text = "Crear Tabla";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCampos)).EndInit();
            this.gbDatosColumna.ResumeLayout(false);
            this.gbDatosColumna.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCampos;
        private System.Windows.Forms.Button btnCrearTabla;
        private System.Windows.Forms.Button btnAgregarCampo;
        private System.Windows.Forms.TextBox tbNombreTabla;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbDatosColumna;
        private System.Windows.Forms.TextBox tbCampo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTipodedato;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLongitud;
        private System.Windows.Forms.CheckBox cbIdentity;
        private System.Windows.Forms.CheckBox cbPk;
        private System.Windows.Forms.CheckBox cbNotNull;
    }
}