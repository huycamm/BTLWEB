using System;
using System.Collections.Generic;

namespace ShoesStore.Models;

public partial class ChiTietGioHang
{
    public string MaGioHang { get; set; } = null!;

    public string MaGiay { get; set; } = null!;

    public int? SoLuong { get; set; }

    public virtual Giay MaGiayNavigation { get; set; } = null!;

    public virtual GioHang MaGioHangNavigation { get; set; } = null!;
}
