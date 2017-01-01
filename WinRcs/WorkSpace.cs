using System;
using System.Collections.Generic;
using System.Text;

namespace WinRcs
{
    /// <summary>
    /// ワークスペースの情報
    /// </summary>
    class WorkSpace
    {
        private string _name = "";
        private string _path = "";
        Dictionary<string , FileInfo> dictFileInfo;

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        public string Path
        {
            get { return this._path; }
            set { this._path = value; }
        }
        public string ManagedFolder
        {
            get { return this._path + @"\RCS"; }
        }

        public Dictionary<string, FileInfo> FileInfoDict
        {
            get { return this.dictFileInfo; }
        }

        public bool UpdateFileInfoList()
        {
           
            /*
            string[] files = System.IO.Directory.GetFiles(this.path,"*",System.IO.SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                FileInfo fi = new FileInfo(this.Path, name);
            }
            */
            this.dictFileInfo = Rcs.Instance.GetRcsFileDict(this._path);
            return true;
        }
    }
}
