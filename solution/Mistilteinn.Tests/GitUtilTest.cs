using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Mistilteinn;

namespace Mistilteinn.Tests
{
    [TestFixture]
    public class GitUtilTest
    {
        [SetUpFixture]
        public class SetUpFixture
        {
            [SetUp]
            public void SetUp()
            {
                if (Directory.Exists(@".\.git") == false)
                {
                    Directory.CreateDirectory(@".\.git");
                }
            }
        }

        static string _(string path)
        {
            return Path.GetFullPath(path);
        }

        static readonly string Root = new DirectoryInfo(".").FullName;

        public class GetGitRepos
        {
            [Test]
            public void 指定ファイルと同じ場所にGitのリポジトリがある場合_指定ファイルが含まれるディレクトリのパスが返る()
            {
                Assert.That(GitUtil.GetRepos(_(@".\test.sln")), Is.EqualTo(Root));
            }

            [Test]
            public void 指定ファイルの上の階層にGitのリポジトリがある場合_指定ファイルの上のディレクトリのパスが返る()
            {
                Directory.CreateDirectory(@".\hoge");
                Assert.That(GitUtil.GetRepos(_(@".\hoge\test.sln")), Is.EqualTo(Root));
                Directory.Delete(@".\hoge");
            }

            [Test]
            public void 指定ファイルの上の階層にGitのリポジトリが無い場合_例外()
            {
                Assert.That(() => GitUtil.GetRepos(@"C:\test.sln"), Throws.TypeOf<GitUtilException>());
            }
        }

        public class GetCommitMessageFile
        {
            [Test]
            public void コミットメッセージのパスを取得できる()
            {
                Assert.That(GitUtil.GetCommitMessagePath(_(@".\test.sln")), Is.EqualTo(Path.Combine(Root, @".git\COMMIT_EDITMSG")));
            }
        }
    }
}
