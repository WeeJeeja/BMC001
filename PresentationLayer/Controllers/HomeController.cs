using DomainLayer;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        IRateService service = new RateService();


        public ActionResult Index()
        {
            var frequency = service.CalculateFrequencyRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 10));

            var occupancy = service.CalculateOccupancyRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 10));

            var utilisation = service.CalculateUtilisationRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 10));


            ViewBag.Message = "Frequency rate is:" + frequency.ToString("0.##\\%") +
                              " Occupancy rate is:" + occupancy.ToString("0.##\\%") +
                              " Utilisation rate is:" + utilisation.ToString("0.##\\%");




            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}

