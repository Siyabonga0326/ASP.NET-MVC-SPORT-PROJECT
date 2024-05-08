using Microsoft.AspNetCore.Mvc;
using SportSelect.Models;

namespace SportSelect.Controllers
{
    public class LoginsController : Controller
    {
        public IActionResult Login()
        {
            return View(new Login()); // Pass an instance of Login to the view
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            // Check if the provided username and password match the admin credentials
            if (model.Username == "@Admin" && model.Password == "@321")
            {
                // Authentication successful, set session variable to indicate admin is logged in
                HttpContext.Session.SetString("IsAdminLoggedIn", "true");

                // Redirect to admin dashboard or any other page
                return RedirectToAction("Admin", "Students");
            }
            else
            {
                // Authentication failed, display error message
                ModelState.AddModelError("", "Invalid username or password");
                return View(model); // Return the login view with validation errors
            }
        }

        public IActionResult Logout()
        {
            // Clear the session variable to indicate admin is logged out
            HttpContext.Session.Remove("IsAdminLoggedIn");

            return RedirectToAction("Login");
        }
    }
}
