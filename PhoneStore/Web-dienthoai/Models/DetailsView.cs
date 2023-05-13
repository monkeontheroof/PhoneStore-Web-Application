using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_dienthoai.Models
{
    public class DetailsView
    {
        public SanPham SanPham { get; set; }

        public List<HinhSP> HinhSP { get; set; }

        public ThongSo ThongSo { get; set; }

        public ThuongHieu ThuongHieu { get; set; }

    }
}