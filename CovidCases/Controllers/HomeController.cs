using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using unirest_net.http;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CovidCases.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            HttpResponse<string> response = Unirest.get("https://covid-19-statistics.p.rapidapi.com/regions")
              .header("x-rapidapi-key", "d24df94486mshde4102c2f75dd1ep101f88jsn83f5a1c881c4")
              .header("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com")
              .header("Accept", "application/json")
              .asJson<string>();
            
            var result = response.Body.ToString();

            RegionObject RegionObj = new RegionObject();
            RegionObj = JsonConvert.DeserializeObject<RegionObject>(result);

            List<RegionsList> RegionInfo = new List<RegionsList>();

            for (int i = 0; i < RegionObj.data.Count(); i++)
            {
                var InsertData = new RegionsList()
                {
                    iso = RegionObj.data[i].iso.ToString(),
                    name = RegionObj.data[i].name.ToString()
                };

                RegionInfo.Add(InsertData);
            }

            ViewBag.GetRegions_result = RegionInfo;

            return View();
        }

        public PartialViewResult RegionCases()
        {
            HttpResponse<string> response = Unirest.get("https://covid-19-statistics.p.rapidapi.com/reports?date=2020-04-16")
              .header("x-rapidapi-key", "d24df94486mshde4102c2f75dd1ep101f88jsn83f5a1c881c4")
              .header("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com")
              .header("Accept", "application/json")
              .asJson<string>();

            var result = response.Body.ToString();

            ProvinceObject CasesObj = new ProvinceObject();
            CasesObj = JsonConvert.DeserializeObject<ProvinceObject>(result);

            List<ProvinceCasesList> CasesInfo = new List<ProvinceCasesList>();

            for (int i = 0; i < CasesObj.data.Count(); i++)
            {
                var InsertData = new ProvinceCasesList()
                {
                    provinceName = CasesObj.data[i].region.name.ToString(),
                    cases = CasesObj.data[i].confirmed,
                    deaths = CasesObj.data[i].deaths
                };

                CasesInfo.Add(InsertData);
            }

            var hold = (from x in CasesInfo
                        orderby x.cases descending
                        select x).Take(10).ToList();

            ViewBag.GetCases_result = hold;

            //var test = JsonConvert.DeserializeXmlNode(result, "data");

            ViewBag.xml_result = XmlUtil.Serializer(typeof(List<ProvinceCasesList>), hold); 

            return PartialView();
        }

        public PartialViewResult DetailCases(string iso, string name)
        {
            HttpResponse<string> response = Unirest.get("https://covid-19-statistics.p.rapidapi.com/reports?date=2020-04-16&region_name=" + name + "&iso=" + iso)
              .header("x-rapidapi-key", "d24df94486mshde4102c2f75dd1ep101f88jsn83f5a1c881c4")
              .header("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com")
              .header("Accept", "application/json")
              .asJson<string>();

            var result = response.Body.ToString();

            ProvinceObject CasesObj = new ProvinceObject();
            CasesObj = JsonConvert.DeserializeObject<ProvinceObject>(result);

            List<ProvinceCasesList> CasesInfo = new List<ProvinceCasesList>();

            for (int i = 0; i < CasesObj.data.Count(); i++)
            {
                var InsertData = new ProvinceCasesList()
                {
                    provinceName = CasesObj.data[i].region.province.ToString(),
                    cases = CasesObj.data[i].confirmed,
                    deaths = CasesObj.data[i].deaths
                };

                CasesInfo.Add(InsertData);
            }

            var hold = (from x in CasesInfo
                        orderby x.cases descending
                        select x).Take(10).ToList();

            ViewBag.GetCases_result = hold;
            ViewBag.xml_result = XmlUtil.Serializer(typeof(List<ProvinceCasesList>), hold);

            return PartialView();
        }
    }
}