using AutoTransition.Context.Concrete;
using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Services
{
    public static class SelectionOfRoutes
    {
        private static IRepository<AutoRoute> _autoRouteRepository;
        private static IRepository<Address> _addressRepository;

        static SelectionOfRoutes()
        {
            _autoRouteRepository = new AutoRouteRepository();
            _addressRepository = new AddressRepository();
        }

        public static List<AutoRoute> SelectAutoRoutesByStartCity(string startCity)
        {
            var autoRoutes = _autoRouteRepository.GetAll();

            var list = new List<AutoRoute>();
            if (startCity != null)
            {
                foreach (var autoRoute in autoRoutes)
                {
                    var startAddress = _addressRepository.GetItemById(autoRoute.StartAddressId);

                    if (startAddress != null)
                    {
                        if (startAddress.City == startCity)
                        {
                            list.Add(autoRoute);
                        }
                    }
                }
            }

            return list;
        }

        public static List<AutoRoute> SelectAutoRoutesByEndCity(string endCity)
        {
            var autoRoutes = _autoRouteRepository.GetAll();

            var list = new List<AutoRoute>();
            if (endCity != null)
            {
                foreach (var autoRoute in autoRoutes)
                {
                    var endAddress = _addressRepository.GetItemById(autoRoute.EndAddressId);

                    if (endAddress != null)
                    {
                        if (endAddress.City == endCity)
                        {
                            list.Add(autoRoute);
                        }
                    }
                }
            }

            return list;
        }

        public static AutoRoute SelectAutoRouteByStartAndEndCities(string startCity, string endCity)
        {
            var autoRoutes = _autoRouteRepository.GetAll();

            AutoRoute route = null;
            if(endCity != null && startCity != null)
            {
                foreach(var autoRoute in autoRoutes)
                {
                    var startAddress = _addressRepository.GetItemById(autoRoute.StartAddressId);
                    var endAddress = _addressRepository.GetItemById(autoRoute.EndAddressId);

                    if (startAddress != null && endAddress != null)
                    {
                        if((startAddress.City == startCity && endAddress.City == endCity) || 
                            (startAddress.City == endCity && endAddress.City == startCity))
                        {
                            route = autoRoute;
                            break;
                        }
                    }
                }
            }

            return route;
        }
    }
}