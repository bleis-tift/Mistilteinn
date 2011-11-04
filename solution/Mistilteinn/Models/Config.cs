using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mistilteinn.Models
{
    public class Config
    {
        public TicketLoaderType TicketLoader { get; set; }

        public class TicketLoaderType
        {
            public Type Type { get; set; }

            public IEnumerable<ArgType> Args { get; set; }

            public class ArgType
            {
                public string Name { get; set; }

                public SourceEnum Source { get; set; }

                public string Value { get; set; }
            }
        }
    }

    public enum SourceEnum
    {
        Value,
        File,
        AppData
    }
}
