namespace BaseDeDatosSQL
{
    partial class formMigracion
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
            this.lblMigrar = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMigrar = new System.Windows.Forms.Button();
            this.cbOrigen = new System.Windows.Forms.ComboBox();
            this.cbDestino = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblMigrar
            // 
            this.lblMigrar.AutoSize = true;
            this.lblMigrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMigrar.Location = new System.Drawing.Point(12, 21);
            this.lblMigrar.Name = "lblMigrar";
            this.lblMigrar.Size = new System.Drawing.Size(112, 15);
            this.lblMigrar.TabIndex = 0;
            this.lblMigrar.Text = "Servidor de Origen:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Destino:";
            // 
            // btnMigrar
            // 
            this.btnMigrar.Location = new System.Drawing.Point(36, 138);
            this.btnMigrar.Name = "btnMigrar";
            this.btnMigrar.Size = new System.Drawing.Size(215, 50);
            this.btnMigrar.TabIndex = 3;
            this.btnMigrar.Text = "Migrar";
            this.btnMigrar.UseVisualStyleBackColor = true;
            this.btnMigrar.Click += new System.EventHandler(this.btnMigrar_Click);
            // 
            // cbOrigen
            // 
            this.cbOrigen.FormattingEnabled = true;
            this.cbOrigen.Location = new System.Drawing.Point(15, 39);
            this.cbOrigen.Name = "cbOrigen";
            this.cbOrigen.Size = new System.Drawing.Size(260, 21);
            this.cbOrigen.TabIndex = 4;
            // 
            // cbDestino
            // 
            this.cbDestino.FormattingEnabled = true;
            this.cbDestino.Location = new System.Drawing.Point(15, 97);
            this.cbDestino.Name = "cbDestino";
            this.cbDestino.Size = new System.Drawing.Size(260, 21);
            this.cbDestino.TabIndex = 5;
            // 
            // formMigracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 205);
            this.Controls.Add(this.cbDestino);
            this.Controls.Add(this.cbOrigen);
            this.Controls.Add(this.btnMigrar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMigrar);
            this.MaximizeBox = false;
            this.Name = "formMigracion";
            this.ShowIcon = false;
            this.Text = "Migrar Base de Datos...";
            this.Load += new System.EventHandler(this.formMigracion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMigrar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMigrar;
        private System.Windows.Forms.ComboBox cbOrigen;
        private System.Windows.Forms.ComboBox cbDestino;
    }
}