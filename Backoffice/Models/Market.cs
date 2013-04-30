using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.Models
{
    public class Market
    {
        public Market()
        {
            IsDefault = false;
        }

        public MarketName Name { get; set; }
        public string Description { get; set; }
        public string CookieValue { get; set; }
        public string CultureCode { get; set; }
        public bool IsDefault { get; set; }
        public List<string> Countries { get; set; }
    }
}