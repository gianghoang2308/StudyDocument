-- Kiểm tra và xóa cơ sở dữ liệu nếu tồn tại
USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'StudyPlatform_BKAP')
BEGIN
    ALTER DATABASE StudyPlatform_BKAP SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE StudyPlatform_BKAP;
END
GO

CREATE DATABASE StudyPlatform_BKAP;
GO

USE StudyPlatform_BKAP;
GO


-- Bảng Category
CREATE TABLE Category (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(250)  NULL,
	Type int null,
    Level nvarchar (100)  null,
	Tag nvarchar (300)  null,
	Title nvarchar (300)  null,
	Descrtiption nvarchar (300)  null,
	Keyword nvarchar (300)  null,
	Ord int  null,
	Status bit  null,
	Image nvarchar (500)  null,
	[Index] int  null,
	Lang nvarchar (20)  null,
);

-- Bảng Admin
CREATE TABLE Admin (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    UserName VARCHAR(300) NOT NULL unique,
    FullName NVARCHAR(300) NOT NULL,
	Avatar varchar(300) NULL,
    Password CHAR(300) NOT NULL,
    DateOfBirth DATE NULL, 
    Address NVARCHAR(300) NULL,  
    Email VARCHAR(200) NOT NULL,
    Phone VARCHAR(200) NULL, 
    IdCard VARCHAR(200) NOT NULL,
    DateOfIssue DATE NULL,
    PlaceOfIssue NVARCHAR(200) NULL, 
    Gender BIT NOT NULL,
    Role INT NOT NULL DEFAULT 1,
    Status BIT NOT NULL DEFAULT 1,
    Create_time DATETIME NOT NULL
);


-- Bảng User
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    UserName VARCHAR(300) NOT NULL UNIQUE,
    FullName NVARCHAR(300) NOT NULL,
    Password CHAR(300) NOT NULL,
	Avatar varchar(300) NULL,
    DateOfBirth DATE NOT NULL,
    Address NVARCHAR(300) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    Phone VARCHAR(200) NOT NULL,
    IdCard VARCHAR(200) NOT NULL,
    DateOfIssue DATE NOT NULL,
    PlaceOfIssue NVARCHAR(200) NOT NULL,
    Gender BIT NOT NULL,
    Status BIT NOT NULL,
    Image1 VARCHAR(200) NULL,
    Image2 VARCHAR(200) NULL,
    Image3 VARCHAR(200) NULL,
    Create_time DATETIME NOT NULL,
	Role INT NOT NULL DEFAULT 0,
);


--Bảng Video--
CREATE TABLE Video (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Category_id INT NOT NULL,
    Name NVARCHAR(200) NULL,
    Link VARCHAR(500) NULL,
    Video VARCHAR(500) NULL,
    VideoType VARCHAR(20) NULL,
    VideoData VARBINARY(MAX) NULL, 
    UserId Int NULL,
    Title NVARCHAR(250) NULL,
    Description NVARCHAR(MAX) NULL,
    Status BIT NULL,
    Thumbnail VARCHAR(500) NULL,
    Views INT DEFAULT 0,
    Create_time DATETIME NULL,
   
    FOREIGN KEY (Category_id) REFERENCES Category(Id)
);



-- Bảng Image
CREATE TABLE Image (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Category_id INT NOT NULL,
    Name NVARCHAR(200) NULL,
    Link VARCHAR(500) NULL,
    Image VARCHAR(200) NULL,
    ImageType VARCHAR(500) NULL,
    ImageData VARBINARY(MAX) NULL,
    Create_time DATETIME NULL,
    Status BIT NULL,
    UserId Int NULL,
   
    FOREIGN KEY (Category_id) REFERENCES Category(Id)
);


-- Bảng Document
CREATE TABLE Document (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Category_id INT NOT NULL,
    Title NVARCHAR(200) NULL,
    Description NVARCHAR(MAX) NULL,
    [File] NVARCHAR(300) NULL,
    FileType NVARCHAR(MAX) NULL, 
    FileData VARBINARY(MAX) NULL,
    Create_time DATETIME NULL,
    Status BIT NULL,
    UserID int  NULL,
   
    FOREIGN KEY (Category_id) REFERENCES Category(Id)
);

