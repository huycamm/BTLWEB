using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Models;

public partial class NhanVien
{
    [Required(ErrorMessage = "Vui lòng nhập mã nhân viên")]
    public string MaNv { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
    public string? TaiKhoan { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    public byte GioiTinh { get; set; }

    public string? HoTenNv { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
    [Range(typeof(DateTime), "1/1/1950", "1/1/2005", ErrorMessage = "Ngày sinh phải đủ 18 tuổi")]
    public DateTime? NgaySinh { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập SDT")]
    [RegularExpression("([0-9]+)", ErrorMessage = "SĐT sai định dạng")]
    [MaxLength(10)]
    [MinLength(10, ErrorMessage = "SDT có phải có 10 chữ số")]
    public string? SoDienThoai { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập chức vụ")]
    public string? ChucVu { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập lương")]
    public decimal? Luong { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tình trạng")]
    public byte? TinhTrangCongViec { get; set; }

    public string? AnhDaiDien { get; set; }

    public virtual ICollection<HoaDonBan> HoaDonBans { get; } = new List<HoaDonBan>();

    public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; } = new List<HoaDonNhap>();

    public virtual Tuser? TaiKhoanNavigation { get; set; }
}
