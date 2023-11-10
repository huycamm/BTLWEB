create database QLBanGiay

drop database QLBanGiay

create table tuser(
	[TaiKhoan] [nvarchar](100) NOT NULL primary key,
	[MatKhau] [nvarchar] (100) NOT NULL,
	[Role] [int] NOT NULL,
)

CREATE TABLE [dbo].[NhanVien](
	[MaNV] [nvarchar](20) NOT NULL primary key,
	[TaiKhoan] [nvarchar](100) NOT NULL,
	[GioiTinh] [tinyint] NOT NULL, -- 1-Nam, 2-Nu, 3-Khac
	[HoTenNV] [nvarchar](255) NULL,
	[NgaySinh] [date] NULL,
	[SoDienThoai] [nvarchar](20) NULL,
	[ChucVu] [nvarchar](50) NULL,
	[Luong] [money],
	[TinhTrangCongViec] [tinyint] NULL, -- 1-Dang lam viec, 2-Da nghi viec
	AnhDaiDien [nvarchar](255),
	FOREIGN KEY (TaiKhoan) REFERENCES tuser(TaiKhoan)
)

CREATE TABLE [dbo].[KhachHang](
	[MaKH] [nvarchar](20) NOT NULL primary key,
	[TaiKhoan] [nvarchar](100) NULL,
	[TenKH] [nvarchar](100) NULL,
	[DiaChi] [text] NULL,
	[SoDienThoai] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[AnhDaiDien] [nvarchar](255) NULL,
	[GhiChu] [Text] NULL,
	FOREIGN KEY (TaiKhoan) REFERENCES tuser(TaiKhoan)
)

CREATE TABLE [dbo].[NhaCungCap](
	[MaNCC] [nvarchar](20) NOT NULL primary key,
	[TenNCC] [nvarchar](255) NULL,
	[SoDienThoai] [nvarchar](20) NULL
)

CREATE TABLE LoaiGiay(
	[MaLoai] [nvarchar](20) NOT NULL primary key,
	[TenLoai] [nvarchar](100) NULL,
)

CREATE TABLE [dbo].[Giay](
	[MaGiay] [nvarchar](20) NOT NULL primary key,
	[MaLoai] [nvarchar](20) NOT NULL,
	[TenGiay] [nvarchar](100) NULL,
	[KichCo] [tinyint] NULL,
	[MauSac] [nvarchar](50) NULL,
	[SoLuong] [int] NULL,
	[GiaNhap] [money] NULL,
	[GiaGoc] [money] NULL,
	[GiaBan] [money] NULL,
	[PhanTramGiam] [float] NULL,
	[TinhTrang] [nvarchar](25) NULL,-- 1 còn hàng, 2- sắp hết hàng, 3-hết hàng
	[DanhGia] [float] NULL,
	[AnhDaiDien] [nvarchar](255) NULL,
	FOREIGN KEY (MaLoai) REFERENCES LoaiGiay(MaLoai)
)

CREATE TABLE [dbo].[GioHang](
	[MaGioHang] [nvarchar](20) NOT NULL primary key,
	[MaKH] [nvarchar](20) NULL,
	[TongTien] [money] NULL,
	FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
)

CREATE TABLE [dbo].[ChiTietGioHang](
	[MaGioHang] [nvarchar](20) NOT NULL,
	[MaGiay] [nvarchar](20) NOT NULL,
	[SoLuong] [int] NULL,
	constraint PK_Giay_ChiTietGioHang primary key(MaGioHang, MaGiay),
	FOREIGN KEY (MaGioHang) REFERENCES GioHang(MaGioHang),
	FOREIGN KEY (MaGiay) REFERENCES Giay(MaGiay)
)

CREATE TABLE [dbo].[HoaDonNhap](
	[MaHDN] [nvarchar](20) NOT NULL primary key,
	[MaNV] [nvarchar](20) NULL,
	[MaNCC] [nvarchar](20) NULL,
	[NgayNhap] [datetime] NULL,
	[TongTien] [money] NULL,	
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
	FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)
)

CREATE TABLE [dbo].[ChiTietHDN](
	[MaHDN] [nvarchar](20) NOT NULL,
	[MaGiay] [nvarchar](20) NOT NULL,
	[SoLuong] [int] NULL,
	[KhuyenMai] [money] NULL,
	constraint PK_Giay_ChiTietHDN primary key(MaHDN, MaGiay),
	FOREIGN KEY (MaHDN) REFERENCES HoaDonNhap(MaHDN),
	FOREIGN KEY (MaGiay) REFERENCES Giay(MaGiay)
)

