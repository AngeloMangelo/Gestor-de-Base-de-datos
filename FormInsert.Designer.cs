namespace BaseDeDatosSQL
{
    partial class FormInsert
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
            this.dgvDataSource = new System.Windows.Forms.DataGridView();
            this.btnInsert = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDataSource
            // 
            this.dgvDataSource.AllowUserToDeleteRows = false;
            this.dgvDataSource.AllowUserToOrderColumns = true;
            this.dgvDataSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataSource.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDataSource.Location = new System.Drawing.Point(12, 48);
            this.dgvDataSource.MultiSelect = false;
            this.dgvDataSource.Name = "dgvDataSource";
            this.dgvDataSource.Size = new System.Drawing.Size(765, 293);
            this.dgvDataSource.TabIndex = 0;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(12, 390);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(133, 48);
            this.btnInsert.TabIndex = 1;
            this.btnInsert.Text = "Insertar nuevo registro...";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // FormInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.dgvDataSource);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInsert";
            this.ShowIcon = false;
            this.Text = "Insertar Registro";
            this.Load += new System.EventHandler(this.FormInsert_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDataSource;
        private System.Windows.Forms.Button btnInsert;
    }
}