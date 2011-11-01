using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mistilteinn.Infos
{
    public static class SolutionInfo
    {
        static string rootDir;
        public static string RootDir
        {
            get { return rootDir; }
            set { rootDir = value; }
        }
    }
}
