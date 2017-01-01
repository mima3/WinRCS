using WinRcs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace TestWinRcs
{
    
    
    /// <summary>
    ///RcsTest のテスト クラスです。すべての
    ///RcsTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class RcsTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private static string _workspace;
        private static string _testfile;
        private static string _testfilename;
        private static string _username;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _workspace = testContext.TestDir + "\\workspace";

            _testfilename = "test file.txt";
            _testfile = _workspace + "\\" + _testfilename;
            _username = System.Environment.UserName;
            //            throw new InternalTestFailureException("テストです！");


        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {

        }
        
        [TestInitialize()]
        public void MyTestInitialize()
        {
            if (Directory.Exists(_workspace))
            {
                Directory.Delete(_workspace, true);
            }

            Directory.CreateDirectory(_workspace);
            Directory.CreateDirectory(_workspace + "\\RCS");
        }

        /// <summary>
        ///Version のテスト
        ///</summary>
        [TestMethod()]
        public void VersionTest()
        {
            Rcs_Accessor target = new Rcs_Accessor();
            target.RcsRootPath = testContextInstance.TestDeploymentDir;
            string expected = "rcs (GNU RCS) 5.9.0\nCopyright (C) 2010-2013 Thien-Thi Nguyen\nCopyright (C) 1990-1995 Paul Eggert\nCopyright (C) 1982,1988,1989 Walter F. Tichy, Purdue CS\nLicense GPLv3+: GNU GPL version 3 or later <http://gnu.org/licenses/gpl.html>\nThis is free software: you are free to change and redistribute it.\nThere is NO WARRANTY, to the extent permitted by law.\n";
            string actual;
            actual = target.Version();
            Assert.AreEqual(expected, actual,"RCSのバージョンが正常に取得できる");
        }

        /// <summary>
        ///初回CheckIn のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FirstCheckInTest.txt", "TestFiles")]
        public void FirstCheckInTest()
        {
            Rcs_Accessor target = new Rcs_Accessor();
            target.RcsRootPath = testContextInstance.TestDeploymentDir;
            string workPath = RcsTest._testfile;

            // 新規ファイルの作成
            RcsCommandResult expected = new RcsCommandResult(RcsCommandResultCode.Ok, "RCS/" + RcsTest._testfilename + ",v  <--  " + RcsTest._testfilename + "\ninitial revision: 1.1\ndone\n");
            RcsCommandResult actual;
            actual = TestCheckIn(target, workPath, "test contents", "first commit", "Exp");
            Assert.AreEqual(expected, actual, "初回チェックインの確認を行う");

            // 追加されたデータの確認
            Dictionary<string, WinRcs.FileInfo> files = target.GetRcsFileDict(RcsTest._workspace);
            Assert.AreEqual(1, files.Count ,"初回チェックインでアイテムが追加されていること");
            Assert.AreEqual(true, files.ContainsKey(RcsTest._testfilename));
            WinRcs.FileInfo actFileInfo = files[RcsTest._testfilename];

            string rlog = GetTextFile(@"TestFiles\FirstCheckInTest.txt");
            WinRcs.FileInfo expFileInfo = new WinRcs.FileInfo(RcsTest._workspace, rlog);
            Assert.AreEqual(expFileInfo, actFileInfo);
            

        }

        /// <summary>
        ///CheckOut のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CheckOutTest.txt", "TestFiles")]
        public void CheckOutTest()
        {
            Rcs_Accessor target = new Rcs_Accessor();
            target.RcsRootPath = testContextInstance.TestDeploymentDir;
            string workPath = RcsTest._testfile;
            RcsCommandResult ret;

            // 初回チェックイン
            ret = TestCheckIn(target, workPath, "test contents", "first commit", "Exp");
            Assert.AreEqual(RcsCommandResultCode.Ok, ret.Result);

            // チェックアウトの実行
            RcsCommandResult expRet = new RcsCommandResult(RcsCommandResultCode.Ok, "RCS/" + _testfilename + ",v  -->  " + _testfilename + "\nrevision 1.1 (locked)\ndone\n");
            ret = target.CheckOut(workPath);
            Assert.AreEqual(expRet, ret);

            Dictionary<string, WinRcs.FileInfo> files = target.GetRcsFileDict(RcsTest._workspace);
            Assert.AreEqual(1, files.Count);
            Assert.AreEqual(true, files.ContainsKey(RcsTest._testfilename));
            WinRcs.FileInfo actFileInfo = files[RcsTest._testfilename];

            string rlog = GetTextFile(@"TestFiles\CheckOutTest.txt");
            WinRcs.FileInfo expFileInfo = new WinRcs.FileInfo(RcsTest._workspace, rlog);
            Assert.AreEqual(expFileInfo, actFileInfo);
        }

        /// <summary>
        ///Revert のテスト
        ///</summary>
        [TestMethod()]
        public void RevertTest()
        {
            string firstContents = "test contents";
            string changeContents = "change contents";
            Rcs_Accessor target = new Rcs_Accessor();
            target.RcsRootPath = testContextInstance.TestDeploymentDir;
            string workPath = RcsTest._testfile;
            RcsCommandResult ret;

            // 初回チェックイン
            ret = TestCheckIn(target, workPath, firstContents, "first commit", "Exp");
            Assert.AreEqual(RcsCommandResultCode.Ok, ret.Result);

            // チェックアウトの実行
            ret = target.CheckOut(workPath);
            Assert.AreEqual(RcsCommandResultCode.Ok, ret.Result);

            // ファイルの変更
            File.WriteAllText(workPath, changeContents);
            Assert.AreEqual(changeContents, GetTextFile(workPath));

            // Revert実行
            ret = target.Revert(workPath);
            RcsCommandResult expRet = new RcsCommandResult(RcsCommandResultCode.Ok, "RCS/" + _testfilename + ",v  -->  " + _testfilename + "\nrevision 1.1 (unlocked)\ndone\n");
            Assert.AreEqual(expRet, ret);
            Assert.AreEqual(firstContents, GetTextFile(workPath));
        }

        /// <summary>
        ///Revert のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("SecondCheckInTest.txt", "TestFiles")]
        public void SecondCheckInTest()
        {
            string firstContents = "test contents";
            string changeContents = "change contents";
            Rcs_Accessor target = new Rcs_Accessor();
            target.RcsRootPath = testContextInstance.TestDeploymentDir;
            string workPath = RcsTest._testfile;
            RcsCommandResult ret;

            // 初回チェックイン
            ret = TestCheckIn(target, workPath, firstContents, "first commit", "Exp");
            Assert.AreEqual(RcsCommandResultCode.Ok, ret.Result);

            // チェックアウトの実行
            ret = target.CheckOut(workPath);
            Assert.AreEqual(RcsCommandResultCode.Ok, ret.Result);

            // ファイルの変更
            File.WriteAllText(workPath, changeContents);
            Assert.AreEqual(changeContents, GetTextFile(workPath));

            // ２回目のチェックイン
            RcsCommandResult expected = new RcsCommandResult(RcsCommandResultCode.Ok, "RCS/" + RcsTest._testfilename + ",v  <--  " + RcsTest._testfilename + "\nnew revision: 1.2; previous revision: 1.1\ndone\n");
            ret = TestCheckIn(target, workPath, changeContents, "second commit", "Exp");
            Assert.AreEqual(expected, ret);

            // 変更されたデータの確認
            Dictionary<string, WinRcs.FileInfo> files = target.GetRcsFileDict(RcsTest._workspace);
            Assert.AreEqual(1, files.Count);
            Assert.AreEqual(true, files.ContainsKey(RcsTest._testfilename));
            WinRcs.FileInfo actFileInfo = files[RcsTest._testfilename];

            string rlog = GetTextFile(@"TestFiles\SecondCheckInTest.txt");
            WinRcs.FileInfo expFileInfo = new WinRcs.FileInfo(RcsTest._workspace, rlog);
            Assert.AreEqual(expFileInfo, actFileInfo);

        }

        private RcsCommandResult TestCheckIn(Rcs_Accessor target, string path, string contents, string comment, string state)
        {
            File.WriteAllText(path, contents);
            return target.CheckIn(path, comment, state);
        }

        private string GetTextFile(string path)
        {
            string result = File.ReadAllText(path);
            result = result.Replace("$FILE_NAME", _testfilename);
            result = result.Replace("$USER_NAME", _username);
            return result;
        }
    }
}
