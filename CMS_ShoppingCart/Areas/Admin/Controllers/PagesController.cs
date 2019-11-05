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
        // GET: Admin Pages
        public ActionResult Index()
        {
            //Declare list of PageViewModel
            List<PageViewModel> PagesList;

            using (Db db = new Db())
            {
                //Initialize the list
                PagesList = db.Pages.ToArray().OrderBy(x => x.Sort).Select(x => new PageViewModel(x)).ToList();

            }
            //Return view with list
            return View(PagesList);
        }
        // GET: Admin AddPages
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin AddPages
        [HttpPost]
        public ActionResult AddPage(PageViewModel viewModel)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(viewModel);
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
                    slug = viewModel.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = viewModel.Slug.Replace(" ", "-").ToLower();
                }
                //Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == viewModel.Title) || db.Pages.Any(x => x.Slug == slug))
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

        // GET: Admin EditPage by id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declaring pageVM
            PageViewModel pageViewModel;
            using (Db db = new Db())
            {
                //Getting the page
                PageDTO pDTO = db.Pages.Find(id);
                //Confirming if the page exists
                if (pDTO == null)
                {
                    return Content("The page does not exist. ");
                }

                //Intializing pageVM
                pageViewModel = new PageViewModel(pDTO);
            }

            //Returning view with model
            return View(pageViewModel);
        }

        //POST: Admin EditPage by id
        [HttpPost]
        public ActionResult EditPage(PageViewModel pageViewModel)
        {
            //Check for model state
            if (!ModelState.IsValid)
            {
                return View(pageViewModel);
            }
            using (Db db = new Db())
            {
                //Get page id
                int id = pageViewModel.Id;


                //Initializing slug
                string slug = "home";
                //Getting the page
                PageDTO pageDTO = db.Pages.Find(id);

                //DTO the title
                pageDTO.Title = pageViewModel.Title;
                //Checking for slug and set it if needed
                if (pageViewModel.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(pageViewModel.Slug))
                    {
                        slug = pageViewModel.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = pageViewModel.Slug.Replace(" ", "-").ToLower();

                    }

                }
                //Make sure that the title and slug are unique

                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == pageViewModel.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))

                {
                    ModelState.AddModelError("", "The title or slug by this name already exists");
                    return View(pageViewModel);
                }

                //DTO the rest
                pageDTO.Slug = slug;
                pageDTO.Body = pageViewModel.Body;
                pageDTO.Sidebar = pageViewModel.Sidebar;


                //Save the DTO
                db.SaveChanges();
                //Set TempData message
                TempData["SM"] = "page edited successfully";
                //Redirect
                return RedirectToAction("EditPage");
            }


        }

        // GET: Admin PageDetails by id
        public ActionResult PageDetails(int id)
        {
            //Declaring PageViewModel
            PageViewModel pageViewModel;
            using (Db db = new Db())
            {
                //Getting the page
                PageDTO dto = db.Pages.Find(id);
                //Confirming if page exists
                if (dto == null)
                {
                    return Content("This page does not exist.");
                }
                //Initializing PageViewModel
                //pageViewModel = new PageViewModel(dto)
                //paged

            }



            //Returning view with model


            return View();
        }


    }
}