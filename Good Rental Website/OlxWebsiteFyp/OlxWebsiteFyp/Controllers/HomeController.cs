using OlxWebsiteFyp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OlxWebsiteFyp.Controllers
{
    public class HomeController : Controller
    {
        private OLX2Entities db = new OLX2Entities();
        public ActionResult Index(string searching, int? id)
        {


          
            var students = from s in db.Items select s;
            if (!String.IsNullOrEmpty(searching))
            {
                students = students.Where(s => s.i_name.Contains(searching));
            }
            if (id != null)
            {
                return View(db.Items.Where(x => x.c_id == id).ToList());
            }
            return View(students.ToList());


        }


        public ActionResult About(int? id)
        {
            var students = from s in db.Items select s;

            if (id != null)
            {
                return View(db.Items.Where(x => x.c_id == id).ToList());
            }

            /* return View(db.Items.ToList());*/
            return RedirectToAction("Index");

           
        }

        public ActionResult OrderPage(int? id)
        {
            var data1 = db.Items.Where(x => x.i_id == id).FirstOrDefault();

            if (data1.i_status == 1)
            {
                int ac = Convert.ToInt32(Session["Id"]);
                if (ac != 0)
                {
                    return View(db.Items.Where(x => x.i_id == id).ToList());
                }
                else
                {
                    TempData["name"] = "Please First Login for Rent Good";
                    return RedirectToAction("LoginForm", "Regstrations");

                }
            }
            else if (data1.i_status == 0)
            {
                TempData["msg11"] = "<script>alert('This Item already reserve');</script>";
                return RedirectToAction("Index","Home");

            }
            else
            {
                TempData["name"] = "Please First Login for Rent Good";
                return RedirectToAction("LoginForm", "Regstrations");
            }


           
           
        
        }

        
      


    }
}