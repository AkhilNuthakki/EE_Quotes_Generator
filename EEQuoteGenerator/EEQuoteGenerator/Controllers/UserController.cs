using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EEQuoteGenerator.Models;
using EEQuoteGenerator.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace EEQuoteGenerator.Controllers
{
    public class UserController : Controller
    {
        private readonly EEDbContext _context;

        public UserController(EEDbContext context)
        {
            _context = context;
        }


        // GET: User/Login
        public IActionResult Login()
        {
            return View();
        }


        // POST: User/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserEmail,Password")] User LoggedInUser)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == LoggedInUser.UserEmail);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Email doesnot exists. Please create a new account.";
            }
            else
            {
                user.Password = EncryptionService.Decrypt(user.Password);
                if(user.Password != LoggedInUser.Password)
                {
                    ViewBag.ErrorMessage = "Incorrect password.";
                }
                else
                {
                    HttpContext.Session.SetString("UserId", user.UserId);
                   
                    return RedirectToAction("Index", "Home");
                }         
            }

            return View(LoggedInUser);
        }

        // GET: User/Create
        public IActionResult Create()
        {

            ViewData["RolesList"] = EEQuoteGenerator.Models.User.GetRolesList();
            return View();
        }


        public IActionResult Logout()
        {

            string UserId = HttpContext.Session.GetString("UserId");
            if (UserId == null)
            {
                return View("Login");
            }
            else
            {
                HttpContext.Session.Clear();
            }
            return View("Login");
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserEmail,FirstName,LastName,UserRole,Password")] User user)
        {


            ViewData["RolesList"] = EEQuoteGenerator.Models.User.GetRolesList();
            var DbUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
            if (DbUser == null)
            {
                
                user.Password = EncryptionService.Encrypt(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                ViewBag.ErrorMessage = "Please login with your registered details";
                return View("Login");
            }
            else
            {
                ViewBag.ErrorMessage = "Email already exists. Please click below to login";
            }

            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
