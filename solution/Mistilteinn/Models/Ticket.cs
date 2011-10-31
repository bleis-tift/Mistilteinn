using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mistilteinn.Models
{
    public class Ticket
    {
        public string ID { get; set; }
        public string Summary { get; set; }
        public string DetailInfo { get; set; }
        public bool IsCurrentBranch { get; set; }
    }
}
