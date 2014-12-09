namespace StatisticalAnalyzer
{
    partial class NewEditGroupData
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
            this.lblInterval = new System.Windows.Forms.Label();
            this.txtLowerBound = new System.Windows.Forms.TextBox();
            this.txtUpperBound = new System.Windows.Forms.TextBox();
            this.lblSeparator = new System.Windows.Forms.Label();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.txtFrequency = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(18, 15);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(45, 13);
            this.lblInterval.TabIndex = 1;
            this.lblInterval.Text = "Interval:";
            // 
            // txtLowerBound
            // 
            this.txtLowerBound.Location = new System.Drawing.Point(69, 12);
            this.txtLowerBound.Name = "txtLowerBound";
            this.txtLowerBound.Size = new System.Drawing.Size(120, 20);
            this.txtLowerBound.TabIndex = 1;
            this.txtLowerBound.TextChanged += new System.EventHandler(this.txtLowerBound_TextChanged);
            // 
            // txtUpperBound
            // 
            this.txtUpperBound.Location = new System.Drawing.Point(211, 12);
            this.txtUpperBound.Name = "txtUpperBound";
            this.txtUpperBound.Size = new System.Drawing.Size(120, 20);
            this.txtUpperBound.TabIndex = 2;
            // 
            // lblSeparator
            // 
            this.lblSeparator.AutoSize = true;
            this.lblSeparator.Location = new System.Drawing.Point(195, 15);
            this.lblSeparator.Name = "lblSeparator";
            this.lblSeparator.Size = new System.Drawing.Size(10, 13);
            this.lblSeparator.TabIndex = 5;
            this.lblSeparator.Text = "-";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(18, 44);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(60, 13);
            this.lblFrequency.TabIndex = 6;
            this.lblFrequency.Text = "Frequency:";
            // 
            // txtFrequency
            // 
            this.txtFrequency.Location = new System.Drawing.Point(84, 41);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.Size = new System.Drawing.Size(247, 20);
            this.txtFrequency.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(275, 67);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(56, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(216, 67);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(53, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // NewEditGroupData
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(346, 96);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtFrequency);
            this.Controls.Add(this.lblFrequency);
            this.Controls.Add(this.lblSeparator);
            this.Controls.Add(this.txtUpperBound);
            this.Controls.Add(this.txtLowerBound);
            this.Controls.Add(this.lblInterval);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "NewEditGroupData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewEditGroupData";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.TextBox txtLowerBound;
        private System.Windows.Forms.TextBox txtUpperBound;
        private System.Windows.Forms.Label lblSeparator;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.TextBox txtFrequency;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAdd;
    }
}