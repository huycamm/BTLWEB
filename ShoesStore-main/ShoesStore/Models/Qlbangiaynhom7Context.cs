using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoesStore.Models.ProcedureModels;

namespace ShoesStore.Models;

public partial class Qlbangiaynhom7Context : DbContext
{
    public Qlbangiaynhom7Context()
    {
    }

    public Qlbangiaynhom7Context(DbContextOptions<Qlbangiaynhom7Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietHdb> ChiTietHdbs { get; set; }

    public virtual DbSet<ChiTietHdn> ChiTietHdns { get; set; }

    public virtual DbSet<Giay> Giays { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HoaDonBan> HoaDonBans { get; set; }

    public virtual DbSet<HoaDonNhap> HoaDonNhaps { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiGiay> LoaiGiays { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<Tuser> Tusers { get; set; }

    public virtual DbSet<ProcTienBan30Ngay> ProcTienBan30Ngays { get; set; }

    public virtual DbSet<ProcTienNhap30Ngay> ProcTienNhap30Ngays { get; set; }

    public virtual DbSet<ProcTongHDB30Ngay> ProcTongHDB30Ngays { get; set; }

    public virtual DbSet<ProcTongTienBanHangThang> ProcTongTienBanHangThangs { get; set; }
    
    public virtual DbSet<ProcPlaceAnOrder> ProcPlaceAnOrders { get; set; }

    //Connection string for deployed database
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("data source=QLBanGiay.mssql.somee.com;initial catalog=QLBanGiay;user id=qlbangiay_SQLLogin_1;password=lertr2t3ef;connect timeout=30;encrypt=false;trustservercertificate=false;applicationintent=readwrite;multisubnetfailover=false");
    //Connection string for local
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer("Server= DESKTOP-NT6Q85Q; Database= QLBanGiay; Integrated Security=True;TrustServerCertificate=true;");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => new { e.MaGioHang, e.MaGiay }).HasName("PK_Giay_ChiTietGioHang");

            entity.ToTable("ChiTietGioHang", tb => tb.HasTrigger("Trig_ChiTietGioHang_TinhTongTien"));

            entity.Property(e => e.MaGioHang).HasMaxLength(20);
            entity.Property(e => e.MaGiay).HasMaxLength(20);

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGiay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietGi__MaGia__36B12243");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGioHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietGi__MaGio__35BCFE0A");
        });

