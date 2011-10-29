using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mistilteinn
{
    public static class GitUtil
    {
        public static void GitNow(string solutionPath)
        {
            throw new NotImplementedException();
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
