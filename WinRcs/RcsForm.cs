using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

[assembly: CLSCompliant(false)]
namespace WinRcs
{
    /// <summary>
    /// WinRCSのメインウィンドウ
    /// </summary>
    public partial class RcsForm : Form
    {
        private WorkSpaceDictionary dictWkSp = null;
        private SendToMenu sendToMenu;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
                                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RcsForm()
        {
            Application.ApplicationExit += new EventHandler(Application_AppicationExit);
            InitializeComponent();

            Rcs rcs = Rcs.Instance;
            Console.WriteLine(rcs.Version());
            this.dictWkSp = new WorkSpaceDictionary();
            this.toolTipMain.SetToolTip(this.btnWkSpAdd, Properties.Resources.BtnWkSpAddToolTip);
            this.toolTipMain.SetToolTip(this.btnWkSpDel, Properties.Resources.BtnWkSpDelToolTip);
            this.lvWorkSpace.MultiSelect = false;
            this.RefreshWorkSpaceList();

            String fntName = Properties.Settings.Default.Font;
            if (!string.IsNullOrEmpty(fntName))
            {
                this.Font = new Font(fntName, Properties.Settings.Default.FontSize);
            }
            this.sendToMenu = new SendToMenu(this.menuItemSendTo, new SendToMenu.ClickSendToMenu(this.sendToMenu_ClickSendToMenu));
        }

        /// <summary>
        /// アプリケーションの終了処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_AppicationExit(object sender, EventArgs e)
        {
            Application.ApplicationExit -= new EventHandler(Application_AppicationExit);
            this.sendToMenu = null;

            // 終了処理
            Rcs.Instance.Dispose();
        }

        /// <summary>
        /// ワークスペースの追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWkSpAdd_Click(object sender, EventArgs e)
        {
            AddWorkSpace();
        }

        /// <summary>
        /// ワークスペースの追加処理
        /// </summary>
        private void AddWorkSpace()
        {
            AddWorkSpaceForm dlg = new AddWorkSpaceForm();
            DialogResult dlgRet = dlg.ShowDialog(this);
            if (dlgRet == DialogResult.OK)
            {
                if (dictWkSp.AddWorkSpace(dlg.WorkSpaceName, dlg.WorkSpacePath))
                {
                    RefreshWorkSpaceList();
                }
            }
        }

        /// <summary>
        /// ワークスペースリストビューの更新
        /// </summary>
        private void RefreshWorkSpaceList()
        {
            this.lvWorkSpace.Items.Clear();

            foreach (string path in this.dictWkSp.Paths)
            {
                WorkSpace wk = this.dictWkSp.GetWorkSpace(path);
                string[] items = { wk.Name, wk.Path };
                this.lvWorkSpace.Items.Add(new ListViewItem(items));
            }
        }

        /// <summary>
        /// 現在選択中のワークスペースのパスを取得する
        /// </summary>
        /// <returns>ワークスペースのパス</returns>
        private string SelectedWorkSpacePath()
        {
            foreach (ListViewItem item in this.lvWorkSpace.SelectedItems)
            {
                //Console.WriteLine(item.SubItems[1].Text);
                return item.SubItems[1].Text;
            }
            return "";
        }

        /// <summary>
        /// ファイル情報の更新
        /// </summary>
        private void UpdateFileInfoList()
        {
            string path = SelectedWorkSpacePath();
            if (String.IsNullOrEmpty(path))
            {
                return;
            }
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);
            if (wksp == null)
            {
                logger.Error("RcsForm::UpdateFileInfoList wksp is null. path:" + path);
                return;
            }