CREATE TABLE [dbo].[HoaDonBan](
	[MaHDB] [nvarchar](20) NOT NULL primary key,
	[MaNV] [nvarchar](20) NULL,
	[MaKH] [nvarchar](20) NULL,
	[NgayBan] [dateTime] NULL,
	[TrangThai] [tinyint] NULL, -- 1-dang chuan bi hang, 2-dang giao hang, 3-da nhan hang, 4-hoan hang
	[PhuongThucThanhToan] [tinyint] NULL, -- 1-cod, 2-momo, 3-banking
	[TongTien] [money] NULL,
	[GhiChu] [Text] NULL
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
	FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
)

CREATE TABLE [dbo].[ChiTietHDB](
	[MaHDB] [nvarchar](20) NOT NULL,
	[MaGiay] [nvarchar](20) NOT NULL,
	[SoLuong] [int] NULL,
	[KhuyenMai] [money] NULL
	constraint PK_Giay_ChiTietHDB primary key(MaHDB, MaGiay),
	FOREIGN KEY (MaHDB) REFERENCES HoaDonBan(MaHDB),
	FOREIGN KEY (MaGiay) REFERENCES Giay(MaGiay)
)


-- ===============================Trigger=======================================
-- done
--Trigger cap nhat so luong tren bang Giay khi them/sua/xoa 1 ChiTietHDB/ChiTietHDN
create or alter trigger Trig_Sach_CapNhatSLKhiNhap on ChiTietHDN
for insert, update, delete as
begin
	update Giay
	set SoLuong = isnull(SoLuong, 0) + isnull(TongSoLuong, 0)
	from (select MaGiay, sum(SoLuong) as TongSoLuong from inserted group by MaGiay) as tableTemp
	where Giay.MaGiay = tableTemp.MaGiay

	update Giay
	set SoLuong = isnull(SoLuong, 0) - isnull(TongSoLuong, 0)
	from (select MaGiay, sum(SoLuong) as TongSoLuong from deleted group by MaGiay) as tableTemp
	where Giay.MaGiay = tableTemp.MaGiay

	UPDATE Giay
	SET TinhTrang = CASE 
		WHEN SoLuong = 0 THEN 'HetHang'
		WHEN SoLuong <= 10 THEN 'SapHetHang'
		ELSE 'ConHang'
	END
	from (select MaGiay, sum(SoLuong) as TongSoLuong from deleted group by MaGiay) as tableTemp
	where Giay.MaGiay = tableTemp.MaGiay
end

-- done
/*---------------------------------------------------*/
create or alter trigger Trig_Sach_CapNhatSLKhiBan on ChiTietHDB
for insert, update, delete as
begin
	update Giay
	set SoLuong = isnull(SoLuong, 0) - isnull(TongSoLuong, 0)
	from (select MaGiay, sum(SoLuong) as TongSoLuong from inserted group by MaGiay) as tableTemp
	where Giay.MaGiay = tableTemp.MaGiay

	update Giay
	set SoLuong = isnull(SoLuong, 0) + isnull(TongSoLuong, 0)
	from (select MaGiay, sum(SoLuong) as TongSoLuong from deleted group by MaGiay) as tableTemp
	where Giay.MaGiay = tableTemp.MaGiay
end

-- done
--Trigger tinh tong tien tren bang HoaDonBan khi them/sua/xoa 1 ChiTietHDB
create or alter trigger Trig_ChiTietHDB_TinhTongTien on ChiTietHDB
for insert, update, delete as
begin
	declare @sohdb1 nvarchar(20), @sohdb2 nvarchar(20), @thanhTien1 money, @thanhTien2 money
	select @sohdb1 = MaHDB, @thanhtien1 = (inserted.SoLuong*GiaBan - KhuyenMai) from inserted join Giay on inserted.MaGiay = Giay.MaGiay
	select @sohdb2 = MaHDB, @thanhtien2 = (deleted.SoLuong*GiaBan - KhuyenMai) from deleted join Giay on deleted.MaGiay = Giay.MaGiay

	update HoaDonBan set TongTien=isnull(TongTien,0) + isnull(@thanhtien1,0) - isnull(@thanhtien2,0) where MaHDB = isnull(@sohdb1, @sohdb2)
