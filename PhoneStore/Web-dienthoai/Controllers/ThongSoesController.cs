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
    public class ThongSoesController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: ThongSoes
        public ActionResult Index(int id)
        {
            var thongSoes = db.ThongSoes.Where(s => s.IdSP== id);
            return PartialView(thongSoes.ToList());
        }

        // GET: ThongSoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongSo thongSo = db.ThongSoes.Find(id);
            if (thongSo == null)
            {
                return HttpNotFound();
            }
            return View(thongSo);
        }

        // GET: ThongSoes/Create
        public ActionResult Create(int id)
        {
            ThongSo thongSo = new ThongSo()
            {
                IdSP = id,
                TenTS = "Tên thông số..",
                Mota = ""
            };

            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP");
            return View(thongSo);
        }

        // POST: ThongSoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSP,TenTS,Mota")] ThongSo thongSo)
        {
            if (ModelState.IsValid)
            {
                db.ThongSoes.Add(thongSo);
                db.SaveChanges();
                return RedirectToAction("Details", "SanPhams", new {id = thongSo.IdSP});
            }

            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP", thongSo.IdSP);
            return View(thongSo);
        }

        // GET: ThongSoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongSo thongSo = db.ThongSoes.Find(id);
            if (thongSo == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP", thongSo.IdSP);
            return View(thongSo);
        }

        // POST: ThongSoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSP,TenTS,Mota")] ThongSo thongSo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thongSo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP", thongSo.IdSP);
            return View(thongSo);
        }

        // GET: ThongSoes/Delete/5
        public ActionResult Delete(int id, string tents)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongSo thongSo = db.ThongSoes.First(ts => ts.IdSP == id && ts.TenTS == tents);
            if (thongSo == null)
            {
                return HttpNotFound();
            }
            return View(thongSo);
        }

        // POST: ThongSoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string tents)
        {
            ThongSo thongSo = db.ThongSoes.First(ts => ts.IdSP == id && ts.TenTS == tents);
            db.ThongSoes.Remove(thongSo);
            db.SaveChanges();
            return RedirectToAction("Details", "SanPhams", new {id = id});
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
