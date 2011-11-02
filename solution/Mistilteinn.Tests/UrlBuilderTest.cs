using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mistilteinn.Utils;

namespace Mistilteinn.Tests
{
    [TestFixture]
    class UrlBuilderTest
    {
        [Test]
        public void URLを構築できる()
        {
            Assert.That(
                UrlBuilder.Build("http://rdm/issue.xml", new { project_id = "hoge" }),
                Is.EqualTo("http://rdm/issue.xml?project_id=hoge"));
        }

        [Test]
        public void 複数のパラメータを持つオブジェクトからURLを構築できる()
        {
            Assert.That(
                UrlBuilder.Build("http://localhost/default.aspx", new { hoge = "piyo", foo = "bar" }),
                Is.EqualTo("http://localhost/default.aspx?hoge=piyo&foo=bar"));
        }

        [Test]
        public void nullのパラメータはスキップされる()
        {
            Assert.That(
                UrlBuilder.Build("http://example.jp/index.html", new { @null = default(string), empty = "", notEmpty = "aaa" }),
                Is.EqualTo("http://example.jp/index.html?empty=&notEmpty=aaa"));
        }
    }
}
