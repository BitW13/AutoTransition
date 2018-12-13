using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Concrete
{
    public class TransportationTypeRepository : IRepository<TransportationTypes>
    {
        private AutoTransitionContext _context;

        public TransportationTypeRepository()
        {
            _context = new AutoTransitionContext();
        }

        public void Create(TransportationTypes item)
        {
            _context.TransportationTypes.Add(item);
            _context.SaveChanges();
        }

        public void Delete(TransportationTypes item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<TransportationTypes> GetAll()
        {
            return _context.TransportationTypes.ToList();
        }

        public TransportationTypes GetElement(TransportationTypes item)
        {
            return _context.TransportationTypes.FirstOrDefault(m => m.TransportationType == item.TransportationType);
        }

        public TransportationTypes GetItemById(Guid id)
        {
            return _context.TransportationTypes.FirstOrDefault(m => m.Id == id);
        }

        public void Update(TransportationTypes item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}