end		

-- done
--Trigger tinh tong tien tren bang HoaDonNhap khi them/sua/xoa 1 ChiTietHDN
create or alter trigger Trig_ChiTietHDN_TinhTongTien on ChiTietHDN
for insert, update, delete as
begin
	declare @sohdn1 nvarchar(20), @sohdn2 nvarchar(20), @thanhTien1 money, @thanhTien2 money
	select @sohdn1 = MaHDN, @thanhtien1 = (inserted.SoLuong*GiaBan - KhuyenMai) from inserted join Giay on inserted.MaGiay = Giay.MaGiay
	select @sohdn2 = MaHDN, @thanhtien2 = (deleted.SoLuong*GiaBan - KhuyenMai) from deleted join Giay on deleted.MaGiay = Giay.MaGiay

	update HoaDonNhap set TongTien=isnull(TongTien,0) + isnull(@thanhtien1,0) - isnull(@thanhtien2,0) where MaHDN = isnull(@sohdn1, @sohdn2)
end	

--Trigger tinh tong tien tren bang GioHang khi them/sua/xoa 1 ChiTietGioHang
create or alter trigger Trig_ChiTietGioHang_TinhTongTien on ChiTietGioHang
for insert, update, delete as
begin
	declare @gh1 nvarchar(20), @gh2 nvarchar(20), @thanhTien1 money, @thanhTien2 money
	select @gh1 = MaGioHang, @thanhtien1 = (inserted.SoLuong*GiaBan) from inserted join Giay on inserted.MaGiay = Giay.MaGiay
	select @gh2 = MaGioHang, @thanhtien2 = (deleted.SoLuong*GiaBan) from deleted join Giay on deleted.MaGiay = Giay.MaGiay

	update GioHang set TongTien=isnull(TongTien,0) + isnull(@thanhtien1,0) - isnull(@thanhtien2,0) where MaGioHang = isnull(@gh1, @gh2)
end	

-- Trigger tinh phan tram giam khi them/sua bang Giay
-- PhanTramGiam = 100 - (GiaBan/GiaGoc)*100
create or alter trigger Trig_Giay_PhanTramGiam on Giay
for insert, update as
begin
	declare @phanTramGiam float
	declare @maGiay nvarchar(20)
	select @phanTramGiam = 1-inserted.GiaBan/inserted.GiaGoc, @maGiay = inserted.MaGiay from inserted join Giay on inserted.MaGiay = Giay.MaGiay

	update Giay set phanTramGiam = ROUND(isnull(@phanTramGiam, 0)*100, 0) where MaGiay = @maGiay
end	


-- =================================Procedure==========================================

-- Procedure tính tổng tiền bán hàng trong 30 ngày gần nhất
CREATE or alter PROCEDURE TongTienBan30Ngay
AS
BEGIN
    DECLARE @StartDate DATETIME = DATEADD(day, -30, GETDATE());
    DECLARE @EndDate DATETIME = GETDATE();

    SELECT isnull(SUM(TongTien),0) AS TotalTongTien
    FROM HoaDonBan
    WHERE NgayBan BETWEEN @StartDate AND @EndDate;
END

-- Procedure tính tổng tiền nhập hàng trong 30 ngày gần nhất
CREATE or alter PROCEDURE TongTienNhap30Ngay
AS
BEGIN
    DECLARE @StartDate DATETIME = DATEADD(day, -30, GETDATE());
    DECLARE @EndDate DATETIME = GETDATE();

    SELECT isnull(SUM(TongTien),0) AS TotalTongTien
    FROM HoaDonNhap
    WHERE NgayNhap BETWEEN @StartDate AND @EndDate;
END

-- Procedure tính số lượng hóa đơn bán trong 30 ngày gần nhất
CREATE or alter PROCEDURE TongHDB30Ngay
AS
BEGIN
    DECLARE @StartDate DATETIME = DATEADD(day, -30, GETDATE());
    DECLARE @EndDate DATETIME = GETDATE();

    SELECT isnull(count(mahdb),0) AS TotalHDB
    FROM HoaDonBan
    WHERE NgayBan BETWEEN @StartDate AND @EndDate;
