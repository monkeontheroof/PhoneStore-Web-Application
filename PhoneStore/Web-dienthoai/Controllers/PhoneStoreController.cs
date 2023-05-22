using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class PhoneStoreController : Controller
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: PhoneStore
        public ActionResult Index(string s)
        {

            var sanpham = from sp in db.SanPhams select sp;

            if (!String.IsNullOrEmpty(s))
            {
                sanpham = sanpham.Where(sp => sp.TenSP.Contains(s) ||
                                        sp.ThuongHieu.TenTH.Contains(s));
            }
            return View(sanpham);
        }


        public ActionResult LayHangDT()
        {
            var dsDT = db.ThuongHieux.ToList();
            return PartialView(dsDT);
        }


        public ActionResult SPTheoHang(string MaTH)
        {
            var sanpham = db.SanPhams.Where(id => id.MaTH == MaTH).ToList();

            var tenTH = db.ThuongHieux.Find(MaTH);
            ViewBag.HangDT = tenTH.TenTH;
            return View(sanpham);
        }

        
        
        public ActionResult Details(int? id)
        {
            SanPham sanpham = db.SanPhams.Find(id);
            
            return View(sanpham);
        }
    }
}