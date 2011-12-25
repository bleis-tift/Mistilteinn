using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mistilteinn.Models;
using Mistilteinn.Configs;
using Mistilteinn.Infos;
using System.IO;
using System.Windows.Forms;

namespace Mistilteinn.Tests
{
    [TestFixture]
    class ConfigTest
    {
        [SetUp]
        public void SetUp()
        {
            ApplicationInfo.RootDir = @".\";
        }

        [Test]
        public void 設定ファイルからITicketLoaderを生成できる_LocalTicketLoader()
        {
            CreateLoaderConfig(
                typeof(LocalTicketLoader).FullName,
                new { name = "ticketFilePath", value = "${root}/tools-conf/mistilteinn/ticketlist" });

            var loader = Configs.ConfigHelper.CreateTicketLoader();
            Assert.That(loader, Is.TypeOf<LocalTicketLoader>());
        }

        [Test]
        public void 設定ファイルからITicketLoaderを生成できる_RedmineTicketLoader()
        {
            CreateLoaderConfig(
                typeof(RedmineTicketLoader).FullName,
                new { name = "accessKey", value = "accesskey", source = "appdata" },
                new { name = "baseUrl", value = "http://rdm/" },
                new { name = "projectId", value = "otrproject00353" });

            var loader = Configs.ConfigHelper.CreateTicketLoader();
            Assert.That(loader, Is.TypeOf<RedmineTicketLoader>());
        }

        private void CreateLoaderConfig(string loaderName, params object[] args)
        {
            CreateDirIfNotExists(@".\tools-conf");
            CreateDirIfNotExists(@".\tools-conf\mistilteinn");

            File.WriteAllText(
                @".\tools-conf\mistilteinn\config",
                string.Format("<config><loader><name>{0}</name><args>{1}</args></loader></config>", loaderName, CreateArgs(args)));
        }

        private string CreateArgs(object[] args)
        {
            var result = new StringBuilder();
            foreach (var arg in args)
            {
                var typ = arg.GetType();
                var name = typ.GetProperty("name").GetValue(arg, null);
                var value = typ.GetProperty("value").GetValue(arg, null);
                var attrs = string.Join(
                    " ",
                    typ.GetProperties().Where(p => p.Name != "name" && p.Name != "value")
                                       .Select(p => p.Name + "=\"" + p.GetValue(arg, null) + "\""));
                result.AppendFormat("<arg name=\"{0}\" {2}>{1}</arg>", name, value, attrs);
            }
            return result.ToString();
        }

        private static void CreateDirIfNotExists(string path)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }
    }
}
