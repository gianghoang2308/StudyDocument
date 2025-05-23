Create database BookStore_Bkap
go
use BookStore_Bkap
go
Create table Publishers
(
	PublisherId int identity primary key,
	PublisherName nvarchar(200),
	Phone varchar(30),
	Address nvarchar(200)
)
go
Create table Categories
(
	CategoryId int identity primary key,
	CategoryName nvarchar(100)
)
go
Create table Books
(
	BookId varchar(10) primary key,
	Title nvarchar(200) not null,
	Author nvarchar(100),
	Release int,
	Price float,
	Description ntext,
	Picture nvarchar(100),
	PublisherId int foreign key references Publishers(PublisherId),
	CategoryId int foreign key references Categories(CategoryId)
)
go
insert into Publishers values(N'Nhà xuất bản trẻ','098746533',N'Hà Nội')
insert into Publishers values(N'Nhà xuất bản kim đồng','096833435',N'Hà Nội')
insert into Publishers values(N'Nhà xuất bản tri thức','097987653',N'Hà Nội')
insert into Publishers values(N'Nhà xuất bản giáo dục','091956464',N'Hà Nội')
insert into Publishers values(N'Nhà xuất bản lao động','091956464',N'Hà Nội')

insert into Categories values(N'Sách kinh tế')
insert into Categories values(N'Sách tin học')
insert into Categories values(N'Sách thiếu nhi')
insert into Categories values(N'Sách nấu ăn')
insert into Categories values(N'Sách văn học')



insert Books values('P67934',N'Đường Đến Thành Công Của Jack Ma',N'Brad Schepp, Debra Schepp',2015,250000,N'chưa có mô tả','/images/books/P67934.jpg',5,1)
insert Books values('P68050',N'Đừng Bao Giờ Đi Ăn Một Mình',N'Tahl Raz, Keith Ferrazzi',2016,95000,N'chưa có mô tả','/images/books/P68050.jpg',1,1)
insert Books values('P68066',N'Vô Thường',N'Nguyễn Bảo Trung',2016,75000,N'chưa có mô tả','/images/books/P68066.jpg',5,5)
insert Books values('P68009',N'Siêu Mèo Capoo',N'Á Lạp',2016,5800,N'chưa có mô tả','/images/books/P68009.jpg',2,3)
insert Books values('P67842',N'Shin - Cậu Bé Bút Chì',N'Yoshito Usui ',2016,18000,N'chưa có mô tả','/images/books/P67842.jpg',2,3)
insert Books values('P67848',N'Chie - Cô Bé Hạt Tiêu',N'Etsumi Haruki',2016,28000,N'chưa có mô tả','/images/books/P67848.jpg',2,3)
insert Books values('P63952',N'Kung Fu Panda',N'Ngọc Linh',2016,28500,N'chưa có mô tả','/images/books/P63952.jpg',2,3)
insert Books values('P68102',N'Dạy Con Làm Giàu - Tập 9: Những Bí Mật Về Tiền Bạc Mà Bạn Không Học Ở Nhà Trường!',N'Robert T. Kiyosaki',2015,32000,N'chưa có mô tả','/images/books/P68102.jpg',1,1)
insert Books values('P68101',N'Dạy Con Làm Giàu - Tập 8: Để Có Những Đồng Tiền Tích Cực',N'Robert T. Kiyosaki',2015,32000,N'chưa có mô tả','/images/books/P68101.jpg',1,1)
insert Books values('P67957',N'36 Kế Trong Kinh Doanh Hiện Đại',N'Lan Bercu',2016,95000,N'chưa có mô tả','/images/books/P67957.jpg',1,1)
insert Books values('P67821',N'Mật Mã Champa - Phát Hành Dự Kiến',N'Giản Tư Hải',2016,113000,N'chưa có mô tả','/images/books/P67821.jpg',1,5)
insert Books values('P67687',N'Vui buồn trong hộp',N'Đoàn Bảo Châu',2015,250000,N'chưa có mô tả','/images/books/P67687.jpg',1,5)


select * from Books
go

select * from Publishers
go

select * from Categories
go