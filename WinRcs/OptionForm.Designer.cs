namespace WinRcs
{
    partial class OptionForm
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
            this.btnRCSPath = new System.Windows.Forms.Button();
            this.txtRCSPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDiffPath = new System.Windows.Forms.Button();
            this.txtDiffPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbFont = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnRCSPath
            // 
            this.btnRCSPath.Location = new System.Drawing.Point(341, 29);
            this.btnRCSPath.Name = "btnRCSPath";
            this.btnRCSPath.Size = new System.Drawing.Size(30, 18);
            this.btnRCSPath.TabIndex = 9;
            this.btnRCSPath.Text = "...";
            this.btnRCSPath.UseVisualStyleBackColor = true;
            this.btnRCSPath.Click += new System.EventHandler(this.btnRCSPath_Click);
            // 
            // txtRCSPath
            // 
            this.txtRCSPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRCSPath.Location = new System.Drawing.Point(22, 30);
            this.txtRCSPath.Name = "txtRCSPath";
            this.txtRCSPath.Size = new System.Drawing.Size(313, 19);
            this.txtRCSPath.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "RCSのフォルダ:";
            // 
            // btnDiffPath
            // 
            this.btnDiffPath.Location = new System.Drawing.Point(341, 82);
            this.btnDiffPath.Name = "btnDiffPath";
            this.btnDiffPath.Size = new System.Drawing.Size(30, 18);
            this.btnDiffPath.TabIndex = 12;
            this.btnDiffPath.Text = "...";
            this.btnDiffPath.UseVisualStyleBackColor = true;
            this.btnDiffPath.Click += new System.EventHandler(this.btnDiffPath_Click);
            // 
            // txtDiffPath
            // 
            this.txtDiffPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDiffPath.Location = new System.Drawing.Point(22, 82);
            this.txtDiffPath.Name = "txtDiffPath";
            this.txtDiffPath.Size = new System.Drawing.Size(313, 19);
            this.txtDiffPath.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "差分ビューアのパス:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(288, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(207, 152);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 28);
            this.btnSet.TabIndex = 13;
            this.btnSet.Text = "設定";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "フォント:";
            // 
            // cmbFont
            // 
            this.cmbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFont.FormattingEnabled = true;
            this.cmbFont.Location = new System.Drawing.Point(56, 126);
            this.cmbFont.Name = "cmbFont";
            this.cmbFont.Size = new System.Drawing.Size(278, 20);
            this.cmbFont.TabIndex = 16;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 192);
            this.Controls.Add(this.cmbFont);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnDiffPath);
            this.Controls.Add(this.txtDiffPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRCSPath);
            this.Controls.Add(this.txtRCSPath);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionForm";
            this.Text = "設定";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRCSPath;
        private System.Windows.Forms.TextBox txtRCSPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDiffPath;
        private System.Windows.Forms.TextBox txtDiffPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbFont;
    }
}