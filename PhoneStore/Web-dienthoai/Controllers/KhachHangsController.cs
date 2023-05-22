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
    public class KhachHangsController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: KhachHangs
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

            var khachHang = from l in db.KhachHangs
                           select l;

            if (!String.IsNullOrEmpty(s))
            {
                khachHang = khachHang.Where(i => i.MaKH.ToString().Contains(s) ||
                i.Hoten.Contains(s) ||
                i.NgaySinh.ToString().Contains(s) ||
                i.Username.Contains(s) ||
                i.SDT.ToString().Contains(s));
            }

            ViewBag.CurrentFilter = s;

            khachHang = khachHang.OrderBy(id => id.Hoten);
            return View(khachHang.ToPagedList(pageNum, pageSize));
        }

        // GET: KhachHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            List<SelectListItem> gioitinh = new List<SelectListItem>();
            gioitinh.Add(new SelectListItem { Text = "Nam", Value = "Nam" });
            gioitinh.Add(new SelectListItem { Text = "Nữ", Value = "Nữ" });
            ViewBag.Gioitinh = gioitinh;

            return View();
        }

        // POST: KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,Hoten,SDT,DiaChi,GioiTinh,NgaySinh,Email,Username,Password")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,Hoten,SDT,DiaChi,GioiTinh,NgaySinh,Email,Username,Password")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khachHang);
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
