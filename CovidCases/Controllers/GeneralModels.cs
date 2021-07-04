using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidCases.Controllers
{
    public class GeneralModels
    {
    }

    public class RegionsList
    {        public string iso { get; set; }
        public string name { get; set; }
    }

    public class ProvinceCasesList
    {
        public string provinceName { get; set; }
        public int cases { get; set; }
        public int deaths { get; set; }
    }
}