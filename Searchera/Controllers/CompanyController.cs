using Microsoft.AspNetCore.Mvc;
using Searchera.Models;
using Searchera.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Searchera.Controllers
{
    public class CompanyController : Controller
    {
        string logo;
        JobBoardSystemContext jobBoardSystemContext;
        public CompanyController(JobBoardSystemContext jobBoardSystemContext)
        {
            this.jobBoardSystemContext =jobBoardSystemContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Company> company = jobBoardSystemContext
                .Companies.ToList();
            if(company == null)
                return NotFound();
            return View("Index",company);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Company company = jobBoardSystemContext
                .Companies.FirstOrDefault(c => c.Id == id)!;
            if(company!=null)
                 return View("Details", company);
            return NotFound();
        }
        public IActionResult Add()
        {
            ViewData["UserId"] = jobBoardSystemContext.Users.ToList();
            return View("Add");
        }   
        public IActionResult SaveAdd(Company company)
        {
            company.CreatedAt = DateTime.Now;
            if (company.LogoProfile != null)
            {
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Logo");
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(company.LogoProfile.FileName);
                string fullPath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    company.LogoProfile.CopyTo(stream);
                }

                company.Logo = fileName;
            }
            if (ModelState.IsValid==true)
            {
                jobBoardSystemContext.Companies.Add(company);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = jobBoardSystemContext.Users.ToList();
            return View("Add", company);
        }
        public IActionResult Delete(int id)
        {
            Company company = jobBoardSystemContext
                .Companies.FirstOrDefault(c => c.Id == id)!;
            if (company != null)
            {
                jobBoardSystemContext.Companies.Remove(company);
                jobBoardSystemContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Search(string Query)
        {
            var searchcomp = jobBoardSystemContext.Companies.Where(x => x.Name.Contains(Query)).ToList();
            return View("SearchCompany", searchcomp);
        }
        public IActionResult Edit(int id)
        {
            Company oldcomp=jobBoardSystemContext.Companies.FirstOrDefault(x => x.Id == id);
            CompanyViewModel model=new CompanyViewModel();
            model.Id = id;
            model.UserId = oldcomp.UserId;
            model.Name = oldcomp.Name;
            model.Logo= oldcomp.Logo;
            model.Discription = oldcomp.Discription;
            model.Locations = oldcomp.Locations;
            model.CreatedAt = oldcomp.CreatedAt;
            model.Email = oldcomp.Email;
            model.Industry = oldcomp.Industry;
            model.Password = oldcomp.Password;
            model.Wepsite = oldcomp.Wepsite;
            ViewData["UserId"] = jobBoardSystemContext.Users.ToList();
            return View("EditCompany", model);
        }
        public IActionResult SaveEdit(CompanyViewModel newcomp, int id)
        {

            Company repcomp = jobBoardSystemContext.Companies.FirstOrDefault(x => x.Id == id);
            repcomp.CreatedAt=DateTime.Now;
            repcomp.Discription = newcomp.Discription;
            repcomp.Name = newcomp.Name;
            repcomp.UserId = newcomp.UserId;
            repcomp.Wepsite = newcomp.Wepsite;
            repcomp.Locations = newcomp.Locations;
            repcomp.Email = newcomp.Email;
            repcomp.Password = newcomp.Password;
            repcomp.Industry = newcomp.Industry;
            repcomp.Logo = newcomp.Logo;
            if (ModelState.IsValid == true)
            {
                if (newcomp.LogoProfile != null)
                {
                    string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Logo");
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(repcomp.LogoProfile.FileName);
                    string fullPath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        repcomp.LogoProfile.CopyTo(stream);
                    }

                    repcomp.Logo = fileName;
                }
                jobBoardSystemContext.Companies.Update(repcomp);
                jobBoardSystemContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = jobBoardSystemContext.Users.ToList();
            return View("EditCompany", newcomp);
        }
    }
}
