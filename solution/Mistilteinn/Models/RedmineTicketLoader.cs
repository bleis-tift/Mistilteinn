using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace Mistilteinn.Models
{
    public class RedmineTicketLoader
    {
        readonly string accessKey;
        readonly string baseUrl;
        readonly string projectId;

        public RedmineTicketLoader(string accessKey, string baseUrl, string projectId)
        {
            this.accessKey = accessKey;
            this.baseUrl = baseUrl;
            this.projectId = projectId;
        }

        public IEnumerable<Ticket> Load(string currentBranch)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(baseUrl + "issues.xml?project_id=" + projectId + "&assigned_to_id=me&key=" + accessKey);
            req.Method = "GET";
            using (var res = req.GetResponse())
            {
                var root = XElement.Load(res.GetResponseStream());
                return root.Descendants("issue").Select(n =>
                {
                    var id = n.Element("id").Value;
                    return new Ticket()
                    {
                        ID = id,
                        Summary = n.Element("subject").Value,
                        DetailInfo = baseUrl + "issues/" + id,
                        IsCurrentBranch = currentBranch == ("id/" + id)
                    };
                });
            }
        }
    }
}
