using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.Specialized;

namespace WinRcs
{
    class WorkSpaceDictionary :   IDisposable
    {
        private Dictionary<string, WorkSpace> dict = null;
        public void Dispose() 
        {
            StringCollection names = new StringCollection();
            StringCollection paths = new StringCollection();

            foreach( KeyValuePair<string,WorkSpace> k in this.dict )
            {
                names.Add( k.Value.Name );
                paths.Add( k.Value.Path );
            }
            Properties.Settings.Default.WorkSpaceName = names;
            Properties.Settings.Default.WorkSpacePath = paths;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        public WorkSpaceDictionary()
        {
            //Debug.Assert(Properties.Settings.Default.WorkSpaceName.Count == Properties.Settings.Default.WorkSpacePath.Count , "PropertyのWorkSpaceNameとWorkSpacePathの登録数が異なる");
            StringCollection names = Properties.Settings.Default.WorkSpaceName;
            StringCollection paths = Properties.Settings.Default.WorkSpacePath;
            this.dict = new Dictionary<string, WorkSpace>();
            if (names == null)
            {
                return;
            }
            for (int i = 0; i < names.Count; ++i)
            {
                AddWorkSpace(names[i], paths[i]);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool AddWorkSpace(string name, string path)
        {
            if (this.dict.ContainsKey(path))
            {
                return false;
            }

            if (!System.IO.Directory.Exists(path))
            {
                return false;
            }
            if (name.Length == 0)
            {
                return false;
            }

            WorkSpace wk = new WorkSpace();
            wk.Name = name;
            wk.Path = path;

            if (!System.IO.Directory.Exists(wk.ManagedFolder))
            {
                System.IO.Directory.CreateDirectory(wk.ManagedFolder);
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(wk.ManagedFolder);
                if (((di.Attributes & System.IO.FileAttributes.Hidden) !=
                      System.IO.FileAttributes.Hidden))
                {
                    di.Attributes |= System.IO.FileAttributes.Hidden;
                }
            }

            this.dict.Add(path,wk);

            return true;
        }

        public Dictionary<string, WorkSpace>.KeyCollection Paths
        {
            get
            {
                return this.dict.Keys;
            }
        }

        public WorkSpace GetWorkSpace(string path)
        {
            return this.dict[path];
        }

        /// <summary>
        /// ワークスペースの削除
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool RemoveWorkSpace(string path)
        {
            if (!this.dict.ContainsKey(path))
            {
                return false;
            }
            this.dict.Remove(path);
            return true;
        }
    }
}
