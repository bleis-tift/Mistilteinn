using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mistilteinn.Utils
{
    public static class UrlBuilder
    {
        public static string Build<TAnonymousType>(string baseUrl, TAnonymousType param)
        {
            var ps = new List<string>();
            foreach (var prop in param.GetType().GetProperties())
            {
                var value = prop.GetValue(param, null);
                if (value != null)
                    ps.Add(prop.Name + "=" + value);
            }
            return baseUrl + "?" + string.Join("&", ps);
        }
    }
}
