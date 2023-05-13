using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class NguoiDungController : Controller
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: NguoiDung
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            if(ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.Hoten))
                    ModelState.AddModelError(string.Empty, "Họ tên không được để trống");
                if (string.IsNullOrEmpty(kh.Username))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.Password))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.SDT))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                //Kiểm tra đã có ai đk tài khoản này hay chưa
                var khachhang = db.KhachHangs.FirstOrDefault(id => id.Username == kh.Username);
                if(khachhang != null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập đã được sử dụng, hãy chọn tên khác");

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.KhachHangs.Add(kh);
                        db.SaveChanges();
                        return RedirectToAction("DangNhap");
                    }
                    catch
                    {
                        return Content("LỖI TẠO MỚI TÀI KHOẢN!");
                    }
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhap");
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangNhap(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.Username))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền tên đăng nhập");
                if (string.IsNullOrEmpty(kh.Password))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền mật khẩu");

                if (ModelState.IsValid)
                {
                    //Tìm KH hợp lệ có trong CSDL
                    var khachhang = db.KhachHangs.FirstOrDefault(id => id.Username == kh.Username && id.Password == kh.Password);
                    if (khachhang != null)
                    {
                        ViewBag.ThongBao = "Đăng nhập thành công!";
                        //Lưu > session
                        Session["KhachHang"] = khachhang;
                        return RedirectToAction("Index", "PhoneStore");
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không hợp lệ!";
                }
            }
            return View();
        }

        
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "PhoneStore");
        }

        public ActionResult HoSo()
        {
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            } else
                return View();
        }
    }
}