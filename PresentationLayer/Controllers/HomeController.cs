using DomainLayer;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Drawing;

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

        public ActionResult WeekChart()
        {
            var chart = GenerateWeekChart(DateTime.Today);
            return View(chart);
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

        private DateTime FindStartDate(DateTime date)
        {
            date = DateTime.Today;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    {
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        date = date.AddDays(-1);
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        date = date.AddDays(-2);
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        date = date.AddDays(-3);
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        date = date.AddDays(-4);
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        date = date.AddDays(-5);
                        break;
                    }
                case DayOfWeek.Sunday:
                    {
                        date = date.AddDays(-6);
                        break;
                    }
            }

            return date;
        }


        private Highcharts GenerateWeekChart(DateTime startDate)
        {
            var date = FindStartDate(startDate);

            //modify data type to make it of array type
            var chartData = new List<ChartData>
            {
                new ChartData(){Day ="Monday", 
                    Frequency = (int)service.CalculateFrequencyRate(date, date),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(date, date),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(date, date)},
                    
                new ChartData(){Day ="Tueday", 
                    Frequency = (int)service.CalculateFrequencyRate(date.AddDays(1), date.AddDays(1)),

                    Occupancy = (int)service.CalculateOccupancyRate(date.AddDays(1), date.AddDays(1)),

                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(1), date.AddDays(1))},
                    
                new ChartData(){Day ="Wednesday", 
                    Frequency = (int)service.CalculateFrequencyRate(date.AddDays(2), date.AddDays(2)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(date.AddDays(2), date.AddDays(2)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(2), date.AddDays(2))},
                    
                new ChartData(){Day ="Thursday", 
                    Frequency = (int)service.CalculateFrequencyRate(date.AddDays(3), date.AddDays(3)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(date.AddDays(3), date.AddDays(3)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(3), date.AddDays(3))},
                    
                new ChartData(){Day ="Friday", 
                    Frequency = (int)service.CalculateFrequencyRate(date.AddDays(4), date.AddDays(4)),
        
                    Occupancy = (int)service.CalculateOccupancyRate(date.AddDays(4), date.AddDays(4)),
    
                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(4), date.AddDays(4))},
                    
            };

            var xData = chartData.Select(i => i.Day).ToArray();

            var yDataFrequency = chartData.Select(i => new object[] { i.Frequency }).ToArray();

            var yDataOccupancy = chartData.Select(i => new object[] { i.Occupancy }).ToArray();

            var yDataUtilisation = chartData.Select(i => new object[] { i.Utilisation }).ToArray();

            //instanciate an object of the Highcharts type
            var chart = new Highcharts("chart")
                //define the type of chart 
                        .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Line })
                //overall Title of the chart 
                        .SetTitle(new Title { Text = "Rates from " + date.ToShortDateString() + " to " + date.AddDays(4).ToShortDateString() })
                //small label below the main Title
                        .SetSubtitle(new Subtitle { Text = "Frquency, Occupancy and Utilisation" })
                //load the X values
                        .SetXAxis(new XAxis { Categories = xData })
                //set the Y title
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Rate %" } })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y; }"
                        })
                        .SetPlotOptions(new PlotOptions
                        {
                            Line = new PlotOptionsLine
                            {
                                DataLabels = new PlotOptionsLineDataLabels
                                {
                                    Enabled = true
                                },
                                EnableMouseTracking = false
                            }
                        })
                //load the Y values 
                        .SetSeries(new[]
                    {
                        new Series {Name = "Frequency", Data = new Data(yDataFrequency)},
                        new Series {Name = "Occupancy", Data = new Data(yDataOccupancy)},
                        new Series {Name = "Utilisation", Data = new Data(yDataUtilisation)},
                            //you can add more y data to create a second line
                            // new Series { Name = "Other Name", Data = new Data(OtherData) }
                    });


            return chart;
        }

    }
}

