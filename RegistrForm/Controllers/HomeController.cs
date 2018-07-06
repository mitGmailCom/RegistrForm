using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrForm.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegistrForm.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        UserRepository repository = UserRepository.Instance;

        public static List<Role> Roles = new List<Role>();

        public ActionResult ToRole()
        {
            ViewBag.Users = repository.GetAll();
            ViewBag.LRoles = Roles;

            TempData["nicklist"] = ViewBag.LRoles;
            TempData["UsersList"] = repository.Users;
            return RedirectToAction("Index", new { controller = "Role"});
        }

        public ActionResult Index()
        {
            if (repository.Users.Count == 0)
            {
                repository.Users.Add(new User { Id = 0, FirstName = "Sergey", LastName = "Sergeevich", Login = "serg", Password = "1111111111", Email = "serg@gmail.com", Phone = "0978765555", Role = new Role { Id = 0, Name = "user" } });
                repository.Users.Add(new User { Id = 1, FirstName = "Petr", LastName = "Petrovich", Login = "petr", Password = "1111111111", Email = "petr@gmail.com", Phone = "0978765444", Role = new Role { Id = 1, Name = "admin" } });
            }

            if (Roles.Count == 0)
            {
                Roles.Add(new Role { Id = 0, Name = "user" });
                Roles.Add(new Role { Id = 1, Name = "admin" });
            }

            if (TempData["listroles"] != null)
            {
                Roles.Clear();
                Roles = (TempData["listroles"] as List<Role>).ToList();
                for (int i = 0; i < repository.Users.Count; i++)
                {
                    for (int j = 0; j < Roles.Count; j++)
                    {
                        if (repository.Users[i].Role.Id == Roles[j].Id)
                            repository.Users[i].Role.Name = Roles[j].Name;
                    }
                }
            }

            ViewBag.Users = repository.GetAll();
            ViewBag.LRoles = Roles;
            return View();
        }

        
        public ActionResult Create()
        {
            User model = new User();
            model.FirstName = "Your_Name";
            model.LastName = "Your_Surname";
            model.Login = "Your_Login";
            model.Password = "Your_Password";
            model.Email = "login@gmail.com";
            model.Phone = "0978887766";
            model.Role = null;
            ViewBag.LRoles = Roles;
            return View(model);
        }

        private void CheckIsNullOrEmpty(FormCollection _formcollection)
        {
            if (string.IsNullOrEmpty(_formcollection["FirstName"]))
            {
                ModelState.AddModelError("Name", "Invalid FirstName");
            }

            if (string.IsNullOrEmpty(_formcollection["LastName"]))
            {
                ModelState.AddModelError("Name", "Invalid LastName");
            }

            if (string.IsNullOrEmpty(_formcollection["Login"]))
            {
                ModelState.AddModelError("Name", "Invalid Login");
            }

            if (string.IsNullOrEmpty(_formcollection["Password"]))
            {
                ModelState.AddModelError("Name", "Invalid Password");
            }

            if (string.IsNullOrEmpty(_formcollection["Email"]))
            {
                ModelState.AddModelError("Name", "Invalid Email");
            }

            if (string.IsNullOrEmpty(_formcollection["Phone"]))
            {
                ModelState.AddModelError("Name", "Invalid Phone");
            }
        }

        private void CheckLength(FormCollection _formcollection)
        {
            if (_formcollection["Login"].Length < 4)
            {
                ModelState.AddModelError("Name", "Invalid length Login. Length must be >3");
            }

            if (_formcollection["Password"].Length < 8)
            {
                ModelState.AddModelError("Name", "Invalid length Password. Length must be >7");
            }
        }

        private bool CheckEmail(FormCollection _formcollection)
        {
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";

            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(_formcollection["Email"]))
                {
                    valid = false;
                }
            else
                valid = check.IsMatch(_formcollection["Email"]);
            return valid;
        }

        private bool CheckPhone(FormCollection _formcollection)
        {
            string pattern = @"\d{5}\d*";

            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(_formcollection["Phone"]))
            {
                valid = false;
            }
            else
                valid = check.IsMatch(_formcollection["Phone"]);
            return valid;
        }


        [HttpPost]
        public ActionResult Create(FormCollection formcollection)
        {
            CheckIsNullOrEmpty(formcollection);
            CheckLength(formcollection);
            if (!CheckEmail(formcollection))
                ModelState.AddModelError("Name", "Invalid Email");
            if (!CheckPhone(formcollection))
                ModelState.AddModelError("Name", "Invalid Phone");


            if (ModelState.IsValid)
            {
                string selectedRole = formcollection["Roles"];
                Role newUserRole = new Role();
                for (int i = 0; i < Roles.Count; i++)
                {
                    if (Roles[i].Id.ToString() == selectedRole)
                        newUserRole = Roles[i];
                }
                User newUser = new User()
                {
                    FirstName = formcollection["FirstName"],
                    LastName = formcollection["LastName"],
                    Login = formcollection["Login"],
                    Password = formcollection["Password"],
                    Email = formcollection["Email"],
                    Phone = formcollection["Phone"],
                    Role = newUserRole,
                    Id = 0
                };

                repository.Create(newUser, newUserRole);
                return RedirectToAction("Index", new { controller = "Home" });
            }
            ViewBag.Message = "Запрос не прошел валидацию";
            ViewBag.LRoles = Roles;
            return View();
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var existingUser = repository.Get((int)id);

            if (existingUser == null)
                return RedirectToAction("Index");

            ViewBag.LRoles = Roles;
            var selectList = new SelectList(Roles, "Id", "Name", existingUser.Role.Id);
            ViewBag.User = selectList;
            return View(existingUser);
        }


        [HttpPost]
        public ActionResult Edit(FormCollection formcollection)
        {
            CheckIsNullOrEmpty(formcollection);
            CheckLength(formcollection);
            if (!CheckEmail(formcollection))
                ModelState.AddModelError("Name", "Invalid Email");
            if (!CheckPhone(formcollection))
                ModelState.AddModelError("Name", "Invalid Phone");


            if (ModelState.IsValid)
            {
                string selectedRole = formcollection["Roles"];
                Role newUserRole = new Role();
                for (int i = 0; i < Roles.Count; i++)
                {
                    if (Roles[i].Id.ToString() == selectedRole)
                        newUserRole = Roles[i];
                }

                User newUser = new User()
                {
                    FirstName = formcollection["FirstName"],
                    LastName = formcollection["LastName"],
                    Login = formcollection["Login"],
                    Password = formcollection["Password"],
                    Email = formcollection["Email"],
                    Phone = formcollection["Phone"],
                    Role = newUserRole,
                    Id = Convert.ToInt32(formcollection["Id"])
                };

                repository.Edit(newUser.Id, newUser);
                return RedirectToAction("Index");
            }

            string selectedRole1 = formcollection["Roles"];
            Role newUserRole1 = new Role();
            for (int i = 0; i < Roles.Count; i++)
            {
                if (Roles[i].Id.ToString() == selectedRole1)
                    newUserRole1 = Roles[i];
            }

            User newUser1 = new User()
            {
                FirstName = formcollection["FirstName"],
                LastName = formcollection["LastName"],
                Login = formcollection["Login"],
                Password = formcollection["Password"],
                Email = formcollection["Email"],
                Phone = formcollection["Phone"],
                Role = newUserRole1,
                Id = Convert.ToInt32(formcollection["Id"])
            };

            ViewBag.Message = "Запрос не прошел валидацию";
            ViewBag.LRoles = Roles;
            return View(newUser1);
        }

       

        public ActionResult Delete(int id)
        {
            bool res = repository.Delete(id);
            TempData["ResForDel"] = res;
            return RedirectToAction("Index");
        }

    }
}