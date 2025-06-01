namespace BaseDeDatosSQL
{
    partial class FormSeleccionObjetos
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
            this.cbBasesDeDatos = new System.Windows.Forms.ComboBox();
            this.clbTablas = new System.Windows.Forms.CheckedListBox();
            this.clbVistas = new System.Windows.Forms.CheckedListBox();
            this.clbProcedimientos = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnMigrarSeleccionados = new System.Windows.Forms.Button();
            this.rbTodos = new System.Windows.Forms.RadioButton();
            this.rbSeleccionManual = new System.Windows.Forms.RadioButton();
            this.rtbResultados = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbBasesDeDatos
            // 
            this.cbBasesDeDatos.FormattingEnabled = true;
            this.cbBasesDeDatos.Location = new System.Drawing.Point(161, 15);
            this.cbBasesDeDatos.Name = "cbBasesDeDatos";
            this.cbBasesDeDatos.Size = new System.Drawing.Size(392, 21);
            this.cbBasesDeDatos.TabIndex = 0;
            this.cbBasesDeDatos.SelectedIndexChanged += new System.EventHandler(this.cbBasesDeDatos_SelectedIndexChanged);
            // 
            // clbTablas
            // 
            this.clbTablas.FormattingEnabled = true;
            this.clbTablas.Location = new System.Drawing.Point(11, 19);
            this.clbTablas.Name = "clbTablas";
            this.clbTablas.Size = new System.Drawing.Size(464, 229);
            this.clbTablas.TabIndex = 1;
            // 
            // clbVistas
            // 
            this.clbVistas.Enabled = false;
            this.clbVistas.FormattingEnabled = true;
            this.clbVistas.Location = new System.Drawing.Point(11, 19);
            this.clbVistas.Name = "clbVistas";
            this.clbVistas.Size = new System.Drawing.Size(267, 94);
            this.clbVistas.TabIndex = 2;
            // 
            // clbProcedimientos
            // 
            this.clbProcedimientos.Enabled = false;
            this.clbProcedimientos.FormattingEnabled = true;
            this.clbProcedimientos.Location = new System.Drawing.Point(11, 19);
            this.clbProcedimientos.Name = "clbProcedimientos";
            this.clbProcedimientos.Size = new System.Drawing.Size(267, 94);
            this.clbProcedimientos.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(77, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Base de datos:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbTablas);
            this.groupBox1.Location = new System.Drawing.Point(20, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 259);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tablas";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clbVistas);
            this.groupBox2.Location = new System.Drawing.Point(535, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 127);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vistas";
            this.groupBox2.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.clbProcedimientos);
            this.groupBox3.Location = new System.Drawing.Point(535, 234);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(284, 126);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Procedimientos Almacenados";
            this.groupBox3.Visible = false;
            // 
            // btnMigrarSeleccionados
            // 
            this.btnMigrarSeleccionados.Location = new System.Drawing.Point(200, 468);
            this.btnMigrarSeleccionados.Name = "btnMigrarSeleccionados";
            this.btnMigrarSeleccionados.Size = new System.Drawing.Size(242, 33);
            this.btnMigrarSeleccionados.TabIndex = 11;
            this.btnMigrarSeleccionados.Text = "Migrar Seleccionados";
            this.btnMigrarSeleccionados.UseVisualStyleBackColor = true;
            this.btnMigrarSeleccionados.Click += new System.EventHandler(this.btnMigrar_Click);
            // 
            // rbTodos
            // 
            this.rbTodos.AutoSize = true;
            this.rbTodos.Location = new System.Drawing.Point(200, 54);
            this.rbTodos.Name = "rbTodos";
            this.rbTodos.Size = new System.Drawing.Size(78, 17);
            this.rbTodos.TabIndex = 12;
            this.rbTodos.TabStop = true;
            this.rbTodos.Text = "Migrar todo";
            this.rbTodos.UseVisualStyleBackColor = true;
            // 
            // rbSeleccionManual
            // 
            this.rbSeleccionManual.AutoSize = true;
            this.rbSeleccionManual.Location = new System.Drawing.Point(294, 54);
            this.rbSeleccionManual.Name = "rbSeleccionManual";
            this.rbSeleccionManual.Size = new System.Drawing.Size(109, 17);
            this.rbSeleccionManual.TabIndex = 13;
            this.rbSeleccionManual.TabStop = true;
            this.rbSeleccionManual.Text = "Seleccion manual";
            this.rbSeleccionManual.UseVisualStyleBackColor = true;
            // 
            // rtbResultados
            // 
            this.rtbResultados.BackColor = System.Drawing.Color.Black;
            this.rtbResultados.ForeColor = System.Drawing.Color.White;
            this.rtbResultados.Location = new System.Drawing.Point(20, 366);
            this.rtbResultados.Name = "rtbResultados";
            this.rtbResultados.ReadOnly = true;
            this.rtbResultados.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbResultados.Size = new System.Drawing.Size(574, 96);
            this.rtbResultados.TabIndex = 15;
            this.rtbResultados.Text = "Log de información...";
            // 
            // FormSeleccionObjetos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 512);
            this.Controls.Add(this.rtbResultados);
            this.Controls.Add(this.rbSeleccionManual);
            this.Controls.Add(this.rbTodos);
            this.Controls.Add(this.btnMigrarSeleccionados);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbBasesDeDatos);
            this.Name = "FormSeleccionObjetos";
            this.Text = "Seleccionar que migrar...";
            this.Load += new System.EventHandler(this.FormSeleccionObjetos_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbBasesDeDatos;
        private System.Windows.Forms.CheckedListBox clbTablas;
        private System.Windows.Forms.CheckedListBox clbVistas;
        private System.Windows.Forms.CheckedListBox clbProcedimientos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnMigrarSeleccionados;
        private System.Windows.Forms.RadioButton rbTodos;
        private System.Windows.Forms.RadioButton rbSeleccionManual;
        private System.Windows.Forms.RichTextBox rtbResultados;
    }
}