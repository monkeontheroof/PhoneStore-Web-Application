using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;
using PagedList;
using Newtonsoft.Json;

namespace Web_dienthoai.Controllers
{
    public class AdminController : Controller
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            ViewBag.Donhang = db.DonHangs.Count();
            ViewBag.Nhanvien = db.NhanViens.Count();
            var doanhthu = from l in db.ChiTietDHs
                           select l;
            ViewBag.Doanhthu = db.DonHangs.Sum(id => id.TriGia);
            var donhang = db.DonHangs.ToList();
            return View(donhang);
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.Useradmin))
                    ModelState.AddModelError(string.Empty, "Vui lòng nhập Username");

                if (string.IsNullOrEmpty(admin.Passadmin))
                    ModelState.AddModelError(string.Empty, "Vui lòng nhập mật khẩu");


                //Kiểm tra có tồn tại Admin dưới DB hay chưa
                var adminDB = db.Admins.FirstOrDefault(ad => ad.Useradmin == admin.Useradmin && ad.Passadmin == admin.Passadmin);
                if (adminDB == null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
                else
                {
                    Session["Admin"] = adminDB;
                    ViewBag.ThongBao = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }


        public ActionResult AdminProfile()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            Admin ad = Session["Admin"] as Admin;
            var userad = db.Admins.FirstOrDefault(un => un.Useradmin == ad.Useradmin);

            return View(userad);
        }


        //[HttpPost]
        //public ActionResult AdminProfile(Admin admin)
        //{
        //    var student = admin.
        //}

    }
}