using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Mistilteinn
{
    public static class GitUtil
    {
        public static void DoGitNow(string solutionPath)
        {
            try
            {
                var gitRepos = GetRepos(solutionPath);
                Git("now --compact", gitRepos);
            }
            catch (GitUtilException e)
            {
#if DEBUG
                MessageBox.Show(e.Message, "oops!");
#endif
            }
        }

        public static void DoGitNowFixup(string solutionPath)
        {
            try
            {
                var gitRepos = GetRepos(solutionPath);
                Git("now --fixup .git/COMMIT_EDITMSG", gitRepos);
            }
            catch (GitUtilException e)
            {
#if DEBUG
                MessageBox.Show(e.Message, "oops!");
#endif
            }
        }

        private static void Git(string gitArg, string gitRepos)
        {
            var info = new ProcessStartInfo("git", gitArg)
            {
                WorkingDirectory = gitRepos,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            using (var proc = Process.Start(info))
            {
                var err = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                if (proc.ExitCode != 0 && err != "")
                {
#if DEBUG
                    MessageBox.Show(err, "git command output error.");
#endif
                }
            }
        }

        public static string GetRepos(string path)
        {
            var crnt = Path.GetDirectoryName(path);
            while (Directory.Exists(Path.Combine(crnt, ".git")) == false)
            {
                crnt = Path.GetDirectoryName(crnt);
                if (crnt == null)
                    throw GitUtilException.NotFound(Path.GetDirectoryName(path));
            }
            return Path.Combine(crnt);
        }

        public static string GetCommitMessagePath(string path)
        {
            return Path.Combine(GetRepos(path), ".git", "COMMIT_EDITMSG");
        }
    }

    public class GitUtilException : Exception
    {
        public GitUtilException(string msg) : base(msg) { }

        internal static GitUtilException NotFound(string path)
        {
            throw new GitUtilException(string.Format("git repository not found: {0}", path));
        }
    }
}
