using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CMS_ShoppingCart.Models.Data;
using CMS_ShoppingCart.Models.ViewModels.Pages;

namespace CMS_ShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list of PageViewModel
            List<PageViewModel> PagesList;

            using (Db db = new Db())
            {            
                //Initialize the list
                PagesList = db.Pages.ToArray().OrderBy(x => x.Sort).Select(x=> new PageViewModel(x)).ToList();

            }
            //Return view with list
            return View(PagesList);
        }
        // GET: Admin/Pages/AddPages
        public ActionResult AddPage()
        {
            return View();
        }

        // GET: Admin/Pages/AddPages
        [HttpPost]
        public ActionResult AddPage(PageViewModel viewModel)
        {
            //Check model state
            if (! ModelState.IsValid)
            {
                return view(viewModel);
            }

            using (Db db = new Db())
            {
                //Declare slug
                string slug;

                //Initialize pageDTO
                PageDTO dto = new PageDTO();

                //DTO title
                dto.Title = viewModel.Title;

                //Check for and set slug if needed
                if (string.IsNullOrWhiteSpace(viewModel.Slug))
                {
                    slug = viewModel.Title.Replace("", "-").ToLower();
                }
                else
                {
                    slug = viewModel.Slug.Replace("", "-").ToLower();
                }
                //Make sure title and slug are unique
                if (db.Pages.Any(x=>x.Title == viewModel.Title) || db.Pages.Any(x=>x.Slug == slug))
                {
                    ModelState.AddModelError("", "Slug and title with this name already exists");
                    return View(viewModel);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = viewModel.Body;
                dto.Sidebar = viewModel.Sidebar;
                //Whenever you add a page will be the last page
                dto.Sort = 100;
                //Save the DTO
                db.Pages.Add(dto);
                db.SaveChanges();

            }
            //Set TempData message
            TempData["SM"] = "You have added a new page!";
            
            //Redirect


            return RedirectToAction("AddPage");
        }
    }
}