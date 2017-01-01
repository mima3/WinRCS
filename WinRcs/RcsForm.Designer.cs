namespace WinRcs
{
    partial class RcsForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            this.dictWkSp.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RcsForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.mnuItemMainTool = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemMainOption = new System.Windows.Forms.ToolStripMenuItem();
            this.cntxtMenuFileList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemSendTo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemDiff = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuItemCheckIn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCheckOut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRevert = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListFileIcon = new System.Windows.Forms.ImageList(this.components);
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.splitMessage = new System.Windows.Forms.SplitContainer();
            this.splitWorkSpace = new System.Windows.Forms.SplitContainer();
            this.btnWkSpDel = new System.Windows.Forms.Button();
            this.btnWkSpAdd = new System.Windows.Forms.Button();
            this.lvWorkSpace = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colPath = new System.Windows.Forms.ColumnHeader();
            this.lblWorkSpace = new System.Windows.Forms.Label();
            this.lvFile = new System.Windows.Forms.ListView();
            this.colFileName = new System.Windows.Forms.ColumnHeader();
            this.colHeadRevision = new System.Windows.Forms.ColumnHeader();
            this.colLockRevision = new System.Windows.Forms.ColumnHeader();
            this.lblFile = new System.Windows.Forms.Label();
            this.richMessage = new System.Windows.Forms.RichTextBox();
            this.colBranch = new System.Windows.Forms.ColumnHeader();
            this.menuMain.SuspendLayout();
            this.cntxtMenuFileList.SuspendLayout();
            this.splitMessage.Panel1.SuspendLayout();
            this.splitMessage.Panel2.SuspendLayout();
            this.splitMessage.SuspendLayout();
            this.splitWorkSpace.Panel1.SuspendLayout();
            this.splitWorkSpace.Panel2.SuspendLayout();
            this.splitWorkSpace.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemMainTool});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(420, 26);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // mnuItemMainTool
            // 
            this.mnuItemMainTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemMainOption});
            this.mnuItemMainTool.Name = "mnuItemMainTool";
            this.mnuItemMainTool.Size = new System.Drawing.Size(56, 22);
            this.mnuItemMainTool.Text = "ツール";
            // 
            // mnuItemMainOption
            // 
            this.mnuItemMainOption.Name = "mnuItemMainOption";
            this.mnuItemMainOption.Size = new System.Drawing.Size(136, 22);
            this.mnuItemMainOption.Text = "オプション";
            this.mnuItemMainOption.Click += new System.EventHandler(this.mnuItemMainOption_Click);
            // 
            // cntxtMenuFileList
            // 
            this.cntxtMenuFileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSendTo,
            this.mnuItemDiff,
            this.mnuItemHistory,
            this.toolStripSeparator1,
            this.toolItemRename,
            this.toolItemDelete,
            this.toolStripSeparator2,
            this.mnuItemCheckIn,
            this.menuItemCheckOut,
            this.menuItemRevert});
            this.cntxtMenuFileList.Name = "cntxtMenuFileList";
            this.cntxtMenuFileList.Size = new System.Drawing.Size(161, 192);
            this.cntxtMenuFileList.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtMenuFileList_Opening);
            // 
            // menuItemSendTo
            // 
            this.menuItemSendTo.Name = "menuItemSendTo";
            this.menuItemSendTo.Size = new System.Drawing.Size(160, 22);
            this.menuItemSendTo.Text = "送る";
            // 
            // mnuItemDiff
            // 
            this.mnuItemDiff.Name = "mnuItemDiff";
            this.mnuItemDiff.Size = new System.Drawing.Size(160, 22);
            this.mnuItemDiff.Text = "比較";
            this.mnuItemDiff.Click += new System.EventHandler(this.mnuItemDiff_Click);
            // 
            // mnuItemHistory
            // 
            this.mnuItemHistory.Name = "mnuItemHistory";
            this.mnuItemHistory.Size = new System.Drawing.Size(160, 22);
            this.mnuItemHistory.Text = "履歴";
            this.mnuItemHistory.Click += new System.EventHandler(this.mnuItemHistory_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // toolItemRename
            // 
            this.toolItemRename.Name = "toolItemRename";
            this.toolItemRename.Size = new System.Drawing.Size(160, 22);
            this.toolItemRename.Text = "名前の変更";
            this.toolItemRename.Click += new System.EventHandler(this.toolItemRename_Click);
            // 
            // toolItemDelete
            // 
            this.toolItemDelete.Name = "toolItemDelete";
            this.toolItemDelete.Size = new System.Drawing.Size(160, 22);
            this.toolItemDelete.Text = "削除";
            this.toolItemDelete.Click += new System.EventHandler(this.toolItemDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuItemCheckIn
            // 
            this.mnuItemCheckIn.Name = "mnuItemCheckIn";
            this.mnuItemCheckIn.Size = new System.Drawing.Size(160, 22);
            this.mnuItemCheckIn.Text = "チェックイン";
            this.mnuItemCheckIn.Click += new System.EventHandler(this.mnuItemCheckIn_Click);
            // 
            // menuItemCheckOut
            // 
            this.menuItemCheckOut.Name = "menuItemCheckOut";
            this.menuItemCheckOut.Size = new System.Drawing.Size(160, 22);
            this.menuItemCheckOut.Text = "チェックアウト";
            this.menuItemCheckOut.Click += new System.EventHandler(this.menuItemCheckOut_Click);
            // 
            // menuItemRevert
            // 
            this.menuItemRevert.Name = "menuItemRevert";
            this.menuItemRevert.Size = new System.Drawing.Size(160, 22);
            this.menuItemRevert.Text = "元に戻す";
            this.menuItemRevert.Click += new System.EventHandler(this.menuItemRevert_Click);
            // 
            // imageListFileIcon
            // 
            this.imageListFileIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFileIcon.ImageStream")));
            this.imageListFileIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListFileIcon.Images.SetKeyName(0, "WorkFile.ico");
            this.imageListFileIcon.Images.SetKeyName(1, "RcsFile.ico");
            this.imageListFileIcon.Images.SetKeyName(2, "CheckOut.ico");
            this.imageListFileIcon.Images.SetKeyName(3, "NoWorkFile.ico");
            // 
            // splitMessage
            // 
            this.splitMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMessage.Location = new System.Drawing.Point(0, 26);
            this.splitMessage.Name = "splitMessage";
            this.splitMessage.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMessage.Panel1
            // 
            this.splitMessage.Panel1.Controls.Add(this.splitWorkSpace);
            // 
            // splitMessage.Panel2
            // 
            this.splitMessage.Panel2.Controls.Add(this.richMessage);
            this.splitMessage.Size = new System.Drawing.Size(420, 289);
            this.splitMessage.SplitterDistance = 184;
            this.splitMessage.TabIndex = 1;
            // 
            // splitWorkSpace
            // 
            this.splitWorkSpace.BackColor = System.Drawing.SystemColors.Control;
            this.splitWorkSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitWorkSpace.Location = new System.Drawing.Point(0, 0);
            this.splitWorkSpace.Name = "splitWorkSpace";
            // 
            // splitWorkSpace.Panel1
            // 
            this.splitWorkSpace.Panel1.Controls.Add(this.btnWkSpDel);
            this.splitWorkSpace.Panel1.Controls.Add(this.btnWkSpAdd);
            this.splitWorkSpace.Panel1.Controls.Add(this.lvWorkSpace);
            this.splitWorkSpace.Panel1.Controls.Add(this.lblWorkSpace);
            // 
            // splitWorkSpace.Panel2
            // 
            this.splitWorkSpace.Panel2.Controls.Add(this.lvFile);
            this.splitWorkSpace.Panel2.Controls.Add(this.lblFile);
            this.splitWorkSpace.Size = new System.Drawing.Size(420, 184);
            this.splitWorkSpace.SplitterDistance = 138;
            this.splitWorkSpace.TabIndex = 2;
            // 
            // btnWkSpDel
            // 
            this.btnWkSpDel.Location = new System.Drawing.Point(34, 15);
            this.btnWkSpDel.Name = "btnWkSpDel";
            this.btnWkSpDel.Size = new System.Drawing.Size(23, 20);
            this.btnWkSpDel.TabIndex = 3;
            this.btnWkSpDel.Text = "-";
            this.btnWkSpDel.UseVisualStyleBackColor = true;
            this.btnWkSpDel.Click += new System.EventHandler(this.btnWkSpDel_Click);
            // 
            // btnWkSpAdd
            // 
            this.btnWkSpAdd.Location = new System.Drawing.Point(5, 15);
            this.btnWkSpAdd.Name = "btnWkSpAdd";
            this.btnWkSpAdd.Size = new System.Drawing.Size(23, 20);
            this.btnWkSpAdd.TabIndex = 2;
            this.btnWkSpAdd.Text = "+";
            this.btnWkSpAdd.UseVisualStyleBackColor = true;
            this.btnWkSpAdd.Click += new System.EventHandler(this.btnWkSpAdd_Click);
            // 
            // lvWorkSpace
            // 
            this.lvWorkSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvWorkSpace.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colPath});
            this.lvWorkSpace.Location = new System.Drawing.Point(3, 41);
            this.lvWorkSpace.Name = "lvWorkSpace";
            this.lvWorkSpace.ShowItemToolTips = true;
            this.lvWorkSpace.Size = new System.Drawing.Size(132, 140);
            this.lvWorkSpace.TabIndex = 1;
            this.lvWorkSpace.UseCompatibleStateImageBehavior = false;
            this.lvWorkSpace.View = System.Windows.Forms.View.Details;
            this.lvWorkSpace.SelectedIndexChanged += new System.EventHandler(this.lvWorkSpace_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Text = "名前";
            // 
            // colPath
            // 
            this.colPath.Text = "パス";
            // 
            // lblWorkSpace
            // 
            this.lblWorkSpace.AutoSize = true;
            this.lblWorkSpace.Location = new System.Drawing.Point(3, 0);
            this.lblWorkSpace.Name = "lblWorkSpace";
            this.lblWorkSpace.Size = new System.Drawing.Size(70, 12);
            this.lblWorkSpace.TabIndex = 0;
            this.lblWorkSpace.Text = "ワークスペース";
            // 
            // lvFile
            // 
            this.lvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName,
            this.colHeadRevision,
            this.colBranch,
            this.colLockRevision});
            this.lvFile.ContextMenuStrip = this.cntxtMenuFileList;
            this.lvFile.Location = new System.Drawing.Point(3, 41);
            this.lvFile.Name = "lvFile";
            this.lvFile.ShowItemToolTips = true;
            this.lvFile.Size = new System.Drawing.Size(272, 140);
            this.lvFile.SmallImageList = this.imageListFileIcon;
            this.lvFile.TabIndex = 2;
            this.lvFile.UseCompatibleStateImageBehavior = false;
            this.lvFile.View = System.Windows.Forms.View.Details;
            this.lvFile.DoubleClick += new System.EventHandler(this.lvFile_DoubleClick);
            // 
            // colFileName
            // 
            this.colFileName.Text = "ファイル名";
            // 
            // colHeadRevision
            // 
            this.colHeadRevision.Text = "HEAD";
            // 
            // colLockRevision
            // 
            this.colLockRevision.Text = "ロック中";
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(3, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(39, 12);
            this.lblFile.TabIndex = 1;
            this.lblFile.Text = "ファイル";
            // 
            // richMessage
            // 
            this.richMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richMessage.Location = new System.Drawing.Point(3, 3);
            this.richMessage.Name = "richMessage";
            this.richMessage.ReadOnly = true;
            this.richMessage.Size = new System.Drawing.Size(412, 95);
            this.richMessage.TabIndex = 0;
            this.richMessage.Text = "";
            // 
            // colBranch
            // 
            this.colBranch.Text = "ブランチ";
            // 
            // RcsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 315);
            this.Controls.Add(this.splitMessage);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "RcsForm";
            this.Text = "WinRCS";
            this.Load += new System.EventHandler(this.RcsForm_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.cntxtMenuFileList.ResumeLayout(false);
            this.splitMessage.Panel1.ResumeLayout(false);
            this.splitMessage.Panel2.ResumeLayout(false);
            this.splitMessage.ResumeLayout(false);
            this.splitWorkSpace.Panel1.ResumeLayout(false);
            this.splitWorkSpace.Panel1.PerformLayout();
            this.splitWorkSpace.Panel2.ResumeLayout(false);
            this.splitWorkSpace.Panel2.PerformLayout();
            this.splitWorkSpace.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.ContextMenuStrip cntxtMenuFileList;
        private System.Windows.Forms.ToolStripMenuItem mnuItemCheckIn;
        private System.Windows.Forms.ToolStripMenuItem menuItemCheckOut;
        private System.Windows.Forms.ImageList imageListFileIcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemRevert;
        private System.Windows.Forms.SplitContainer splitMessage;
        private System.Windows.Forms.SplitContainer splitWorkSpace;
        private System.Windows.Forms.Button btnWkSpDel;
        private System.Windows.Forms.Button btnWkSpAdd;
        private System.Windows.Forms.ListView lvWorkSpace;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.Label lblWorkSpace;
        private System.Windows.Forms.ListView lvFile;
        private System.Windows.Forms.ColumnHeader colFileName;
        private System.Windows.Forms.ColumnHeader colHeadRevision;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.RichTextBox richMessage;
        private System.Windows.Forms.ToolStripMenuItem menuItemSendTo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuItemHistory;
        private System.Windows.Forms.ColumnHeader colLockRevision;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDiff;
        private System.Windows.Forms.ToolStripMenuItem mnuItemMainTool;
        private System.Windows.Forms.ToolStripMenuItem mnuItemMainOption;
        private System.Windows.Forms.ToolStripMenuItem toolItemRename;
        private System.Windows.Forms.ToolStripMenuItem toolItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ColumnHeader colBranch;
    }
}

