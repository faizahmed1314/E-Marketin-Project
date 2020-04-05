using EMarketing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMarketing.Controllers
{
    public class AdminController : Controller
    {
        dbemarketingEntities db = new dbemarketingEntities();
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_admin admin)
        {
            var person = db.tbl_admin.Where(x => x.ad_username == admin.ad_username && x.ad_password == admin.ad_password).SingleOrDefault();
            if (person != null)
            {
                Session["ad_id"] = admin.ad_id.ToString();
                Session["ad_username"] = admin.ad_username.ToString();
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.Message = "Invalid user name and password!";
            }

            return View();
        }

        public ActionResult Create()
        {
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}