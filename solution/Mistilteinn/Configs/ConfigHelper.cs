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
using Mistilteinn.ToolWindows;
using System.ComponentModel;
using System.Xml.XPath;

namespace Mistilteinn.Configs
{
    public static class ConfigHelper
    {
        public static ITicketLoader CreateTicketLoader()
        {
            var conf = Path.Combine(ApplicationInfo.RootDir, "tools-conf", "mistilteinn", "config");
            var loader = XElement.Load(conf).Element("loader");
            switch (loader.Element("name").Value)
            {
                case "Local":
                    return new LocalTicketLoader(loader.XPathSelectElement("//loader/args/arg[@name='file-path']").Value);
                case "Redmine":
                    var args = loader.XPathSelectElement("//loader/args");
                    return new RedmineTicketLoader(
                        args.XPathSelectElement("//arg[@name='access-key']").Value,
                        args.XPathSelectElement("//arg[@name='base-url']").Value,
                        args.XPathSelectElement("//arg[@name='project-id']").Value);
                case "Github":
                    return new GithubTicketLoader(loader.XPathSelectElement("//loader/args/arg[@name='project-id']").Value);
            }
            throw new Exception(string.Format("{0} is invalid ticket loader.", loader.Element("name")));
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
