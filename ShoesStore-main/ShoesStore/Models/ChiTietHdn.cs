using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Models;

public partial class ChiTietHdn
{
    public string? MaHdn { get; set; }

    public string? MaGiay { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số lượng")]
    public int? SoLuong { get; set; }

    public decimal? KhuyenMai { get; set; }

    public virtual Giay? MaGiayNavigation { get; set; }

    public virtual HoaDonNhap? MaHdnNavigation { get; set; }
}
