using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Searchera.Models;

namespace Searchera.Controllers
{
    public class ApplicationController : Controller
    {
        JobBoardSystemContext jobBoardSystemContext;
        public ApplicationController(JobBoardSystemContext jobBoardSystemContext)
        {
            this.jobBoardSystemContext =jobBoardSystemContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Application> applications = jobBoardSystemContext
                .Applications.ToList();
                return View("Index", applications);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Application application = jobBoardSystemContext
                .Applications.FirstOrDefault(c => c.Id == id)!;
            if (application != null)
                return View("Details", application);
            return NotFound();
        }
        public IActionResult Add()
        {
            ViewData["JobID"] = jobBoardSystemContext.JobListings.ToList();
            ViewData["UserId"] = jobBoardSystemContext.Users.Where(x=>x.Role.Contains("Job Seeker")).ToList();
            return View("Add");
        }
        public IActionResult SaveAdd(Application application)
        {
            
            if (ModelState.IsValid==true)
            {
                jobBoardSystemContext.Applications.Add(application);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["JobID"] = jobBoardSystemContext.JobListings.ToList();
            ViewData["UserId"] = jobBoardSystemContext.Users.ToList();
            return View("Add", application);
        }
        public IActionResult Delete(int id)
        {
            Application application = jobBoardSystemContext.Applications
                .FirstOrDefault(c => c.Id == id)!;
            if (application != null)
            {
                jobBoardSystemContext.Applications.Remove(application);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
