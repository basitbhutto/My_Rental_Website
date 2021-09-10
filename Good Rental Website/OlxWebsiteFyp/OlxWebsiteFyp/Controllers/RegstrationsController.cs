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
    public class RegstrationsController : Controller
    {
        private OLX2Entities db = new OLX2Entities();

        // GET: Regstrations
        public ActionResult Index()
        {
            var regstrations = db.Regstrations.Include(r => r.Type);
            return View(regstrations.ToList());
        }

        public ActionResult Status(int? id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Status([Bind(Include = "i_id,re_id,i_name,i_price,i_image,i_status,i_contact,i_date,c_id,i_active")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ConfirmForm");
            }
            ViewBag.c_id = new SelectList(db.Categories, "c_id", "c_name", item.c_id);
            return View(item);
        }


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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "i_id,re_id,i_name,i_price,i_image,i_status,i_contact,i_date,c_id,i_active")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LoginForm","Regstrations");
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
            return RedirectToAction("Index");
        }






        //// GET: Regstrations/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Regstration regstration = db.Regstrations.Find(id);
        //    if (regstration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(regstration);
        //}







        // GET: Regstrations/Create
        public ActionResult Create()
        {
            ViewBag.t_id = new SelectList(db.Types.Where(x => x.t_id == 2 || x.t_id == 3).ToList(), "t_id", "t_name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "re_id,re_name,re_email,re_gender,re_age,re_Photo,re_city,re_contact,t_id")] Regstration regstration, HttpPostedFileBase ImageFile)
        {
            string path = Uploadimage(ImageFile);
            if (ModelState.IsValid)
            {
                var row = db.Regstrations.Where(x => x.re_email == regstration.re_email).SingleOrDefault();
                if(row != null)
                {
                    TempData["Same Email"] = "Email already Available !!  Please Login Here.";
                    return RedirectToAction("LoginForm");
                }
                else
                {
                    regstration.re_Photo = path;
                    db.Regstrations.Add(regstration);
                    db.SaveChanges();
                    return RedirectToAction("LoginForm");
                }
               
            }

            ViewBag.t_id = new SelectList(db.Types, "t_id", "t_name", regstration.t_id);
            return View(regstration);
        }

        //// GET: Regstrations/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
                
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Regstration regstration = db.Regstrations.Find(id);
        //    if (regstration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.t_id = new SelectList(db.Types, "t_id", "t_name", regstration.t_id);
        //    return View(regstration);
        //}

        //// POST: Regstrations/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "re_id,re_name,re_email,re_gender,re_age,re_Photo,re_city,re_contact,t_id")] Regstration regstration)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(regstration).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.t_id = new SelectList(db.Types, "t_id", "t_name", regstration.t_id);
        //    return View(regstration);
        //}

        //// GET: Regstrations/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Regstration regstration = db.Regstrations.Find(id);
        //    if (regstration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(regstration);
        //}

        public ActionResult Rao()
        {
            return View();
        }

        //// POST: Regstrations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Regstration regstration = db.Regstrations.Find(id);
        //    db.Regstrations.Remove(regstration);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult LoginForm()
        {
            return View();
        }


        [HttpPost]
        public ActionResult LoginForm(Regstration reg)
        {
      


            var u = db.Regstrations.Where(x => x.re_email == reg.re_email && x.re_age == reg.re_age ).SingleOrDefault();
            if (u != null)
            {
                var row = db.Regstrations.Where(x => x.re_email == u.re_email).SingleOrDefault();

                Session["Id"] = row.t_id.ToString();

                Session["re_id"] = row.re_id.ToString();

                if (Session["Id"].Equals("3"))
                {
                   
                    //Session["idinfo"] = reg.re_id.ToString();
                   Session["name"] = reg.re_email;
                    Session["Password"] = reg.re_age;

                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.InsertMessage = "Data uploadkldfjdfklssssssss";
                    return RedirectToAction("ConfirmForm", "Regstrations");

                }
                else if (Session["Id"].Equals("2"))
                {
                    TempData["after registration"] = "Thanks you for your Registration Please select Item for Rent Good";
                    return RedirectToAction("Index", "Home");
                }

                else if (Session["Id"].Equals("1"))
                {
                    TempData["AdminPanel"] = "Thanks you for your Registration";
                    return RedirectToAction("Index", "AdminPage");
                }

            }
            else
            {
                TempData["AdminPanel"] = "Invalid Email Or Password";
                return RedirectToAction("LoginForm", "Regstrations");
            }

            return RedirectToAction("Rao", "Regstrations");
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

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult ConfirmForm()
        {
             if(Session["re_id"] != null)
            {
                string raheem = Session["re_id"].ToString();
                int basit = Convert.ToInt32(raheem);
                var aaa = db.Regstrations.Where(x => x.re_id == basit).FirstOrDefault();

                Session["rare_name01"] = aaa.re_name;
                Session["rare_name11"] = aaa.re_Photo;
                Session["rare_name21"] = aaa.re_contact;
                Session["rare_name31"] = aaa.re_gender;





                int abc = Convert.ToInt32(Session["re_id"]);
                //var abc = Session["name"];

                
                return View(db.Items.Where(x=>x.re_id ==abc).ToList());
            }
            else
            {
                ViewBag.loginfirstitem = "You Id is not matched Please First Upload Items";
                return View();
            }  
        }
        public ActionResult Type_Checker()
        {

            if (Session["Id"].Equals("3"))
            {

              
               
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.InsertMessage = "Data uploadkldfjdfklssssssss";
                return RedirectToAction("ConfirmForm", "Regstrations");

            }
            else if (Session["Id"].Equals("2"))
            {
                TempData["after registration"] = "Thanks you for your Registration Please select Item for Rent Good";
                return RedirectToAction("Index", "Home");
            }

            else if (Session["Id"].Equals("1"))
            {
                TempData["AdminPanel"] = "Thanks you for your Registration";
                return RedirectToAction("Index", "AdminPage");
            }

        
            else
            {
                TempData["AdminPanel"] = "Invalid Email Or Password";
                return RedirectToAction("Index", "Home");
    }
            

                
        }
       

     


    }
}
