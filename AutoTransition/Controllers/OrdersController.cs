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
        private IRepository<UserClaims> _userClaimsRepository;
        private IRepository<CargoDimensions> _cargoDimensionsRepository;


        public OrdersController()
        {
            _orderRepository = new OrderRepository();
            _userRepository = new UserRepository();
            _addressRepository = new AddressRepository();
            _autoRouteRepository = new AutoRouteRepository();
            _transportationTypeRepository = new TransportationTypeRepository();
            _userClaimsRepository = new UserClaimsRepository();
            _cargoDimensionsRepository = new CargoDimensionsRepository();
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
                var startAddress = _addressRepository.GetItemById(route.StartAddressId);
                var endAddress = _addressRepository.GetItemById(route.EndAddressId);

                if(startAddress.AddressInCity == null && endAddress.AddressInCity == null)
                {
                    models.Add(new IndexRouteViewModel()
                    {
                        StartCity = startAddress.City,
                        EndCity = endAddress.City,
                        Distance = route.Distance
                    });
                }                
            }

            return PartialView(models);
        }

        //GET: Orders/OrderInfo
        [MyAuth]
        public ActionResult OrderInfo(Guid? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            var order = _orderRepository.GetItemById(id.Value);

            if(order == null)
            {
                return HttpNotFound();
            }

            var autoRoute = _autoRouteRepository.GetItemById(order.AutoRouteId);

            var startAddress = _addressRepository.GetItemById(autoRoute.StartAddressId);

            var endAddress = _addressRepository.GetItemById(autoRoute.EndAddressId);

            var transportationType = _transportationTypeRepository.GetItemById(order.TransportationTypeId);

            var cargo = _cargoDimensionsRepository.GetItemById(order.CargoDimensionsId);

            var model = new OrderIndexViewModel()
            {
                Id = id.Value,
                CargoDimensions = (cargo.Width.ToString() + "/" + cargo.Length.ToString() + "/" + cargo.Hight.ToString()),
                StartCity = startAddress.City,
                StartAddressInCity = startAddress.AddressInCity,
                EndCity = endAddress.City,
                EndAddressInCity = endAddress.AddressInCity,
                LoadDate = order.LoadDate,
                UnloadDate = order.UnloadDate,
                TransportationType = transportationType.TransportationType,
                Weight = order.Weight,
                Price = order.Price,
                Status = order.Status
            };

            return View(model);
        }



        // GET: Orders/Create
        [MyAuth]
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
            else
            {
                ModelState.AddModelError("", "Неверно введённые данные");
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

        [Admin]
        [MyAuth]
        [HttpGet]
        public ActionResult ChangeCoefficientsForRate()
        {
            return View(new ChangeCoefficientsForRateViewModel());
        }

        [HttpPost]
        public ActionResult ChangeCoefficientsForRate(ChangeCoefficientsForRateViewModel model)
        {
            if (ModelState.IsValid)
            {
                double coeffCompleteLoads = 0;
                double coeffDangerousLoads = 0;
                double coeffDistanceByHundred = 0;
                double coeffGroupLoads = 0;
                double coeffProfit = 0;
                double coeffRefrTransport = 0;

                try
                {
                    coeffCompleteLoads = Convert.ToDouble(model.CoeffCompleteLoads);
                    coeffDangerousLoads = Convert.ToDouble(model.CoeffDangerousLoads);
                    coeffDistanceByHundred = Convert.ToDouble(model.CoeffDistanceByHundred);
                    coeffGroupLoads = Convert.ToDouble(model.CoeffGroupLoads);
                    coeffProfit = Convert.ToDouble(model.CoeffProfit);
                    coeffRefrTransport = Convert.ToDouble(model.CoeffRefrTransport);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Неверно введённые данные");
                    return View(model);
                }

                ConstractOrder.CoeffCompleteLoads = coeffCompleteLoads;
                ConstractOrder.CoeffDangerousLoads = coeffDangerousLoads;
                ConstractOrder.CoeffDistanceByHundred = coeffDistanceByHundred;
                ConstractOrder.CoeffGroupLoads = coeffGroupLoads;
                ConstractOrder.CoeffProfit = coeffProfit;
                ConstractOrder.CoeffRefrTransport = coeffRefrTransport;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Неверно введённые данные");
            }
            return View(model);
        }
    }
}
