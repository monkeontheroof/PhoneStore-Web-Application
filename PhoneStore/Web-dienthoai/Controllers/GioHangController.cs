using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class GioHangController : Controller
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();
        // GET: GioHang
        public List<MatHangMua> LayGioHang()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;

            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }

        public ActionResult ThemSP(int IdSP)
        {
            List<MatHangMua> gioHang = LayGioHang();


            MatHangMua phone = gioHang.FirstOrDefault(s => s.IdSP == IdSP);
            if (phone == null)
            {
                phone = new MatHangMua(IdSP);
                gioHang.Add(phone);
            }
            else
            {
                phone.SoLuong++;
            }
            return RedirectToAction("Details", "PhoneStore", new { id = IdSP });
        }


        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
            {
                tongSL = gioHang.Sum(id => id.SoLuong);
            }
            return tongSL;
        }

        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
            {
                TongTien = gioHang.Sum(sp => sp.ThanhTien());
            }
            return TongTien;
        }


        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = LayGioHang();

            if (gioHang == null || gioHang.Count == 0)
            {
                return RedirectToAction("Index", "PhoneStore");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }


        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        public ActionResult XoaMatHang(int IdSP)
        {
            List<MatHangMua> gioHang = LayGioHang();

            //Lấy sp trong giỏ
            var sanpham = gioHang.FirstOrDefault(id => id.IdSP == IdSP);
            if(sanpham != null)
            {
                gioHang.RemoveAll(id => id.IdSP == IdSP);
                return RedirectToAction("HienThiGioHang"); //Route về trang giỏ hàng
            }
            if(gioHang.Count == 0)
            {
                return RedirectToAction("Index", "PhoneStore"); //Route về trang chủ nếu không có gì
            }
            return RedirectToAction("HienThiGioHang");
        }


        public ActionResult CapNhatMatHang(int IdSP, int Soluong)
        {
            //Lấy giỏ hàng
            List<MatHangMua> gioHang = LayGioHang();

            //Lấy SP trong giỏ hàng
            var sanpham = gioHang.FirstOrDefault(id => id.IdSP == IdSP);
            if (sanpham != null)
            {
                //Cập nhật số lượng tương ứng
                //Số lượng luôn >= 1
                sanpham.SoLuong = Soluong;
            }
            return RedirectToAction("HienThiGioHang");
        }


        public ActionResult DatHang()
        {
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0)
                return RedirectToAction("Index", "PhoneStore");

            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }

        
        public ActionResult DongYDatHang(string HTThanhToan)
        {
            KhachHang kh = Session["KhachHang"] as KhachHang;
            List<MatHangMua> gioHang = LayGioHang();

            DonHang donHang = new DonHang();
            donHang.MaKH = kh.MaKH;
            donHang.NgayDH = DateTime.Now;
            donHang.TriGia = (decimal)TinhTongTien();
            donHang.TinhTrang = "Chờ xác nhận";
            donHang.TenNguoiNhan = kh.Hoten;
            donHang.SDTnhan = kh.SDT;
            donHang.DiaChiNhan= kh.DiaChi;
            donHang.HTThanhToan = HTThanhToan;
            donHang.HTGiaohang = "Ship COD";

            db.DonHangs.Add(donHang);
            db.SaveChanges();

            foreach(var item in gioHang)
            {
                ViewBag.ThanhTien = TinhTongTien();
                ChiTietDH detail = new ChiTietDH();
                detail.MaDH = donHang.MaDH;
                detail.SoLuong = item.SoLuong;
                detail.Dongia = (decimal)item.DonGia;
                detail.IdSP = item.IdSP;
                db.ChiTietDHs.Add(detail);
            }

            db.SaveChanges();

            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
    }
}