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
        public ActionResult Index()
        {

            var sanpham = db.SanPhams.ToList();


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

            return View(sanpham);
        }

        
        
        public ActionResult Details(int id)
        {
            var list = new List<DetailsView>();

            var phone = db.SanPhams.FirstOrDefault(s => s.IdSP == id);
            var tskt = db.ThongSoes.FirstOrDefault(s => s.IdSP == id);

            var pics = (from h in db.HinhSPs
                        join s in db.Maus on h.MaMau equals s.MaMau
                        where s.IdSP == id
                        select h).ToList();

            var myview = new DetailsView()
            {
                SanPham = phone,
                ThongSo = tskt,
                HinhSP = pics
            };


            list.Add(myview);
            
            return View(myview);
        }
    }
}