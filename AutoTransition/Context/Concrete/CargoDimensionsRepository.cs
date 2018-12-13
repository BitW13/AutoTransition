using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Concrete
{
    public class CargoDimensionsRepository : IRepository<CargoDimensions>
    {
        private AutoTransitionContext _context;

        public CargoDimensionsRepository()
        {
            _context = new AutoTransitionContext();
        }

        public void Create(CargoDimensions item)
        {
            _context.CargoDimensions.Add(item);
            _context.SaveChanges();
        }

        public void Delete(CargoDimensions item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<CargoDimensions> GetAll()
        {
            return _context.CargoDimensions.ToList();
        }

        public CargoDimensions GetElement(CargoDimensions item)
        {
            throw new NotImplementedException();
        }

        public CargoDimensions GetItemById(Guid id)
        {
            return _context.CargoDimensions.FirstOrDefault(m => m.Id == id);
        }

        public void Update(CargoDimensions item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}