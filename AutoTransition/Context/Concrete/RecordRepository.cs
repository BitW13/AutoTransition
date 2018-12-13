using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Concrete
{
    public class RecordRepository : IRepository<Record>
    {
        private AutoTransitionContext _context;

        public RecordRepository()
        {
            _context = new AutoTransitionContext();
        }

        public void Create(Record item)
        {
            _context.Records.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Record item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<Record> GetAll()
        {
            return _context.Records.ToList();
        }

        public Record GetElement(Record item)
        {
            var record = _context.Records.FirstOrDefault(m => m.Title == item.Title && m.Description == item.Description);
            return record;
        }

        public Record GetItemById(Guid id)
        {
            return _context.Records.FirstOrDefault(m => m.Id == id);
        }

        public void Update(Record item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}