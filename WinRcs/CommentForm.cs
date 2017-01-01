using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinRcs
{
    public partial class CommentForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommentForm()
        {
            InitializeComponent();
            string fntName = Properties.Settings.Default.Font;
            if (!string.IsNullOrEmpty(fntName))
            {
                this.Font = new Font(fntName, Properties.Settings.Default.FontSize);
            }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get { return this.txtComment.Text; }
        }

        /// <summary>
        /// 状態
        /// </summary>
        public string State
        {
            get { return this.txtState.Text; }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
