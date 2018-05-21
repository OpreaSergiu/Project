using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProjectPlatform.Models;

namespace ProjectPlatform.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public StudentController()
        {
        }

        public StudentController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Student
        public ActionResult Index()
        {
            return View(db.StudentModels.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentModels studentModels = db.StudentModels.Find(id);
            if (studentModels == null)
            {
                return HttpNotFound();
            }
            return View(studentModels);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> CreateAsync([Bind(Include = "Id,Email,Name,Class")] StudentModels studentModels)
        {
            if (ModelState.IsValid)
            {
                db.StudentModels.Add(studentModels);
                db.SaveChanges();

                var user = new ApplicationUser { UserName = studentModels.Email, Email = studentModels.Email };
                var result = await UserManager.CreateAsync(user, "Divine123!");
                if (result.Succeeded)
                {
                    var InfoToAdd = new AccountInfoModels();

                    InfoToAdd.AccountId = user.Id;
                    InfoToAdd.Email = user.Email;
                    InfoToAdd.Name = studentModels.Name;
                    InfoToAdd.Role = "Student";

                    await UserManager.AddToRoleAsync(user.Id, "Student");

                    db.AccountInfoModels.Add(InfoToAdd);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(studentModels);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentModels studentModels = db.StudentModels.Find(id);
            if (studentModels == null)
            {
                return HttpNotFound();
            }
            return View(studentModels);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Name,Class")] StudentModels studentModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentModels);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentModels studentModels = db.StudentModels.Find(id);
            if (studentModels == null)
            {
                return HttpNotFound();
            }
            return View(studentModels);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentModels studentModels = db.StudentModels.Find(id);
            AccountInfoModels accountInfoModels = db.AccountInfoModels.Where(s => s.Email == studentModels.Email).SingleOrDefault();

            db.AccountInfoModels.Remove(accountInfoModels);
            db.StudentModels.Remove(studentModels);
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
    }
}
