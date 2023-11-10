using System;
using System.Collections.Generic;

namespace ShoesStore.Models;

public partial class GioHang
{
    public string MaGioHang { get; set; } = null!;

    public string? MaKh { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; } = new List<ChiTietGioHang>();

    public virtual KhachHang? MaKhNavigation { get; set; }
}
