using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class LoginController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Login(LoginModel model)
        {
            User user = db.Users.Where(o => o.Email.Equals(model.Email)).Single();

            if(user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Login = "Password/Email not a match";
            if(user != null)
            {
                if(user.Password.CompareTo(model.Password) == 0)
                {

                    ViewBag.Login = "Sucessfull";
                    ViewBag.FirstName = user.FirstName;
                    ViewBag.LastName = user.LastName;
                    ViewBag.Location = user.Location;
                    Session["LoginEmail"] = model.Email;
                    Session["loginid"] = user.Id;
                    Session["LoginName"] = user.FirstName;
                    //Will be hashed later
                    Session["LoginPass"] = model.Password;
                    return View("~/Views/Home/Index.cshtml");
                }
            }


            return View("~/Views/Home/ResultTest.cshtml");
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,Location,FirstName,LastName,Password")] User user)
        {
            var idCalc = (int)db.Users.LongCount();
            user.Id = idCalc+1;
            user.IsAdmin = -1;

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                ViewData["loginid"] = user.Id;
                return RedirectToAction("~/Views/Home/Index.cshtml");
            }

            return View(user);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Login/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsAdmin,Email,Location,FirstName,LastName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
