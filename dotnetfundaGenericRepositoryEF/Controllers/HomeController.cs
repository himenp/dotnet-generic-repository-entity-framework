using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dotnetfundaGenericRepositoryEF.Models;
namespace dotnetfundaGenericRepositoryEF.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Post> repository = null;
        private IRepository<Tag> tagrepository = null;
        private bool Success = false;
        public HomeController()
        {
            this.repository = new Repository<Post>();
            this.tagrepository = new Repository<Tag>();
        }
        public HomeController(IRepository<Post> repository, IRepository<Tag> tagrepository)
        {
            this.repository = repository;
            this.tagrepository = tagrepository;
        }
        public ActionResult Index()
        {
            try
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                var model = (List<Post>)repository.SelectAll();
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public ActionResult NewPost()
        {
            ViewBag.PageTitle = "Post | New Post";
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View("Post", new PostModel());
        }
        [HttpPost]
        public ActionResult NewPost(PostModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.PageTitle = "Post | New Post";
                    return View("Post", new PostModel());
                }

                var post = new Post
                {
                    Author = "Himen Patel",
                    Title = model.Title,
                    Content = model.Content,
                    CreatedDate = DateTime.UtcNow,
                    Tags = Utilities.ConvertToCollection(model),
                };
                repository.Insert(post);
                Success = repository.Save();
                if (Success)
                {
                    TempData["Message"] = "Post Successfully Created.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.PageTitle = "Post | New Post";
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.PageTitle = "Post | Edit Post";
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                Post post = repository.SelectByID(id);
                var model = new PostModel
                {
                    Title = post.Title,
                    Content = post.Content,
                    Tags = Utilities.ConvertCollectionToString(post.Tags),
                    PostId = id
                };
                return View("Post", model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(PostModel model, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Edit", new { id = id });
                }
                bool success = DeleteTagsByPostId(id);
                if (success)
                {
                    var post = new Post
                    {
                        PostId = id,
                        Author = "Himen Patel",
                        Title = model.Title,
                        Content = model.Content,
                        CreatedDate = DateTime.UtcNow,
                        Tags = Utilities.ConvertToCollection(model),
                    };
                    repository.Update(post);
                    Success = repository.Save();
                    if (Success)
                    {
                        TempData["Message"] = "Post Successfully Updated.";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("Edit", new { id = id });
            }

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                bool success = DeleteTagsByPostId(id);
                if (success)
                {
                    repository.Delete(id);
                    Success = repository.Save();
                    if (Success)
                    {
                        TempData["Message"] = "Post Successfully Deleted.";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [NonAction]
        public bool DeleteTagsByPostId(int id)
        {
            try
            {
                var tags = tagrepository.SelectAll().Where(t => t.PostId == id);
                foreach (var t in tags)
                {
                    tagrepository.Delete(t.TagId);
                }
                Success = tagrepository.Save();
                return Success; ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
