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
            double price = 1.25 * 1.6 * route.Distance / 100 * SelectCoefficientByTransportationType(model.TransportationType);

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
                coeff = 2.5;
            }
            else if(type == "С температурным режимом")
            {
                coeff = 1.8;
            }
            else if(type == "Сборный груз")
            {
                coeff = 2.0;
            }
            else if(type == "Комплектный груз")
            {
                coeff = 1.9;
            }

            return coeff;
        }
    }
}