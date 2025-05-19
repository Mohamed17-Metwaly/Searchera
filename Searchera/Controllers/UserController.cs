using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Searchera.Models;
using Searchera.ViewModels;

namespace Searchera.Controllers
{
    public class UserController : Controller
    {
        JobBoardSystemContext jobBoardSystemContext;
        public UserController(JobBoardSystemContext jobBoardSystemContext)
        {
               this.jobBoardSystemContext = jobBoardSystemContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<User> users = jobBoardSystemContext
                .Users.ToList();
            if(users!=null)
                return View("Index", users);
            return NotFound();
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            User user = jobBoardSystemContext
                .Users.FirstOrDefault(c => c.Id == id)!;
            if (user != null)
                return View("Details", user);
            return NotFound();
        }
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        public IActionResult SaveAdd(User user)
        {

            if (ModelState.IsValid == true)
            {

                if (user.ProfileImageFile != null)
                {
                    string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.ProfileImageFile.FileName);
                    string fullPath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        user.ProfileImageFile.CopyTo(stream);
                    }

                    user.ProfileImage = fileName;
                }
                if (user.RoleID == 1)
                {
                    user.Role = "Admin";

                }
                else if (user.RoleID == 2)
                {
                    user.Role = "Job Seeker";
                }
                else if (user.RoleID == 3)
                {
                    user.Role = "Employer";
                }
                jobBoardSystemContext.Users.Add(user);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Add", user);
        }
        public IActionResult Delete(int id)
        {
            User user = jobBoardSystemContext.Users
                .Include(u => u.Applications)
                .Include(u => u.Companies)
                .Include(u => u.JobListings)
                .Include(u => u.Reviews)
                .FirstOrDefault(c => c.Id == id)!;
            if (user != null)
            {
                jobBoardSystemContext.Set<Application>().RemoveRange(user.Applications);
                jobBoardSystemContext.Set<Company>().RemoveRange(user.Companies);
                jobBoardSystemContext.Set<JobListing>().RemoveRange(user.JobListings);
                jobBoardSystemContext.Set<Review>().RemoveRange(user.Reviews);
                jobBoardSystemContext.Users.Remove(user);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        
        public IActionResult Search(string Query)
        {
            List<User> users=jobBoardSystemContext.Users.Where(x=> x.Name.Contains(Query)).ToList();
            return View("SearchUsers",users);
        }
        public IActionResult Edit(int id)
        {
            User olduser = jobBoardSystemContext.Users.FirstOrDefault(x => x.Id == id);
            UserViewModel model = new UserViewModel();
            model.Name = olduser.Name;
            model.Email = olduser.Email;
            model.Password=olduser.Password;
            model.ProfileImage = olduser.ProfileImage;
            model.Role = olduser.Role;
            return View("Edit", model);
        }
        public IActionResult SaveEdit(UserViewModel user,int id)
        {
            User olduser = jobBoardSystemContext.Users.FirstOrDefault(x => x.Id == id);
            olduser.Name = user.Name;
            olduser.Email = user.Email;
            olduser.Password = user.Password;
            olduser.ProfileImage = user.ProfileImage;
            if (ModelState.IsValid == true)
            {
                if (user.ProfileImageFile != null)
                {
                    string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.ProfileImageFile.FileName);
                    string fullPath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                        user.ProfileImageFile.CopyTo(stream);
                    user.ProfileImage = fileName;
                }
                if (user.RoleID == 1)
                    user.Role = "Admin";
                else if (user.RoleID == 2)
                    user.Role = "Job Seeker";
                else if (user.RoleID == 3)
                    user.Role = "Employer";
                jobBoardSystemContext.Users.Update(olduser);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            jobBoardSystemContext.Users.Update(olduser);
            jobBoardSystemContext.SaveChanges();
            return RedirectToAction("Index");

            //return View("Edit", user);
        }
    }
}