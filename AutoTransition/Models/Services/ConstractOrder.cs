using AutoTransition.Context.Concrete;
using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using AutoTransition.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Services
{
    public static class ConstractOrder
    {
        public static double CoeffDangerousLoads { get; set; }
        public static double CoeffRefrTransport { get; set; }
        public static double CoeffGroupLoads { get; set; }
        public static double CoeffCompleteLoads { get; set; }
        public static double CoeffDistanceByHundred { get; set; }
        public static double CoeffProfit { get; set; }

        private static IRepository<CargoDimensions> _cargoRepository;
        private static IRepository<TransportationTypes> _transportationTypeRepository;
        
        static ConstractOrder()
        {
            _cargoRepository = new CargoDimensionsRepository();
            _transportationTypeRepository = new TransportationTypeRepository();
        }

        public static Order CreateOrder(CalcRateViewModel model, Guid userId)
        {
            AutoRoute route = SearchRoute(model.StartCity, model.EndCity);

            double price = CalcPrice(model, route);

            CargoDimensions cargo = CreateCargoDimensions(model.CargoDimensions);

            _cargoRepository.Create(cargo);

            DateTime unloadDate = CalcUnloadDate(model.LoadDate, route.Distance);

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Price = price,
                AutoRouteId = route.Id,
                CargoDimensionsId = cargo.Id,
                LoadDate = model.LoadDate,
                UnloadDate = unloadDate,
                TransportationTypeId = (_transportationTypeRepository.GetElement(
                    new TransportationTypes() { TransportationType = model.TransportationType })).Id,
                UserId = userId,
                Weight = model.Weight,
                Status = SelectStatus(model.LoadDate, unloadDate)
            };

            return order;
        }

        private static string SelectStatus(DateTime loadDate, DateTime unloadDate)
        {
            if(loadDate < DateTime.Now && unloadDate > DateTime.Now)
            {
                return "Active";
            }
            else
            {
                return "Inactive";
            }
        }
        private static DateTime CalcUnloadDate(DateTime loadDate, double distance)
        {
            int days = Convert.ToInt32(distance / 300);

            int hoursForRoad = Convert.ToInt32(distance / 70);

            int wholeHours = days / 24 + hoursForRoad;

            return  loadDate.AddHours(wholeHours);
        }

        private static CargoDimensions CreateCargoDimensions(string cargoDimensions)
        {
            string[] cargo = (cargoDimensions).Split('/');

            CargoDimensions dimensions = new CargoDimensions()
            {
                Id = Guid.NewGuid(),
                Width = Convert.ToDouble(cargo[0]),
                Length = Convert.ToDouble(cargo[1]),
                Hight = Convert.ToDouble(cargo[2])
            };

            return dimensions;
        }

        private static AutoRoute SearchRoute(string startCity, string endCity)
        {
            return SelectionOfRoutes.SelectAutoRouteByStartAndEndCities(startCity, endCity);
        }

        private static double CalcPrice(CalcRateViewModel model, AutoRoute route)
        {
            if(CoeffProfit == 0)
            {
                CoeffProfit = 1.25;
            }
            if(CoeffDistanceByHundred == 0)
            {
                CoeffDistanceByHundred = 1.6;
            }
            double price = CoeffProfit * CoeffDistanceByHundred * route.Distance 
                / 100 * SelectCoefficientByTransportationType(model.TransportationType);

            string[] cargoDimensions = (model.CargoDimensions).Split('/');

            price *= Convert.ToInt32(cargoDimensions[0]) * Convert.ToInt32(cargoDimensions[1]) * Convert.ToInt32(cargoDimensions[2])/90;
            price *= model.Weight / 15000;

            return price;
        }

        private static double SelectCoefficientByTransportationType(string type)
        {
            double coeff = 1;

            if(type == "Опасный груз")
            {
                coeff = CoeffDangerousLoads;
                if (CoeffDangerousLoads == 0)
                {
                    coeff = 1.29;
                }                
            }
            else if(type == "С температурным режимом")
            {
                coeff = CoeffRefrTransport;
                if(CoeffRefrTransport == 0)
                {
                    coeff = 1.45;
                }
            }
            else if(type == "Сборный груз")
            {
                coeff = CoeffGroupLoads;
                if(CoeffGroupLoads == 0)
                {
                    coeff = 1.5;
                }
            }
            else if(type == "Комплектный груз")
            {
                coeff = CoeffCompleteLoads;
                if(CoeffCompleteLoads == 0)
                {
                    coeff = 1.6;
                }
            }

            return coeff;
        }
    }
}