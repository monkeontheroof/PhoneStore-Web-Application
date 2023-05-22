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

            try
            {
                db.DonHangs.Add(donHang);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                TempData["ThongBaoFailed"] = "Tạo đơn hàng thất bại!";
                return RedirectToAction("HienThiGioHang");
            }
            try
            {
                foreach (var item in gioHang)
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
            catch(Exception ex)
            {
                TempData["ThongBaoFailed"] = "Tạo đơn hàng thất bại!";
                return RedirectToAction("HienThiGioHang");
            }
        }

        public ActionResult HoanThanhDonHang()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ThongTinMuaHang()
        {
            List<MatHangMua> giohang = LayGioHang();
            if (giohang == null || giohang.Count == 0) return RedirectToAction("Index", "PhoneStore");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            ViewBag.TongCong = TinhTongTien();
            return View(giohang);
        }
        [HttpPost]
        public ActionResult ThongTinMuaHang(DonHang dh)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(dh.TenNguoiNhan))
                    ModelState.AddModelError(String.Empty, "Họ tên không được để trống ");
                if (String.IsNullOrEmpty(dh.SDTnhan))
                    ModelState.AddModelError(String.Empty, "SDT không được để trống ");
                if (String.IsNullOrEmpty(dh.DiaChiNhan))
                    ModelState.AddModelError(String.Empty, "Địa chỉ không được để trống ");
                if (String.IsNullOrEmpty(dh.HTThanhToan))
                    ModelState.AddModelError(String.Empty, " Hình Thức Thanh Toán không được để trống ");
                if (String.IsNullOrEmpty(dh.HTGiaohang))
                    ModelState.AddModelError(String.Empty, " Hình Thức Giao Hàng không được để trống ");
                if (ModelState.IsValid)
                {
                    dh.TriGia = ((decimal)TinhTongTien());
                    dh.TinhTrang = "Chờ Duyệt";
                    dh.NgayDH = DateTime.Now;
                }
                else
                    return View();
            }
            if (dh.HTGiaohang == "Giao hàng tiêu chuẩn") dh.TriGia += 30000;
            else dh.TriGia += 66000;
            db.DonHangs.Add(dh);//luu don hang vao database
            db.SaveChanges(); //luu truoc khi tạo chi tiết don hàng để có mã đơn hàng trong database

            DonHang dh1 = LayTTDon(); //lay thong tin don trong sesion neu co
            dh1 = dh;//gan  don hang moi dien 
            Session["TTdonHang"] = dh1; //luu vao sesion khi tao tk moi cho kh co madh tim databse


            //them chi tiet don hang vao database
            
            List<MatHangMua> giohang = LayGioHang();
            foreach (var item in giohang)
            {
                ChiTietDH ctdh = new ChiTietDH();
                ctdh = new ChiTietDH();
                ctdh.MaDH = dh.MaDH;
                ctdh.IdSP = item.IdSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.Thanhtien = ((int)item.ThanhTien());
                db.ChiTietDHs.Add(ctdh);
                db.SaveChanges();
            }


            //ktr dang nhap neu co dang nhap thi luu don hàng cho tai khoan do
            var kh = Session["KhachHang"] as KhachHang;

            if (kh != null)
            {//dang dang nhap
                DonHang x = db.DonHangs.FirstOrDefault(s => s.MaDH == dh.MaDH);
                x.MaKH = kh.MaKH;// gan mã khách vào đơn hàng moi tao
                db.SaveChanges();
                return RedirectToAction("DatHangThanhCong");
            }
            else
            {

                return RedirectToAction("CauHoiDKTK"); //tao khach hang moi bang thong tin giao hang
            }


        }

        public ActionResult DatHangThanhCong()
        {
            Session["GioHang"] = null;
            Session["TTDonHang"] = null;
            return View();
        }

        public DonHang LayTTDon()
        {
            DonHang dh = Session["TTDonHang"] as DonHang;
            //neu gio hang chua ton tai thi  tao moi dua vao session
            if (dh == null)
            {
                dh = new DonHang();
                Session["TTDonHang"] = dh;
            }
            return dh;
        }

        public ActionResult CauHoiDKTK()
        {

            if (Session["TaiKhoan"] == null)// chua dang nhap
            {
                return View();
            }
            return RedirectToAction("DatHangThanhCong");
        }

        public ActionResult ThongTinDKTK()
        {

            var dh = LayTTDon(); //lay ma don hang vua dien buoc truoc gan vao tai khoan moi tao
            if (dh == null) return RedirectToAction("HienThiGioHang");//test loi code k lien quan
            if (db.KhachHangs.Any(s => s.SDT == dh.SDTnhan))//lay sdt lm tk kiem tra co trung tk khong 
            {
                ViewBag.thongbao = "SDT đã được dùng không thể tạo tài khoản mới.";
                return View();
            }
            else
            {

                var tk = new KhachHang();
                tk.Hoten = dh.TenNguoiNhan;
                tk.SDT = dh.SDTnhan;
                tk.DiaChi = dh.DiaChiNhan;
                tk.Username = dh.SDTnhan;
                tk.Password = dh.SDTnhan;
                db.KhachHangs.Add(tk);
                db.SaveChanges();
                ViewBag.TenDN = tk.SDT;
                ViewBag.Password = tk.Password;
                ViewBag.TENKH = tk.Hoten;
                ViewBag.DIACHI = tk.DiaChi;
                ViewBag.thongbao = "Đơn hàng đã được lưu vào tài khoản";
                //gan ma khachhang vao don hang vua mua
                var a = db.KhachHangs.FirstOrDefault(s => s.SDT == tk.SDT);
                var donhang = db.DonHangs.FirstOrDefault(s => s.MaDH == dh.MaDH);
                donhang.MaKH = a.MaKH;
                db.SaveChanges();
                Session["GioHang"] = null;
                Session["TTDonHang"] = null;
                return View();
            }

        }
    }
}