﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectECommerce.Models;
using ProjectECommerce.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ECommerceContext _context;

        public LoginController(ECommerceContext context)
        {
            _context = context;
        }

        public IActionResult Index(LoginViewModel loginViewModel)
        {
            LoginViewModel viewModel = new LoginViewModel();
            var loginDetails = _context.Users.Where(x => loginViewModel.UserDetails != null && x.Username == loginViewModel.UserDetails.Username && x.Password == loginViewModel.UserDetails.Password).FirstOrDefault();
            if (loginDetails != null)
            {
                Response.Cookies.Append("UserId", loginDetails.Id.ToString());
                return RedirectToAction("Index", "Home");
            }
            else if (loginDetails == null && loginViewModel.UserDetails != null)
            {
                viewModel.ErrorMessage = "Invalid Username or password";
                viewModel.UserDetails = null;
            }
            return View(viewModel);
        }

        public IActionResult Registration(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                var isUserExist = _context.Users.Where(x => x.Username == user.Username);
                if (isUserExist.Count() > 0)
                {
                    user.ErrorMessage = "Username already exist.";
                    return View(user);
                }
                User userDetails = new User();
                userDetails.Username = user.Username;
                userDetails.Password = user.Password;
                userDetails.Name = user.Name;
                userDetails.EmailId = user.Username;
                userDetails.MobileNumber = user.MobileNumber;
                userDetails.DateOfBirth = user.DateOfBirth;
                _context.Users.Add(userDetails);
                _context.SaveChanges();
                Response.Cookies.Append("UserId", userDetails.Id.ToString());
                return RedirectToAction("Index", "Home");
            }

            ModelState.Clear();
            return View("Registration");
        }
    }
}
