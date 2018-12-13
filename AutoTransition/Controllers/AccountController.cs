using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoTransition.Context;
using AutoTransition.Context.Models;
using AutoTransition.Models.Entities;
using AutoTransition.Context.Interfaces;
using AutoTransition.Context.Concrete;
using AutoTransition.Filters;

namespace AutoTransition.Controllers
{
    public class AccountController : Controller
    {
        private IRepository<User> _userRepository;
        private IRepository<UserClaims> _userClaimsRepository;

        public AccountController()
        {
            _userRepository = new UserRepository();
            _userClaimsRepository = new UserClaimsRepository();
        }

        //public async Task<ActionResult> Index()
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userRepository.GetItemById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        #region Register
        // GET: Account/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CreateUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                User oldUser = new User { Email = user.Email };
                if(_userRepository.GetElement(oldUser) == null)
                {
                    if (user.Password == user.ConfirmPassword)
                    {
                        var newUserClaims = new UserClaims()
                        {
                            Name = user.Name,
                            LastName = user.LastName,
                            Phone = user.Phone
                        };
                        var id = Guid.NewGuid();
                        newUserClaims.Id = id;

                        _userClaimsRepository.Create(newUserClaims);

                        User newUser = new User()
                        {
                            Id = Guid.NewGuid(),
                            Email = user.Email,
                            Password = user.Password,
                            UserClaimsId = id,
                            UserRole = "User"
                        };

                        _userRepository.Create(newUser);

                        const int timeout = 72;

                        Response.Cookies["LoggedIn"].Value = "Accepted";
                        Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddHours(timeout);

                        Response.Cookies["User"].Value = newUserClaims.Name;
                        Response.Cookies["User"].Expires = DateTime.Now.AddHours(timeout);

                        Response.Cookies["Id"].Value = newUser.Id.ToString();
                        Response.Cookies["Id"].Expires = DateTime.Now.AddHours(timeout);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пароли не совпадают");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такой логин уже существует");
                }                
            }

            return View(user);
        }

        #endregion

        #region Авторизация

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserViewModel model)
        {
            ClearCookie();

            if (ModelState.IsValid)
            {

                var user = new User() { Email = model.Email };

                var currentUser = _userRepository.GetElement(user);

                if (currentUser != null)
                {
                    if (model.Password == currentUser.Password)
                    {
                        const int timeout = 73;

                        Response.Cookies["Id"].Value = currentUser.Id.ToString();
                        Response.Cookies["Id"].Expires = DateTime.Now.AddHours(timeout);

                        Response.Cookies["LoggedIn"].Value = "Accepted";
                        Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddHours(timeout);

                        var userClaims = _userClaimsRepository.GetItemById(currentUser.UserClaimsId);

                        if (currentUser.UserRole == "User")
                        {
                            Response.Cookies["User"].Value = userClaims.Name;
                            Response.Cookies["User"].Expires = DateTime.Now.AddHours(timeout);
                        }
                        else if (currentUser.UserRole == "Admin")
                        {
                            Response.Cookies["Admin"].Value = userClaims.Name;
                            Response.Cookies["Admin"].Expires = DateTime.Now.AddHours(timeout);
                        }
                        else if (currentUser.UserRole == "Moder")
                        {
                            Response.Cookies["Moder"].Value = userClaims.Name;
                            Response.Cookies["Moder"].Expires = DateTime.Now.AddHours(timeout);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такого пользователя не существует");
                }
            }

            return View(model);
        }
        #endregion

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

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            ClearCookie();

            return RedirectToAction("Index", "Home");
        }

        //GET, POST: Account/GetAdmin
        #region Получение прав администратора

        [MyAuth]
        public ActionResult GetAdmin()
        {
            Guid id = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);

            var user = _userRepository.GetItemById(id);

            var model = new GetRightsUserViewModel { Id = user.Id };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAdmin(GetRightsUserViewModel model)
        {
            const int timeCookie = 72;
            const int negativeTime = -73;

            if (ModelState.IsValid)
            {
                var user = _userRepository.GetItemById(model.Id);

                if (model.SecurityCode == "qm16po007fh")
                {
                    if (user.Password == model.Password)
                    {
                        user.UserRole = "Admin";

                        Response.Cookies["User"].Expires = DateTime.Now.AddHours(negativeTime);
                        Response.Cookies["Moder"].Expires = DateTime.Now.AddHours(negativeTime);

                        Response.Cookies["Admin"].Value = user.Email;
                        Response.Cookies["Admin"].Expires = DateTime.Now.AddHours(timeCookie);


                        _userRepository.Update(user);

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный код");
                }
            }

            return View(model);
        }
        #endregion

        //GET, POST: Account/GetModer
        #region Получение прав модератора

        [MyAuth]
        public ActionResult GetModer()
        {
            Guid id = Guid.Parse(HttpContext.Request.Cookies["Id"].Value);

            var user = _userRepository.GetItemById(id);

            var model = new GetRightsUserViewModel { Id = user.Id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetModer(GetRightsUserViewModel model)
        {
            const int timeCookie = 72;
            const int negativeTime = -73;

            if (ModelState.IsValid)
            {
                var user = _userRepository.GetItemById(model.Id);

                if (model.SecurityCode == "bmd78zl4r1")
                {
                    if (user.Password == model.Password)
                    {
                        user.UserRole = "Moder";
                        Response.Cookies["User"].Expires = DateTime.Now.AddHours(negativeTime);
                        Response.Cookies["Admin"].Expires = DateTime.Now.AddHours(negativeTime);

                        Response.Cookies["Moder"].Value = user.Email;
                        Response.Cookies["Moder"].Expires = DateTime.Now.AddMinutes(timeCookie);

                        _userRepository.Update(user);

                        return RedirectToAction("AccountIndex", "Manage");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неверный код");
                }
            }

            return View(model);
        }
        #endregion
    }
}
