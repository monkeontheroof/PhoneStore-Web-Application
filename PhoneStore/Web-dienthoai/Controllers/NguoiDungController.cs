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
                        ViewBag.ThongBao = "Đăng ký thành công!";
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
                        ViewBag.name = khachhang.Hoten;
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

        public ActionResult UserPartial()

        {
            var kh = Session["KhachHang"] as KhachHang;
            if (kh != null) ViewBag.name = kh.Hoten.ToString();
            else ViewBag.name = "";


            return PartialView();
        }

        public ActionResult TrangTK() /*trang taikhoan khi link vao taikhoan*/
        {
            var kh = Session["KhachHang"] as KhachHang;
            if (kh == null)
            {
                return RedirectToAction("DangNhap");
            }
            return View(kh);
        }

        public ActionResult TrangThongTinTK() /*trang thong tin ca nhan */
        {
            var kh = Session["KhachHang"] as KhachHang;
            if (kh == null)
            {
                return RedirectToAction("DangNhap");
            }
            return View(kh);
        }

        public ActionResult LichSuMuaHang()
        {
            var kh = Session["KhachHang"] as KhachHang;
            if (kh != null)
            {
                var listDonhang = db.DonHangs.Where(dh => dh.MaKH == kh.MaKH).ToList();
                List<Itemdonhang> listdh = new List<Itemdonhang>();
                foreach(var d in listDonhang)
                {
                    Itemdonhang dh = new Itemdonhang(d.MaDH);
                    listdh.Add(dh);
                }
                return View(listdh);
            }
            else
                return View("TrangTK");
        }

        private int TinhTongSL(int MaDH)
        {
            int tongSL = 0;
            var donhang = db.ChiTietDHs.Where(d => d.MaDH == MaDH).ToList();
            if(donhang.Count() > 0)
            {
                tongSL = (int)donhang.Sum(c => c.SoLuong);
            }
            return tongSL;
        }

        public ActionResult HuyDon(long id)
        {
            DonHang dh = db.DonHangs.FirstOrDefault(s => s.MaDH == id);
            if (dh.TinhTrang == "Chờ Duyệt")
            {
                dh.TinhTrang = "Hủy";
                db.SaveChanges();
                ViewBag.thongbao = "Hủy đơn hàng thành công";
                return RedirectToAction("LichSuMuaHang");
            }
            ViewBag.thongbao = "Quý khách có thể tiếp tục mua hàng!";
            return RedirectToAction("LichSuMuaHang");
        }
        public ActionResult XacNhanHuyDon(long madh)
        {
            var kh = Session["KhachHang"] as KhachHang;
            ViewBag.makh = kh.MaKH;
            ViewBag.madh = madh;
            return View();
        }
    }
}