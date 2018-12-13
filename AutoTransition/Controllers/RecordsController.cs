using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoTransition.Context;
using AutoTransition.Context.Concrete;
using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Models;
using AutoTransition.Filters;
using AutoTransition.Models.Entities;

namespace AutoTransition.Controllers
{
    public class RecordsController : Controller
    {
        private IRepository<Record> _repository;
        private IRepository<User> _userRepository;

        public RecordsController()
        {
            _repository = new RecordRepository();
            _userRepository = new UserRepository();
        }

        // GET: Records
        public ActionResult News()
        {
            var records = _repository.GetAll();
            return View(records);
        }

        public ActionResult TableData(string recordName)
        {
            var list = _repository.GetAll();

            if (recordName != null)
            {
                list = list.Where(m => m.Title.Contains(recordName));
            }

            return PartialView(list.Reverse());
        }

        public ActionResult Details(Guid? id)
        {
            if(id== null)
            {
                return HttpNotFound();
            }
            var record = _repository.GetItemById(id.Value);

            return View(record);
        }

        // GET: Records/Create
        [Moder]
        public ActionResult AddRecord()
        {            
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecord(AddRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);

                var user = _userRepository.GetItemById(userId);

                var record = new Record { Email = user.Email, Title = model.Title, Description = model.Description };

                var oldRecord = _repository.GetElement(record);               

                if (oldRecord == null)
                {
                    _repository.Create(new Record()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Email = user.Email,
                        Title = model.Title,
                        Description = model.Description,
                        RecordDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                        DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Такая новость уже существует");
                }
            }

            return View(model);
        }

        // GET: Records/Edit/5
        public ActionResult Edit(Guid? id)
        {
            var record = _repository.GetItemById(id.Value);

            var model = new EditRecordViewModel { Id = record.Id, UserId = record.UserId };

            return View(model);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var record = _repository.GetItemById(model.Id);

                if (record != null)
                {
                    record.Title = model.Title;
                    record.Description = model.Description;

                    _repository.Update(record);

                    return RedirectToAction("News", "Record");
                }
                else
                {
                    ModelState.AddModelError("", "Такой записи не сущетвует");
                }
            }

            return View(model);
        }

        // GET: Records/Delete/5
        [Moder]
        public ActionResult DeleteRecord(Guid? id)
        {
            var record = _repository.GetItemById(id.Value);

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecord(Record model)
        {
            if (ModelState.IsValid)
            {
                var record = _repository.GetItemById(model.Id);

                if (record != null)
                {
                    _repository.Delete(record);

                    return RedirectToAction("News", "Record");
                }
                else
                {
                    ModelState.AddModelError("", "Такой записи не существует");
                }
            }

            return View(model);
        }
    }
}
