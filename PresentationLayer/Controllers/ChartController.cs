using DomainLayer;
using wrapper = DomainLayer.WrapperModels;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using PresentationLayer.HelperMethods;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace PresentationLayer.Controllers
{
    public class ChartController : Controller
    {

        #region Fields

        IBookingService bookingService   = new BookingService();
        IUserService userService         = new UserService();
        ISlotService slotService         = new SlotService();
        IRateService service             = new RateService();
        IResourceService resourceService = new ResourceService();
        ModelConversitions converter     = new ModelConversitions();

        #endregion

        public ActionResult WeekChart()
        {
            var date = FindStartDate(DateTime.Today);

            var frequency = service.CalculateFrequencyRate(date, date.AddDays(4));

            var occupancy = service.CalculateOccupancyRate(date, date.AddDays(4));

            var utilisation = service.CalculateUtilisationRate(date, date.AddDays(4));

            ViewBag.Message = new string[] {
                              String.Format("Frequency rate is: {0}", frequency.ToString("0.##\\%")),
                              "Frequency rate is: " + frequency.ToString("0.##\\%"),
                              "Occupancy rate is " + occupancy.ToString("0.##\\%"),
                              "Utilisation rate is: " + utilisation.ToString("0.##\\%") };

            var chart = GenerateWeekChart(DateTime.Today);

            var resources = converter.ConvertResourceListFromWrapper(resourceService.GetResources());

            var model = new ChartViewModel
            {
                Chart = chart,
            };

            foreach (Resource resource in resources)
            {
                model.Resources.Add(new ResourceViewModel
                {
                    Resource    = resource,
                    Utilisation = service.CalculateResourceUtilisationRate(date, date.AddDays(4), converter.ConvertResourceToWrapper(resource)),
                    Frequency   = service.CalculateResourceFrequencyRate(date, date.AddDays(4), converter.ConvertResourceToWrapper(resource)),
                    Occupancy   = service.CalculateResourceOccupancyRate(date, date.AddDays(4), converter.ConvertResourceToWrapper(resource)),
                });
            }
            return View(model);
        }

        //
        // GET: /ChartSample/

        public ActionResult Index()
        {
            //create a collection of data

            //modify data type to make it of array type
            var test = new List<ChartData>
            {
                new ChartData(){Day ="Monday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 06)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 06)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 06))},
                    
                new ChartData(){Day ="Tueday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 07), new DateTime(2017, 03, 07)),

                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 07), new DateTime(2017, 03, 07)),

                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 07), new DateTime(2017, 03, 07))},
                    
                new ChartData(){Day ="Wednesday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 08), new DateTime(2017, 03, 08)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 08), new DateTime(2017, 03, 08)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 08), new DateTime(2017, 03, 08))},
                    
                new ChartData(){Day ="Thursday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 09), new DateTime(2017, 03, 09)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 09), new DateTime(2017, 03, 09)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 09), new DateTime(2017, 03, 09))},
                    
                new ChartData(){Day ="Friday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 10), new DateTime(2017, 03, 10)),
        
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 10), new DateTime(2017, 03, 10)),
    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 10), new DateTime(2017, 03, 10))},
                    
            };

            var xData = test.Select(i => i.Day).ToArray();

            var yDataFrequency = test.Select(i => new object[] { i.Frequency }).ToArray();

            var yDataOccupancy = test.Select(i => new object[] { i.Occupancy }).ToArray();

            var yDataUtilisation = test.Select(i => new object[] { i.Utilisation }).ToArray();

            //instanciate an object of the Highcharts type
            var chart = new Highcharts("chart")
                //define the type of chart 
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Line })
                //overall Title of the chart 
                        .SetTitle(new Title { Text = "Rates for 06/03/3017 to 09/03/2017" })
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


            return View(chart);
        }

        //
        // GET: /ChartSample/Details/5

        public ActionResult Details()
        {
            //create a collection of data
            var transactionCounts = new List<TransactionCount> { 
                           new TransactionCount(){  MonthName="Monday", Count=30},
                           new TransactionCount(){  MonthName="Tuesday", Count=40},
                           new TransactionCount(){  MonthName="Wednesday", Count=4},
                           new TransactionCount(){  MonthName="Thursday", Count=30},
                           new TransactionCount(){  MonthName="Friday", Count=53}
                            };

            //modify data type to make it of array type
            var xDataMonths = transactionCounts.Select(i => i.MonthName).ToArray();
            var yDataCounts = transactionCounts.Select(i => new object[] { i.Count }).ToArray();

            //instanciate an object of the Highcharts type
            var chart = new Highcharts("chart")
                //define the type of chart 
                        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Line })
                //overall Title of the chart 
                        .SetTitle(new Title { Text = "Rates for 06/03/3017 to 09/03/2017" })
                //small label below the main Title
                        .SetSubtitle(new Subtitle { Text = "Frequency, occupancy, utilisation" })
                //load the X values
                        .SetXAxis(new XAxis { Categories = xDataMonths })
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
                        new Series {Name = "Frequency", Data = new Data(yDataCounts)},
                            //you can add more y data to create a second line
                            // new Series { Name = "Other Name", Data = new Data(OtherData) }
                    });


            return View(chart);
        }

        //
        // GET: /ChartSample/Details/5

        public ActionResult Test()
        {
            //instanciate an object of the Highcharts type

            object[][] series = new object[][] { new object[] { 0, 0, 10 }, new object[] { 0, 1, 19 }, new object[] { 0, 2, 8 }, new object[] { 0, 3, 24 }, new object[] { 0, 4, 67 }, new object[] { 1, 0, 92 }, new object[] { 1, 1, 58 }, new object[] { 1, 2, 78 }, new object[] { 1, 3, 117 }, new object[] { 1, 4, 48 }, new object[] { 2, 0, 35 }, new object[] { 2, 1, 15 }, new object[] { 2, 2, 123 }, new object[] { 2, 3, 64 }, new object[] { 2, 4, 52 }, new object[] { 3, 0, 72 }, new object[] { 3, 1, 132 }, new object[] { 3, 2, 114 }, new object[] { 3, 3, 19 }, new object[] { 3, 4, 16 }, new object[] { 4, 0, 38 }, new object[] { 4, 1, 5 }, new object[] { 4, 2, 8 }, new object[] { 4, 3, 117 }, new object[] { 4, 4, 115 }, new object[] { 5, 0, 88 }, new object[] { 5, 1, 32 }, new object[] { 5, 2, 12 }, new object[] { 5, 3, 6 }, new object[] { 5, 4, 120 }, new object[] { 6, 0, 13 }, new object[] { 6, 1, 44 }, new object[] { 6, 2, 88 }, new object[] { 6, 3, 98 }, new object[] { 6, 4, 96 }, new object[] { 7, 0, 31 }, new object[] { 7, 1, 1 }, new object[] { 7, 2, 82 }, new object[] { 7, 3, 32 }, new object[] { 7, 4, 30 }, new object[] { 8, 0, 85 }, new object[] { 8, 1, 97 }, new object[] { 8, 2, 123 }, new object[] { 8, 3, 64 }, new object[] { 8, 4, 84 }, new object[] { 9, 0, 47 }, new object[] { 9, 1, 114 }, new object[] { 9, 2, 31 }, new object[] { 9, 3, 48 }, new object[] { 9, 4, 91 } };

            Series Hestavollane = new Series
            {
                Name = "Test",
                Data = new Data(series),
                Color = Color.White,
                UpColor = Color.Blue,
                

            };


            var chart = new Highcharts("chart")
                //define the type of chart 
                        .InitChart(new Chart { Type = ChartTypes.Heatmap, DefaultSeriesType = ChartTypes.Heatmap , PlotBorderWidth = 1})
                //overall Title of the chart 
                        .SetTitle(new Title { Text = "Test heatmap" })
                //small label below the main Title
                        .SetSubtitle(new Subtitle { Text = "Subtitle" })
                //load the X values
                        .SetXAxis(new XAxis { Categories = new String[] { "Alexander", "Marie", "Maximilian", "Sophia", "Lukas", "Maria", "Leon", "Anna", "Tim", "Laura" }})
                        //set the Y title
                        .SetYAxis(new YAxis
                        {
                            Title = new YAxisTitle { Text = "Rate %" },
                            Categories = new String[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" },
                            //Min = 0,
                            //MinColor = Color.White,         
                            //MaxColor = Color.Blue,
                        })
                        .SetLegend(new Legend
                        {
                            Enabled = true,
                            Align = HorizontalAligns.Right,
                            Layout = Layouts.Vertical,
                            Margin = 0,
                            VerticalAlign = VerticalAligns.Top,
                            Y = 25,
                            SymbolHeight = 280,
                        })


    //                    colorAxis: {
    //    min: 0,
    //    minColor: '#FFFFFF',
    //    maxColor: Highcharts.getOptions().colors[0]
    //},

    //legend: {
    //    align: 'right',
    //    layout: 'vertical',
    //    margin: 0,
    //    verticalAlign: 'top',
    //    y: 25,
    //    symbolHeight: 280
    //},
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = @"function () { return '<b>' + this.series.xAxis.categories[this.point.x] + '</b> sold <br><b>'+ this.point.value + '</b> items on <br><b>' + this.series.yAxis.categories[this.point.y] + '</b>'; }",
                        })
                        .SetSeries(new[] {
                            Hestavollane,
                        });

            return View(chart);
        }


        //
        // GET: /ChartSample/Details/5

        public ActionResult Test2()
        {
            

            return View();
        }

        //
        // POST: /ChartSample/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return View();
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ChartSample/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ChartSample/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #region HelperMethods


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
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Rate %" }, Min = 0 })
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

        #endregion
    }
}
