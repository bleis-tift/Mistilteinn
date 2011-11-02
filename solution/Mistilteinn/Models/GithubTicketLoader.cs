using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mistilteinn.Utils;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace Mistilteinn.Models
{
    public class GithubTicketLoader : ITicketLoader
    {
        readonly string project;
        public GithubTicketLoader(string project)
        {
            this.project = project;
        }

        public IEnumerable<Ticket> Load(string currentBranch)
        {
            var url = UrlBuilder.Build("http://github.com/api/v2/xml/issues/list/" + project + "/open", new { notUse = default(string) });
            var req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            using (var res = req.GetResponse())
            {
                var root = XElement.Load(res.GetResponseStream());
                return root.Descendants("issue").Select(n =>
                {
                    var id = n.Element("number").Value;
                    return new Ticket()
                    {
                        ID = id,
                        Summary = n.Element("title").Value,
                        DetailInfo = n.Element("html-url").Value, 
                        IsCurrentBranch = currentBranch == ("id/" + id)
                    };
                });
            }
        }
    }
}
