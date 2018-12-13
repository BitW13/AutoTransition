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
using AutoTransition.Models.Services;

namespace AutoTransition.Controllers
{
    public class ManageController : Controller
    {
        private IRepository<User> _userRepository;
        private IRepository<UserClaims> _userClaimsRepository;

        public ManageController()
        {
            _userRepository = new UserRepository();
            _userClaimsRepository = new UserClaimsRepository();
        }

        // GET: Manage
        public ActionResult AccountIndex()
        {
            Guid id = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);

            var user = _userRepository.GetItemById(id);

            var userClaims = _userClaimsRepository.GetItemById(user.UserClaimsId);

            var model = new AccountIndexViewModel()
            {
                Id = id,
                Email = user.Email,
                Name = userClaims.Name,
                LastName = userClaims.LastName,
                Phone = userClaims.Phone,
                CountOfActiveOrders = SelectionOfOrders.GetCountOfActiveOrdersByUserId(id)
            };

            return View(model);
        }


        [MyAuth]
        [Admin]
        public ActionResult ListOfUsers()
        {
            return View();
        }

        public ActionResult TableData(string UserEmail)
        {
            var list = _userRepository.GetAll();

            if (UserEmail != null)
            {
                list = list.Where(m => m.Email.Contains(UserEmail));
            }

            return PartialView(list);
        }

        [MyAuth]
        [Admin]
        public ActionResult UserInfo(Guid? id)
        {
            var user = _userRepository.GetItemById(id.Value);
            var userClaims = _userClaimsRepository.GetItemById(user.UserClaimsId);

            var model = new AccountIndexViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = userClaims.Name,
                LastName = userClaims.LastName,
                Phone = userClaims.Phone,
                CountOfActiveOrders = SelectionOfOrders.GetCountOfActiveOrdersByUserId(id)
            };

            return View(model);
        }

        // GET: Manage/Edit/5
        public ActionResult EditUserData()
        {
            Guid id = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userRepository.GetItemById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserClaims userClaims = _userClaimsRepository.GetItemById(user.UserClaimsId);

            var model = new EditUserViewModel()
            {
                Id = userClaims.Id,
                Name = userClaims.Name,
                LastName = userClaims.LastName,
                Phone = userClaims.Phone
            };

            return View(model);
        }

        // POST: Manage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserData(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userClaims = _userClaimsRepository.GetItemById(model.Id);

                if (userClaims == null)
                {
                    return HttpNotFound();
                }

                userClaims.Name = model.Name;
                userClaims.LastName = model.LastName;
                userClaims.Phone = model.Phone;

                _userClaimsRepository.Update(userClaims);

                return RedirectToAction("AccountIndex");
            }
            return View(model);
        }

        //GET: Manage/EditUserPassword
        [HttpGet]
        public ActionResult EditUserPassword()
        {
            Guid id = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);

            EditUserPasswordViewModel model = new EditUserPasswordViewModel()
            {
                Id = id
            };

            return View(model);
        }

        //POST: Manage/EditUserPassword
        [HttpPost]
        public ActionResult EditUserPassword(EditUserPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetItemById(model.Id);

                if(user == null)
                {
                    return HttpNotFound();
                }

                if (user.Password == model.OldPassword)
                {
                    if (model.NewPassword == model.ConfirmPassword)
                    {
                        user.Password = model.NewPassword;

                        _userRepository.Update(user);

                        return RedirectToAction("AccountIndex");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный пароль");
                }
            }

            return View(model);
        }
        // GET: Manage/Delete/5
        public ActionResult Delete()
        {
            Guid id = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);

            var user = _userRepository.GetItemById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            DeleteUserViewModel model = new DeleteUserViewModel()
            {
                Email = user.Email
            };

            return View(model);
        }

        // POST: Manage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DeleteUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid id = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);

                var user = _userRepository.GetItemById(id);

                if(user.Password == model.Password)
                {
                    var userClaims = _userClaimsRepository.GetItemById(user.UserClaimsId);

                    _userClaimsRepository.Delete(userClaims);
                    _userRepository.Delete(user);

                    ClearCookie();

                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        #region Метод удаления куков

        public void ClearCookie()
        {
            const int negativeTime = -73;

            if (Request.Cookies["Id"] != null)
            {
                Response.Cookies["Id"].Expires = DateTime.Now.AddHours(negativeTime);
                Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddHours(negativeTime);
                Response.Cookies["User"].Expires = DateTime.Now.AddHours(negativeTime);
                Response.Cookies["Admin"].Expires = DateTime.Now.AddHours(negativeTime);
                Response.Cookies["Moder"].Expires = DateTime.Now.AddHours(negativeTime);
            }
        }

        #endregion

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
