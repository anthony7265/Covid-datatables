using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidCases.Controllers
{
    public class DataItem
    {
        public string iso { get; set; }
        public string name { get; set; }
    }

    public class RegionObject
    {
        public List<DataItem> data { get; set; }
    }
}