using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_dienthoai.Models
{
    public class MatHangMua
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();

        public int IdSP { get; set; }

        public string MaSP { get; set; }

        public string TenSP { get; set; }

        public string Hinhminhhoa { get; set; }

        public double DonGia { get; set; }

        public int SoLuong { get; set; }


        public double ThanhTien()
        {
            return SoLuong * DonGia;
        }

        public MatHangMua(int IdSP)
        {
            this.IdSP = IdSP;
            var phone = db.SanPhams.Single(s => s.IdSP == this.IdSP);
            this.TenSP = phone.TenSP;
            this.DonGia = double.Parse(phone.Gia.ToString());
            this.SoLuong = 1;
            this.Hinhminhhoa = phone.HinhMinhHoa;
        }
    }
}