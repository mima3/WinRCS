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
    /// ワークスペースの追加フォーム
    /// </summary>
    public partial class AddWorkSpaceForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddWorkSpaceForm()
        {
            InitializeComponent();
            string fntName = Properties.Settings.Default.Font;
            if (!string.IsNullOrEmpty(fntName))
            {
                this.Font = new Font(fntName, Properties.Settings.Default.FontSize);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// ワークスペース名
        /// </summary>
        public string WorkSpaceName
        {
            get { return this.txtWorkSpaceName.Text; }
        }

        /// <summary>
        /// ワークスペースへのパス
        /// </summary>
        public string WorkSpacePath
        {
            get { return this.txtWorkSpacePath.Text; }
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示する説明テキストを指定する
            fbd.Description = Properties.Resources.FolderSelectGuide;
            //ルートフォルダを指定する
            //デフォルトでDesktop
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            //fbd.SelectedPath = @"C:\Windows";
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                this.txtWorkSpacePath.Text = fbd.SelectedPath;
            }
        }
    }
}
