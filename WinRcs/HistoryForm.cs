using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinRcs
{
    /// <summary>
    /// ファイルのチェックイン履歴用フォーム
    /// </summary>
    public partial class HistoryForm : Form
    {
        FileInfo fileInfo;
        Dictionary<string, RevisionInfo> dictRevisions;
        AdvancedDataGridView.TreeGridNode rootNode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RcsFileChangedHandler(HistoryForm sender, EventArgs e);

        /// <summary>
        /// RcsFileの更新イベント
        /// </summary>
        public event RcsFileChangedHandler RcsFileChangedEvent;

        private void BuildTreeRevition(AdvancedDataGridView.TreeGridNode parent, string branch)
        {
            string revision;
            int minor = 1;
            revision = branch + "." + minor.ToString();
            while (this.dictRevisions.ContainsKey(revision))
            {
                RevisionInfo info = this.dictRevisions[revision];
                AdvancedDataGridView.TreeGridNode node = parent.Nodes.Add(revision, info.UpdateTime.ToString(),info.Author,info.State, info.Comment);
                
                foreach (string b in info.Branches)
                {
                    AdvancedDataGridView.TreeGridNode branchNode = node.Nodes.Add(b);
                    BuildTreeRevition(branchNode, b);
                }
                ++minor;
                revision = branch + "." + minor.ToString();
            }

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileInfo">FileInfo.</param>
        public HistoryForm(WinRcs.FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;

            InitializeComponent();
            this.Text = System.IO.Path.GetFileName( fileInfo.WorkPath ) + " の履歴";
            this.dictRevisions = this.fileInfo.RevisionDictonary;
            this.rootNode = this.treeHistory.Nodes.Add("1", "", "");

            this.BuildTreeRevition(this.rootNode, "1");

            string fntName = Properties.Settings.Default.Font;
            if (!string.IsNullOrEmpty(fntName))
            {
                this.Font = new Font(fntName, Properties.Settings.Default.FontSize);
            }

        }

        private void treeHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 比較　メニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemDiff_Click(object sender, EventArgs e)
        {
            if (this.treeHistory.SelectedRows.Count != 2)
            {
                MessageBox.Show("差分を比較するリビジョンを２つ選択してください。");
                return;
            }
            string left = this.treeHistory.SelectedRows[0].Cells[0].Value as string ;
            string right = this.treeHistory.SelectedRows[1].Cells[0].Value as string ;
            if (!this.dictRevisions.ContainsKey(left) || !this.dictRevisions.ContainsKey(right))
            {
                MessageBox.Show("差分を比較するリビジョンを２つ選択してください。");
                return;
            }
            RcsCommandResult ret = Rcs.Instance.DiffRevision(this.fileInfo.WorkPath, left, right);
            if (ret.Result != RcsCommandResultCode.Ok)
            {
                MessageBox.Show(ret.Message);
                return;
            }

        }

        /// <summary>
        /// 指定のバージョンのチェックアウト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemCheckOut_Click(object sender, EventArgs e)
        {
            if (this.treeHistory.SelectedRows.Count != 1)
            {
                MessageBox.Show("チェックアウトするリビジョンを１つ選択してください。");
                return;
            }
            string sel = this.treeHistory.SelectedRows[0].Cells[0].Value as string;
            if (!this.dictRevisions.ContainsKey(sel))
            {
                MessageBox.Show("チェックアウトするリビジョンを１つ選択してください。");
                return;
            }
            RcsCommandResult ret = Rcs.Instance.CheckOut(this.fileInfo.WorkPath, sel);
            if (ret.Result != RcsCommandResultCode.Ok)
            {
                MessageBox.Show(ret.Message);
                return;
            }
            RcsFileChangedEvent(this, null);
            MessageBox.Show(sel + "をチェックアウトしました.");
        }


    }
}
