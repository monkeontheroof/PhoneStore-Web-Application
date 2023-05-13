using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class DonHangsController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: DonHangs
        public ActionResult Index(string currentFilter, string s, int? page)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            int pageSize = 7;
            int pageNum = (page ?? 1);

            if (s != null)
            {
                page = 1;
            }
            else
            {
                s = currentFilter;
            }

            var donHangs = from l in db.DonHangs
                           select l;
            donHangs = donHangs.Where(i => i.TinhTrang.Contains("Đang xử lý"));
            if (!String.IsNullOrEmpty(s))
            {
                donHangs = donHangs.Where(id => id.MaDH.ToString().Contains(s) || id.TenNguoiNhan.Contains(s) || id.NgayDH.ToString().Contains(s) || id.SDTnhan.ToString().Contains(s));
            }

            donHangs = donHangs.OrderBy(i => i.NgayDH);
            return View(donHangs.ToPagedList(pageNum, pageSize));
        }

        public ActionResult DonHangDaGiao(string currentFilter, string s, int? page)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            int pageSize = 7;
            int pageNum = (page ?? 1);

            if (s != null)
            {
                page = 1;
            }
            else
            {
                s = currentFilter;
            }

            var donHangs = from l in db.DonHangs
                           select l;
            donHangs = donHangs.Where(i => i.TinhTrang.Contains("Đã giao"));
            if (!String.IsNullOrEmpty(s))
            {
                donHangs = donHangs.Where(id => id.MaDH.ToString().Contains(s) || id.TenNguoiNhan.Contains(s) || id.NgayDH.ToString().Contains(s) || id.SDTnhan.ToString().Contains(s));
            }

            donHangs = donHangs.OrderBy(i => i.NgayDH);
            return View(donHangs.ToPagedList(pageNum, pageSize));
        }

        // GET: DonHangs/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // GET: DonHangs/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Hoten");
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDH,MaKH,TenNguoiNhan,SDTnhan,DiaChiNhan,TriGia,TinhTrang,NgayDH,HTThanhToan,HTGiaohang,MaNV")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Hoten", donHang.MaKH);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten", donHang.MaNV);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Hoten", donHang.MaKH);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten", donHang.MaNV);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDH,MaKH,TenNguoiNhan,SDTnhan,DiaChiNhan,TriGia,TinhTrang,NgayDH,HTThanhToan,HTGiaohang,MaNV")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Hoten", donHang.MaKH);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten", donHang.MaNV);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
