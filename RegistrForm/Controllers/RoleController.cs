using RegistrForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistrForm.Controllers
{
    public class RoleController : Controller
    {
        RoleRepository roleRepository = RoleRepository.Instance;

        static List<Role> Roles = new List<Role>();
        static List<User> Users = new List<Models.User>();


        public ActionResult Index()
        {
            var listUsers = TempData["UsersList"];
            if (listUsers != null)
            {
                Users.Clear();
                Users = listUsers as List<User>;
            }
            var listTempData = TempData["nicklist"];
            if(listTempData is List<Role>)
            {
                for (int i = 0; i < (listTempData as List<Role>).Count; i++)
                {
                    Roles.Add((listTempData as List<Role>)[i]);
                }
            }

            ViewBag.listOfRoles = Roles;
            return View();
        }



        public ActionResult ToUsersList()
        {
            TempData["listroles"] = Roles;
            return RedirectToAction("Index", new { controller = "Home" });
        }



        public ActionResult Create()
        {
            Role model = new Role();
            model.Name = "new_role";
            return View(model);
        }


        private void CheckIsNullOrEmpty(Role _name)
        {
            if (string.IsNullOrEmpty(_name.Name))
            {
                ModelState.AddModelError("Name", "Invalid Name of Role");
            }
        }

        [HttpPost]
        public ActionResult Create(Role _name)
        {
            CheckIsNullOrEmpty(_name);

            if (ModelState.IsValid)
            {
                Role newRole = new Role();
                newRole = _name;
                roleRepository.Create(newRole, Roles);
                return RedirectToAction("Index", new { controller = "Role" });
            }
            ViewBag.Message = "Non Valid";
            return RedirectToAction("Index", new { controller = "Role" });
        }


        public ActionResult Delete(int id)
        {
            bool res = roleRepository.Delete(id, Roles, Users);
            TempData["ResForDelRole"] = res;
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var existingUser = roleRepository.Get((int)id, Roles);

            if (existingUser == null)
                return RedirectToAction("Index");

            ViewBag.LRoles = Roles;

            return View(existingUser);
        }


        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                Role newRole = new Role()
                {
                    Name = role.Name,
                    Id = role.Id
                };

                roleRepository.Edit(newRole.Id, newRole, Roles);
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Non Valid";
            return RedirectToAction("Index");
        }
    }
}