            wksp.UpdateFileInfoList();
            this.lvFile.Items.Clear();
            foreach (KeyValuePair<string, FileInfo> kvp in wksp.FileInfoDict)
            {
                string locklist = "";
                if (kvp.Value.LockList != null)
                {
                    foreach (LockInfo l in kvp.Value.LockList)
                    {
                        locklist += l.Revision + " ";
                    }
                }

                string[] items = { kvp.Value.Name, kvp.Value.Head, kvp.Value.Branch, locklist };
                ListViewItem item = this.lvFile.Items.Add(new ListViewItem(items));
                if (!kvp.Value.IsExistRcs)
                {
                    item.ImageIndex = 0;
                }
                else
                {
                    if (!kvp.Value.IsExistWorkFile)
                    {
                        item.ImageIndex = 3;
                    }
                    else if (kvp.Value.IsCheckOutByCurrentUser())
                    {
                        item.ImageIndex = 2;
                    }
                    else
                    {
                        item.ImageIndex = 1;
                    }
                }
            }
        }
        /// <summary>
        /// ワークスペースのアイテムの変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvWorkSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFileInfoList();
        }

        private void btnWkSpDel_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            if (String.IsNullOrEmpty( path ) )
            {
                logger.Error("RcsForm::btnWkSpDel_Click path is null or blank.");
                return;
            }
            string confirm = Properties.Resources.ConfirmDelWkSp + "\n" + path;
            DialogResult ret = MessageBox.Show(confirm, this.Text,
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question,
                                               MessageBoxDefaultButton.Button2);
            if (ret == DialogResult.Yes)
            {
                this.dictWkSp.RemoveWorkSpace(path);
                this.RefreshWorkSpaceList();
            }
        }

        /// <summary>
        /// メニュー・「チェックイン」
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemCheckIn_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);
            richMessage.Text = "";
            string comment = "";
            string state = "exp";

            CommentForm dlg = new CommentForm();
            DialogResult dlgRet = dlg.ShowDialog(this);
            if (dlgRet == DialogResult.OK)
            {
                comment = dlg.Comment;
                state = dlg.State;
            }
            else
            {
                return;
            }

            Regex regN = new Regex(@"\n", RegexOptions.IgnoreCase);
            Regex regC = new Regex(@";", RegexOptions.IgnoreCase);

            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                //Console.WriteLine(item.SubItems[1].Text);
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];

                RcsCommandResult ret = fi.CheckIn(comment, state);

                if (ret.Result != RcsCommandResultCode.Ok)
                {
                    MessageBox.Show(ret.Message);
                    break;
                }
                richMessage.Text = richMessage.Text + ret.Message;

                // デフォルトのブランチ変更
                string[] lines = regN.Split(ret.Message);
                if (lines.Length > 2)
                {
                    string line = lines[1];
                    if (line.Length > 14 && line.Substring(0, 14) == "new revision: ")
                    {
                        string[] revs = regC.Split(line.Substring(14));
                        ret = fi.ChangeBranch(revs[0]);

                        if (ret.Result != RcsCommandResultCode.Ok)
                        {
                            MessageBox.Show(ret.Message);
                            break;
                        }
                        richMessage.Text = richMessage.Text + ret.Message;
                    }
                }

            }
            UpdateFileInfoList();
        }

        /// <summary>
        /// [チェックアウト]メニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemCheckOut_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);
            if (wksp == null)
            {

            }
            richMessage.Text = "";

            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                //Console.WriteLine(item.SubItems[1].Text);
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];
                RcsCommandResult ret = fi.CheckOut();

                if (ret.Result != RcsCommandResultCode.Ok)
                {
                    MessageBox.Show(ret.Message);
                    break;
                }
                richMessage.Text = richMessage.Text + ret.Message;
            }
            UpdateFileInfoList();

        }

        /// <summary>
        /// [元に戻す]メニュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemRevert_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);
            richMessage.Text = "";
            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];
                string msg;
                msg = string.Format(Properties.Resources.RevertMessage, fi.WorkPath);
                DialogResult dlgRet = MessageBox.Show(msg, "WinRcs", MessageBoxButtons.OKCancel);
                if (dlgRet == DialogResult.OK)
                {
                    RcsCommandResult ret = fi.Revert();
                    if (ret.Result != RcsCommandResultCode.Ok)
                    {
                        MessageBox.Show(ret.Message);
                        break;
                    }
                    richMessage.Text = richMessage.Text + ret.Message;
                }
            }
            UpdateFileInfoList();
        }

        /// <summary>
        /// ファイルをダブルクリックする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvFile_DoubleClick(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);

            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                //Console.WriteLine(item.SubItems[1].Text);
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = fi.WorkPath;
                p.StartInfo.WorkingDirectory = path;
                p.Start();
                
            }
        }

        /// <summary>
        /// 送るメニューで選択された場合のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendToMenu_ClickSendToMenu(object sender, SendToMenuEventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);

            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                //Console.WriteLine(item.SubItems[1].Text);
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = e.ApplicationPath;
                p.StartInfo.Arguments = fi.WorkPath;
                p.StartInfo.WorkingDirectory = path;
                p.Start();

            }
        }

        /// <summary>
        /// 履歴コマンド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuItemHistory_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);

            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                //Console.WriteLine(item.SubItems[1].Text);
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];
                if (fi.IsExistRcs)
                {
                    HistoryForm dlg = new HistoryForm(fi);
                    dlg.RcsFileChangedEvent += new HistoryForm.RcsFileChangedHandler(this.HistoryFormRcsFileChangedEvent);
                    dlg.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// HistoryFormでRcsFileの情報が書き変わった場合。
        /// </summary>
        /// <param name="histDlg"></param>
        /// <param name="e"></param>
        private void HistoryFormRcsFileChangedEvent(HistoryForm histDlg, EventArgs e)
        {
            UpdateFileInfoList();
        }

        private void mnuItemDiff_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);

            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                //Console.WriteLine(item.SubItems[1].Text);
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];
                string rev = fi.Branch;
                if (String.IsNullOrEmpty(rev))
                {
                    rev = fi.Head;
                }
                RcsCommandResult ret = Rcs.Instance.DiffFile(fi.WorkPath, rev);
                if (ret.Result != RcsCommandResultCode.Ok)
                {
                    MessageBox.Show(ret.Message);
                    return;
                }
                break;
            }

        }

        private void RcsForm_Load(object sender, EventArgs e)
        {

        }

        private void mnuItemMainOption_Click(object sender, EventArgs e)
        {
            OptionForm dlg = new OptionForm();
            dlg.ShowDialog(this);
        }

        private void toolItemRename_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);
            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                RenameForm dlg = new RenameForm();
                DialogResult dlgRet = dlg.ShowDialog(this);
                if (dlgRet == DialogResult.OK)
                {
                    FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];
                    RcsCommandResult ret = fi.Rename(dlg.NewName);
                    if (ret.Result != RcsCommandResultCode.Ok )
                    {
                        MessageBox.Show(ret.Message);
                    }
                    UpdateFileInfoList();
                    return;
                }
                else
                {
                    return;
                }
            }

        }

        private void toolItemDelete_Click(object sender, EventArgs e)
        {
            string path = SelectedWorkSpacePath();
            WorkSpace wksp = this.dictWkSp.GetWorkSpace(path);
            foreach (ListViewItem item in this.lvFile.SelectedItems)
            {
                FileInfo fi = wksp.FileInfoDict[item.SubItems[0].Text];
                string msg;
                msg = string.Format(Properties.Resources.DeleteMessage, fi.WorkPath);
                DialogResult dlgRet = MessageBox.Show(msg, "WinRcs", MessageBoxButtons.OKCancel);
                if (dlgRet == DialogResult.OK)
                {
                    RcsCommandResult ret = fi.Delete();
                    if (ret.Result != RcsCommandResultCode.Ok)
                    {
                        MessageBox.Show(ret.Message);
                    }
                    UpdateFileInfoList();
                    return;
                }
                else
                {
                    return;
                }
            }
        }


        private void cntxtMenuFileList_Opening(object sender, CancelEventArgs e)
        {
            //TODO:選択状態に合わせて実行できるメニューを指定します
            //this.mnuItemDiff.Enabled = false;
        }


    }
}
