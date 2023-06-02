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
    public class TaiKhoanNVsController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: TaiKhoanNVs
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

            var taiKhoanNV = from l in db.TaiKhoanNVs.Include(t => t.NhanVien)
                           select l;

            if (!String.IsNullOrEmpty(s))
            {
                taiKhoanNV = taiKhoanNV.Where(i => i.Username.Contains(s) || i.NhanVien.Hoten.Contains(s));
            }
            ViewBag.CurrentFilter = s;

            taiKhoanNV = taiKhoanNV.OrderBy(id => id.Username);

            return View(taiKhoanNV.ToPagedList(pageNum, pageSize));
        }

        // GET: TaiKhoanNVs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoanNV taiKhoanNV = db.TaiKhoanNVs.Find(id);
            if (taiKhoanNV == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanNV);
        }

        // GET: TaiKhoanNVs/Create
        public ActionResult Create()
        {
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten");
            return View();
        }

        // POST: TaiKhoanNVs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,Username,Password")] TaiKhoanNV taiKhoanNV)
        {
            if (ModelState.IsValid)
            {
                var validTK = db.TaiKhoanNVs.Include(taiKhoanNV.Username);
                if (validTK == null)
                {
                    ViewBag["ThongBaoFailed"] = "Đã tồn tại tên tài khoản! Vui lòng chọn tên khác";
                    return View("Create");
                }
                else
                {
                    db.TaiKhoanNVs.Add(taiKhoanNV);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten", taiKhoanNV.MaNV);
            return View(taiKhoanNV);
        }

        // GET: TaiKhoanNVs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoanNV taiKhoanNV = db.TaiKhoanNVs.Find(id);
            if (taiKhoanNV == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten", taiKhoanNV.MaNV);
            return View(taiKhoanNV);
        }

        // POST: TaiKhoanNVs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,Username,Password")] TaiKhoanNV taiKhoanNV)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoanNV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Hoten", taiKhoanNV.MaNV);
            return View(taiKhoanNV);
        }

        // GET: TaiKhoanNVs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoanNV taiKhoanNV = db.TaiKhoanNVs.Find(id);
            if (taiKhoanNV == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanNV);
        }

        // POST: TaiKhoanNVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoanNV taiKhoanNV = db.TaiKhoanNVs.Find(id);
            db.TaiKhoanNVs.Remove(taiKhoanNV);
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
