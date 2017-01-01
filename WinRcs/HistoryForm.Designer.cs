namespace WinRcs
{
    /// <summary>
    /// 履歴用フォーム
    /// </summary>
    partial class HistoryForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.treeHistory = new AdvancedDataGridView.TreeGridView();
            this.colRevision = new AdvancedDataGridView.TreeGridColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cntxtMenuHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemCheckOut = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.treeHistory)).BeginInit();
            this.cntxtMenuHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeHistory
            // 
            this.treeHistory.AllowUserToAddRows = false;
            this.treeHistory.AllowUserToDeleteRows = false;
            this.treeHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRevision,
            this.colDate,
            this.colAuthor,
            this.colState,
            this.colComment});
            this.treeHistory.ContextMenuStrip = this.cntxtMenuHistory;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeHistory.DefaultCellStyle = dataGridViewCellStyle1;
            this.treeHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.treeHistory.ImageList = null;
            this.treeHistory.Location = new System.Drawing.Point(8, 10);
            this.treeHistory.Name = "treeHistory";
            this.treeHistory.RowHeadersVisible = false;
            this.treeHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.treeHistory.ShowLines = false;
            this.treeHistory.Size = new System.Drawing.Size(591, 216);
            this.treeHistory.TabIndex = 0;
            this.treeHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.treeHistory_CellContentClick);
            // 
            // colRevision
            // 
            this.colRevision.DefaultNodeImage = null;
            this.colRevision.FillWeight = 105.8824F;
            this.colRevision.HeaderText = "Revision";
            this.colRevision.Name = "colRevision";
            this.colRevision.ReadOnly = true;
            this.colRevision.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRevision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRevision.Width = 150;
            // 
            // colDate
            // 
            this.colDate.FillWeight = 97.05882F;
            this.colDate.HeaderText = "日付";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDate.Width = 137;
            // 
            // colAuthor
            // 
            this.colAuthor.HeaderText = "作者";
            this.colAuthor.Name = "colAuthor";
            this.colAuthor.ReadOnly = true;
            this.colAuthor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colState
            // 
            this.colState.HeaderText = "State";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            this.colState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colState.Width = 60;
            // 
            // colComment
            // 
            this.colComment.FillWeight = 97.05882F;
            this.colComment.HeaderText = "コメント";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            this.colComment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colComment.Width = 138;
            // 
            // cntxtMenuHistory
            // 
            this.cntxtMenuHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemDiff,
            this.mnuItemCheckOut});
            this.cntxtMenuHistory.Name = "cntxtMenuHistory";
            this.cntxtMenuHistory.Size = new System.Drawing.Size(269, 70);
            // 
            // mnuItemDiff
            // 
            this.mnuItemDiff.Name = "mnuItemDiff";
            this.mnuItemDiff.Size = new System.Drawing.Size(268, 22);
            this.mnuItemDiff.Text = "比較";
            this.mnuItemDiff.Click += new System.EventHandler(this.mnuItemDiff_Click);
            // 
            // mnuItemCheckOut
            // 
            this.mnuItemCheckOut.Name = "mnuItemCheckOut";
            this.mnuItemCheckOut.Size = new System.Drawing.Size(268, 22);
            this.mnuItemCheckOut.Text = "指定のリビジョンをチェックアウト";
            this.mnuItemCheckOut.Click += new System.EventHandler(this.mnuItemCheckOut_Click);
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 238);
            this.Controls.Add(this.treeHistory);
            this.Name = "HistoryForm";
            this.Text = "履歴";
            ((System.ComponentModel.ISupportInitialize)(this.treeHistory)).EndInit();
            this.cntxtMenuHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AdvancedDataGridView.TreeGridView treeHistory;
        private AdvancedDataGridView.TreeGridColumn colRevision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.ContextMenuStrip cntxtMenuHistory;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDiff;
        private System.Windows.Forms.ToolStripMenuItem mnuItemCheckOut;

    }
}