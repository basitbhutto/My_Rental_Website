using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OlxWebsiteFyp.Models;

namespace OlxWebsiteFyp.Controllers
{
    public class AdminPageController : Controller
    {
        private OLX2Entities db = new OLX2Entities();

        // GET: AdminPage
        public ActionResult Index()
        {
            if (Session["re_id"] != null)
            {
                string raheem = Session["re_id"].ToString();
                int basit = Convert.ToInt32(raheem);
                var aaa = db.Regstrations.Where(x => x.re_id == basit).FirstOrDefault();

                Session["rare_name0"] = aaa.re_name;
                Session["rare_name1"] = aaa.re_Photo;
                Session["rare_name2"] = aaa.re_contact;
                Session["rare_name3"] = aaa.re_gender;


                var items = db.Items.Include(i => i.Category);
                    return View(items.ToList());

            }
            else
            {
                TempData["adminerror"] = "Please First Login";
                return RedirectToAction("Index", "Home");
            }

          

        }

        // GET: AdminPage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: AdminPage/Create
        public ActionResult Create()
        {
            ViewBag.c_id = new SelectList(db.Categories, "c_id", "c_name");
            return View();
        }

        // POST: AdminPage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "i_id,re_id,i_name,i_price,i_image,i_status,i_contact,i_date,c_id,i_active")] Item item)
        {
            if (ModelState.IsValid)
            {

                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            
            }

            ViewBag.c_id = new SelectList(db.Categories, "c_id", "c_name", item.c_id);
            return View(item);
        }

        // GET: AdminPage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.c_id = new SelectList(db.Categories, "c_id", "c_name", item.c_id);
            return View(item);
        }

        // POST: AdminPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "i_id,re_id,i_name,i_price,i_image,i_status,i_contact,i_date,c_id,i_active")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.c_id = new SelectList(db.Categories, "c_id", "c_name", item.c_id);
            return View(item);
        }

        // GET: AdminPage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: AdminPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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


        //registration form for admin


        public ActionResult CreateAdmin()
        {
            ViewBag.t_id = new SelectList(db.Types.ToList(), "t_id", "t_name");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin([Bind(Include = "re_id,re_name,re_email,re_gender,re_age,re_Photo,re_city,re_contact,t_id")] Regstration regstration, HttpPostedFileBase ImageFile)
        {
            string path = Uploadimage(ImageFile);
            if (ModelState.IsValid)
            {
                var row = db.Regstrations.Where(x => x.re_email == regstration.re_email).SingleOrDefault();
                if (row != null)
                {
                    string a = "4";
                    Session["newaccount"] = a;
                    TempData["Same Email"] = "Email already Available !!  Please Login Here.";
                    return RedirectToAction("LoginForm","Regstrations");
                }
                else
                {
                    regstration.re_Photo = path;
                db.Regstrations.Add(regstration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
            ViewBag.t_id = new SelectList(db.Types, "t_id", "t_name", regstration.t_id);
            return View(regstration);
        }

        public string Uploadimage(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null)
            {
                string extension = Path.GetExtension(file.FileName);

                path = Path.Combine(Server.MapPath("~/images/"), random + Path.GetFileName(file.FileName));
                file.SaveAs(path);
                path = "~/images/" + random + Path.GetFileName(file.FileName);
            }
            else
            {
                Response.Write("<script> alert('Please Select a file  ..'); <script>");
                path = "-1";

            }
            return path;
        }

    }
}
