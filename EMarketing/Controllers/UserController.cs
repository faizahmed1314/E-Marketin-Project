using EMarketing.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMarketing.Controllers
{
    public class UserController : Controller
    {
        dbemarketingEntities1 db = new dbemarketingEntities1();
        // GET: User
        public ActionResult Index(int? page)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
            
        }

        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(tbl_user tblUser,HttpPostedFileBase imgFile)
        {
            string path = UploadImageFile(imgFile);
            if (path.Equals("-1"))
            {
                ViewBag.ErrorMessage = "File could not be uploaded......";
            }
            else
            {
                tbl_user user = new tbl_user();

                user.u_name = tblUser.u_name;
                user.u_email = tblUser.u_email;
                user.u_password = tblUser.u_password;
                user.u_image = path;
                user.u_contact = tblUser.u_contact;
                db.tbl_user.Add(user);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_user user)
        {
            var person = db.tbl_user.Where(x => x.u_email == user.u_email && x.u_password == user.u_password).SingleOrDefault();
            if (person != null)
            {
                Session["u_id"] = person.u_id.ToString();
                Session["u_name"] = person.u_name.ToString();
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.Message = "Invalid email and password!";
            }

            return View();
        }

        [NonAction]
        public string UploadImageFile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
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