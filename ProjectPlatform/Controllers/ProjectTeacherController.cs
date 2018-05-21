using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectPlatform.Models;

namespace ProjectPlatform.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ProjectTeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectStudent
        public ActionResult Index()
        {
            string user_name = User.Identity.GetUserName();

            var teacher = db.TeacherModels.Where(s => s.Email == user_name).SingleOrDefault();

            if (User.IsInRole("Admin"))
            {
                return View(db.ProjectStudentModels.ToList());
            }
            else
            {
                return View(db.ProjectStudentModels.Where(s => s.Class == teacher.Class).Where(s => s.Subject == teacher.Subject).ToList());
            }
        }

        // GET: ProjectStudent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectStudentModels projectStudentModels = db.ProjectStudentModels.Find(id);
            if (projectStudentModels == null)
            {
                return HttpNotFound();
            }

            var model = new PrijectFilesViewModels()
            {
                Project = projectStudentModels,

                Files = Directory.EnumerateFiles(projectStudentModels.FolderPath)
            };

            return View(model);
        }

        public FileResult DownloadFile(string Path, string Name)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Name);
        }

        public ActionResult Mark(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectStudentModels projectStudentModels = db.ProjectStudentModels.Find(id);
            if (projectStudentModels == null)
            {
                return HttpNotFound();
            }

            return View(projectStudentModels);
        }

        [HttpPost]
        public ActionResult Mark([Bind(Include = "Id,ProjectName,Subject,StudentEmail,Class,StudentName,Grade,FolderPath")] ProjectStudentModels projectStudentModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectStudentModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectStudentModels);
        }

        public ActionResult Comment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectStudentModels projectStudentModels = db.ProjectStudentModels.Find(id);

            var model = new ProjectCommentsViewModels()
            {
                Project = projectStudentModels,

                Comments = db.CommentModels.Where(m => m.ProjectId == id).ToList()
            };

            if (projectStudentModels == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Comment(string Content, int id)
        {
            string username = User.Identity.GetUserName();
            var commentModels = new CommentModels();
            commentModels.Content = Content;
            commentModels.PostDate = DateTime.Now;
            commentModels.UserEmail = username;

            string name = db.AccountInfoModels.Where(s => s.Email == username).SingleOrDefault().Name;
            if (name == null)
            {
                commentModels.UserName = username;
            }
            else
            {
                commentModels.UserName = name;
            }

            commentModels.ProjectId = id;
            db.CommentModels.Add(commentModels);
            db.SaveChanges();

            string redirectUrl = "/ProjectTeacher/Comment/" + id;
            return Redirect(redirectUrl);
        }
    }
}
