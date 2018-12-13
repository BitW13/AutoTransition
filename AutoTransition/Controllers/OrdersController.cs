using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoTransition.Context;
using AutoTransition.Context.Concrete;
using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using AutoTransition.Filters;
using AutoTransition.Models.Entities;
using AutoTransition.Models.Services;

namespace AutoTransition.Controllers
{
    public class OrdersController : Controller
    {
        private IRepository<Order> _orderRepository;
        private IRepository<User> _userRepository;
        private IRepository<Address> _addressRepository;
        private IRepository<AutoRoute> _autoRouteRepository;
        private IRepository<TransportationTypes> _transportationTypeRepository;


        public OrdersController()
        {
            _orderRepository = new OrderRepository();
            _userRepository = new UserRepository();
            _addressRepository = new AddressRepository();
            _autoRouteRepository = new AutoRouteRepository();
            _transportationTypeRepository = new TransportationTypeRepository();
        }

        // GET: Orders
        [MyAuth]
        [Admin]
        public ActionResult ListOfOrders()
        {            
            return View();
        }        

        public ActionResult TableData(string UserEmail)
        {
            var orders = _orderRepository.GetAll();

            var user = _userRepository.GetElement(
                new User()
                {
                    Email = UserEmail
                });

            var models = new List<AdminIndexOrderViewModel>();

            
            if (UserEmail != null)
            {
                orders = orders.Where(m => m.UserId == user.Id);
            }

            foreach (var order in orders)
            {
                var currentUser = _userRepository.GetItemById(order.UserId);

                models.Add(new AdminIndexOrderViewModel()
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    Email = currentUser.Email,
                    LoadDate = order.LoadDate,
                    UnloadDate = order.UnloadDate,
                    Price = order.Price,
                    Status = order.Status
                });
            }

            return PartialView(models);
        }

        [MyAuth]
        [Admin]
        public ActionResult ListOfRoutes()
        {
            return View();
        }

        public ActionResult TableDataForRoutes(string StartCity, string EndCity)
        {           
            var models = new List<IndexRouteViewModel>();
            IEnumerable<AutoRoute> list = new List<AutoRoute>();
            if (EndCity != null)
            {
                list = SelectionOfRoutes.SelectAutoRoutesByEndCity(EndCity);
            }
            else if(StartCity != null)
            {
                list = SelectionOfRoutes.SelectAutoRoutesByStartCity(StartCity);
            }
            else
            {
                list = _autoRouteRepository.GetAll();
            }

            foreach (var route in list)
            {
                models.Add(new IndexRouteViewModel()
                {
                    StartCity = (_addressRepository.GetItemById(route.StartAddressId)).City,
                    EndCity = (_addressRepository.GetItemById(route.EndAddressId)).City,
                    Distance = route.Distance
                });
            }

            return PartialView(models);
        }

        //[MyAuth]
        //[Admin]
        //public ActionResult UserInfo(Guid? id)
        //{
        //    var user = _userRepository.GetItemById(id.Value);
        //    var userClaims = _userClaimsRepository.GetItemById(user.UserClaimsId);

        //    var model = new AccountIndexViewModel()
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        Name = userClaims.Name,
        //        LastName = userClaims.LastName,
        //        Phone = userClaims.Phone,
        //        CountOfActiveOrders = SelectionOfOrders.GetCountOfActiveOrdersByUserId(id)
        //    };

        //    return View(model);
        //}

        // GET: Orders/Create
        public ActionResult CalcRate()
        {           
            ViewBag.TransportationTypeList = new SelectList(SelectionData.SelectTransportationTypes());
            ViewBag.CitiesList = new SelectList(SelectionData.SelectCities());

            return View(new CalcRateViewModel());
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalcRate(CalcRateViewModel order)
        {
            if (ModelState.IsValid)
            {
                Order newOrder = ConstractOrder.CreateOrder(order, Guid.Parse(HttpContext.Request.Cookies["Id"].Value));

                var autoRoute = _autoRouteRepository.GetItemById(newOrder.AutoRouteId);

                var oldStartAddress = _addressRepository.GetItemById(autoRoute.StartAddressId);

                var oldEndAddress = _addressRepository.GetItemById(autoRoute.EndAddressId);

                var idNewStartAddress = Guid.NewGuid();

                Address newStartAddress = new Address()
                {
                    Id = idNewStartAddress,
                    City = oldStartAddress.City,
                    AddressInCity = order.StartAddress
                };

                var idNewEndAddress = Guid.NewGuid();

                Address newEndAddress = new Address()
                {
                    Id = idNewEndAddress,
                    City = oldEndAddress.City,
                    AddressInCity = order.EndAddress
                };

                _addressRepository.Create(newStartAddress);
                _addressRepository.Create(newEndAddress);

                var idNewAutoRoute = Guid.NewGuid();

                var newAutoRoute = new AutoRoute()
                {
                    Id = idNewAutoRoute,
                    StartAddressId = idNewStartAddress,
                    EndAddressId = idNewEndAddress,
                    Distance = autoRoute.Distance
                };

                _autoRouteRepository.Create(newAutoRoute);

                newOrder.AutoRouteId = idNewAutoRoute;

                _orderRepository.Create(newOrder);

                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        //GET: Orders/AddRoute
        [Admin]
        [HttpGet]
        public ActionResult AddRoute()
        {
            return View(new AddRouteViewModel());
        }

        //POST: Orders/AddRoute
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoute(AddRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var route = new AutoRoute()
                {
                    Id = Guid.NewGuid(),
                    Distance = model.Distance
                };

                Guid startAddressId = Guid.NewGuid();
                var startAddress = new Address()
                {
                    Id = startAddressId,
                    City = model.StartCity,
                    AddressInCity = null
                };
                var databaseStartAddress = _addressRepository.GetElement(startAddress);
                if (databaseStartAddress == null)
                {
                    _addressRepository.Create(startAddress);
                    route.StartAddressId = startAddressId;
                }
                else
                {
                    route.StartAddressId = databaseStartAddress.Id;
                }

                Guid endAddressId = Guid.NewGuid();
                var endAddress = new Address()
                {
                    Id = endAddressId,
                    City = model.EndCity,
                    AddressInCity = null
                };

                var databaseEndAddress = _addressRepository.GetElement(endAddress);
                if (databaseEndAddress == null)
                {
                    _addressRepository.Create(endAddress);
                    route.EndAddressId = endAddressId;
                }
                else
                {
                    route.EndAddressId = databaseEndAddress.Id;
                }
                _autoRouteRepository.Create(route);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        //GET: Orders/AddTransportationType
        [Admin]
        [HttpGet]
        public ActionResult AddTransportationType()
        {
            return View(new TransportationTypes());
        }

        //POST: Orders/AddTransportationType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTransportationType(TransportationTypes model)
        {
            if (ModelState.IsValid)
            {
                if(_transportationTypeRepository.GetElement(model) == null)
                {
                    model.Id = Guid.NewGuid();
                    _transportationTypeRepository.Create(model);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
