USE [Shana_2002]
GO
/****** Object:  Table [dbo].[Designations]    Script Date: 30-06-2024 21:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designations](
	[DesignationsId] [int] NULL,
	[Name] [varchar](150) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 30-06-2024 21:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[DOB] [date] NOT NULL,
	[Age] [int] NOT NULL,
	[Designation] [varchar](150) NOT NULL,
	[Gender] [char](1) NULL,
	[Status] [bit] NULL,
	[ImageURL] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 30-06-2024 21:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[Passwword] [varchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_EmployeeCRUD]    Script Date: 30-06-2024 21:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_EmployeeCRUD]
    @Action NVARCHAR(10),
    @EmployeeID INT = NULL,
    @Name NVARCHAR(100) = NULL,
    @DOB DATE = NULL,
    @Age INT = NULL,
    @Designation NVARCHAR(50) = NULL,
    @Gender CHAR(1) = NULL,
    @Status BIT = NULL,
    @ImageURL NVARCHAR(MAX) = NULL
AS
BEGIN
    IF @Action = 'CREATE'
    BEGIN
        INSERT INTO Employee (Name, DOB, Age, Designation, Gender, Status, ImageURL)
        VALUES (@Name, @DOB, @Age, @Designation, @Gender, @Status, @ImageURL);
    END
    ELSE IF @Action = 'READ'
    BEGIN
        SELECT * FROM Employee WHERE EmployeeID = @EmployeeID;
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE Employee
        SET Name = @Name, DOB = @DOB, Age = @Age, Designation = @Designation, Gender = @Gender, Status = @Status, ImageURL = @ImageURL
        WHERE EmployeeID = @EmployeeID;
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM Employee WHERE EmployeeID = @EmployeeID;
    END
    ELSE
    BEGIN
        SELECT * FROM Employee;
    END
END
GO
