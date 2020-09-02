using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NonProfit.Models;

namespace NonProfit.Controllers
{
    public class HomeController : Controller
    {
        private NonProfitContext db;
        public HomeController(NonProfitContext context)
        {
            db = context;
        }
        private int? activeUser
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                // If Any already user exists that matches the email.
                if(db.Users.Any( user => user.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "is taken");
                    // To display newly added error message
                    return View("Index");
                }
            }
            if(ModelState.IsValid == false)
            {
                // To display the custom error message above IF it was added, or to display the other validation errors
                return View("Index");
            }

            // hash pw
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserName", newUser.FirstName);
            return RedirectToAction("Dashboard");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser loginUser)
        {
            // a vague error message should be used so we don't reveal to potential hackers if the email is registered in our db or not
            string genericErrorMsg = "Invalid credentials";
            if(ModelState.IsValid == false)
            {
                // display validation errors
                return View("Index");
            }

            User dbUser= db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);
            
            if(dbUser == null)
            {
                ModelState.AddModelError("LoginEmail", genericErrorMsg);
                // ModelState.AddModelError("LoginEmail", "Email not found");
                return View("Index");
            }

            // user was found because above return did not happen
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            // right click the PasswordVerficationResult and go to definition for more info
            PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

            if(pwCompareResult == 0)
            {
                ModelState.AddModelError("LoginEmail", genericErrorMsg);
                // ModelState.AddModelError("LoginEmail", "wrong password");
                return View("Index");
            }

            // no returns happen everything is good
            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            HttpContext.Session.SetString("UserName", dbUser.FirstName);
            return RedirectToAction("Dashboard");
        }
        
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            List<Volunteer> Volunteers = db.Volunteers.Include(vol => vol.Students).ToList();
            ViewBag.Students = db.Students.Include(vol => vol.Mentor).ToList();
            ViewBag.Time = db.Times.Include(time => time.Mentor).ToList();
            return View("Dashboard", Volunteers);
        }

        

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
