using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WinRcs
{
    /// <summary>
    /// RcsCommandの結果コード
    /// </summary>
    public enum RcsCommandResultCode
    {
        /// <summary>
        /// 正常終了
        /// </summary>
        Ok,

        /// <summary>
        /// RCSの実行によるエラー
        /// </summary>
        RcsError,  

        /// <summary>
        /// 当プロセスが検知したエラー
        /// </summary>
        ProcessError
    };

    /// <summary>
    /// Rcsに対するコマンドの結果
    /// </summary>
    public class RcsCommandResult
    {

        private string _message;
        private RcsCommandResultCode _result;

        /// <summary>
        /// RcsCommandの作成
        /// </summary>
        /// <param name="result">結果コード</param>
        /// <param name="message">メッセージ</param>
        public RcsCommandResult(RcsCommandResultCode result, string message)
        {
            this._message = message;
            this._result = result;
        }

        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message
        {
            get { return this._message; }
        }

        /// <summary>
        /// 結果コード
        /// </summary>
        public RcsCommandResultCode Result
        {
            get { return this._result; }
        }

        /// <summary>
        /// RcsCommandResult同士の比較
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null || this.GetType() != obj.GetType()) 
            {
                return false;
            }
            RcsCommandResult r = (RcsCommandResult)obj;
            if (r._result != this._result)
            {
                return false;
            }
            if (r._message != this._message)
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
    }

    /// <summary>
    /// コマンドを実行した結果
    /// </summary>
    public class DoCommandResult
    {
        private string _stdout;
        private string _errout;
        private RcsCommandResultCode result;

        /// <summary>
        /// DoCommandResultの生成
        /// </summary>
        /// <param name="result">RCSコマンドの結果</param>
        /// <param name="stdout">標準出力の内容</param>
        /// <param name="errout">エラー出力の内容</param>
        public DoCommandResult(RcsCommandResultCode result, string stdout, string errout)
        {
            this._stdout = stdout;
            this._errout = errout;
            this.result = result;
        }

        /// <summary>
        /// RCSコマンドの実行結果
        /// </summary>
        public RcsCommandResultCode Result
        {
            get { return this.result; }
        }

        /// <summary>
        /// 標準出力の内容
        /// </summary>
        public string StdOut
        {
            get { return this._stdout; }
        }

        /// <summary>
        /// エラー出力の内容
        /// </summary>
        public string ErrOut
        {
            get { return this._errout; }
        }
    }

    /// <summary>
    /// RCSに対する操作クラス
    /// </summary>
    public class Rcs
    {
        private static Rcs _instance;
        private string _rcsRootPath;
        private string _diffAppPath;
        private Dictionary<string, string> _tempFiles;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
                                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Rcs()
        {
            _instance = new Rcs();
        }

        private Rcs()
        {
            //
            this._rcsRootPath = Properties.Settings.Default.RcsPath;
            if (String.IsNullOrEmpty(this.RcsRootPath))
            {
                this.RcsRootPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            }

            //
            this._diffAppPath = Properties.Settings.Default.DiffPath;

            //
            this._tempFiles = new Dictionary<string, string>();

            logger.Info("rcsRootPath:" + this._rcsRootPath);
            logger.Info("diffAppPath:" + this._diffAppPath);
        }

        /// <summary>
        /// RCS.exeの存在するフォルダ
        /// </summary>
        public string RcsRootPath
        {
            get { return this._rcsRootPath; }
            set
            {
                this._rcsRootPath = value;
                Properties.Settings.Default.RcsPath = this._rcsRootPath;
                logger.Info("Set rcsRootPath:" + this._rcsRootPath);
            }
        }

        /// <summary>
        /// 差分比較用のアプリケーションへのパス
        /// </summary>
        public string DiffApplicationPath
        {
            get { return this._diffAppPath; }
            set
            {
                this._diffAppPath = value;
                Properties.Settings.Default.DiffPath = this._diffAppPath;
                logger.Info("Set diffAppPath:" + this._diffAppPath);
            }
        }

        /// <summary>
        /// 後始末
        /// </summary>
        public void Dispose()
        {
            // 一時ファイルの削除
            foreach (string path in this._tempFiles.Values)
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (System.IO.IOException e)
                {
                    logger.Error(String.Format("RCS::Dispose() File.Delete Error Path:{0}  Error:{1}",path, e.Message) );
                }
            }
        }
        private static string TempFileKey(string wkFile, string revision)
        {
            return wkFile + "-" + revision;
        }

        /// <summary>
        /// リビジョン同士の比較
        /// </summary>
        /// <param name="workPath">対象のワークパス</param>
        /// <param name="leftrev">比較対象のリビジョン（左）</param>
        /// <param name="rightrev">比較対象のリビジョン（右）</param>
        /// <returns>RcsCommandResult</returns>
        public RcsCommandResult DiffRevision(string workPath, string leftrev, string rightrev)
        {
            if (String.IsNullOrEmpty(this._diffAppPath))
            {
                logger.Warn("RCS::DiffRevision diffAppPath is empty.");
                return new RcsCommandResult(RcsCommandResultCode.ProcessError, "差分比較用のアプリケーションが指定されていません。\n\rツール→オプションより差分比較用のアプリケーションを指定してください。");
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = this._diffAppPath;
            string left = GetTempFilePath(workPath, leftrev);
            if (String.IsNullOrEmpty(left))
            {
                logger.Warn(String.Format( "RCS::DiffRevision left is empty. wrorPath:{0}, lefrev:{1}, rightrev:{2}",workPath,leftrev,rightrev ));
                return new RcsCommandResult(RcsCommandResultCode.ProcessError, "リビジョン " + leftrev + " の取得に失敗しました。");
            }
            string right = GetTempFilePath(workPath, rightrev);
            if (String.IsNullOrEmpty(right))
            {
                logger.Warn(String.Format("RCS::DiffRevision right is empty. wrorPath:{0}, lefrev:{1}, rightrev:{2}", workPath, leftrev, rightrev));
                return new RcsCommandResult(RcsCommandResultCode.ProcessError, "リビジョン " + rightrev + " の取得に失敗しました。");
            }
            p.StartInfo.Arguments = "\"" + left + "\" \"" + right + "\"";
            p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(workPath);
            p.Start();
            return new RcsCommandResult(RcsCommandResultCode.Ok, "");
        }

        /// <summary>
        /// 現在のワークファイルと指定の
        /// </summary>
        /// <param name="workPath">ワークファイル</param>
        /// <param name="revision">比較対象のリビジョン</param>
        /// <returns>RcsCommandResult</returns>
        public RcsCommandResult DiffFile(string workPath, string revision)
        {
            if (String.IsNullOrEmpty(this._diffAppPath))
            {
                logger.Warn("RCS::DiffFile diffAppPath is empty.");
                return new RcsCommandResult(RcsCommandResultCode.ProcessError , "差分比較用のアプリケーションが指定されていません。\n\rツール→オプションより差分比較用のアプリケーションを指定してください。") ;
            }

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = this._diffAppPath;
            string tmpPath = GetTempFilePath(workPath, revision);
            if (String.IsNullOrEmpty(tmpPath))
            {
                return new RcsCommandResult(RcsCommandResultCode.ProcessError, "リビジョン " + revision + " の取得に失敗しました。");
            }
            p.StartInfo.Arguments = "\"" + workPath + "\" \"" + tmpPath + "\"";
            p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(workPath);
            p.Start();

            return new RcsCommandResult(RcsCommandResultCode.Ok, "");
        }
            
        private string GetTempFilePath(string workPath, string revision)
        {
            string key = TempFileKey(workPath,revision);
            if (!this._tempFiles.ContainsKey(key))
            {
                string path;
                path = System.IO.Path.GetTempFileName() +  System.IO.Path.GetExtension(workPath);
                DoCommandResult ret = DoCommandStdToFile("co -p -r" + revision + " " + ConvFileName(System.IO.Path.GetFileName(workPath)), System.IO.Path.GetDirectoryName(workPath), "", path);
                if (ret.Result != RcsCommandResultCode.Ok)
                {
                    return "";
                }
                this._tempFiles.Add( key, path );
            }
            return this._tempFiles[key];
        }
        /// <summary>
        /// 唯一のインスタンスの取得
        /// </summary>
        public static Rcs Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// コマンドを出力して標準出力をファイルに展開する
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="workDir"></param>
        /// <param name="inputstream"></param>
        /// <param name="outputfile"></param>
        /// <returns></returns>
        private DoCommandResult DoCommandStdToFile(string cmd, string workDir, string inputstream,string outputfile)
        {
            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            string errout;
            RcsCommandResultCode result;
            try
            {
                //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
                p.StartInfo.FileName = this._rcsRootPath + @"\rcs.bat";
                //出力を読み取れるようにする
                //出力を読み取れるようにする
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.Arguments = cmd;
                p.StartInfo.EnvironmentVariables["PATH"] = _rcsRootPath;
                p.StartInfo.WorkingDirectory = workDir;
                //p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                //p.StartInfo.StandardOutputEncoding = Encoding.UTF8;

                //起動
                p.Start();
                //出力を読み取る
                if (!String.IsNullOrEmpty(inputstream))
                {
                    p.StandardInput.WriteLine(inputstream + "\n");
                }
                byte[] data = new byte[1024];
                System.IO.FileStream fw = new System.IO.FileStream(outputfile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                int nByte =0;
                while((nByte = p.StandardOutput.BaseStream.Read(data,0,1024)) > 0 )
                {
                    for (int i = 0; i < nByte; i++)
                    {
                        fw.WriteByte(data[i]);
                    }
                }
                errout = p.StandardError.ReadToEnd();

                fw.Close();

                //プロセス終了まで待機する
                //WaitForExitはReadToEndの後である必要がある
                //(親プロセス、子プロセスでブロック防止のため)
                p.WaitForExit();
                if (p.ExitCode == 0)
                {
                    result = RcsCommandResultCode.Ok;
                }
                else
                {
                    result = RcsCommandResultCode.RcsError;
                }
                p.WaitForExit();
                if (p.ExitCode == 0)
                {
                    result = RcsCommandResultCode.Ok;
                }
                else
                {
                    result = RcsCommandResultCode.RcsError;
                }
                p.Close();
                logger.Debug(String.Format("RCS::DoCommandStdToFile cmd:{0} workDir:{1} inputstream:{2} outputfile:{3} ", cmd, workDir, inputstream, outputfile));
                logger.Debug(String.Format("result:{0} errout:{1} ", result, errout));
            }
            catch (InvalidOperationException e)
            {
                logger.Error(String.Format("RCS::DoCommandStdToFile raised InvalidOperationException. cmd:{0} workDir:{1} inputstream:{2} outputfile:{3} error:{4}", cmd, workDir, inputstream, outputfile, e.Message));
                return new DoCommandResult(RcsCommandResultCode.ProcessError, "", e.Message);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                logger.Error(String.Format("RCS::DoCommandStdToFile raised Win32Exception. cmd:{0} workDir:{1} inputstream:{2} outputfile:{3} error:{4}", cmd, workDir, inputstream, outputfile, e.Message));
                return new DoCommandResult(RcsCommandResultCode.ProcessError, "", e.Message);
            }

            return new DoCommandResult(result, "", "");
        }
        private DoCommandResult DoCommand(string cmd, string workDir, string inputstream)
        {

            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            string stdout, errout;
            RcsCommandResultCode result;
            try
            {
                //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
                p.StartInfo.FileName = _rcsRootPath + "\\rcs.bat";
                //出力を読み取れるようにする
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.Arguments = cmd;
                p.StartInfo.EnvironmentVariables["PATH"] = _rcsRootPath;
                p.StartInfo.WorkingDirectory = workDir;
                //p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                //p.StartInfo.StandardOutputEncoding = Encoding.UTF8;

                //起動
                p.Start();

                //出力を読み取る
                if (!String.IsNullOrEmpty( inputstream ))
                {
                    p.StandardInput.WriteLine(inputstream + "\n");
                }
                stdout = p.StandardOutput.ReadToEnd();
                errout = p.StandardError.ReadToEnd();

                //プロセス終了まで待機する
                //WaitForExitはReadToEndの後である必要がある
                //(親プロセス、子プロセスでブロック防止のため)
                p.WaitForExit();
                if (p.ExitCode == 0)
                {
                    result = RcsCommandResultCode.Ok;
                }
                else
                {
                    result = RcsCommandResultCode.RcsError;
                } 
                p.Close();
                logger.Debug(String.Format("RCS::DoCommand cmd:{0} workDir:{1} inputstream:{2} ", cmd, workDir, inputstream));
                logger.Debug(String.Format("result:{0} stdout:{1} errout:{2} ", result,stdout, errout));
            }
            catch (InvalidOperationException e) 
            {
                logger.Error(String.Format("RCS::DoCommand raised InvalidOperationException. cmd:{0} workDir:{1} inputstream:{2} error:{3}", cmd, workDir, inputstream, e.Message));
                return new DoCommandResult(RcsCommandResultCode.ProcessError , "", e.Message);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                logger.Error(String.Format("RCS::DoCommand raised Win32Exception. cmd:{0} workDir:{1} inputstream:{2} error:{3}", cmd, workDir, inputstream, e.Message));
                return new DoCommandResult(RcsCommandResultCode.ProcessError, "", e.Message);
            }

            return new DoCommandResult(result, stdout,errout) ;
        }

        /// <summary>
        /// バージョンの取得
        /// </summary>
        /// <returns>バージョン番号。取得失敗時は空文字</returns>
        public string Version()
        {
            DoCommandResult ret = this.DoCommand("--version", "", "");
            if (ret.Result == RcsCommandResultCode.Ok)
            {
                return ret.StdOut;
            }
            return "";
        }

        /// <summary>
        /// RCSの管理用ファイルを取得する
        /// </summary>
        /// <param name="folder">操作対象のフォルダ</param>
        /// <param name="wkfileName">ワークファイル名</param>
        /// <returns></returns>
        public static string GetRcsPath(string folder, string wkfileName)
        {
            return folder + @"\RCS\" + wkfileName + ",v";
        }

        /// <summary>
        /// ワークスペース中のファイルを取得する
        /// </summary>
        /// <param name="wkSpace">ワークスペースのパス</param>
        /// <returns>ファイル名：FileInfoのディクショナリ</returns>
        public Dictionary<string, FileInfo> GetRcsFileDict(string wkSpace)
        {
            // rcsのrlogコマンドでバージョン管理下のファイルを取得する
            Dictionary<string, FileInfo> ret = new Dictionary<string, FileInfo>();

            DoCommandResult commandResult = DoCommand("rlog -h -N " + "RCS/*", wkSpace, "");
            // プロセス以上の場合はただちに異常とする。（RCSが出したエラーについては、OKとする。）
            if (commandResult.Result == RcsCommandResultCode.ProcessError )
            {
                logger.Fatal(String.Format("RCS::GetRcsFileDict is ProcessError. cmd:{0} ", wkSpace));
                return null;
            }
            Regex r = new Regex(@"=============================================================================\n",RegexOptions.IgnoreCase);
            string[] substring = r.Split(commandResult.StdOut);
            foreach (string s  in substring)
            {
                if (String.IsNullOrEmpty(s))
                {
                    continue;
                }
                FileInfo fi = new FileInfo(wkSpace, s);
                ret.Add(fi.Name, fi);
            }
            // RCS管理外のファイルの一覧
            string[] files = System.IO.Directory.GetFiles(wkSpace, "*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                if (!ret.ContainsKey(name))
                {
                    FileInfo fi = new FileInfo(file);
                    ret.Add(fi.Name,fi);
                }
            }
            return ret;
        }

        /// <summary>
        /// 指定のファイルのリビジョンのディクショナリを取得する
        /// </summary>
        /// <param name="workPath">対象のファイル</param>
        /// <returns>リビジョン-RevisionInfo のディクショナリ</returns>
        public Dictionary<string, RevisionInfo> GetRevisionDictonary(string workPath)
        {
            Dictionary<string, RevisionInfo> dictRevisions = new Dictionary<string, RevisionInfo>();
            DoCommandResult commandResult = DoCommand("rlog " + ConvFileName(System.IO.Path.GetFileName(workPath)), System.IO.Path.GetDirectoryName(workPath), "");
            if (commandResult.Result != RcsCommandResultCode.Ok)
            {
                logger.Debug(String.Format("GetRevisionDictonary error. StdOut:{0} ErrOut:{1} ", commandResult.StdOut, commandResult.ErrOut));
                return null;
            }

            Regex r = new Regex(@"=============================================================================\n", RegexOptions.IgnoreCase);
            string[] substring = r.Split(commandResult.StdOut);
            foreach (string s in substring)
            {
                if (String.IsNullOrEmpty(s))
                {
                    continue;
                }
                Regex rRevs = new Regex(@"----------------------------", RegexOptions.IgnoreCase);
                string[] subRevs = rRevs.Split(s);
                int i = 0;
                foreach (string revstr in subRevs)
                {
                    // 1つめの----はスキップ
                    i = i + 1;
                    if (i == 1)
                    {
                        continue;
                    }
                    RevisionInfo rev = new RevisionInfo(revstr);
                    dictRevisions.Add(rev.Revision, rev);
                }
            }

            return dictRevisions;
        }

        /// <summary>
        /// チェックイン
        /// </summary>
        /// <param name="workPath"></param>
        /// <param name="comment"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public RcsCommandResult CheckIn(string workPath, string comment, string state)
        {
            DoCommandResult ret = DoCommand("ci -u -s" + state + " " + ConvFileName(System.IO.Path.GetFileName(workPath)) , System.IO.Path.GetDirectoryName(workPath), comment + "\n.");
            return new RcsCommandResult(ret.Result, ret.ErrOut);
        }

        /// <summary>
        /// チェックアウトコマンド
        /// </summary>
        /// <param name="workPath"></param>
        /// <returns></returns>
        public RcsCommandResult CheckOut(string workPath)
        {
            DoCommandResult ret = DoCommand("co -l " + ConvFileName(System.IO.Path.GetFileName(workPath)) , System.IO.Path.GetDirectoryName(workPath), "");
            return new RcsCommandResult(ret.Result, ret.ErrOut);
        }


        /// <summary>
        /// 指定リビジョンのチェックアウトコマンド
        /// </summary>
        /// <param name="workPath"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        public RcsCommandResult CheckOut(string workPath, string rev)
        {
            DoCommandResult ret = DoCommand("co -l" + rev + " " + ConvFileName(System.IO.Path.GetFileName(workPath)), System.IO.Path.GetDirectoryName(workPath), "");
            return new RcsCommandResult(ret.Result, ret.ErrOut);
        }


        /// <summary>
        /// 元に戻すコマンド
        /// </summary>
        /// <param name="workPath">ワークファイルのパス</param>
        /// <returns></returns>
        public RcsCommandResult Revert(string workPath)
        {
            try
            {
                System.IO.File.Delete(workPath);
            }
            catch (System.IO.IOException e)
            {
                return new RcsCommandResult(RcsCommandResultCode.ProcessError, e.Message);
            }
            DoCommandResult ret = DoCommand("co -u " + ConvFileName(System.IO.Path.GetFileName(workPath)), System.IO.Path.GetDirectoryName(workPath), "");
            return new RcsCommandResult(ret.Result, ret.ErrOut);
        }

        /// <summary>
        /// ブランチの変更
        /// </summary>
        /// <param name="workPath">対象のファイル</param>
        /// <param name="rev">リビジョン番号</param>
        /// <returns></returns>
        public RcsCommandResult ChangeBranch(string workPath, string rev)
        {
            DoCommandResult ret = DoCommand("rcs -b" + rev + " " + ConvFileName(System.IO.Path.GetFileName(workPath)), System.IO.Path.GetDirectoryName(workPath), "");
            return new RcsCommandResult(ret.Result, ret.ErrOut);
        }

        private static string ConvFileName(string fileName)
        {
            return "'" + fileName + "'";
        }
    }
}