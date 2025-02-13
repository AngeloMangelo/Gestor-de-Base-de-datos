namespace BaseDeDatosSQL
{
    partial class FormVerRegistros
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDataSource
            // 
            this.dgvDataSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataSource.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDataSource.Location = new System.Drawing.Point(12, 42);
            this.dgvDataSource.Name = "dgvDataSource";
            this.dgvDataSource.ReadOnly = true;
            this.dgvDataSource.Size = new System.Drawing.Size(765, 372);
            this.dgvDataSource.TabIndex = 1;
            // 
            // FormVerRegistros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvDataSource);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVerRegistros";
            this.ShowIcon = false;
            this.Text = "Ver Registros";
            this.Load += new System.EventHandler(this.FormVerRegistros_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDataSource;
    }
}