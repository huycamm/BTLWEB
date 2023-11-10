using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoesStore.Models;

public partial class LoaiGiay
{
    [Required(ErrorMessage = "Vui lòng nhập mã loại")]
    public string MaLoai { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập tên loại")]
    public string? TenLoai { get; set; }

    public virtual ICollection<Giay> Giays { get; } = new List<Giay>();
}
