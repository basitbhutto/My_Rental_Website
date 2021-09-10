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
    public class ItemsController : Controller
    {
        private OLX2Entities db = new OLX2Entities();

        // GET: Items
        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.Category);
            return View(items.ToList());
        }

        // GET: Items/Details/5
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

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.c_id = new SelectList(db.Categories, "c_id", "c_name");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "i_id,re_id,i_name,i_price,i_image,i_status,i_contact,i_date,c_id,i_active")] Item item, HttpPostedFileBase ImageFile)
        {
            string path = Uploadimage(ImageFile);
            if (ModelState.IsValid)
            {

                item.re_id = Convert.ToInt32(Session["re_id"]);
                item.i_active = 0;
                item.i_status = 0;
                item.i_image = path;
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("ConfirmForm","Regstrations");
            }

            ViewBag.c_id = new SelectList(db.Categories, "c_id", "c_name", item.c_id);
            //return View(item);
            return RedirectToAction("ConfirmForm", "Items");
        }

        // GET: Items/Edit/5
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

        // POST: Items/Edit/5
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

        // GET: Items/Delete/5
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

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("LoginForm","Regstrations");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

        public ActionResult FeedBack()
        {
            return View();
         
           
        }
        [HttpPost]
        public ActionResult FeedBack(Feed fd)
        {
            db.Feeds.Add(fd);
            db.SaveChanges();
            TempData["feed"] = "Thanks for FeedBack";
            return RedirectToAction("LoginForm", "Regstrations");
        }

        public ActionResult FeedIndex()
        {
         
                return View(db.Feeds.ToList());
          

           
        }

    }
}
