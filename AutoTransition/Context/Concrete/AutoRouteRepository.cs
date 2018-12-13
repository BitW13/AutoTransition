using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Concrete
{
    public class AutoRouteRepository : IRepository<AutoRoute>
    {
        private AutoTransitionContext _context;

        public AutoRouteRepository()
        {
            _context = new AutoTransitionContext();
        }

        public void Create(AutoRoute item)
        {
            _context.AutoRoutes.Add(item);
            _context.SaveChanges();
        }

        public void Delete(AutoRoute item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<AutoRoute> GetAll()
        {
            return _context.AutoRoutes.ToList();
        }

        public AutoRoute GetElement(AutoRoute item)
        {
            throw new NotImplementedException();
        }

        public AutoRoute GetItemById(Guid id)
        {
            return _context.AutoRoutes.FirstOrDefault(m => m.Id == id);
        }

        public void Update(AutoRoute item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}