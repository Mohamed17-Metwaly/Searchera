using Microsoft.AspNetCore.Mvc;
using Searchera.Models;

namespace Searchera.Controllers
{
    public class ReviewController : Controller
    {
        JobBoardSystemContext jobBoardSystemContext;
        public ReviewController(JobBoardSystemContext jobBoardSystemContext)
        {
            this.jobBoardSystemContext = jobBoardSystemContext;
         }
        [HttpGet]
        public IActionResult Index()
        {
            List<Review> reviews = jobBoardSystemContext
                .Reviews.ToList();
            return View("Index",reviews);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Review review = jobBoardSystemContext
                .Reviews.FirstOrDefault(c => c.Id == id)!;
            if (review != null)
                return View("Details", review);
            return NotFound();
        }
        public IActionResult Add()
        {
            ViewData["UserId"] = jobBoardSystemContext.Users.ToList();
            ViewData["CompanyID"] = jobBoardSystemContext.Companies.ToList();
            return View("Add");
        }
        public IActionResult SaveAdd(Review review)
        {
            if (ModelState.IsValid==true)
            {
                jobBoardSystemContext.Reviews.Add(review);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = jobBoardSystemContext.Users.ToList();
            ViewData["CompanyID"] = jobBoardSystemContext.Companies.ToList();
            return View("Add", review);
        }
        public IActionResult Delete(int id)
        {
            Review review = jobBoardSystemContext
                .Reviews.FirstOrDefault(c => c.Id == id)!;
            if (review != null)
            {
                jobBoardSystemContext.Reviews.Remove(review);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
