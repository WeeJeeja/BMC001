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

namespace PresentationLayer.Controllers
{
    public class ChartSampleController : Controller
    {

        #region Fields

        IBookingService bookingService = new BookingService();
        IUserService userService = new UserService();
        ISlotService slotService = new SlotService();

        IRateService service = new RateService();
        ModelConversitions converter = new ModelConversitions();

        #endregion
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

        public PartialViewResult RetrieveAvailableResources(CreateBooking booking)
        {
            var availableResources = bookingService.GetAvailableResources(booking.SingleBooking.Date, booking.SingleBooking.Slot);
            var rs = new ResourceService();
            var resources = converter.ConvertResourceListFromWrapper(availableResources);
            booking.Resources = resources;

            booking.SingleBooking.Time = slotService.GetSlot(booking.SingleBooking.Slot).Time;

            ViewBag.Message = booking.Resources.Count() + " found for " +
                booking.SingleBooking.Date + " at " + booking.SingleBooking.Time;

            return PartialView("_resources", booking);
        }

        [HttpPost]
        public ActionResult Book(CreateBooking booking)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                booking.User = converter.ConvertUserFromWrapper(user);

                var convertedBooking = converter.ConvertSingleBookingToWrapper(booking);

                bookingService.AddBooking(convertedBooking);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }

        //
        // POST: /ChartSample/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ChartSample/Edit/5

        public ActionResult Edit()
        {
            List<Slot> slots = new List<Slot>();
            var slotData = slotService.GetSlots();
            foreach (wrapper.Slot data in slotData)
            {
                slots.Add(new Slot
                {
                    Time = data.Time,
                    SlotId = data.SlotId,
                });
            }

            var model = new CreateBooking
            {
                Slots = slots
            };

            return View(model);
        }


        public PartialViewResult RetrieveAvailableResourcesForBlockBooking(CreateBooking booking)
        {
            var availableResources = bookingService.GetAvailableResourcesForBlockBooking(
                booking.BlockBooking.StartDate, 
                booking.BlockBooking.EndDate, 
                booking.BlockBooking.StartSlot, 
                booking.BlockBooking.EndSlot);

            var rs = new ResourceService();
            var resources = converter.ConvertResourceListFromWrapper(availableResources);
            booking.Resources = resources;

            var startTime = slotService.GetSlot(booking.BlockBooking.StartSlot).StartTime;
            var endTime = slotService.GetSlot(booking.BlockBooking.EndSlot).StartTime;

            ViewBag.Message = booking.Resources.Count() + " resources are available from " +
                booking.BlockBooking.StartDate + " to " + booking.BlockBooking.EndDate +
                " between " + startTime.ToString() + " - " + endTime.ToString();

            return PartialView("_blockResources", booking);
        }

        [HttpPost]
        public ActionResult BookBlock(CreateBooking booking)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                bookingService.AddBlockBooking(
                    booking.BlockBooking.StartDate, 
                    booking.BlockBooking.EndDate, 
                    booking.BlockBooking.StartSlot, 
                    booking.BlockBooking.EndSlot, 
                    booking.Resource, 
                    user.UserId);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }

        //
        // POST: /ChartSample/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
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
    }
}
