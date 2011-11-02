using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mistilteinn.Models;
using Mistilteinn.Infos;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Mistilteinn.Configs
{
    public static class Config
    {
        public static ITicketLoader CreateTicketLoader()
        {
            var conf = Path.Combine(ApplicationInfo.RootDir, "tools-conf", "mistilteinn", "config");
            var loader = XElement.Load(conf).Element("loader");
            var typ = Assembly.GetExecutingAssembly().GetType(loader.Element("name").Value);
            var constructor = typ.GetConstructor(Enumerable.Repeat(typeof(string), loader.Element("args").Elements("arg").Count()).ToArray());
            var posDict = new Dictionary<string, int>();
            foreach (var p in constructor.GetParameters())
            {
                posDict[p.Name] = p.Position;
            }
            var args = new object[posDict.Count];
            foreach (var n in loader.Element("args").Elements("arg"))
            {
                var pos = posDict[n.Attribute("name").Value];
                var source = n.Attribute("source");
                args[pos] = GetValue(n, source);
            }
            return (ITicketLoader)constructor.Invoke(args);
        }

        private static object GetValue(XElement node, XAttribute source)
        {
            if (source == null)
                return ExpandValue(node.Value);

            switch (source.Value)
            {
                case "file":
                    {
                        var path = Path.Combine(ApplicationInfo.RootDir, "tools-conf", "mistilteinn", node.Value);
                        if (File.Exists(path))
                            return File.ReadAllText(path);
                        return "";
                    }
                case "appdata":
                    {
                        var path = Path.Combine(Application.UserAppDataPath, node.Value);
                        if (File.Exists(path))
                            return File.ReadAllText(path);
                        return "";
                    }
                default:
                    throw new Exception(source.Value + " is not supported source.");
            }
        }

        private static object ExpandValue(string value)
        {
            if (value.Contains("\\"))
                return Path.Combine(value.Split('\\').Select(v => v == "${root}" ? ApplicationInfo.RootDir : v).ToArray());
            return value;
        }
    }
}
