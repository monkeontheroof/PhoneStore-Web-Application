using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_dienthoai.Models
{
    public class Itemdonhang
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();

        public long MaDH { get; set; }
        public string TenNguoiNhan { get; set; }
        public string SDTnhan { get; set; }
        public string DiaChiNhan { get; set; }
        public string HTThanhToan { get; set; }
        public string HTGiaoHang { get; set; }
        public string NgayDH { get; set; }
        public string TinhTrang { get; set; }
        public int? TriGia { get; set; }
        public string TongSL { get; set; }

        public String TongSoSP()
        {
            int? sl;
            List<ChiTietDH> listctdh = db.ChiTietDHs.Where(s => s.MaDH == MaDH).ToList();
            sl = listctdh.Sum(s => s.SoLuong);
            return sl.ToString();
        }

        public Itemdonhang(long MaDH)
        {
            var dh = db.DonHangs.Find(MaDH);
            var ct = db.ChiTietDHs.FirstOrDefault(s => s.MaDH == this.MaDH);
            this.MaDH = MaDH;
            this.TongSL = TongSoSP();
            this.TriGia = (int?)dh.TriGia;
            this.TinhTrang = dh.TinhTrang;
            this.TenNguoiNhan = dh.TenNguoiNhan;
            this.SDTnhan = dh.SDTnhan;
            this.DiaChiNhan = dh.DiaChiNhan;
            this.HTThanhToan = dh.HTThanhToan;
            this.HTGiaoHang = dh.HTGiaohang;
            this.NgayDH = dh.NgayDH.ToString();
        }

    }
}