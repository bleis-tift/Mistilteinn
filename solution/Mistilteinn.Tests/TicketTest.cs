using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mistilteinn.ToolWindows;
using Mistilteinn.Models;
using System.IO;

namespace Mistilteinn.Tests
{
    [TestFixture]
    class TicketTest
    {
        [Test]
        public void チケットの等価性チェックができる()
        {
            var ticket = new Ticket() { ID = "1", Summary = "hoge", DetailInfo = "piyo", IsCurrentBranch = false };
            Assert.That(ticket, Is.EqualTo(new Ticket() { ID = "1", Summary = "hoge", DetailInfo = "piyo", IsCurrentBranch = false }));
        }

        void CreateDirIfNotExists(string dir)
        {
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
        }

        void CreateFileForce(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);
            using (var w = File.CreateText(path))
                w.WriteLine(content);
        }

        string Content(string id = "1", string summary = "hoge", string detailInfo = "piyo")
        {
            const string Sep = "  ";
            return id + Sep + "TODO" + Sep + summary + "\n         " + string.Join("\n         ", detailInfo.Split(','));
        }

        [SetUp]
        public void SetUp()
        {
            CreateDirIfNotExists(@".\tools-conf");
            CreateDirIfNotExists(@".\tools-conf\mistilteinn");
        }

        [Test]
        public void ローカルファイルからチケットを読み込める()
        {
            CreateFileForce(
                @".\tools-conf\mistilteinn\ticketlist", Content());

            var loader = new LocalTicketLoader(@".\tools-conf\mistilteinn\ticketlist");
            var ticket = loader.Load("master").First();
            Assert.That(ticket, Is.EqualTo(new Ticket() { ID = "1", Summary = "hoge", DetailInfo = "piyo", IsCurrentBranch = false }));
        }

        [Test]
        public void ローカルファイルからチケットIDを読み込める()
        {
            CreateFileForce(
                @".\tools-conf\mistilteinn\ticketlist", Content(id: "2"));

            var loader = new LocalTicketLoader(@".\tools-conf\mistilteinn\ticketlist");
            var ticket = loader.Load("master").First();
            Assert.That(ticket.ID, Is.EqualTo("2"));
        }

        [Test]
        public void ローカルファイルからチケットのサマリを読み込める()
        {
            CreateFileForce(
                @".\tools-conf\mistilteinn\ticketlist", Content(summary: "aaa"));

            var loader = new LocalTicketLoader(@".\tools-conf\mistilteinn\ticketlist");
            var ticket = loader.Load("master").First();
            Assert.That(ticket.Summary, Is.EqualTo("aaa"));
        }

        [Test]
        public void ローカルファイルからチケットの詳細を読み込める()
        {
            CreateFileForce(
                @".\tools-conf\mistilteinn\ticketlist", Content(detailInfo: "hogehoge,piyopiyo,foobar"));

            var loader = new LocalTicketLoader(@".\tools-conf\mistilteinn\ticketlist");
            var ticket = loader.Load("master").First();
            Assert.That(ticket.DetailInfo, Is.EqualTo("hogehoge\npiyopiyo\nfoobar"));
        }
    }
}
