﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace Mistilteinn.Models
{
    [DisplayName("Local")]
    public class LocalTicketLoader : ITicketLoader
    {
        readonly string filePath;

        public LocalTicketLoader(string filePath)
        {
            this.filePath = filePath;
        }

        public IEnumerable<Ticket> Load(string currentBranch)
        {
            var tickets = new List<Ticket>();
            using (var stream = new StreamReader(filePath, Encoding.UTF8))
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
