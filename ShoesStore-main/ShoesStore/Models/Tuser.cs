using System;
using System.Collections.Generic;

namespace ShoesStore.Models;

public partial class Tuser
{
    public string TaiKhoan { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public int Role { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; } = new List<KhachHang>();

    public virtual ICollection<NhanVien> NhanViens { get; } = new List<NhanVien>();
}
