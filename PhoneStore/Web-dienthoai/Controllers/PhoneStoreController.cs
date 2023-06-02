using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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

        [Route("PhoneStore/SPTheoHang/{MaTH}&{s}")]
        public ActionResult SPTheoHang(string MaTH, string s)
        {
            var sanpham = from sp in db.SanPhams.Where(d => d.MaTH.Contains(MaTH))
                          select sp;
            if (!String.IsNullOrEmpty(s))
            {
                sanpham = sanpham.Where(sp => sp.TenSP.Contains(s) && sp.MaTH == MaTH);
            }
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