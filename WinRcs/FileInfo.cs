using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace WinRcs
{
    /// <summary>
    /// リビジョン情報
    /// </summary>
    public class RevisionInfo
    {
        private string _revision="";
        private DateTime _updateTime;
        private string _author = "";
        private string _comment = "";
        private string _state = "";
        private string _lockedby = "";
        private List<string> _branches;

        static private Dictionary<string,string> AnalyzeLine(string line)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Regex r = new Regex(@";", RegexOptions.IgnoreCase);
            string[] tags = r.Split(line);
            foreach (string t in tags)
            {
                int nIx = t.IndexOf(": ");
                if (nIx == -1)
                {
                    continue;
                }
                string key = t.Substring(0, nIx);
                string value = t.Substring(nIx+2);
                ret.Add(key.Trim(), value);
            }
            return ret;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="str">rlogで取得した内容のリビジョンについての文字列</param>
        public RevisionInfo(string str)
        {
            Regex r = new Regex(@"\n", RegexOptions.IgnoreCase);
            Regex t = new Regex(@"\t", RegexOptions.IgnoreCase);
            string[] lines = r.Split(str);
            this._branches = new List<string>();
            int mode = 0; 
            foreach (string line in lines)
            {
                if (mode == 0 && line.Length > 9 && line.Substring(0, 9) == "revision ")
                {
                    // revision 1.2  lockusername
                    string[] revs = t.Split(line.Substring(9));
                    this._revision = revs[0];
                    if (revs.Length > 1)
                    {
                        this._lockedby = revs[1];
                    }
                    mode = 1;
                }
                else if (mode == 1 && line.Length > 5 && line.Substring(0, 5) == "date:")
                {
                    // date: 2013/06/04 07:19:22;  author: mitagaki;  state: Exp;  lines: +1 -0
                    Dictionary<string, string> tags = AnalyzeLine(line);
                    if (tags.ContainsKey("date"))
                    {
                        this._updateTime = DateTime.Parse(tags["date"]).ToLocalTime();
                    }
                    if (tags.ContainsKey("author"))
                    {
                        this._author = tags["author"];
                    }
                    if (tags.ContainsKey("state"))
                    {
                        this._state = tags["state"];
                    }
                    mode = 2;
                }
                else if (mode > 1 && line.Length > 9 && line.Substring(0, 9) == "branches:")
                {
                    // branches:
                    Regex rb = new Regex(@";", RegexOptions.IgnoreCase);
                    string[] branches = rb.Split(line.Substring(10));
                    foreach (string branch in branches)
                    {
                        string b = branch.Trim();
                        if (String.IsNullOrEmpty(b))
                        {
                            continue;
                        }
                        this._branches.Add(b);
                    }
                }
                else if (mode > 1 && mode < 3 ) // && line.Substring(0, 1) == ":")
                {
                    mode = 3;
                    this._comment = line;
                }
                else if (mode == 3)
                {
                    this._comment = this._comment + "\n" + line;
                }
            }
        }

        /// <summary>
        /// リビジョン
        /// </summary>
        public string Revision
        {
            get { return this._revision; }
        }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime UpdateTime
        {
            get { return this._updateTime; }
        }

        /// <summary>
        /// 更新者
        /// </summary>
        public string Author
        {
            get { return this._author; }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get { return this._comment; }
        }

        /// <summary>
        /// ステータス
        /// </summary>
        public string State
        {
            get { return this._state; }
        }

        /// <summary>
        /// ブランチ
        /// </summary>
        public ReadOnlyCollection<string> Branches
        {
            get
            {
                return new ReadOnlyCollection<string>(this._branches);
            }
        }

        /// <summary>
        /// ロックをしているユーザ名
        /// </summary>
        public string LockedBy
        {
            get
            {
                return _lockedby;
            }
        }
    }

    /// <summary>
    /// ロック情報
    /// </summary>
    public class LockInfo
    {
        private string _user;
        private string _revision;

        /// <summary>
        /// LockInfo同士の比較
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            LockInfo f = (LockInfo)obj;


            if (f._user != this._user)
            {
                return false;
            }
            if (f._revision != this._revision)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="user">ロックを行ったユーザ名</param>
        /// <param name="revision">ロック中のリビジョン</param>
        public LockInfo(string user, string revision)
        {
            this._user = user;
            this._revision = revision;

        }

        /// <summary>
        /// ロックを行ったユーザ名
        /// </summary>
        public string User
        {
            get { return this._user; }
            set { this._user = value; }
        }

        /// <summary>
        /// ロック中のリビジョン
        /// </summary>
        public string Revision
        {
            get { return this._revision; }
            set { this._revision = value; }
        }


    }

    /// <summary>
    /// ファイル情報
    /// </summary>
    public class FileInfo
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
                                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _workPath="";
        private string _rcsPath="";
        private string _name="";
        private string _head="";
        private string _branch="";
        private StringCollection _accesslist;
        private string _locks="";
        private List<LockInfo> _locklist;
        private string _keywordSubstitution="";
        private bool _isExistWorkFile;
        private bool _isExistRcs;

        /// <summary>
        /// FileInfo同士の比較
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            FileInfo f = (FileInfo)obj;

            
            if (f._workPath != this._workPath)
            {
                return false;
            }
            if (f._rcsPath != this._rcsPath)
            {
                return false;
            }
            if (f._name != this._name)
            {
                return false;
            }
            if (f._head != this._head)
            {
                return false;
            }
            if (f._branch != this._branch)
            {
                return false;
            }
            if (f._locks != this._locks)
            {
                return false;
            }
            if (f._keywordSubstitution != this._keywordSubstitution)
            {
                return false;
            }
            if (f._isExistWorkFile != this._isExistWorkFile)
            {
                return false;
            }
            if (f._isExistRcs != this._isExistRcs)
            {
                return false;
            }
        
            if (f._accesslist.Count != this._accesslist.Count)
            {
                return false;
            }
            for (int i = 0; i < f._accesslist.Count; ++i)
            {
                if (f._accesslist[i] != this._accesslist[i])
                {
                    return false;
                }
            }
            if (f._locklist.Count != this._locklist.Count)
            {
                return false;
            }
            for (int i = 0; i < f._locklist.Count; ++i) 
            {
                if (!f._locklist[i].Equals(this._locklist[i]))
                {
                    return false;
                }
            }
        
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// RCS管理外のファイルについてのコンストラクタ
        /// </summary>
        /// <param name="workFile">ワークファイル</param>
        public FileInfo(string workFile)
        {
            this._workPath = workFile;
            this._isExistWorkFile = System.IO.File.Exists(this._workPath);
            this._name = System.IO.Path.GetFileName( this._workPath );
        }

        /// <summary>
        /// RCS管理中のファイルについてのコンストラクタ
        /// </summary>
        /// <param name="workSpacePath">ワークスペースのパス</param>
        /// <param name="rlog">rlogの内容</param>
        public FileInfo(string workSpacePath, string rlog)
        {
            Regex r = new Regex(@"\n", RegexOptions.IgnoreCase);
            string[] lines = r.Split(rlog);
            int nMode = 0;
            this._accesslist = new StringCollection();
            this._locklist = new List<LockInfo>();

            foreach (string line in lines)
            {
                if (line.Length == 0)
                {
                    continue;
                }
                if (line.Substring(0,1) == "\t")
                {
                    if (nMode == 1)
                    {
                        // locks
                        int nIx = line.IndexOf(":");
                        if (nIx == -1)
                        {
                            continue;
                        }
                        string key = line.Substring(0, nIx);
                        string val = "";
                        if (line.Length > nIx + 2)
                        {
                            val = line.Substring(nIx + 2);
                        }

                        string user = key.Substring(1);
                        this._locklist.Add(new LockInfo(user, val));
                    }
                    else if (nMode == 2)
                    {
                        // access
                        this._accesslist.Add(line.Substring(1));
                    }
                }
                else
                {
                    nMode = 0;
                    int nIx = line.IndexOf(":");
                    if (nIx == -1)
                    {
                        continue;
                    }

                    string key = line.Substring(0, nIx);
                    string val = "";
                    if (line.Length > nIx + 2)
                    {
                        val = line.Substring(nIx + 2);
                    }

                    if (key == "RCS file")
                    {
                        this._rcsPath = workSpacePath + @"\" + val.Replace("/", @"\");
                        this._isExistRcs = true;
                    }
                    else if (key == "Working file")
                    {
                        this._workPath = workSpacePath + @"\" + val;
                        this._isExistWorkFile = System.IO.File.Exists(this._workPath);
                        this._name = val;
                    }
                    else if (key == "head")
                    {
                        this._head = val;
                    }
                    else if (key == "branch")
                    {
                        this._branch = val;
                    }
                    else if (key == "locks")
                    {
                        this._locks = val;
                        nMode = 1;
                    }
                    else if (key == "access list")
                    {
                        nMode = 2;
                    }
                    else if (key == "keyword substitution")
                    {
                        this._keywordSubstitution = val;
                    }
                }
            }
        }

        /// <summary>
        /// ワークファイルのパス
        /// </summary>
        public string WorkPath
        {
            get { return this._workPath; }
            set { this._workPath = value; }
        }

        /// <summary>
        /// RCSの管理用ファイルへのパス
        /// </summary>
        public string RcsPath
        {
            get { return this._rcsPath; }
            set { this._rcsPath = value; }
        }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// HEAD
        /// </summary>
        public string Head
        {
            get { return this._head; }
        }

        /// <summary>
        /// ロックの一覧
        /// </summary>
        public ReadOnlyCollection<LockInfo> LockList
        {
            get
            {
                if (this._locklist == null)
                {
                    return null;
                }
                else
                {
                    return new ReadOnlyCollection<LockInfo>(this._locklist);
                }
            }
        }

        /// <summary>
        /// RCS管理ファイルの有無
        /// Trueの場合はRCS管理ファイルが存在して、このファイルはRCS管理下である
        /// Falseの場合はRCS管理ファイルが存在せず、このファイルはRCSで管理されていない
        /// </summary>
        public bool IsExistRcs
        {
            get { return this._isExistRcs; }
        }

        /// <summary>
        /// Locks
        /// </summary>
        public string Locks
        {
            get { return this._locks; }
        }
        /// <summary>
        /// Branch
        /// </summary>
        public string Branch
        {
            get { return this._branch; }
        }
        /// <summary>
        /// ワークファイルが存在するか
        /// </summary>
        public bool IsExistWorkFile
        {
            get { return this._isExistWorkFile; }
        }
        /// <summary>
        /// KeywordSubstitution
        /// </summary>
        public string KeywordSubstitution
        {
            get { return this._keywordSubstitution; }
        }
        /// <summary>
        /// 現在のユーザがチェックアウトしているかどうか？
        /// ログインユーザでロックをかけていればチェックアウト中とみなす
        /// </summary>
        /// <returns></returns>
        public bool IsCheckOutByCurrentUser()
        {
            foreach ( LockInfo l in this._locklist) 
            {
                if( l.User == Environment.UserName )
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// リビジョン情報を格納したディレクトリ
        /// </summary>
        public Dictionary<string, RevisionInfo> RevisionDictonary
        {
            get { return Rcs.Instance.GetRevisionDictonary(this._workPath); }
        }

        /// <summary>
        /// チェックインを実行
        /// </summary>
        /// <param name="comment">チェックインコメント</param>
        /// <param name="state">ステータス</param>
        /// <returns></returns>
        public RcsCommandResult CheckIn(string comment, string state)
        {
            return Rcs.Instance.CheckIn(_workPath, comment, state);
        }

        /// <summary>
        /// チェックアウトコマンド
        /// </summary>
        /// <returns></returns>
        public RcsCommandResult CheckOut()
        {
            return Rcs.Instance.CheckOut(this._workPath);
        }


        /// <summary>
        /// 元に戻すコマンド
        /// </summary>
        /// <returns></returns>
        public RcsCommandResult Revert()
        {
            return Rcs.Instance.Revert(this._workPath);
        }

        /// <summary>
        /// 改名
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        public RcsCommandResult Rename(string newName)
        {
            try
            {
                if (this.IsExistWorkFile)
                {
                    string newPath;
                    newPath = System.IO.Path.GetDirectoryName(this.WorkPath) + "\\" + newName;
                    logger.InfoFormat("FileInfo:Rename WorkFile {0} -> {1}", this.WorkPath, newPath );
                    System.IO.File.Move(this.WorkPath , newPath );
                }
                if (this.IsExistRcs)
                {
                    string newPath;
                    newPath = System.IO.Path.GetDirectoryName(this.RcsPath) + "\\" + newName + ",v";
                    logger.InfoFormat("FileInfo:Rename RCSFile {0} -> {1}", this.RcsPath, newPath);
                    System.IO.File.Move(this.RcsPath, newPath);
                }
            }
            catch (System.IO.IOException e)
            {
                logger.ErrorFormat("FileInfo:Rename Error {0}", e.Message );
                return new RcsCommandResult(RcsCommandResultCode.ProcessError, e.Message);
            }
            return new RcsCommandResult(RcsCommandResultCode.Ok, "");
        }
        
        /// <summary>
        /// 削除
        /// </summary>
        /// <returns></returns>
        public RcsCommandResult Delete()
        {
            try
            {
                if( this.IsExistWorkFile )
                {
                    System.IO.File.Delete(this.WorkPath);
                }
                if( this.IsExistRcs )
                {
                    System.IO.File.Delete(this.RcsPath);
                }
            }
            catch (System.IO.IOException e)
            {
                logger.ErrorFormat("FileInfo:Delete Error {0}", e.Message);
                return new RcsCommandResult(RcsCommandResultCode.ProcessError, e.Message);
            }
            return new RcsCommandResult(RcsCommandResultCode.Ok , "");
        }

        /// <summary>
        /// ブランチの変更
        /// </summary>
        /// <param name="rev">リビジョン番号</param>
        /// <returns></returns>
        public RcsCommandResult ChangeBranch(string rev)
        {
            return Rcs.Instance.ChangeBranch(this._workPath, rev);
        }
    }
}
