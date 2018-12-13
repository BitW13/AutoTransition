using AutoTransition.Context.Concrete;
using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Services
{
    public static class SelectionData
    {
        private static IRepository<Order> _orderRepository;
        private static IRepository<TransportationTypes> _transportationTypesRepository;
        private static IRepository<Address> _addressRepository;
        
        static SelectionData()
        {
            _orderRepository = new OrderRepository();
            _transportationTypesRepository = new TransportationTypeRepository();
            _addressRepository = new AddressRepository();
        }

        public static List<string> SelectTransportationTypes()
        {
            var transportTypes = _transportationTypesRepository.GetAll();
            var list = new List<string>();

            foreach (var type in transportTypes)
            {
                list.Add(type.TransportationType);
            }
            return list;
        }

        public static List<string> SelectCities()
        {
            var addresses = _addressRepository.GetAll();
            var list = new List<string>();

            foreach (var address in addresses)
            {
                bool exist = false;

                foreach (var country in list)
                {
                    if (country == address.City)
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)
                {
                    list.Add(address.City);
                }
            }

            return list;
        }
    }
}