using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Models;

public partial class Giay
{
    [Required(ErrorMessage = "Vui lòng nhập mã giày")]
    public string MaGiay { get; set; } = null!;

    public string? MaLoai { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên giày")]
    public string? TenGiay { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập kích cỡ")]
    public byte? KichCo { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập màu sắc")]
    public string? MauSac { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số lượng")]
    public int? SoLuong { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá nhập")]
    public decimal? GiaNhap { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá gốc")]
    public decimal? GiaGoc { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá bán")]
    public decimal? GiaBan { get; set; }

    public double? PhanTramGiam { get; set; }

    public byte? TinhTrang { get; set; }

    public double? DanhGia { get; set; }

    public string? AnhDaiDien { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; } = new List<ChiTietGioHang>();

    public virtual ICollection<ChiTietHdb> ChiTietHdbs { get; } = new List<ChiTietHdb>();

    public virtual ICollection<ChiTietHdn> ChiTietHdns { get; } = new List<ChiTietHdn>();

    public virtual LoaiGiay? MaLoaiNavigation { get; set; }
}
