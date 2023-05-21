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
                try
                {
                    db.ThongSoes.Add(thongSo);
                    db.SaveChanges();
                    TempData["ThongBaoSuccess"] = "Thêm thành công " + thongSo.TenTS.ToString();
                    return RedirectToAction("Create", new {id = thongSo.IdSP});
                }
                catch (Exception ex)
                {
                    TempData["ThongBaoFailed"] = "Không thành công! Hãy thử lại.";
                    return RedirectToAction("Create", new { id = thongSo.IdSP });
                }
            }

            ViewBag.IdSP = new SelectList(db.SanPhams, "IdSP", "MaSP", thongSo.IdSP);
            return View(thongSo);
        }

        // GET: ThongSoes/Edit/5
        public ActionResult Edit(int id, string tents)
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
                try
                {
                    db.Entry(thongSo).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["ThongBaoSuccess"] = "Cập nhật thông số " + thongSo.TenTS.ToString() + " thành công!";
                    return RedirectToAction("Edit", new {id = thongSo.TenTS});
                }
                catch(Exception ex)
                {
                    TempData["ThongBaoFailed"] = "Cập nhật thất bại!";
                    return RedirectToAction("Edit", new { id = thongSo.TenTS });
                }
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
            try
            {
                db.ThongSoes.Remove(thongSo);
                db.SaveChanges();
                TempData["ThongBaoSuccess"] = "Xóa thành công thông số " + thongSo.TenTS.ToString();
                return RedirectToAction("Details", "SanPhams", new { id = id });
            }
            catch(Exception ex)
            {
                TempData["ThongBaoFailed"] = "Xóa thất bại!";
                return RedirectToAction("Details", "SanPhams", new { id = id });
            }
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
