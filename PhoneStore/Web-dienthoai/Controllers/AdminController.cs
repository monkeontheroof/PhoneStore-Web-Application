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
            if (Session["Admin"] == null && Session["Nhanvien"] == null)
                return RedirectToAction("Login", "Admin");

            ViewBag.Donhang = db.DonHangs.Count();
            ViewBag.Nhanvien = db.NhanViens.Count();
            ViewBag.Sanpham = db.SanPhams.Count();
            var doanhthu = from l in db.ChiTietDHs
                           select l;
            ViewBag.Doanhthu = (decimal)db.DonHangs.Sum(id => id.TriGia);
            var donhang = db.DonHangs.ToList();
            return View(donhang);
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TaiKhoanNV admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.Username))
                    ModelState.AddModelError(string.Empty, "Vui lòng nhập Username");

                if (string.IsNullOrEmpty(admin.Password))
                    ModelState.AddModelError(string.Empty, "Vui lòng nhập mật khẩu");


                //Kiểm tra có tồn tại Admin dưới DB hay chưa
                var adminDB = db.TaiKhoanNVs.FirstOrDefault(ad => ad.Username == admin.Username && ad.Password == admin.Password);
                if (adminDB == null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
                else
                {
                    if (adminDB.NhanVien.ChucVu == "NV")
                        Session["Nhanvien"] = adminDB;

                    else if (adminDB.NhanVien.ChucVu == "AD")
                        Session["Admin"] = adminDB;

                    Session["Username"] = adminDB.Username;
                    ViewBag.ThongBao = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }


        public ActionResult Logout()
        {
            Session["Admin"] = null;
            Session["Nhanvien"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }


        public ActionResult AdminProfile()
        {
            if (Session["Admin"] == null && Session["Nhanvien"] == null)
                return RedirectToAction("Login", "Admin");

            else if (Session["Admin"] != null)
            {
                TaiKhoanNV ad = Session["Admin"] as TaiKhoanNV;
                var userad = db.NhanViens.FirstOrDefault(un => un.TaiKhoanNV.Username == ad.Username);
                return View(userad);
            }
            else
            {
                TaiKhoanNV nv = Session["Nhanvien"] as TaiKhoanNV;
                var userNV = db.NhanViens.FirstOrDefault(stf => stf.TaiKhoanNV.Username == nv.Username); 
                return View(userNV);
            }
        }


        //[HttpPost]
        //public ActionResult AdminProfile(Admin admin)
        //{
        //    var student = admin.
        //}

    }
}