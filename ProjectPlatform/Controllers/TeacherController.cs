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
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public TeacherController()
        {
        }

        public TeacherController(ApplicationUserManager userManager)
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

        // GET: Teacher
        public ActionResult Index()
        {
            return View(db.TeacherModels.ToList());
        }

        // GET: Teacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherModels teacherModels = db.TeacherModels.Find(id);
            if (teacherModels == null)
            {
                return HttpNotFound();
            }
            return View(teacherModels);
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> CreateAsync([Bind(Include = "Id,Email,Class,Subject,SubjectId")] TeacherModels teacherModels)
        {
            if (ModelState.IsValid)
            {
                db.TeacherModels.Add(teacherModels);
                db.SaveChanges();

                var user = new ApplicationUser { UserName = teacherModels.Email, Email = teacherModels.Email };
                var result = await UserManager.CreateAsync(user, "Divine123!");
                if (result.Succeeded)
                {
                    var InfoToAdd = new AccountInfoModels();

                    InfoToAdd.AccountId = user.Id;
                    InfoToAdd.Email = user.Email;
                    InfoToAdd.Role = "Teacher";

                    await UserManager.AddToRoleAsync(user.Id, "Teacher");

                    db.AccountInfoModels.Add(InfoToAdd);
                    db.SaveChanges();
                }

                    return RedirectToAction("Index");
            }

            return View(teacherModels);
        }

        // GET: Teacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherModels teacherModels = db.TeacherModels.Find(id);
            if (teacherModels == null)
            {
                return HttpNotFound();
            }
            return View(teacherModels);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Class,Subject,SubjectId")] TeacherModels teacherModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacherModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacherModels);
        }

        // GET: Teacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherModels teacherModels = db.TeacherModels.Find(id);
            if (teacherModels == null)
            {
                return HttpNotFound();
            }
            return View(teacherModels);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TeacherModels teacherModels = db.TeacherModels.Find(id);
            AccountInfoModels accountInfoModels = db.AccountInfoModels.Where(s => s.Email == teacherModels.Email).SingleOrDefault();

            db.TeacherModels.Remove(teacherModels);
            db.AccountInfoModels.Remove(accountInfoModels);

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
