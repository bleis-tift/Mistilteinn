using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mistilteinn.Infos
{
    public static class SolutionInfo
    {
        public static string RootDir
        {
            get { return Path.GetDirectoryName(FullPath); }
        }

        public static string FullPath { get; set; }
    }
}