END


select * from HoaDonBan

-- Procedure tính tiền bán hàng mỗi tháng trong 1 năm bất kì
-- only show month which have record
CREATE or alter PROCEDURE TongTienBanHangThang
    @Year INT
AS
BEGIN
    SELECT 
        MONTH(NgayBan) AS Thang, 
        isnull(SUM(TongTien),0) AS TongTien 
    FROM 
        HoaDonBan
    WHERE 
        YEAR(NgayBan) = @Year
    GROUP BY 
        MONTH(NgayBan)
    HAVING 
        SUM(TongTien) = 0 OR SUM(TongTien) IS NULL
    ORDER BY 
        Thang ASC
END

-- show all month
CREATE or alter PROCEDURE TongTienBanHangThang
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;
    
    WITH Months AS (
        SELECT 1 AS MonthNumber, 'January' AS MonthName
        UNION SELECT 2, 'February'
        UNION SELECT 3, 'March'
        UNION SELECT 4, 'April'
        UNION SELECT 5, 'May'
        UNION SELECT 6, 'June'
        UNION SELECT 7, 'July'
        UNION SELECT 8, 'August'
        UNION SELECT 9, 'September'
        UNION SELECT 10, 'October'
        UNION SELECT 11, 'November'
        UNION SELECT 12, 'December'
    )
    SELECT 
        Months.MonthNumber AS Thang,
        ISNULL(SUM(HoaDonBan.TongTien), 0) AS TotalTongTien
    FROM Months
    LEFT JOIN HoaDonBan ON 
        MONTH(HoaDonBan.NgayBan) = Months.MonthNumber AND 
        YEAR(HoaDonBan.NgayBan) = @Year
    GROUP BY Months.MonthNumber
    ORDER BY Months.MonthNumber
END

CREATE or alter PROCEDURE InsertUser @TaiKhoan NVARCHAR(255), @MatKhau NVARCHAR(255)
AS
BEGIN
   insert into tuser values (@TaiKhoan,@MatKhau,1);

   DECLARE @NewMaKH NVARCHAR(20)
   DECLARE @NewMaGH NVARCHAR(20)
   SELECT TOP 1 @NewMaKH = MaKH
    FROM KhachHang
    ORDER BY MaKH DESC

    -- Extract the numeric part of the latest ID and increment by 1
    DECLARE @NumericPart INT
    SET @NumericPart = CAST(SUBSTRING(@NewMaKH, 3, LEN(@NewMaKH) - 2) AS INT) + 1

    -- Create the new ID by concatenating 'KH' and the incremented numeric part
    SET @NewMaKH = 'KH0' + CAST(@NumericPart AS NVARCHAR(10))
	set @NewMaGH = 'GH0' + CAST(@NumericPart AS NVARCHAR(10))

	insert into KhachHang values (@NewMaKH, @TaiKhoan, '','', '', '','','');

	INSERT [dbo].[GioHang] ([MaGioHang], [MaKH], [TongTien])
	VALUES (@NewMaGH, @NewMaKH,null)
END


--===========================================================================================================
delete from tuser

delete from NhanVien

insert into tuser values ('admin','PcSWOJPRSRO4TEoTjP/Xeg==',0);
insert into tuser values ('u1','DRm/CCSDkWP50AAlIAPyjQ==',1);

insert into tuser values ('admin1','123',0);
insert into tuser values ('admin2','123',0);
insert into tuser values ('admin3','123',0);
insert into tuser values ('user1','123',1);
insert into tuser values ('user2','123',1);
insert into tuser values ('user3','123',1);
insert into tuser values ('user4','123',1);
insert into tuser values ('user5','123',1);
insert into tuser values ('user6','123',1);
insert into tuser values ('user7','123',1);
insert into tuser values ('user8','123',1);
insert into tuser values ('user9','123',1);
insert into tuser values ('user10','123',1);




