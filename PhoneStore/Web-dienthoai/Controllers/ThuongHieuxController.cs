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
    public class ThuongHieuxController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: ThuongHieux
        public ActionResult Index(int? page)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            var thuongHieu = db.ThuongHieux.ToList();
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(thuongHieu.ToPagedList(pageNum, pageSize));
        }

        // GET: ThuongHieux/Details/5
        public ActionResult Details(string id)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // GET: ThuongHieux/Create
        public ActionResult Create()
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            return View();
        }

        // POST: ThuongHieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTH,TenTH")] ThuongHieu thuongHieu)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            if (ModelState.IsValid)
            {
                db.ThuongHieux.Add(thuongHieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thuongHieu);
        }

        // GET: ThuongHieux/Edit/5
        public ActionResult Edit(string id)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTH,TenTH")] ThuongHieu thuongHieu)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            if (ModelState.IsValid)
            {
                db.Entry(thuongHieu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thuongHieu);
        }

        // GET: ThuongHieux/Delete/5
        public ActionResult Delete(string id)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            db.ThuongHieux.Remove(thuongHieu);
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
