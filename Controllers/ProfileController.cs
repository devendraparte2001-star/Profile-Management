using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfileManagement.DAL;
using ProfileManagement.Models;

namespace ProfileManagement.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserDAL userDAL = new UserDAL();

        // GET: Profile/Index
        public ActionResult Index()
        {
            var users = userDAL.GetAllUsers();
            return View(users);
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                if (userDAL.UsernameExists(user.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }

                if (userDAL.EmailExists(user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(user);
                }

                bool result = userDAL.CreateUser(user);

                if (result)
                {
                    TempData["Success"] = "Profile created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create profile");
                }
            }

            return View(user);
        }

        // GET: Profile/Edit/5
        public ActionResult Edit(int id)
        {
            User user = userDAL.GetUserById(id);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                if (userDAL.UsernameExists(user.Username, user.UserId))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }

                if (userDAL.EmailExists(user.Email, user.UserId))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(user);
                }

                bool result = userDAL.UpdateUser(user);

                if (result)
                {
                    TempData["Success"] = "Profile updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update profile");
                }
            }

            return View(user);
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            User user = userDAL.GetUserById(id);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool result = userDAL.DeleteUser(id);

            if (result)
            {
                TempData["Success"] = "Profile deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete profile";
            }

            return RedirectToAction("Index");
        }

        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            User user = userDAL.GetUserById(id);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            return View(user);
        }
    }
}