using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Concrete
{
    public class OrderRepository : IRepository<Order>
    {
        private AutoTransitionContext _context;

        public OrderRepository()
        {
            _context = new AutoTransitionContext();
        }

        public void Create(Order item)
        {
            _context.Orders.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Order item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public Order GetElement(Order item)
        {
            throw new NotImplementedException();
        }

        public Order GetItemById(Guid id)
        {
            return _context.Orders.FirstOrDefault(m => m.Id == id);
        }

        public void Update(Order item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}