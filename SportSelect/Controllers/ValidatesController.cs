using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportSelect.Data;
using SportSelect.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace SportSelect.Controllers
{
    public class ValidatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ValidatesController> _logger;

        public ValidatesController(ApplicationDbContext context, ILogger<ValidatesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get the UserID of the current user
            var currentUserID = User.Identity.Name;

            // Query the Validates table to get data only for the current user
            var currentUserValidation = await _context.Validates
                .Where(v => v.UserID == currentUserID)
                .ToListAsync();

            return View(currentUserValidation);
        }



        // GET: Validates/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validate = await _context.Validates
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (validate == null)
            {
                return NotFound();
            }

            return View(validate);
        }

        // GET: Validates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Validates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Weight,Strength,Endurance,Passion,Determination,TeamWork,Age")] Validate validation)
        {
            try
            {
                // Get the UserID of the current user
                var currentUserID = User.Identity.Name;

                // Check if a validation record already exists for the current user
                var existingValidation = await _context.Validates.FirstOrDefaultAsync(v => v.UserID == currentUserID);

                if (existingValidation != null)
                {
                    // If a validation record already exists, return an error message or redirect as needed
                    TempData["ErrorMessage"] = "You have already been a validated, contact admin for further assistance";
                    return RedirectToAction(nameof(Create));
                }

                // Calculate total
                int total = validation.Strength + validation.Endurance + validation.Passion + validation.Determination + validation.TeamWork;

                // Set ValidationStatus
                string validationStatus = total >= 35 ? "Eligible" : "Not Eligible";

                // Set the UserID and ValidationStatus
                validation.UserID = currentUserID;
                validation.ValidationStatus = validationStatus;

                _context.Add(validation);
                await _context.SaveChangesAsync();
                // If a validation record already exists, return an error message or redirect as needed
                TempData["ErrorMessage"] = "You have succesfully validated and Eligible to participate in any sport";
                return RedirectToAction(nameof(Create));
             
            }
            catch (Exception ex)
            {
                // Log detailed error message
                _logger.LogError(ex, "An error occurred while creating a new validation entry.");

                // Redirect to an error page or display a generic error message
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Validates/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validate = await _context.Validates.FindAsync(id);
            if (validate == null)
            {
                return NotFound();
            }
            return View(validate);
        }

        // POST: Validates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserID,Weight,Strength,Endurance,Passion,Determination,TeamWork,ValidationStatus,Age")] Validate validate)
        {
            if (id != validate.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(validate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValidateExists(validate.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(validate);
        }

        // GET: Validates/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validate = await _context.Validates
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (validate == null)
            {
                return NotFound();
            }

            return View(validate);
        }

        // POST: Validates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var validate = await _context.Validates.FindAsync(id);
            if (validate != null)
            {
                _context.Validates.Remove(validate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValidateExists(string id)
        {
            return _context.Validates.Any(e => e.UserID == id);
        }
    }
}

