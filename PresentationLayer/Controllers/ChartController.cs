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
using System.Web.Mvc;
using System.Drawing;
using PresentationLayer.Models.DahsboardViewModel;

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

        public ActionResult WeekInformation(DateTime date)
        {
            date = FindStartDate(date);

            var chart = GenerateWeekChart(DateTime.Today, "WeekChart");

            var model = new WeekOverview
            {
                Chart       = chart,
                Frequency   = service.CalculateFrequencyRate(date, date.AddDays(4)).ToString("0.##\\%"),
                Occupancy   = service.CalculateOccupancyRate(date, date.AddDays(4)).ToString("0.##\\%"),
                Utilisation = service.CalculateUtilisationRate(date, date.AddDays(4)).ToString("0.##\\%"),
                StartDate   = date,
                EndDate     = date.AddDays(4),
            };

            model.DateInformation = new DateInformation
            {
                StartDate = date,
            };

            var resources = converter.ConvertResourceListFromWrapper(resourceService.GetResources());

            foreach (Resource resource in resources)
            {
                model.Resources.Add(new ResourceOverview
                {
                    Resource    = resource,
                    Utilisation = service.CalculateResourceUtilisationRate(date, date.AddDays(4), resource.ResourceId),
                    Frequency   = service.CalculateResourceFrequencyRate(date, date.AddDays(4), resource.ResourceId),
                    Occupancy   = service.CalculateResourceOccupancyRate(date, date.AddDays(4), resource.ResourceId),
                });
            }

            return View(model);
        }

        public ActionResult DayInformation(DateTime date)
        {
            var frequency = service.CalculateFrequencyRate(date, date.AddDays(4));

            var occupancy = service.CalculateOccupancyRate(date, date.AddDays(4));

            var utilisation = service.CalculateUtilisationRate(date, date.AddDays(4));

            ViewBag.Message = new string[] {
                              String.Format("Frequency rate is: {0}", frequency.ToString("0.##\\%")),
                              "Frequency rate is: " + frequency.ToString("0.##\\%"),
                              "Occupancy rate is " + occupancy.ToString("0.##\\%"),
                              "Utilisation rate is: " + utilisation.ToString("0.##\\%") };

            var resources = converter.ConvertResourceListFromWrapper(resourceService.GetResources());

            var model = new DayOverview
            {
                Frequency   = service.CalculateFrequencyRate(date, date),
                Occupancy   = service.CalculateOccupancyRate(date, date),
                Utilisation = service.CalculateUtilisationRate(date, date),
                DayChart    = GenerateDayChart(date, "mondayChart"),
                Day         = date.DayOfWeek.ToString(),
                Date        = date,
            };

            model.DateInformation = new DateInformation
            {
                StartDate = FindStartDate(date),
                ActiveDay = date.DayOfWeek.ToString(),
            };


            var slots = slotService.GetSlots();

            foreach (wrapper.Slot slot in slots)
            {
                model.Slots.Add(new SlotOverview
                    {
                        Frequency             = service.CalculateSlotFrequencyRate(date, slot.SlotId),
                        Occupancy             = service.CalculateSlotOccupancyRate(date, slot.SlotId),
                        Utilisation           = service.CalculateSlotUtilisationRate(date, slot.SlotId),
                        NumberOfResources     = resourceService.GetResources(date).Count(),
                        NumberOfResourcesUsed = service.GetResourcesUsedInSlot(date, slot.SlotId),
                    });
            }

            return View(model);
        }

        public ActionResult ResourceInformation(Guid? resourceId)
        {
            var resource = resourceService.GetResource(resourceId);
            
            var date = FindStartDate(DateTime.Today);

            var model = new ResourceOverview
            {
                Resource    = converter.ConvertResourceFromWrapper(resource),
                Frequency   = service.CalculateResourceFrequencyRate(date, date.AddDays(4), resource.ResourceId),
                Occupancy   = service.CalculateResourceOccupancyRate(date, date.AddDays(4), resource.ResourceId),
                Utilisation = service.CalculateResourceUtilisationRate(date, date.AddDays(4), resource.ResourceId),
            };

            var slots = slotService.GetSlots();

            foreach(wrapper.Slot slot in slots)
            {
                model.Table.Add(new ResourceRateTable
                    {
                        Slot = converter.ConvertSlotFromWrapper(slot),
                    });
            }

            var bookings = bookingService.GetThisWeeksBookingsForAResource(resource.ResourceId);

            #region Add bookings to table

            foreach (wrapper.Booking booking in bookings)
            {
                //all of the nine o'clock bookings
                var entry = model.Table.Where(e => e.Slot.SlotId.Equals(booking.Slot.SlotId)).FirstOrDefault();

                switch (booking.Date.DayOfWeek.ToString())
                {
                    case "Monday":
                        {
                            entry.MondayRates = AddEntry(booking, resource, date);
                            break;
                        }
                    case "Tuesday":
                        {
                            entry.TuesdayRates = AddEntry(booking, resource, date);
                            break;
                        }
                    case "Wednesday":
                        {
                            entry.WednesdayRates = AddEntry(booking, resource, date);
                            break;
                        }

                    case "Thursday":
                        {
                            entry.ThursdayRates = AddEntry(booking, resource, date);
                            break;
                        }

                    case "Friday":
                        {
                            entry.FridayRates = AddEntry(booking, resource, date);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            #endregion

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
                new ChartData(){yCategories ="Monday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 06)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 06)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 06), new DateTime(2017, 03, 06))},
                    
                new ChartData(){yCategories ="Tueday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 07), new DateTime(2017, 03, 07)),

                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 07), new DateTime(2017, 03, 07)),

                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 07), new DateTime(2017, 03, 07))},
                    
                new ChartData(){yCategories ="Wednesday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 08), new DateTime(2017, 03, 08)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 08), new DateTime(2017, 03, 08)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 08), new DateTime(2017, 03, 08))},
                    
                new ChartData(){yCategories ="Thursday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 09), new DateTime(2017, 03, 09)),
                    
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 09), new DateTime(2017, 03, 09)),
                    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 09), new DateTime(2017, 03, 09))},
                    
                new ChartData(){yCategories ="Friday", 
                    Frequency = (int)service.CalculateFrequencyRate(new DateTime(2017, 03, 10), new DateTime(2017, 03, 10)),
        
                    Occupancy = (int)service.CalculateOccupancyRate(new DateTime(2017, 03, 10), new DateTime(2017, 03, 10)),
    
                    Utilisation = (int)service.CalculateUtilisationRate(new DateTime(2017, 03, 10), new DateTime(2017, 03, 10))},
                    
            };

            var xData = test.Select(i => i.yCategories).ToArray();

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
            return View();
        }

        public ActionResult GetChartData(List<string> pData)
        {
            var data = new List<CityPopulation>();
            data.Add(new CityPopulation
                {
                    city_name = "A",
                    population = 100,
                });
            data.Add(new CityPopulation
            {
                city_name = "B",
                population = 200,
            });
            data.Add(new CityPopulation
            {
                city_name = "C",
                population = 300,
            });

            var dataForChart = data.Select(x => new { name = x.city_name, y = x.population });

            return Json(dataForChart, JsonRequestBehavior.AllowGet);
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

        private Highcharts GenerateWeekChart(DateTime startDate, string title)
        {
            var date = FindStartDate(startDate);

            //modify data type to make it of array type
            var chartData = new List<ChartData>
            {
                new ChartData()
                {
                    yCategories ="Monday", 
                    Frequency   = (int)service.CalculateFrequencyRate(date, date),
                    Occupancy   = (int)service.CalculateOccupancyRate(date, date),
                    Utilisation = (int)service.CalculateUtilisationRate(date, date)
                },
                    
                new ChartData()
                {
                    yCategories ="Tueday", 
                    Frequency   = (int)service.CalculateFrequencyRate(date.AddDays(1), date.AddDays(1)),
                    Occupancy   = (int)service.CalculateOccupancyRate(date.AddDays(1), date.AddDays(1)),
                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(1), date.AddDays(1))
                },
                    
                new ChartData()
                {
                    yCategories ="Wednesday", 
                    Frequency   = (int)service.CalculateFrequencyRate(date.AddDays(2), date.AddDays(2)),
                    Occupancy   = (int)service.CalculateOccupancyRate(date.AddDays(2), date.AddDays(2)),
                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(2), date.AddDays(2))
                },
                    
                new ChartData()
                {
                    yCategories ="Thursday", 
                    Frequency   = (int)service.CalculateFrequencyRate(date.AddDays(3), date.AddDays(3)),
                    Occupancy   = (int)service.CalculateOccupancyRate(date.AddDays(3), date.AddDays(3)),
                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(3), date.AddDays(3))
                },
                    
                new ChartData()
                {
                    yCategories ="Friday", 
                    Frequency   = (int)service.CalculateFrequencyRate(date.AddDays(4), date.AddDays(4)),
                    Occupancy   = (int)service.CalculateOccupancyRate(date.AddDays(4), date.AddDays(4)),
                    Utilisation = (int)service.CalculateUtilisationRate(date.AddDays(4), date.AddDays(4))
                },
                    
            };

            var xData = chartData.Select(i => i.yCategories).ToArray();

            var yDataFrequency = chartData.Select(i => new object[] { i.Frequency }).ToArray();

            var yDataOccupancy = chartData.Select(i => new object[] { i.Occupancy }).ToArray();

            var yDataUtilisation = chartData.Select(i => new object[] { i.Utilisation }).ToArray();

            //instanciate an object of the Highcharts type
            var chart = new Highcharts(title)
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
                        new Series {Name = "Frequency", Data = new Data(yDataFrequency), Color = Color.Aqua},
                        new Series {Name = "Occupancy", Data = new Data(yDataOccupancy),  Color = Color.DeepPink},
                        new Series {Name = "Utilisation", Data = new Data(yDataUtilisation),  Color = Color.DarkOrange},
                    });

            return chart;
        }

        private Highcharts GenerateDayChart(DateTime date, string chartName)
        {
            var slots = slotService.GetSlots();

            //modify data type to make it of array type
            var chartData = new List<ChartData>();

            foreach(wrapper.Slot slot in slots)
            {
                chartData.Add(new ChartData
                {
                    yCategories = slot.Time,
                    Frequency   = (int)service.CalculateSlotFrequencyRate(date, slot.SlotId),
                    Occupancy   = (int)service.CalculateSlotOccupancyRate(date, slot.SlotId),
                    Utilisation = (int)service.CalculateSlotUtilisationRate(date, slot.SlotId)
                });
            };
            
            var xData = chartData.Select(i => i.yCategories).ToArray();

            var yDataFrequency = chartData.Select(i => new object[] { i.Frequency }).ToArray();

            var yDataOccupancy = chartData.Select(i => new object[] { i.Occupancy }).ToArray();

            var yDataUtilisation = chartData.Select(i => new object[] { i.Utilisation }).ToArray();

            //instanciate an object of the Highcharts type
            var dayChart = new Highcharts(chartName)
                //define the type of chart 
                        .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Line })
                //overall Title of the chart 
                        .SetTitle(new Title { Text = "Rates for " + date.ToShortDateString() })
                //small label below the main Title
                        .SetSubtitle(new Subtitle { Text = "Frequency, Occupancy and Utilisation" })
                //load the X values
                        .SetXAxis(new XAxis { Categories = xData, Min = 0 })
                //set the Y title
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Rate %" } })
                        .SetTooltip(new Tooltip
                        {
                            Enabled = true,
                            Formatter = "function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y; }"
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
                        new Series {Name = "Frequency", Data = new Data(yDataFrequency), Color = Color.Aqua},
                        new Series {Name = "Occupancy", Data = new Data(yDataOccupancy), Color = Color.DeepPink},
                        new Series {Name = "Utilisation", Data = new Data(yDataUtilisation), Color = Color.DarkOrange},
                    });

            return dayChart;
        }

        private SlotOverview AddSlotInformationToDayView(DateTime date, Guid? slotId)
        {
            var totalResources = resourceService.GetResources(date).Count();

            var resourcesUsed = service.GetResourcesUsedInSlot(date, slotId);

            var resourcesNotUsed = totalResources - resourcesUsed;

            var data = new List<SlotChartData>();
            data.Add(new SlotChartData
            {
                Name = "Number of resources used",
                Value = resourcesUsed,
            });
            data.Add(new SlotChartData
            {
                Name = "Number of resources NOT used",
                Value = resourcesNotUsed,
            });

            var dataForChart = data.Select(x => new { name = x.Name, y = x.Value });

            var model = new SlotOverview
            {
                Frequency = service.CalculateSlotFrequencyRate(date, slotId),
                Occupancy = service.CalculateSlotOccupancyRate(date, slotId),
                Utilisation = service.CalculateSlotUtilisationRate(date, slotId),
            };

            ViewBag.ChartData = dataForChart;

            return model;

        }

        //private Highcharts GenerateSlotFrequencyChart(DateTime date, Guid? slotId, string chartName)
        //{
        //    modify data type to make it of array type

        //    var totalResources = resourceService.GetResources(date).Count();

        //    var resourcesUsed = service.GetResourcesUsedInSlot(date, slotId);

        //    var resourcesNotUsed = totalResources - resourcesUsed;

        //    var data = new List<SlotChartData>();
        //    data.Add(new SlotChartData
        //    {
        //        Name = "Number of resources used",
        //        Value = resourcesUsed,
        //    });
        //    data.Add(new SlotChartData
        //    {
        //        Name = "Number of resources NOT used",
        //        Value = resourcesNotUsed,
        //    });

        //    var dataForChart = data.Select(x => new { name = x.Name, y = x.Value });

            
        //    chart: 
        //            credits: {
        //                enabled: false,
        //            },
        //            exporitng: {
        //                enabled: false,
        //            },

        //            plotOptions: {
        //                pie: {
        //                    allowPointSelect: true,
        //                    cursor: 'pointer',
        //                    dataLabels: {
        //                        enabled: true,
        //                        format: '<b>{point.name}</b>: {point.percentage.1f} %',
        //                        style: {
        //                            color: (Highcharts.theme && Highcharts.theme.constratTestColor) || 'black',
        //                        },
        //                    }

        //                }
        //            },
        //            series: series,
        //            title: {
        //                text: title
        //            }
        //        });
        //    }

        //    instanciate an object of the Highcharts type
        //    var chart = new Highcharts(chartName)
        //        define the type of chart 
        //                .InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Pie })
        //        overall Title of the chart 
        //                .SetTitle(new Title { Text = "Rates for " + date.ToShortDateString() })
        //        small label below the main Title
        //                .SetSubtitle(new Subtitle { Text = "Frequency, Occupancy and Utilisation" })
        //                .SetTooltip(new Tooltip
        //                {
        //                    Enabled = true,
        //                    Formatter = "function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y; }"
        //                })
        //                .SetPlotOptions(new PlotOptions
        //                {
        //                    Pie = new PlotOptionsPie
        //                    {
        //                        AllowPointSelect = true,
        //                        Cursor = Cursors.Pointer,
        //                        DataLabels = new PlotOptionsPieDataLabels
        //                        {
        //                            Enabled = true,
        //                            Formatter = "<b>{point.name}</b>: {point.percentage.1f} %",
                                    
        //                        }
        //                    }
        //                })
        //                .SetSeries(new[]
        //            {
        //                new SeriesData = dataForChart,
        //                new Series {Name = "Pie chart label on series", dataForChart},
        //            });

        //    return chart;
        //}

        private ResourceRateData AddEntry(wrapper.Booking booking, wrapper.Resource resource, DateTime date)
        {
            var attendees = 1;
            if (booking.GroupBooking == true)
            {
                var data = bookingService.GetBooking(booking.BookingId);
                attendees = data.ConfirmedAttendees.Count();
            }

            var entry = new ResourceRateData
            {
                BookedBy          = converter.ConvertUserFromWrapper(booking.BookedBy),
                NumberOfAttendees = attendees,
            };

            return entry;
        }

        #endregion
    }
}
