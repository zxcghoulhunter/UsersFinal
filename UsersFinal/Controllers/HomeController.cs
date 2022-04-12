using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersChat.Models;
using UsersFinal.Models;

namespace UsersFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UsersContext db;
        public HomeController(UsersContext context)
        {
            db = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<User> obj = db.Users.ToList();
            var user = db.Users.FirstOrDefault(p => p.Email == HttpContext.User.Identity.Name);
            if (user.IsBanned)
            {
                Logout();
            }
            UserList objBind = new UserList();
            objBind.Users = obj;
            return View(objBind);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Index(UserList Obj, string action)
        {
            UserList userList = new UserList();
            userList.Users = new List<User>();
            foreach (var item in Obj.Users)
            {

                if (item.IsChecked)
                {
                    userList.Users.Add(item);
                }
            }
            if(action == "Block")
            {
                foreach(var item in userList.Users)
                {
                    Ban(item.Id);
                    if (HttpContext.User.Identity.Name == item.Email)
                    {
                        await Logout();
                    }
                }
            }
            else if(action == "Delete")
            {
                foreach(var item in userList.Users)
                {
                    Delete(item.Id);
                    if(HttpContext.User.Identity.Name == item.Name)
                    {
                       await Logout();
                    }
                }
            }
            else
            {
                foreach (var item in userList.Users)
                {
                    item.IsBanned = false;
                    db.Users.Update(item);
                    db.SaveChanges();
                }
            }
            string d = HttpContext.User.Identity.Name;
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void Delete(int? Id)
        {
            var obj = db.Users.Find(Id);
            db.Users.Remove(obj);
            db.SaveChanges();
        }
        public void Ban(int? Id)
        {
            var obj = db.Users.Find(Id);
            obj.IsBanned = true;
            db.Users.Update(obj);
            db.SaveChanges();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
