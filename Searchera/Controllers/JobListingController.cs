using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Searchera.Models;

namespace Searchera.Controllers
{
    public class JobListingController : Controller
    {
        JobBoardSystemContext jobBoardSystemContext;
        public JobListingController(JobBoardSystemContext jobBoardSystemContext)
        {
           this.jobBoardSystemContext = jobBoardSystemContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<JobListing> jobListings = jobBoardSystemContext
                .JobListings.ToList();
            if(jobListings!=null)
                return View("Index",jobListings);
            return NotFound();
        }
        
        public IActionResult Add()
        {
            ViewData["UserId"] = jobBoardSystemContext.Users.Where(x => x.Role.Contains("Admin") || x.Role.Contains("Employer")).ToList();
            ViewData["CompanyID"] =jobBoardSystemContext.Companies.ToList();
            return View("Add");
        }
        public IActionResult SaveAdd(JobListing jobListing)
        {
            jobListing.CreatedAt = DateTime.Now;
            if(jobListing.JobTypeId==1)
            {
                jobListing.JobType = "Full-Time";
            }
          else if (jobListing.JobTypeId == 2)
            {
                jobListing.JobType = "Part-Time";
            }
            else if (jobListing.JobTypeId == 3)
            {
                jobListing.JobType = "Internship";
            }
            else if (jobListing.JobTypeId == 4)
            {
                jobListing.JobType = "Contract";
            }
            else
            {
                jobListing.JobType = null;
            }

            if (jobListing.StatusId == 1)
            {
                jobListing.Status = "Pending";
            }
            else if (jobListing.StatusId == 2)
            {
                jobListing.Status = "Accepted";
            }
            else if (jobListing.StatusId == 3)
            {
                jobListing.Status = "Rejected";
            }
            if (ModelState.IsValid==true)
            {
                jobBoardSystemContext.JobListings.Add(jobListing);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = jobBoardSystemContext.Users.Where(x => x.Role.Contains("Admin") || x.Role.Contains("Employer")).ToList();
            ViewData["CompanyID"] = jobBoardSystemContext.Companies.ToList();
            return View("Add", jobListing);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            JobListing jobListing = jobBoardSystemContext.JobListings
                .FirstOrDefault(c => c.Id == id)!;
            return View("Details", jobListing);
        }
        public IActionResult Delete(int id)
        {
            var job = jobBoardSystemContext.JobListings
                .Include(x => x.User)
                .Include(x => x.Applications)
                .FirstOrDefault(c => c.Id == id);

            if (job != null)
            {
                jobBoardSystemContext.Set<Application>().RemoveRange(job.Applications);
                //jobBoardSystemContext.Set<User>().RemoveRange(job.User);
                jobBoardSystemContext.JobListings.Remove(job);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public IActionResult Search(string Query)
        {
            var joblis = jobBoardSystemContext.JobListings.Include(x => x.User).Include(x => x.Company).Where(x => x.Title.Contains(Query)).ToList();
            return View("SearchJob",joblis);
        }
        public IActionResult Edit(int id)
        {
            JobListing oldjob=jobBoardSystemContext.JobListings.FirstOrDefault(x=>x.Id == id);
            ViewData["UserId"] = jobBoardSystemContext.Users.Where(x => x.Role.Contains("Admin") || x.Role.Contains("Employer")).ToList();
            ViewData["CompanyID"] = jobBoardSystemContext.Companies.ToList();
            return View("EditJobListing",oldjob);   
        }
        public IActionResult SaveEdit(JobListing newjob,int id)
        {
            JobListing repjob = jobBoardSystemContext.JobListings.FirstOrDefault(x => x.Id == id);
            repjob.Title= newjob.Title;
            repjob.CompanyID = newjob.CompanyID;
            repjob.UserId = newjob.UserId;
            repjob.Locations = newjob.Locations;
            repjob.ExpiryDate = newjob.ExpiryDate;
            repjob.Discription=newjob.Discription;
            repjob.SalaryRange = newjob.SalaryRange;
            repjob.Requirement = newjob.Requirement;

            if (newjob.JobTypeId == 1)
            {
                repjob.JobType = "Full-Time";
            }
            else if (newjob.JobTypeId == 2)
            {
                repjob.JobType = "Part-Time";
            }
            else if (newjob.JobTypeId == 3)
            {
                repjob.JobType = "Internship";
            }
            else if (newjob.JobTypeId == 4)
            {
                repjob.JobType = "Contract";
            }

            if (newjob.StatusId == 1)
            {
                repjob.Status = "Pending";
            }
            else if (newjob.StatusId == 2)
            {
                repjob.Status = "Accepted";
            }
            else if (newjob.StatusId == 3)
            {
                repjob.Status = "Rejected";
            }
            if(ModelState.IsValid==true)
            {
                jobBoardSystemContext.JobListings.Update(repjob);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = jobBoardSystemContext.Users.Where(x => x.Role.Contains("Admin") || x.Role.Contains("Employer")).ToList();
            ViewData["CompanyID"] = jobBoardSystemContext.Companies.ToList();
            jobBoardSystemContext.JobListings.Update(repjob);
            jobBoardSystemContext.SaveChanges();
            return RedirectToAction("Index");

            return View("EditJobListing", newjob);

        }
    }
}
