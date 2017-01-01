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
    /// オプション設定フォーム
    /// </summary>
    public partial class OptionForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OptionForm()
        {
            InitializeComponent();
            this.txtRCSPath.Text = Rcs.Instance.RcsRootPath;
            this.txtDiffPath.Text = Rcs.Instance.DiffApplicationPath;

            System.Drawing.Text.InstalledFontCollection ifc = 
                new System.Drawing.Text.InstalledFontCollection();
            //インストールされているすべてのフォントファミリアを取得
            FontFamily[] ffs = ifc.Families;

            string fntName = Properties.Settings.Default.Font;
            if (!string.IsNullOrEmpty(fntName))
            {
                this.Font = new Font(fntName, Properties.Settings.Default.FontSize);
            }
            foreach (FontFamily ff in ffs)
            {
                this.cmbFont.Items.Add( ff.GetName(0) );
                if (fntName == ff.GetName(0))
                {
                    this.cmbFont.SelectedIndex = this.cmbFont.Items.Count - 1;
                }
            }
        }
        /// <summary>
        /// キャンセルボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 設定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            Rcs.Instance.RcsRootPath = this.txtRCSPath.Text;
            Rcs.Instance.DiffApplicationPath = this.txtDiffPath.Text;
            Properties.Settings.Default.Font = this.cmbFont.Text;
            this.Close();
        }

        private void btnRCSPath_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {

                //上部に表示する説明テキストを指定する
                fbd.Description = Properties.Resources.FolderSelectGuide;
                //ルートフォルダを指定する
                //デフォルトでDesktop
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                //最初に選択するフォルダを指定する
                //RootFolder以下にあるフォルダである必要がある
                fbd.SelectedPath = this.txtRCSPath.Text;

                //ユーザーが新しいフォルダを作成できるようにする
                //デフォルトでTrue
                fbd.ShowNewFolderButton = true;

                //ダイアログを表示する
                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    //選択されたフォルダを表示する
                    this.txtRCSPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnDiffPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "差分比較用のアプリケーションを選択してください。";
                if (!String.IsNullOrEmpty(this.txtDiffPath.Text))
                {
                    dlg.InitialDirectory = System.IO.Path.GetDirectoryName(this.txtDiffPath.Text);
                    dlg.FileName = System.IO.Path.GetFileName(this.txtDiffPath.Text);
                }
                dlg.Filter = "実行ファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*";
                dlg.FilterIndex = 1;
                dlg.CheckPathExists = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtDiffPath.Text = dlg.FileName;
                }
            }
        }
    }
}