--Bảng Comment--
CREATE TABLE Comment (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Image_id INT NOT NULL,
    Video_id INT NOT NULL,
    Document_id INT NOT NULL,
    User_id INT NOT NULL,
    Name NVARCHAR(200) NULL,
    Address NVARCHAR(MAX) NULL,
    Phone VARCHAR(200) NULL,
    Email VARCHAR(200) NULL,
    DateOfBirth DATE NULL,
    Message NVARCHAR(MAX) NULL,
    Date DATETIME NULL,
    Status BIT NULL,
    
    FOREIGN KEY (User_id) REFERENCES [User](Id),
    FOREIGN KEY (Image_id) REFERENCES Image(Id),
    FOREIGN KEY (Video_id) REFERENCES Video(Id),
    FOREIGN KEY (Document_id) REFERENCES Document(Id),
);

-- Bảng Viewer
CREATE TABLE Viewer (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    LocalIp NVARCHAR(300) NULL,
    PublicIp NVARCHAR(300) NULL,
    AccessTime DATETIME DEFAULT GETDATE(),
    UserAgent NVARCHAR(500) NULL,        
    UserId INT NULL,                    
    Location NVARCHAR(500) NULL,        
    DeviceType NVARCHAR(50) NULL,        
    Status BIT DEFAULT 1,
    TotalAccessCountWeek INT DEFAULT 0, 
    TotalAccessCountMonth INT DEFAULT 0, 
    TotalAccessCountYear INT DEFAULT 0
);

-- Bảng Contact
CREATE TABLE Contact (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(300) NULL,
    Email VARCHAR(200) NULL,
    Address NVARCHAR(300) NULL,
    Phone NVARCHAR(300) NULL,
    Message NVARCHAR(MAX) NULL,
    Status BIT NULL,
    Create_time DATETIME NULL
);


-- Bảng Config
CREATE TABLE Config (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Mail VARCHAR(200) NULL,
    Mail_Port VARCHAR(200) NULL,
    Mail_Info VARCHAR(200) NULL,
    Mail_noreply VARCHAR(200) NULL,
    Mail_Password VARCHAR(200) NULL,
    PlaceHead VARCHAR(200) NULL,
    PlaceBody VARCHAR(200) NULL,
    GoogleId VARCHAR(200) NULL,
    Contact VARCHAR(200) NULL,
    Coppyright VARCHAR(200) NULL,
    Title VARCHAR(200) NULL,
    Description VARCHAR(200) NULL,
    Keyword VARCHAR(200) NULL,
    Lang VARCHAR(200) NULL,
    Hotline VARCHAR(200) NULL,
    SocialLink1 VARCHAR(200) NULL,
    SocialLink2 VARCHAR(200) NULL,
    SocialLink3 VARCHAR(200) NULL,
    SocialLink4 VARCHAR(200) NULL,
    SocialLink5 VARCHAR(200) NULL,
    SocialLink6 VARCHAR(200) NULL,
    LinkVideo1 VARCHAR(200) NULL,
    LinkVideo2 VARCHAR(200) NULL,
    LinkVideo3 VARCHAR(200) NULL,
    LinkVideo4 VARCHAR(200) NULL
);


-- Bảng Menu
CREATE TABLE Menu (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(300) NULL,
    Tag NVARCHAR(300) NULL,
    Content NTEXT NULL,
    Detail NTEXT NULL,
    Level VARCHAR(200) NULL,
    Title NVARCHAR(200) NULL,
    Description NTEXT NULL,
    Keyword NVARCHAR(300) NULL,
    Type INT NULL,
    Link VARCHAR(300) NULL,
    Target VARCHAR(100) NULL,
    [Index] INT NULL,
    Position INT NULL,
    Ord INT NULL,
    Status BIT NULL DEFAULT 1, -- Trạng thái mặc định là 1 (hoạt động)
    Lang VARCHAR(5) NULL
);


-- Bảng Advertise
CREATE TABLE Advertise (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Title NVARCHAR(200) NULL,
    ImageUrl VARCHAR(200) NULL,
    VideoUrl VARCHAR(200) NULL,
    Link VARCHAR(200) NULL,
    Position INT NULL, -- Fixed typo from Possition to Position
    Status BIT NULL DEFAULT 1,
    Start DATETIME NULL,
    [End] DATETIME NULL, 
    Create_time DATETIME NULL DEFAULT GETDATE()
);

select * from Admin


select * from [User]

select * from Menu