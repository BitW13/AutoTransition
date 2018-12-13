using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Concrete
{
    public class AddressRepository : IRepository<Address>
    {
        private AutoTransitionContext _context;

        public AddressRepository()
        {
            _context = new AutoTransitionContext();
        }

        public void Create(Address item)
        {
            _context.Addresses.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Address item)
        {
            _context.Entry(item).State = EntityState.Deleted;
        }

        public IEnumerable<Address> GetAll()
        {
            return _context.Addresses.ToList();
        }

        public Address GetElement(Address item)
        {
            return _context.Addresses.FirstOrDefault(m => m.City == item.City && m.AddressInCity == null);
        }

        public Address GetItemById(Guid id)
        {
            return _context.Addresses.FirstOrDefault(m => m.Id == id);
        }

        public void Update(Address item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}