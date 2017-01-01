using System;
using System.Collections.Generic;
using System.Text;

namespace WinRcs
{
    /// <summary>
    /// 送るメニューの各項目
    /// </summary>
    class SendToItem
    {
        private string targetPath;  /// 送り先のアプリケーションのフルパス
        private string name;        /// 名称
        private SendToMenu parent;  /// SendToMenuへの参照

        public SendToItem(SendToMenu parent, string name, string target)
        {
            this.parent = parent;
            this.targetPath = target;
            this.name = name;
        }
        public string Name
        {
            get { return this.name; }
        }

        public string TargetPath
        {
            get { return this.targetPath; }
        }

        public void OnClick(object sender, EventArgs e)
        {
            SendToMenuEventArgs sendevent = new SendToMenuEventArgs();
            sendevent.ApplicationPath = this.targetPath;
            this.parent.OnClickSendToMenu(sendevent);
        }
    }

    /// <summary>
    /// 送るメニューをクリックしたときの引数の情報
    /// </summary>
    class SendToMenuEventArgs : EventArgs 
    {
        public string ApplicationPath;
    }

    /// <summary>
    /// 送るメニューの実装
    /// </summary>
    class SendToMenu
    {
        List<SendToItem> mnuItems = null;
        public delegate void ClickSendToMenu(object sender, SendToMenuEventArgs e);
        private event ClickSendToMenu clickHandler;

        /// <summary>
        /// メニューをクリックした場合の処理
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickSendToMenu(SendToMenuEventArgs e)
        {
            if (this.clickHandler != null)
            {
                this.clickHandler(this, e);
            }
        }

        public SendToMenu(System.Windows.Forms.ToolStripMenuItem mnuParent, ClickSendToMenu e)
        {
            this.mnuItems = new List<SendToItem>();
            this.clickHandler = e;
            string sendToPath = System.Environment.GetFolderPath(Environment.SpecialFolder.SendTo);
            string[] files = System.IO.Directory.GetFiles(sendToPath, "*.lnk", System.IO.SearchOption.TopDirectoryOnly);
            IWshRuntimeLibrary.WshShellClass shell = new IWshRuntimeLibrary.WshShellClass();

            foreach (string file in files)
            {
                //参照設定の「COM」タブの「Windows Script Host Object Model」を追加
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(file) as IWshRuntimeLibrary.IWshShortcut;
                SendToItem item = new SendToItem(this,System.IO.Path.GetFileNameWithoutExtension(file), shortcut.TargetPath);
                this.mnuItems.Add(item);
                System.Drawing.Icon icon;
                try
                {
                    icon = System.Drawing.Icon.ExtractAssociatedIcon(item.TargetPath);
                }
                catch( System.IO.FileNotFoundException ex )
                {
                    Console.WriteLine(ex.Message);
                    icon = null;
                }

                if (icon != null)
                {
                    mnuParent.DropDownItems.Add(item.Name, icon.ToBitmap(),new EventHandler(item.OnClick));
                }
                else
                {
                    mnuParent.DropDownItems.Add(item.Name, null, new EventHandler(item.OnClick));
                }
            }
        }
    }
}
