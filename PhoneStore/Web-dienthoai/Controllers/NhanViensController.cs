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
    public class NhanViensController : Controller
    {
        private QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: NhanViens
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

            var nhanVien = from l in db.NhanViens
                           select l;

            if (!String.IsNullOrEmpty(s))
            {
                nhanVien = nhanVien.Where(i => i.MaNV.ToString().Contains(s) ||
                i.Hoten.Contains(s) ||
                i.ChucVu.Contains(s) ||
                i.TaiKhoanNV.Username.Contains(s) ||
                i.SDT.ToString().Contains(s));
            }
            ViewBag.CurrentFilter = s;

            nhanVien = nhanVien.OrderBy(id => id.Hoten);

            return View(nhanVien.ToPagedList(pageNum, pageSize));
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            List<SelectListItem> gioitinh = new List<SelectListItem>();
            gioitinh.Add(new SelectListItem { Text = "Nam", Value = "Nam" });
            gioitinh.Add(new SelectListItem { Text = "Nữ", Value = "Nữ" });
            ViewBag.Gioitinh = gioitinh;

            List<SelectListItem> chucVu = new List<SelectListItem>();
            chucVu.Add(new SelectListItem { Text = "Admin", Value = "AD" });
            chucVu.Add(new SelectListItem { Text = "Nhân viên", Value = "NV" });
            ViewBag.Chucvu = chucVu;
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,Hoten,SDT,GioiTinh,DiaChi,NgaySinh,ChucVu")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.NhanViens.Add(nhanVien);
                    db.SaveChanges();
                    TempData["ThongBaoSuccess"] = "Thêm thành công nhân viên " + nhanVien.Hoten;
                    return RedirectToAction("Create");
                }
                catch (Exception ex)
                {
                    TempData["ThongBaoFailed"] = "Thêm nhân viên thất bại!";
                    return RedirectToAction("Create");
                }
                
            }

            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            List<SelectListItem> gioitinh = new List<SelectListItem>();
            gioitinh.Add(new SelectListItem { Text = "Nam", Value = "Nam" });
            gioitinh.Add(new SelectListItem { Text = "Nữ", Value = "Nữ" });
            ViewBag.Gioitinh = gioitinh;

            List<SelectListItem> chucVu = new List<SelectListItem>();
            chucVu.Add(new SelectListItem { Text = "Admin", Value = "AD" });
            chucVu.Add(new SelectListItem { Text = "Nhân viên", Value = "NV" });
            ViewBag.Chucvu = chucVu;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,Hoten,SDT,GioiTinh,DiaChi,NgaySinh,ChucVu")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(nhanVien).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["ThongBaoSuccess"] = "Cập nhật thành công nhân viên " + nhanVien.Hoten;
                    return RedirectToAction("Edit", new {id = nhanVien.MaNV});
                }
                catch (Exception ex)
                {
                    TempData["ThongBaoFailed"] = "Cập nhật nhân viên thất bại!";
                    return RedirectToAction("Edit", new { id = nhanVien.MaNV });
                }
                
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                NhanVien nhanVien = db.NhanViens.Find(id);
                TaiKhoanNV tkNhanvien = db.TaiKhoanNVs.Find(id);
                if(tkNhanvien != null)
                    db.TaiKhoanNVs.Remove(tkNhanvien);

                db.NhanViens.Remove(nhanVien);
                db.SaveChanges();
                TempData["ThongBaoSuccess"] = "Xóa thành công nhân viên " + nhanVien.Hoten.ToString().ToUpper();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ThongBaoFailed"] = "Xóa nhân viên thất bại!";
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
