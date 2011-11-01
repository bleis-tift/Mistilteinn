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

        public override bool Equals(object obj)
        {
            var other = obj as Ticket;
            if (other == null)
                return false;
            return ID == other.ID && Summary == other.Summary && DetailInfo == other.DetailInfo && IsCurrentBranch == other.IsCurrentBranch;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() ^ Summary.GetHashCode() ^ DetailInfo.GetHashCode() ^ IsCurrentBranch.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name +
                string.Format(@"{{ ID = ""{0}"", Summary = ""{1}"", DetailInfo = ""{2}"", IsCurrentBranch = {3} }}", ID, Summary, DetailInfo, IsCurrentBranch);
        }
    }
}
