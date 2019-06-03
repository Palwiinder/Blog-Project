using BlogAssignment.Models;
using BlogAssignment.Models.Domain;
using BlogAssignment.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogAssignment.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext DbContext;

        public HomeController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var model = DbContext.PostDatabase
                .Select(p => new IndexPostViewModel
                {
                    Title = p.Title,
                    Body = p.Body,
                    Published = p.Published,
                    DateCreated = p.DateCreated,
                    DateUpdated = p.DateUpdated,
                    MediaUrl = p.MediaUrl,
                    User = p.User,
                    PostId = p.PostId
                }).ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            //PopulateViewBag();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CreatePostViewModel formData)
        {
            return SavePost(null, formData);
        }

        private ActionResult SavePost(int? id, CreatePostViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string fileExtension;

            //Validating file upload
            if (formData.Media != null)
            {
                fileExtension = Path.GetExtension(formData.Media.FileName);

                if (!Constants.AllowedFileExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("", "File extension is not allowed.");

                    return View();
                }
            }


            Post post;

            if (!id.HasValue)
            {
                post = new Post();
                DbContext.PostDatabase.Add(post);
            }
            else
            {
                post = DbContext.PostDatabase.FirstOrDefault(p => p.PostId == id);

                if (post == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            post.Title = formData.Title;
            post.Body = formData.Body;
            post.Published = formData.Published;
            post.DateUpdated = DateTime.Now;

            if (formData.Media != null)
            {
                if (!Directory.Exists(Constants.MappedUploadFolder))
                {
                    Directory.CreateDirectory(Constants.MappedUploadFolder);
                }

                var fileName = formData.Media.FileName;
                var fullPathWithName = Constants.MappedUploadFolder + fileName;

                formData.Media.SaveAs(fullPathWithName);

                post.MediaUrl = Constants.UploadFolder + fileName;
            }

            DbContext.SaveChanges();

            return RedirectToAction(nameof(HomeController.Index));
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            var post = DbContext.PostDatabase.FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            var model = new CreatePostViewModel();
            model.Title = post.Title;
            model.Body = post.Body;
            model.DateCreated = post.DateUpdated;
            model.MediaUrl = post.MediaUrl;

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, CreatePostViewModel formData)
        {
            return SavePost(id, formData);
        }

        [HttpGet]
        public ActionResult fullDetail(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            var post = DbContext.PostDatabase.FirstOrDefault(p => p.PostId == id);
            if (post == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            var model = new ShowPostsViewModel()
            {
                Title = post.Title,
                Body = post.Body,
                DateCreated = post.DateCreated,
                MediaUrl = post.MediaUrl
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            var post = DbContext.PostDatabase.FirstOrDefault(p => p.PostId == id);
            if (post != null)
            {
                DbContext.PostDatabase.Remove(post);
                DbContext.SaveChanges();
            }
            return RedirectToAction(nameof(HomeController.Index));
        }
    }
}




