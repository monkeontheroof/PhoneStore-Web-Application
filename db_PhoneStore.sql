use master 
if exists(select * from sysdatabases where name='db_PhoneStore') 
drop database db_PhoneStore
GO
create database db_PhoneStore
GO
use db_PhoneStore
GO


-------------------TẠO BẢNG-----------------

--ThuongHieu
CREATE TABLE ThuongHieu
(
	MaTH CHAR(2) PRIMARY KEY NOT NULL,
	TenTH nvarchar(50) NOT NULL
)
--SanPham
CREATE TABLE SanPham
(
	IdSP int identity(1,1),
	MaSP AS 'SP' + RIGHT('00' + CAST(IdSP as varchar(2)), 2) PERSISTED,
	TenSP nvarchar(50) NOT NULL,
	MoTaSP NTEXT,
	Gia money check (gia >= 0),
	HinhMinhHoa nvarchar(50),
	Tinhtrang ntext,
	MaTH char(2) NOT NULL,
	CONSTRAINT PK_SanPham PRIMARY KEY (IdSP)
)
GO

--Mau
CREATE TABLE Mau
(
	MaMau char(3) primary key NOT NULL,
	IdSP int,
	TenMau nvarchar(50) NOT NULL,
	Soluong int,
	Tinhtrang ntext
)
GO

--HinhSP
CREATE TABLE HinhSP
(
	MaMau char(3) NOT NULL,
	IdSP int,
	MaHinh varchar(20),
	CONSTRAINT PK_HinhSP PRIMARY KEY (MaMau, IdSP, MaHinh)
)
GO


--KHACHHANG
CREATE TABLE KhachHang
(
	MaKH int IDENTITY(1,1) primary key NOT NULL,
	Hoten nvarchar(50) NOT NULL,
	SDT char(10) ,
	DiaChi nvarchar(100),
	GioiTinh nvarchar(3) check (GioiTinh in(N'Nam',N'Nữ')),
	NgaySinh  SMALLDATETIME,
	Email varchar(50),
	Username varchar(18),
	Password varchar(18)
)
GO

CREATE TABLE NhanVien(
	MaNV int IDENTITY(1,1) primary key,
	Hoten nvarchar(50) NOT NULL,
	SDT char(11),
	GioiTinh nvarchar(3) check (GioiTinh in(N'Nam',N'Nữ')),
	DiaChi nvarchar(100),
	NgaySinh smalldatetime,
	ChucVu char(2),
	Anhdaidien nvarchar(50)
)
GO

CREATE TABLE TaiKhoanNV(
	MaNV int primary key,
	Username varchar(30),
	Password varchar (30) NOT NULL
)
GO
--DonHang
CREATE TABLE DonHang
(
	MaDH BIGINT IDENTITY(1,1) primary key NOT NULL,
	MaKH int NOT NULL,
	TenNguoiNhan nvarchar(50) NOT NULL,
	SDTnhan char(10)  NOT NULL,
	DiaChiNhan nvarchar(100)  NOT NULL,
	TriGia MONEY check (TriGia > 0),
	TinhTrang nvarchar(25),
	NgayDH SMALLDATETIME,
	HTThanhToan varchar(50),
	HTGiaohang varchar(50),
	MaNV int,
)
GO


--ChiTietDH
CREATE TABLE ChiTietDH
(
	MaDH BIGINT,
	IdSP int,
	SoLuong int check(Soluong>0),
	Dongia MONEY check(Dongia >=0 ),
	Thanhtien AS SoLuong * Dongia
	CONSTRAINT PK_ChiTietDH PRIMARY KEY (MaDH, IdSP)
)

GO
--TSKTSP--
CREATE TABLE ThongSo
(
	IdSP int,
	TenTS nvarchar(50),
	Mota nvarchar(100),
	CONSTRAINT PK_ThongSo PRIMARY KEY (IdSP,TenTS)
)
GO


GO
ALTER TABLE [dbo].[ChiTietDH]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDH_DonHang] FOREIGN KEY([MaDH])
REFERENCES [dbo].[DonHang] ([MaDH])
GO
ALTER TABLE [dbo].[ChiTietDH] CHECK CONSTRAINT [FK_ChiTietDH_DonHang]
GO
ALTER TABLE [dbo].[ChiTietDH]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDH_SanPham] FOREIGN KEY([IdSP])
REFERENCES [dbo].[SanPham] ([IdSP])
GO
ALTER TABLE [dbo].[ChiTietDH] CHECK CONSTRAINT [FK_ChiTietDH_SanPham]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_KhachHang] FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachHang] ([MaKH])
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_KhachHang]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_NhanVien] FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_NhanVien]
GO
ALTER TABLE [dbo].[HinhSP]  WITH CHECK ADD  CONSTRAINT [FK_HinhSP_Mau] FOREIGN KEY([MaMau])
REFERENCES [dbo].[Mau] ([MaMau])
GO
ALTER TABLE [dbo].[HinhSP] CHECK CONSTRAINT [FK_HinhSP_Mau]
GO
ALTER TABLE [dbo].[Mau]  WITH CHECK ADD  CONSTRAINT [FK_Mau_SanPham] FOREIGN KEY([IdSP])
REFERENCES [dbo].[SanPham] ([IdSP])
GO
ALTER TABLE [dbo].[Mau] CHECK CONSTRAINT [FK_Mau_SanPham]
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD  CONSTRAINT [FK_SanPham_ThuongHieu] FOREIGN KEY([MaTH])
REFERENCES [dbo].[ThuongHieu] ([MaTH])
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [FK_SanPham_ThuongHieu]
GO
ALTER TABLE [dbo].[TaiKhoanNV]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoanNV_NhanVien] FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanVien] ([MaNV])
GO
ALTER TABLE [dbo].[TaiKhoanNV] CHECK CONSTRAINT [FK_TaiKhoanNV_NhanVien]
GO
ALTER TABLE [dbo].[ThongSo]  WITH CHECK ADD  CONSTRAINT [FK_ThongSo_SanPham] FOREIGN KEY([IdSP])
REFERENCES [dbo].[SanPham] ([IdSP])
GO
ALTER TABLE [dbo].[ThongSo] CHECK CONSTRAINT [FK_ThongSo_SanPham]
GO
ALTER TABLE [dbo].[ChiTietDH]  WITH CHECK ADD CHECK  (([Dongia]>=(0)))
GO
ALTER TABLE [dbo].[ChiTietDH]  WITH CHECK ADD CHECK  (([Soluong]>(0)))
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD CHECK  (([TriGia]>(0)))
GO
ALTER TABLE [dbo].[KhachHang]  WITH CHECK ADD CHECK  (([GioiTinh]=N'Nữ' OR [GioiTinh]=N'Nam'))
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD CHECK  (([GioiTinh]=N'Nữ' OR [GioiTinh]=N'Nam'))
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD CHECK  (([gia]>=(0)))
GO



-------------------------------
insert into ThuongHieu
values ('IP','IPHONE')
insert into ThuongHieu
values ('SS','SAMSUNG')
insert into ThuongHieu
values ('OP','OPPO')
insert into ThuongHieu
values ('HW','HUAWEI')
insert into ThuongHieu
values ('RM','REALME')
insert into ThuongHieu
values ('VV','VIVO')
insert into ThuongHieu
values ('XM','XIAO MI')


-----------KHACH HANG------------

insert into NhanVien(Hoten, ChucVu)
values('test', 'AD');

insert into TaiKhoanNV values(1, 'admin', 'Admin@123');

select * from TaiKhoanNV