insert into KhachHang values ('KH001', 'user1', 'Nguyen Van A','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','Khach hay boom, can than');
insert into KhachHang values ('KH002', 'user2', 'Nguyen Van B','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_2.jpg','');
insert into KhachHang values ('KH003', 'user3', 'Nguyen Van C','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_3.jpg','');
insert into KhachHang values ('KH004', 'user4', 'Nguyen Van D','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_4.jpg','');
insert into KhachHang values ('KH005', 'user5', 'Nguyen Van E','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','');
insert into KhachHang values ('KH006', 'user6', 'Nguyen Van F','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','');
insert into KhachHang values ('KH007', 'user7', 'Nguyen Van G','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','');
insert into KhachHang values ('KH008', 'user8', 'Nguyen Van H','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','');
insert into KhachHang values ('KH009', 'user9', 'Nguyen Van I','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','');
insert into KhachHang values ('KH010', 'user10', 'Nguyen Van J','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','');
insert into KhachHang values ('KH011', 'u1', 'Nguyen Van J','1 Trang Thi, Hoan Kiem, Ha Noi', '0331231234', 'a@gmail.com','person_1.jpg','');


insert into NhanVien values ('NV001', 'admin1', 1, 'Tran Van A', '1999-06-20', '0123456678', 'Marketing',10000000, 1, 'person_2.jpg');
insert into NhanVien values ('NV002', 'admin2', 2, 'Tran Van B', '1999-09-22', '0123456679', 'Accouting',15000000, 1, 'person_2.jpg');
insert into NhanVien values ('NV003', 'admin3', 2, 'Tran Van C', '1989-09-22', '0123456679', 'Staff',7000000, 2, 'person_3.jpg');

INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [SoDienThoai])
VALUES (N'NCC01', N'Gucci', N'0912306199')
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [SoDienThoai])
VALUES (N'NCC02', N'Dolce', N'0904020211')
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [SoDienThoai])
VALUES (N'NCC03', N'Adidas', N'01685897371')
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [SoDienThoai])
VALUES (N'NCC04', N'ThuongDinh', N'0903902290')
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [SoDienThoai])
VALUES (N'NCC05', N'TienPhong', N'0983885168')
INSERT [dbo].[NhaCungCap] ([MaNCC], [TenNCC], [SoDienThoai])
VALUES (N'NCC06', N'Nike', N'0904183670')

INSERT [dbo].[HoaDonNhap] ([MaHDN], [MaNV], [MaNCC], [NgayNhap], [TongTien])
VALUES (N'HDN001', N'NV001',N'NCC01', CAST(N'1980-01-10T00:00:00.000'
AS DateTime),null)
INSERT [dbo].[HoaDonNhap] ([MaHDN], [MaNV], [MaNCC], [NgayNhap], [TongTien]) 
VALUES (N'HDN002', N'NV002',N'NCC02',CAST(N'1981-02-23T00:00:00.000'
AS DateTime),null)
INSERT [dbo].[HoaDonNhap] ([MaHDN], [MaNV], [MaNCC], [NgayNhap], [TongTien])
VALUES (N'HDN003', N'NV003',N'NCC03',CAST(N'1982-03-30T00:00:00.000'
AS DateTime),null)
INSERT [dbo].[HoaDonNhap] ([MaHDN], [MaNV], [MaNCC], [NgayNhap], [TongTien])
VALUES (N'HDN004', N'NV001',N'NCC04',CAST(N'1983-11-02T00:00:00.000'
AS DateTime),null)
INSERT [dbo].[HoaDonNhap] ([MaHDN], [MaNV], [MaNCC], [NgayNhap], [TongTien])
VALUES (N'HDN005', N'NV002',N'NCC05',CAST(N'1984-12-25T00:00:00.000'
AS DateTime),null)
INSERT [dbo].[HoaDonNhap] ([MaHDN], [MaNV], [MaNCC], [NgayNhap], [TongTien])
VALUES (N'HDN006', N'NV003',N'NCC06',CAST(N'1985-06-12T00:00:00.000'
AS DateTime),null)


INSERT [dbo].[GioHang] ([MaGioHang], [MaKH], [TongTien])
VALUES (N'GH001', N'KH001',null)
INSERT [dbo].[GioHang] ([MaGioHang], [MaKH], [TongTien])
VALUES (N'GH002', N'KH002',null)
INSERT [dbo].[GioHang] ([MaGioHang], [MaKH], [TongTien])
VALUES (N'GH003', N'KH003',null)
INSERT [dbo].[GioHang] ([MaGioHang], [MaKH], [TongTien])
VALUES (N'GH004', N'KH004',null)
INSERT [dbo].[GioHang] ([MaGioHang], [MaKH], [TongTien])
VALUES (N'GH005', N'KH005',null)
INSERT [dbo].[GioHang] ([MaGioHang], [MaKH], [TongTien])
VALUES (N'GH011', N'KH011',null)



INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB001', N'NV001',N'KH001',CAST(N'2022-01-01T00:00:00.000' AS DateTime), 0, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB002', N'NV002',N'KH002', CAST(N'2022-02-06T00:00:00.000' AS DateTime), 1, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB003', N'NV003',N'KH003',CAST(N'2022-03-11T00:00:00.000' AS DateTime), 2, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB004', N'NV003',N'KH004',CAST(N'2022-11-24T00:00:00.000' AS DateTime), 3, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB005', N'NV002',N'KH005',CAST(N'2022-04-30T00:00:00.000' AS DateTime), 2, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB006', N'NV001',N'KH001',CAST(N'2022-05-23T00:00:00.000' AS DateTime), 2, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB007', N'NV001',N'KH001',CAST(N'2022-06-01T00:00:00.000' AS DateTime), 0, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB008', N'NV002',N'KH002', CAST(N'2022-07-06T00:00:00.000' AS DateTime), 1, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB009', N'NV003',N'KH003',CAST(N'2022-08-11T00:00:00.000' AS DateTime), 2, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB010', N'NV003',N'KH004',CAST(N'2022-10-24T00:00:00.000' AS DateTime), 3, null)
INSERT [dbo].[HoaDonBan] ([MaHDB], [MaNV], [MaKH], [NgayBan], [TrangThai], [TongTien])
VALUES (N'HDB011', N'NV002',N'KH005',CAST(N'2022-09-30T00:00:00.000' AS DateTime), 2, null)

insert into LoaiGiay values ('L01', N'Sneaker');
insert into LoaiGiay values ('L02', N'Classic');
insert into LoaiGiay values ('L03', N'Formal');
insert into LoaiGiay values ('L04', N'Party');

INSERT Giay VALUES ('G001','L02','Pala GN400', 40, 'Blue', 10, 80, 199, 159,null,null,4.5,'product-1.png');
INSERT Giay VALUES ('G002','L02','Pala GN400', 41, 'Blue', 10, 80, 199, 159,null,null,4.5,'product-1.png');
INSERT Giay VALUES ('G003','L02','Pala GN400', 42, 'Blue', 10, 80, 199, 159,null,null,4.5,'product-1.png');
INSERT Giay VALUES ('G004','L02','Buuck', 40, 'Brown', 12, 60, 199, 139,null,null,4,'product-2.png');
INSERT Giay VALUES ('G005','L02','Buuck', 41, 'Brown', 10, 60, 199, 139,null,null,4,'product-2.png');
INSERT Giay VALUES ('G006','L02','Buuck', 42, 'Brown', 8, 60, 199, 139,null,null,4,'product-2.png');
INSERT Giay VALUES ('G007','L02','Buuck', 43, 'Brown', 15, 60, 199, 139,null,null,4,'product-2.png');
INSERT Giay VALUES ('G008','L04','Gucci Mokassin', 40, 'Red', 12, 60, 199, 139,null,null,4,'product-3.png');
INSERT Giay VALUES ('G009','L04','Gucci Mokassin', 41, 'Red', 10, 60, 199, 139,null,null,4,'product-3.png');
INSERT Giay VALUES ('G010','L04','Gucci Mokassin', 42, 'Red', 8, 60, 199, 139,null,null,4,'product-3.png');
INSERT Giay VALUES ('G011','L04','Gucci Mokassin', 43, 'Red', 15, 60, 199, 139,null,null,4,'product-3.png');
INSERT Giay VALUES ('G012','L03','Vans Slip-on', 38, 'Blue', 10, 70, 239, 199,null,null,5,'product-4.png');
INSERT Giay VALUES ('G013','L03','Vans Slip-on', 39, 'Blue', 10, 70, 239, 199,null,null,5,'product-4.png');
INSERT Giay VALUES ('G014','L03','Vans Slip-on', 40, 'Blue', 10, 70, 239, 199,null,null,5,'product-4.png');
INSERT Giay VALUES ('G015','L03','Vans Slip-on', 41, 'Blue', 10, 70, 239, 199,null,null,5,'product-4.png');
INSERT Giay VALUES ('G016','L03','Vans Slip-on', 42, 'Blue', 10, 70, 239, 199,null,null,5,'product-4.png');
INSERT Giay VALUES ('G017','L03','LMX', 36, 'Brown', 10, 100, 299, 249,null,null,4.5,'product-5.png');
INSERT Giay VALUES ('G018','L03','LMX', 37, 'Brown', 10, 100, 299, 249,null,null,4.5,'product-5.png');
INSERT Giay VALUES ('G019','L03','LMX', 38, 'Brown', 10, 100, 299, 249,null,null,4.5,'product-5.png');
INSERT Giay VALUES ('G020','L03','LMX', 39, 'Brown', 10, 100, 299, 249,null,null,4.5,'product-5.png');
INSERT Giay VALUES ('G021','L02','Vans Classic', 40, 'Yellow', 10, 80, 229, 159,null,null,4.5,'product-6.png');
INSERT Giay VALUES ('G022','L02','Vans Classic', 41, 'Yellow', 10, 80, 229, 159,null,null,4.5,'product-6.png');
INSERT Giay VALUES ('G023','L02','Vans Classic', 42, 'Yellow', 10, 80, 229, 159,null,null,4.5,'product-6.png');
INSERT Giay VALUES ('G024','L04','Chanel', 36, 'Black Blue', 10, 60, 199, 139,null,null,3.5,'product-7.png');
INSERT Giay VALUES ('G025','L04','Chanel', 37, 'Black Blue', 10, 60, 199, 139,null,null,3.5,'product-7.png');
INSERT Giay VALUES ('G026','L04','Chanel', 38, 'Black Blue', 10, 60, 199, 139,null,null,3.5,'product-7.png');
INSERT Giay VALUES ('G027','L04','Chanel', 39, 'Black Blue', 10, 60, 199, 139,null,null,3.5,'product-7.png');
INSERT Giay VALUES ('G028','L01','Bitis Hunter', 40, 'Pink', 10, 150, 369, 299,null,null,4,'product-8.png');
INSERT Giay VALUES ('G029','L01','Bitis Hunter', 41, 'Pink', 10, 150, 369, 299,null,null,4,'product-8.png');
INSERT Giay VALUES ('G030','L01','Bitis Hunter', 42, 'Pink', 10, 150, 369, 299,null,null,4,'product-8.png');


INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDN001', N'G001',100,null)
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDN002', N'G002',100,null)
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDN003', N'G003',80,null)
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDN004', N'G004',80,null)
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDN005', N'G005',80,null)
INSERT [dbo].[ChiTietHDN] ([MaHDN], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDN006', N'G006',70,null)



INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB001', N'G001',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB002', N'G001',2,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB003', N'G002',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB004', N'G003',2,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB005', N'G004',3,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB006', N'G004',2,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB001', N'G005',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB002', N'G006',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB003', N'G007',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB004', N'G008',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB005', N'G008',2,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB006', N'G008',3,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB007', N'G012',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB008', N'G015',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB009', N'G016',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB010', N'G010',1,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB011', N'G004',2,null)
INSERT [dbo].[ChiTietHDB] ([MaHDB], [MaGiay], [SoLuong], [KhuyenMai]) VALUES (N'HDB001', N'G022',3,null)



INSERT [dbo].[ChiTietGioHang] ([MaGioHang], [MaGiay], [SoLuong]) VALUES (N'GH001', N'G001',1)
INSERT [dbo].[ChiTietGioHang] ([MaGioHang], [MaGiay], [SoLuong]) VALUES (N'GH002', N'G002',2)
INSERT [dbo].[ChiTietGioHang] ([MaGioHang], [MaGiay], [SoLuong]) VALUES (N'GH003', N'G003',2)
INSERT [dbo].[ChiTietGioHang] ([MaGioHang], [MaGiay], [SoLuong]) VALUES (N'GH004', N'G002',3)
INSERT [dbo].[ChiTietGioHang] ([MaGioHang], [MaGiay], [SoLuong]) VALUES (N'GH005', N'G012',1)
INSERT [dbo].[ChiTietGioHang] ([MaGioHang], [MaGiay], [SoLuong]) VALUES (N'GH001', N'G005',2)
 

 select * from tuser
 select * from KhachHang
 select * from NhanVien
 select * from GioHang

 delete from KhachHang where MaKH = 'KH11'