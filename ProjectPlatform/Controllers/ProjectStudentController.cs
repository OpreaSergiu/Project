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
    [Authorize(Roles = "Student")]
    public class ProjectStudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectStudent
        public ActionResult Index()
        {
            string user_name = User.Identity.GetUserName();

            if (User.IsInRole("Admin"))
            {
                return View(db.ProjectStudentModels.ToList());
            }
            else
            {
                return View(db.ProjectStudentModels.Where(s => s.StudentEmail == user_name).ToList());
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

            string user_name = User.Identity.GetUserName();
            if (projectStudentModels.StudentEmail != user_name)
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

        public ActionResult DeleteFile(string Path, int Id)
        {
            ProjectStudentModels projectStudentModels = db.ProjectStudentModels.Find(Id);
            string user_name = User.Identity.GetUserName();
            if (projectStudentModels.StudentEmail != user_name)
            {
                return HttpNotFound();
            }

            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }

            string redirectUrl = "/ProjectStudent/Details/" + Id;
            return Redirect(redirectUrl);
        }

        // GET: ProjectStudent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectStudent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectName,Subject")] ProjectStudentModels projectStudentModels)
        {
            if (ModelState.IsValid)
            {
                string user_name = User.Identity.GetUserName();

                string project_name = projectStudentModels.ProjectName;

                string project_subject = projectStudentModels.Subject;

                projectStudentModels.StudentEmail = user_name;

                string Class = db.StudentModels.Where(s => s.Email == user_name).SingleOrDefault().Class;

                projectStudentModels.Class = Class;

                string Name = db.AccountInfoModels.Where(s => s.Email == user_name).SingleOrDefault().Name;

                projectStudentModels.StudentName = Name;

                string path = Server.MapPath("~/Projects/") + user_name + "/";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                db.ProjectStudentModels.Add(projectStudentModels);
                db.SaveChanges();

                var project = db.ProjectStudentModels.Where(s => s.StudentEmail == user_name).Where(b => b.ProjectName == project_name).Where(c => c.Subject == project_subject).SingleOrDefault();

                string path1 = Server.MapPath("~/Projects/") + user_name + "/" + project.Id + "/";

                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }

                project.FolderPath = path1;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(projectStudentModels);
        }

        [HttpGet]
        public ActionResult UploadFiles(int Id)
        {
            ProjectStudentModels projectStudentModels = db.ProjectStudentModels.Find(Id);
            string user_name = User.Identity.GetUserName();
            if (projectStudentModels.StudentEmail != user_name)
            {
                return HttpNotFound();
            }

            return View();
        }


        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase postedFile, int Id)
        {
            if (postedFile != null)
            {
                string folder_path = db.ProjectStudentModels.Find(Id).FolderPath;

                if (!Directory.Exists(folder_path))
                {
                    Directory.CreateDirectory(folder_path);
                }

                postedFile.SaveAs(folder_path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }

            return View();
        }

        // GET: ProjectStudent/Edit/5
        public ActionResult Edit(int? id)
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
            string user_name = User.Identity.GetUserName();
            if (projectStudentModels.StudentEmail != user_name)
            {
                return HttpNotFound();
            }

            return View(projectStudentModels);
        }

        // POST: ProjectStudent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectName,Subject,StudentEmail,Class,StudentName,Grade,FolderPath")] ProjectStudentModels projectStudentModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectStudentModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectStudentModels);
        }

        // GET: ProjectStudent/Delete/5
        public ActionResult Delete(int? id)
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
            string user_name = User.Identity.GetUserName();
            if (projectStudentModels.StudentEmail != user_name)
            {
                return HttpNotFound();
            }

            return View(projectStudentModels);
        }

        // POST: ProjectStudent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectStudentModels projectStudentModels = db.ProjectStudentModels.Find(id);

            if (Directory.Exists(projectStudentModels.FolderPath))
            {
                Directory.Delete(projectStudentModels.FolderPath);
            }

            db.ProjectStudentModels.Remove(projectStudentModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Comment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectStudentModels projectStudentModels = db.ProjectStudentModels.Find(id);

            string user_name = User.Identity.GetUserName();
            if (projectStudentModels.StudentEmail != user_name)
            {
                return HttpNotFound();
            }

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
            if( name == null)
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

            string redirectUrl = "/ProjectStudent/Comment/" + id;
            return Redirect(redirectUrl);
        }
    }
}
