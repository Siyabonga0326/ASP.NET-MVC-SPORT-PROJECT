using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportSelect.Data;
using SportSelect.Models;

namespace SportSelect.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ApplicationDbContext context, ILogger<StudentsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [Authorize]
        // GET: Students
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Get the UserID of the current user
            var currentUserID = User.Identity.Name;

            // Query the Students table to get data only for the current user
            var currentUserStudents = await _context.Students
                .Include(s => s.Validates)
                .Where(s => s.UserID == currentUserID)
                .ToListAsync();

            return View(currentUserStudents);
        }


        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Validates)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        // GET: Students/Create
        // GET: Students/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            // Check if the current user has a validation entry
            var userValidation = await _context.Validates.FirstOrDefaultAsync(v => v.UserID == User.Identity.Name);

            if (userValidation == null)
            {
                TempData["ErrorMessage"] = "You need to first create a validation entry.";
                return RedirectToAction(nameof(Index));
            }

            // Check if the user's validation status is "Eligible"
            if (userValidation.ValidationStatus != "Eligible")
            {
                TempData["ErrorMessage"] = "You are not eligible to create a student record.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserID"] = new SelectList(_context.Validates, "UserID", "UserID");
            return View();
        }


        // POST: Students/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Age,PhoneNumber,SportSelected,Experience,ApplicationStatus,ValidationStatus,UserID")] Student students)
        {
            try
            {
                // Set student properties
                students.UserID = User.Identity.Name;
                students.ApplicationStatus = "--pending--";
                students.ValidationStatus = "Eligible";

                // Add student to context and save changes
                _context.Add(students);
                await _context.SaveChangesAsync();

                TempData["ErrorMessage"] = "You have succesfully made an application";
                return RedirectToAction(nameof(Create));
            }
            catch (Exception ex)
            {
                // Log detailed error message
                _logger.LogError(ex, "An error occurred while creating a new student entry.");

                // Redirect to an error page or display a generic error message
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }



        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Validates, "UserID", "UserID", student.UserID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Age,PhoneNumber,SportSelected,Experience,ApplicationStatus,ValidationStatus,UserID")] Student student)
        {
            try
            {
                if (id != student.Id)
                {
                    return NotFound();
                }

                // Set student properties
                student.UserID = User.Identity.Name;
                student.ApplicationStatus = "--pending--";
                student.ValidationStatus = "Eligible";

                // Update student in context and save changes
                _context.Update(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log detailed error message
                _logger.LogError(ex, "An error occurred while updating the student entry.");

                // Redirect to an error page or display a generic error message
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Validates)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
        public IActionResult About()
        {
            return View();
        }
        [Authorize]
        public IActionResult Announcement()
        {
            return View();
        }
        public IActionResult FAQ()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Selected()
        {
            // Query students with ApplicationStatus "Approved"
            var selectedStudents = await _context.Students
                .Where(s => s.ApplicationStatus == "Successful")
                .ToListAsync();

            return View(selectedStudents);
        }
        [Authorize]
        public IActionResult ViewSport()
        {
            return View();
        }
        public async Task<IActionResult> Admin()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }
        // POST: Students/Approve
        [HttpPost]
        public async Task<IActionResult> Approve(string userId, string sport)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserID == userId && s.SportSelected == sport);
                if (student != null)
                {
                    student.ApplicationStatus = "Successful";
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return Ok("Application approved successfully.");
                }
                else
                {
                    return NotFound("Student not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the application.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: Students/Reject
        [HttpPost]
        public async Task<IActionResult> Reject(string userId, string sport)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserID == userId && s.SportSelected == sport);
                if (student != null)
                {
                    student.ApplicationStatus = "Unsuccessful";
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return Ok("Application rejected successfully.");
                }
                else
                {
                    return NotFound("Student not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while rejecting the application.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: Students/DeleteApplication
        [HttpPost]
        public async Task<IActionResult> DeleteApplication(string userId, string sport)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserID == userId && s.SportSelected == sport);
                if (student != null)
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                    return Ok("Application deleted successfully.");
                }
                else
                {
                    return NotFound("Student not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the application.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        public IActionResult Search()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> SeEarch(string sportSelected)
        //{
        //    if (string.IsNullOrEmpty(sportSelected))
        //    {
        //        TempData["ErrorMessage"] = "Please select a sport.";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    // Find the application with the selected sport for the current user
        //    var currentUserID = User.Identity.Name;
        //    var student = await _context.Students.FirstOrDefaultAsync(s => s.UserID == currentUserID && s.SportSelected == sportSelected);

        //    if (student == null)
        //    {
        //        TempData["ErrorMessage"] = "No application found for the selected sport.";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    // Pass the student model to the Details view
        //    return RedirectToAction("Details", "Students", new { id = student.Id });
        //}





    }
}
