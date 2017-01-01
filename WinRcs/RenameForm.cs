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
    /// 名前変更用ダイアログ
    /// </summary>
    public partial class RenameForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RenameForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 新しい名前
        /// </summary>
        public string NewName
        {
            get { return this.txtNewName.Text; }
        }
    }
}
