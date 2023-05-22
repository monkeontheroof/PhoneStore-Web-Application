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
        public ActionResult Index(string currentFilter, string s, int? page)
        {
            //Admin Session Check
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            var thuongHieu = from l in db.ThuongHieux 
                             select l;
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

            if (!String.IsNullOrEmpty(s))
            {
                thuongHieu = thuongHieu.Where(i => i.MaTH.Contains(s) || i.TenTH.Contains(s));
            }
            ViewBag.CurrentFilter = s;
            thuongHieu = thuongHieu.OrderBy(id => id.TenTH);

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
                if(db.ThuongHieux.Any(th => th.TenTH == thuongHieu.TenTH))
                {
                    TempData["ThongBaoFailed"] = "Đã tồn tại thương hiệu " + thuongHieu.TenTH;
                    return RedirectToAction("Create");
                }
                else if(db.ThuongHieux.Any(th => th.MaTH == thuongHieu.MaTH))
                {
                    TempData["ThongBaoFailed"] = "Đã tồn tại mã thương hiệu " + thuongHieu.MaTH;
                    return RedirectToAction("Create");
                }
                try
                {
                    db.ThuongHieux.Add(thuongHieu);
                    db.SaveChanges();
                    TempData["ThongBaoSuccess"] = "Thêm thành công thương hiệu " + thuongHieu.TenTH.ToString();
                    return RedirectToAction("Create");
                }
                catch (Exception ex)
                {
                    TempData["ThongBaoFailed"] = "Thêm thương hiệu thất bại";
                    return RedirectToAction("Create");
                }
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
                if (db.ThuongHieux.Any(th => th.TenTH == thuongHieu.TenTH))
                {
                    TempData["ThongBaoFailed"] = "Đã tồn tại thương hiệu " + thuongHieu.TenTH;
                    return RedirectToAction("Edit", new { id = thuongHieu.MaTH });
                }
                try
                {
                    db.Entry(thuongHieu).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["ThongBaoSuccess"] = "Cập nhật thành công " + thuongHieu.TenTH;
                    return RedirectToAction("Edit", new {id = thuongHieu.MaTH});
                }
                catch(Exception ex)
                {
                    TempData["ThongBaoFailed"] = "Cập nhật thất bại!";
                    return RedirectToAction("Edit", new { id = thuongHieu.MaTH });
                }
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
            try
            {
                db.ThuongHieux.Remove(thuongHieu);
                db.SaveChanges();
                TempData["ThongBaoSuccess"] = "Xóa thành công thương hiệu " + thuongHieu.TenTH.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ThongBaoFailed"] = "Xóa thất bại!";
                return RedirectToAction("Index");
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
