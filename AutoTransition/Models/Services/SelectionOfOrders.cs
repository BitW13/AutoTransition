using AutoTransition.Context.Concrete;
using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Services
{
    public static class SelectionOfOrders
    {
        private static IRepository<Order> _repository;

        static SelectionOfOrders()
        {
            _repository = new OrderRepository();
        }

        public static IEnumerable<Order> SelectActiveOrdersByUserId(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            var orders = _repository.GetAll();

            List<Order> selectedOrders = new List<Order>();

            foreach(var order in orders)
            {
                if(order.UserId == id && order.Status == "Active")
                {
                    selectedOrders.Add(order);
                }
            }

            return selectedOrders;
        }

        public static IEnumerable<Order> SelectOrdersByUserId(Guid? id)
        {
            if(id == null)
            {
                return null;
            }

            var orders = _repository.GetAll();

            List<Order> selectedOrders = new List<Order>();

            foreach (var order in orders)
            {
                if (order.UserId == id)
                {
                    selectedOrders.Add(order);
                }
            }

            return selectedOrders;
        }

        public static int GetCountOfActiveOrdersByUserId(Guid? id)
        {
            if(id == null)
            {
                return -1;
            }
            return (SelectActiveOrdersByUserId(id)).Count();
        }
    }
}