        modelBuilder.Entity<ChiTietHdb>(entity =>
        {
            entity.HasKey(e => new { e.MaHdb, e.MaGiay }).HasName("PK_Giay_ChiTietHDB");

            entity.ToTable("ChiTietHDB", tb =>
                {
                    tb.HasTrigger("Trig_ChiTietHDB_TinhTongTien");
                    tb.HasTrigger("Trig_Sach_CapNhatSLKhiBan");
                });

            entity.Property(e => e.MaHdb)
                .HasMaxLength(20)
                .HasColumnName("MaHDB");
            entity.Property(e => e.MaGiay).HasMaxLength(20);
            entity.Property(e => e.KhuyenMai).HasColumnType("money");

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.ChiTietHdbs)
                .HasForeignKey(d => d.MaGiay)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHD__MaGia__45F365D3");

            entity.HasOne(d => d.MaHdbNavigation).WithMany(p => p.ChiTietHdbs)
                .HasForeignKey(d => d.MaHdb)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHD__MaHDB__44FF419A");
        });

        modelBuilder.Entity<ChiTietHdn>(entity =>
        {
            entity.HasKey(e => new { e.MaHdn, e.MaGiay }).HasName("PK_Giay_ChiTietHDN");

            entity.ToTable("ChiTietHDN", tb =>
                {
                    tb.HasTrigger("Trig_ChiTietHDN_TinhTongTien");
                    tb.HasTrigger("Trig_Sach_CapNhatSLKhiNhap");
                });

            entity.Property(e => e.MaHdn)
                .HasMaxLength(20)
                .HasColumnName("MaHDN");
            entity.Property(e => e.MaGiay).HasMaxLength(20);
            entity.Property(e => e.KhuyenMai).HasColumnType("money");

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.ChiTietHdns)
                .HasForeignKey(d => d.MaGiay)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHD__MaGia__3E52440B"); 

            entity.HasOne(d => d.MaHdnNavigation).WithMany(p => p.ChiTietHdns)
                .HasForeignKey(d => d.MaHdn)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHD__MaHDN__3D5E1FD2");
        });

        modelBuilder.Entity<Giay>(entity =>
        {
            entity.HasKey(e => e.MaGiay).HasName("PK__Giay__747065AE20948FF1");

            entity.ToTable("Giay", tb => tb.HasTrigger("Trig_Giay_PhanTramGiam"));

            entity.Property(e => e.MaGiay).HasMaxLength(20);
            entity.Property(e => e.AnhDaiDien).HasMaxLength(255);
            entity.Property(e => e.GiaBan).HasColumnType("money");
            entity.Property(e => e.GiaGoc).HasColumnType("money");
            entity.Property(e => e.GiaNhap).HasColumnType("money");
            entity.Property(e => e.MaLoai).HasMaxLength(20);
            entity.Property(e => e.MauSac).HasMaxLength(50);
            entity.Property(e => e.TenGiay).HasMaxLength(100);
            entity.Property(e => e.TinhTrang).HasMaxLength(25);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Giays)
                .HasForeignKey(d => d.MaLoai)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Giay__MaLoai__300424B4");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA3EAC0D6CB");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGioHang).HasMaxLength(20);
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__GioHang__MaKH__32E0915F");
        });

        modelBuilder.Entity<HoaDonBan>(entity =>
        {
            entity.HasKey(e => e.MaHdb).HasName("PK__HoaDonBa__3C90E8FA1829A93C");

            entity.ToTable("HoaDonBan");

            entity.Property(e => e.MaHdb)
                .HasMaxLength(20)
                .HasColumnName("MaHDB");
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayBan).HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDonBans)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__HoaDonBan__MaKH__4222D4EF");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDonBans)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__HoaDonBan__GhiCh__412EB0B6");
        });

        modelBuilder.Entity<HoaDonNhap>(entity =>
        {
            entity.HasKey(e => e.MaHdn).HasName("PK__HoaDonNh__3C90E8C6617FB51C");

            entity.ToTable("HoaDonNhap");

            entity.Property(e => e.MaHdn)
                .HasMaxLength(20)
                .HasColumnName("MaHDN");
            entity.Property(e => e.MaNcc)
                .HasMaxLength(20)
                .HasColumnName("MaNCC");
            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayNhap).HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.HoaDonNhaps)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK__HoaDonNha__MaNCC__3A81B327");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDonNhaps)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__HoaDonNhap__MaNV__398D8EEE");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E41FA3856");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.AnhDaiDien).HasMaxLength(255);
            entity.Property(e => e.DiaChi).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TaiKhoan).HasMaxLength(100);
            entity.Property(e => e.TenKh)
                .HasMaxLength(100)
                .HasColumnName("TenKH");

            entity.HasOne(d => d.TaiKhoanNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.TaiKhoan)
                .HasConstraintName("FK__KhachHang__TaiKh__29572725");
        });

        modelBuilder.Entity<LoaiGiay>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiGiay__730A575901F4DBA7");

            entity.ToTable("LoaiGiay");

            entity.Property(e => e.MaLoai).HasMaxLength(20);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEB77CC8C96");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc)
                .HasMaxLength(20)
                .HasColumnName("MaNCC");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenNcc)
                .HasMaxLength(255)
                .HasColumnName("TenNCC");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A9E7578A5");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(20)
                .HasColumnName("MaNV");
            entity.Property(e => e.AnhDaiDien).HasMaxLength(255);
            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.HoTenNv)
                .HasMaxLength(255)
                .HasColumnName("HoTenNV");
            entity.Property(e => e.Luong).HasColumnType("money");
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TaiKhoan).HasMaxLength(100);

            entity.HasOne(d => d.TaiKhoanNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.TaiKhoan)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__TaiKho__267ABA7A");
        });

        modelBuilder.Entity<Tuser>(entity =>
        {
            entity.HasKey(e => e.TaiKhoan).HasName("PK__tuser__D5B8C7F1D98CE31A");

            entity.ToTable("tuser");

            entity.Property(e => e.TaiKhoan).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
