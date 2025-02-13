namespace BaseDeDatosSQL
{
    partial class FormSelect
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
            this.rtbQuery = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEjecutarQuery = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDataSource
            // 
            this.dgvDataSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataSource.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDataSource.Location = new System.Drawing.Point(12, 116);
            this.dgvDataSource.Name = "dgvDataSource";
            this.dgvDataSource.ReadOnly = true;
            this.dgvDataSource.Size = new System.Drawing.Size(765, 293);
            this.dgvDataSource.TabIndex = 1;
            // 
            // rtbQuery
            // 
            this.rtbQuery.Location = new System.Drawing.Point(70, 12);
            this.rtbQuery.Name = "rtbQuery";
            this.rtbQuery.Size = new System.Drawing.Size(332, 96);
            this.rtbQuery.TabIndex = 2;
            this.rtbQuery.Text = " ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Query:";
            // 
            // btnEjecutarQuery
            // 
            this.btnEjecutarQuery.Location = new System.Drawing.Point(634, 69);
            this.btnEjecutarQuery.Name = "btnEjecutarQuery";
            this.btnEjecutarQuery.Size = new System.Drawing.Size(99, 39);
            this.btnEjecutarQuery.TabIndex = 4;
            this.btnEjecutarQuery.Text = "Ejecutar query";
            this.btnEjecutarQuery.UseVisualStyleBackColor = true;
            this.btnEjecutarQuery.Click += new System.EventHandler(this.btnEjecutarQuery_Click);
            // 
            // FormSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEjecutarQuery);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbQuery);
            this.Controls.Add(this.dgvDataSource);
            this.Name = "FormSelect";
            this.Text = "Select...";
            this.Load += new System.EventHandler(this.FormSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDataSource;
        private System.Windows.Forms.RichTextBox rtbQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEjecutarQuery;
    }
}