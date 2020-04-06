using EMarketing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace EMarketing.Controllers
{
    public class AdminController : Controller
    {
        dbemarketingEntities1 db = new dbemarketingEntities1();
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
                Session["ad_id"] = person.ad_id.ToString();
                Session["ad_username"] = person.ad_username.ToString();
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

        [HttpPost]
        public ActionResult Create(tbl_category category,HttpPostedFileBase imgFile)
        {

            string path = UploadImageFile(imgFile);
            if (path.Equals("-1"))
            {
                ViewBag.ErrorMessage = "File could not be uploaded......";
            }
            else
            {
                tbl_category cat = new tbl_category();
                cat.cat_name = category.cat_name;
                cat.cat_image = path;
                cat.cat_status = 1;
                cat.cat_fk_ad = Convert.ToInt32(Session["ad_id"].ToString());
                db.tbl_category.Add(cat);
                db.SaveChanges();
                
                return RedirectToAction("Create");
            }
            return View();
        }

        public ActionResult ViewCategory(int? page)
        {

            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cat_status == 1).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);


            
        }

        [NonAction]
        public string UploadImageFile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if(file!=null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if(extension.ToLower().Equals(".jpg")|| extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch(Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg jpeg and png file format are acceptable.....');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file.....');</script>");
                path = "-1";
            }
            return path;
        }
    }
}