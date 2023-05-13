using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class SanPhamsController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: SanPhams
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

            var sanPhams = from l in db.SanPhams
                           select l;

            if (!String.IsNullOrEmpty(s))
            {
                sanPhams = sanPhams.Where(i => i.MaSP.Contains(s) || i.ThuongHieu.TenTH.Contains(s) || i.TenSP.Contains(s));
            }
            ViewBag.CurrentFilter = s;

            sanPhams = sanPhams.OrderBy(id => id.MaTH);

            return View(sanPhams.ToPagedList(pageNum, pageSize));
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH");
            ViewBag.IdSP = new SelectList(db.ThongSoes, "IdSP", "CongNgheManHinh");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenSP,MoTaSP,Gia,HinhMinhHoa,MaTH")] SanPham sanPham, HttpPostedFileBase Hinhminhhoa)
        {
            if (ModelState.IsValid)
            {
                //Lấy tên file hình được up
                var fileName = Path.GetFileName(Hinhminhhoa.FileName);

                //Tạo đường dẫn tới file
                var path = Path.Combine(Server.MapPath("~/Images/ProductView"), fileName);

                //Kiểm tra hình đã tồn tại hay chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình đã tồn tại";
                }
                else
                {
                    Hinhminhhoa.SaveAs(path);
                }

                sanPham.HinhMinhHoa = fileName;

                ThongSo thongSo = new ThongSo();
                thongSo.IdSP = sanPham.IdSP;
                sanPham.Tinhtrang = "Còn";
                db.SanPhams.Add(sanPham);
                db.ThongSoes.Add(thongSo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH", sanPham.MaTH);
            ViewBag.IdSP = new SelectList(db.ThongSoes, "IdSP", "CongNgheManHinh", sanPham.IdSP);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH", sanPham.MaTH);
            ViewBag.IdSP = new SelectList(db.ThongSoes, "IdSP", "CongNgheManHinh", sanPham.IdSP);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSP, MaSP,TenSP,MaTH,Gia,Tinhtrang,MoTaSP,HinhMinhHoa")] SanPham sanPham, HttpPostedFileBase Hinhminhhoa)
        {
            if (ModelState.IsValid)
            {
                if(Hinhminhhoa != null)
                {
                    //Lấy tên file hình được up
                    var fileName = Path.GetFileName(Hinhminhhoa.FileName);

                    //Tạo đường dẫn tới file
                    var path = Path.Combine(Server.MapPath("~/Images/ProductView"), fileName);

                    //Lưu tên
                    sanPham.HinhMinhHoa = fileName;
                    Hinhminhhoa.SaveAs(path);
                }
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "SanPhams", new { id = sanPham.IdSP});
            }
            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH", sanPham.MaTH);
            ViewBag.IdSP = new SelectList(db.ThongSoes, "IdSP", "CongNgheManHinh", sanPham.IdSP);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThongSo thongSo = db.ThongSoes.Find(id);
            SanPham sanPham = db.SanPhams.Find(id);
            db.ThongSoes.Remove(thongSo);
            db.SanPhams.Remove(sanPham);
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
