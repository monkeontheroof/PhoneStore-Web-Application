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
        public ActionResult Index(string currentFilter, string s, int? page)
        {

            int pageSize = 15;
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
                sanPhams = sanPhams.Where(i => i.MaSP.Contains(s) || i.ThuongHieu.TenTH.Contains(s) || i.TenSP.Contains(s) || i.Gia.ToString().Contains(s));
            }
            ViewBag.CurrentFilter = s;

            sanPhams = sanPhams.OrderBy(id => id.Gia);

            return View(sanPhams.ToPagedList(pageNum, pageSize));
        }


        public ActionResult LayHangDT()
        {
            var dsDT = db.ThuongHieux.ToList();
            return PartialView(dsDT);
        }


        public ActionResult SPTheoHang(string MaTH, int? page)
        {
            var sanpham = from sp in db.SanPhams.Where(d => d.MaTH.Contains(MaTH))
                          select sp;
            
            int pageSize = 9;
            int pageNum = (page ?? 1);

            var tenTH = db.ThuongHieux.Find(MaTH);
            ViewBag.HangDT = tenTH.TenTH;

            sanpham = sanpham.OrderBy(s => s.Gia);
            return View(sanpham.ToPagedList(pageNum, pageSize));
        }

        
        
        public ActionResult Details(int? id)
        {
            SanPham sanpham = db.SanPhams.Find(id);
            
            return View(sanpham);
        }
    }
}