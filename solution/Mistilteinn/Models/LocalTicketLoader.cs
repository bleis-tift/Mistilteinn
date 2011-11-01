using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mistilteinn.Models
{
    public class LocalTicketLoader
    {
        readonly string ticketFilePath;

        public LocalTicketLoader(string ticketFilePath)
        {
            this.ticketFilePath = ticketFilePath;
        }

        public IEnumerable<Ticket> Load(string currentBranch)
        {
            var tickets = new List<Ticket>();
            using (var stream = new StreamReader(ticketFilePath, Encoding.UTF8))
            {
                Ticket pre = null;
                string line = "";
                while ((line = stream.ReadLine()) != null)
                {
                    if (line == "")
                        continue;
                    else if (line.StartsWith(" "))
                        pre.DetailInfo += (pre.DetailInfo == "" ? "" : "\n") + line.TrimStart();
                    else
                    {
                        var elem = line.Split(new[] { "  " }, StringSplitOptions.None);
                        var ticket = new Ticket()
                        {
                            ID = elem[0],
                            Summary = elem[2],
                            DetailInfo = "",
                            IsCurrentBranch = ("id/" + elem[0]) == currentBranch
                        };
                        pre = ticket;
                        tickets.Add(ticket);
                    }
                }
            }
            return tickets;
        }
    }
}
