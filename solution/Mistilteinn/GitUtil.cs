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
        public static void DoGitNow(string solutionPath) { DoGit("now --compact", solutionPath); }

        public static void DoGitNowFixup(string solutionPath) { DoGit("now --fixup .git/COMMIT_EDITMSG", solutionPath); }

        public static void DoGitMaster(string solutionPath) { if (DoGit("master", solutionPath)) MessageBox.Show("success.", "complete!"); }

        public static void DoGitGC(string solutionPath) { DoGit("gc", solutionPath); }

        public static void DoGitCheckout(string solutionPath, string newBranch)
        {
            if (DoGit("checkout " + newBranch, solutionPath) == false)
                DoGit("checkout -b " + newBranch, solutionPath);
        }

        static bool DoGit(string gitArg, string solutionPath)
        {
            try
            {
                var gitRepos = GetRepos(solutionPath);
                return DoGitImpl(gitArg, gitRepos);
            }
            catch (GitUtilException e)
            {
#if DEBUG
                MessageBox.Show(e.Message, "oops!");
#endif
                return false;
            }
        }

        static bool DoGitImpl(string gitArg, string gitRepos)
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
                    return false;
                }
            }
            return true;
        }

        public static string GetRepos(string path)
        {
            var crnt = Directory.Exists(path) ? path : Path.GetDirectoryName(path);
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

        public static string GetCurrentBranch(string path)
        {
            var info = new ProcessStartInfo("git", "branch -l")
            {
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            try
            {
                using (var proc = Process.Start(info))
                {
                    var branches = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    return branches.Split('\n').Single(b => b.StartsWith("*")).Split(new[] { ' ' }, 2)[1];
                }
            }
            catch
            {
                return "master";
            }
        }

        public static IEnumerable<string> GetGitOnelineLogs(string path, string currentBranch)
        {
            var num = currentBranch.Split('/')[1];
            var info = new ProcessStartInfo("git", "log --oneline --grep=\"refs #\"" + num + "$")
            {
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            try
            {
                using (var proc = Process.Start(info))
                {
                    var log = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    return log.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch
            {
                return new string[0];
            }
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
