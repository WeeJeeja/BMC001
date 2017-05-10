﻿using DomainLayer;
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

namespace PresentationLayer.Controllers
{
    public class DashboardController : Controller
    {

        #region Fields

        IBookingService bookingService   = new BookingService();
        ISlotService slotService         = new SlotService();
        IRateService service             = new RateService();
        IResourceService resourceService = new ResourceService();
        ModelConversitions converter     = new ModelConversitions();

        #endregion

        public ActionResult WeekInformation(DateTime date)
        {
            date = FindStartDate(date);

            var chart = GenerateWeekChart(date, "WeekChart");

            var model = new WeekOverview
            {
                Chart           = chart,
                Frequency       = (service.CalculateFrequencyRate(date, date.AddDays(4)) * 100).ToString("0.##\\%"),
                Occupancy       = (service.CalculateOccupancyRate(date, date.AddDays(4)) * 100).ToString("0.##\\%"),
                Utilisation     = (service.CalculateUtilisationRate(date, date.AddDays(4)) * 100).ToString("0.##\\%"),
                DateInformation = new DateInformation
                {
                    StartDate = date,
                    EndDate = date.AddDays(4),
                },
            };

            var resources = converter.ConvertResourceListFromWrapper(resourceService.GetResources());

            foreach (Resource resource in resources)
            {
                model.Resources.Add(new ResourceOverview
                {
                    Resource    = resource,
                    Utilisation = (service.CalculateResourceUtilisationRate(date, date.AddDays(4), resource.ResourceId) *100).ToString("0.##\\%"),
                    Frequency   = (service.CalculateResourceFrequencyRate(date, date.AddDays(4), resource.ResourceId) *100).ToString("0.##\\%"),
                    Occupancy   = (service.CalculateResourceOccupancyRate(date, date.AddDays(4), resource.ResourceId) *100).ToString("0.##\\%"),
                });
            }
            return View(model);
        }

        public ActionResult DayInformation(DateTime date)
        {
            var model = new DayOverview
            {
                Frequency   = (service.CalculateFrequencyRate(date, date) * 100).ToString("0.##\\%"),
                Occupancy   = (service.CalculateOccupancyRate(date, date) * 100).ToString("0.##\\%"),
                Utilisation = (service.CalculateUtilisationRate(date, date) * 100).ToString("0.##\\%"),
                DayChart    = GenerateDayChart(date, date.DayOfWeek + "Chart"),
                Day         = date.DayOfWeek.ToString(),
                Date        = date,
            };

            model.DateInformation = new DateInformation
            {
                StartDate = FindStartDate(date),
                ActiveDay = date.DayOfWeek.ToString(),
            };

            return View(model);
        }

        public ActionResult ResourceInformation(Guid? resourceId, DateTime date)
        {
            var resource = resourceService.GetResource(resourceId);
            
            date = FindStartDate(date);

            var model = new ResourceOverview
            {
                Resource        = converter.ConvertResourceFromWrapper(resource),
                Frequency       = (service.CalculateResourceFrequencyRate(date, date.AddDays(4), resource.ResourceId) * 100).ToString("0.##\\%"),
                Occupancy       = (service.CalculateResourceOccupancyRate(date, date.AddDays(4), resource.ResourceId) * 100).ToString("0.##\\%"),
                Utilisation     = (service.CalculateResourceUtilisationRate(date, date.AddDays(4), resource.ResourceId) * 100).ToString("0.##\\%"),
                DateInformation = new DateInformation
                {
                    StartDate = date,
                },
            };

            var slots = slotService.GetSlots();

            foreach(wrapper.Slot slot in slots)
            {
                model.Table.Add(new ResourceRateTable
                    {
                        Slot = converter.ConvertSlotFromWrapper(slot),
                    });
            }

            var bookings = bookingService.GetThisWeeksBookingsForAResource(resource.ResourceId, date);

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
                    Frequency   = (int)(service.CalculateFrequencyRate(date, date)*100),
                    Occupancy   = (int)(service.CalculateOccupancyRate(date, date)*100),
                    Utilisation = (int)(service.CalculateUtilisationRate(date, date)*100)
                },
                    
                new ChartData()
                {
                    yCategories ="Tueday", 
                    Frequency   = (int)(service.CalculateFrequencyRate(date.AddDays(1), date.AddDays(1))*100),
                    Occupancy   = (int)(service.CalculateOccupancyRate(date.AddDays(1), date.AddDays(1))*100),
                    Utilisation = (int)(service.CalculateUtilisationRate(date.AddDays(1), date.AddDays(1))*100)
                },
                    
                new ChartData()
                {
                    yCategories ="Wednesday", 
                    Frequency   = (int)(service.CalculateFrequencyRate(date.AddDays(2), date.AddDays(2))*100),
                    Occupancy   = (int)(service.CalculateOccupancyRate(date.AddDays(2), date.AddDays(2))*100),
                    Utilisation = (int)(service.CalculateUtilisationRate(date.AddDays(2), date.AddDays(2))*100)
                },
                    
                new ChartData()
                {
                    yCategories ="Thursday", 
                    Frequency   = (int)(service.CalculateFrequencyRate(date.AddDays(3), date.AddDays(3))*100),
                    Occupancy   = (int)(service.CalculateOccupancyRate(date.AddDays(3), date.AddDays(3))*100),
                    Utilisation = (int)(service.CalculateUtilisationRate(date.AddDays(3), date.AddDays(3))*100)
                },
                    
                new ChartData()
                {
                    yCategories ="Friday", 
                    Frequency   = (int)(service.CalculateFrequencyRate(date.AddDays(4), date.AddDays(4))*100),
                    Occupancy   = (int)(service.CalculateOccupancyRate(date.AddDays(4), date.AddDays(4))*100),
                    Utilisation = (int)(service.CalculateUtilisationRate(date.AddDays(4), date.AddDays(4))*100)
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
                        new Series {Name = "Frequency", Data = new Data(yDataFrequency), Color = Color.Turquoise},
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
                    Frequency   = (int)(service.CalculateSlotFrequencyRate(date, slot.SlotId) * 100),
                    Occupancy   = (int)(service.CalculateSlotOccupancyRate(date, slot.SlotId) * 100),
                    Utilisation = (int)(service.CalculateSlotUtilisationRate(date, slot.SlotId) * 100)
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
                        new Series {Name = "Frequency", Data = new Data(yDataFrequency), Color = Color.Turquoise},
                        new Series {Name = "Occupancy", Data = new Data(yDataOccupancy), Color = Color.DeepPink},
                        new Series {Name = "Utilisation", Data = new Data(yDataUtilisation), Color = Color.DarkOrange},
                    });

            return dayChart;
        }

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
