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
    public class ChiTietDHsController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: ChiTietDHs
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            var chiTietDHs = db.ChiTietDHs.Include(c => c.DonHang).Include(c => c.SanPham);
            return View(chiTietDHs.ToList());
        }

        // GET: ChiTietDHs/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDH chiTietDH = db.ChiTietDHs.Find(id);
            if (chiTietDH == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDH);
        }

        // GET: ChiTietDHs/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "TenNguoiNhan");
            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP");
            return View();
        }

        // POST: ChiTietDHs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDH,IdSP,SoLuong,Dongia,Thanhtien")] ChiTietDH chiTietDH)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietDHs.Add(chiTietDH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "TenNguoiNhan", chiTietDH.MaDH);
            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP", chiTietDH.IdSP);
            return View(chiTietDH);
        }

        // GET: ChiTietDHs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDH chiTietDH = db.ChiTietDHs.Find(id);
            if (chiTietDH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "TenNguoiNhan", chiTietDH.MaDH);
            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP", chiTietDH.IdSP);
            return View(chiTietDH);
        }

        // POST: ChiTietDHs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDH,IdSP,SoLuong,Dongia,Thanhtien")] ChiTietDH chiTietDH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietDH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "TenNguoiNhan", chiTietDH.MaDH);
            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP", chiTietDH.IdSP);
            return View(chiTietDH);
        }

        // GET: ChiTietDHs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDH chiTietDH = db.ChiTietDHs.Find(id);
            if (chiTietDH == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDH);
        }

        // POST: ChiTietDHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ChiTietDH chiTietDH = db.ChiTietDHs.Find(id);
            db.ChiTietDHs.Remove(chiTietDH);
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
