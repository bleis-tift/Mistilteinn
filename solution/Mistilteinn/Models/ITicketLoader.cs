using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mistilteinn.Models
{
    public interface ITicketLoader
    {
        IEnumerable<Ticket> Load(string currentBranch);
    }
}
