using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Models;

public partial class HoaDonNhap
{
    [Required(ErrorMessage = "Vui lòng nhập mã hoá đơn")]
    public string MaHdn { get; set; } = null!;

    public string? MaNv { get; set; }

    public string? MaNcc { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập ngày nhập")]
    public DateTime? NgayNhap { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<ChiTietHdn> ChiTietHdns { get; } = new List<ChiTietHdn>();

    public virtual NhaCungCap? MaNccNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
