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
    
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.ChiTietDHs = new HashSet<ChiTietDH>();
            this.Maus = new HashSet<Mau>();
        }
    
        public int IdSP { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string MoTaSP { get; set; }
        public Nullable<decimal> Gia { get; set; }
        public string HinhMinhHoa { get; set; }
        public string Tinhtrang { get; set; }
        public string MaTH { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDH> ChiTietDHs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mau> Maus { get; set; }
        public virtual ThuongHieu ThuongHieu { get; set; }
        public virtual ThongSo ThongSo { get; set; }
    }
}
