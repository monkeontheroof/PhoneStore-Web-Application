//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_dienthoai.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTietDH
    {
        public long MaDH { get; set; }
        public int IdSP { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public Nullable<decimal> Dongia { get; set; }
        public Nullable<decimal> Thanhtien { get; set; }
    
        public virtual DonHang DonHang { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}
