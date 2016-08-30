USE [BugOutNet]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 8/30/2016 3:44:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [varbinary](max) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[IsVerified] [bit] NOT NULL CONSTRAINT [DF_Users_IsVerified]  DEFAULT ((0)),
	[IsBlocked] [bit] NOT NULL CONSTRAINT [DF_Users_IsBlocked]  DEFAULT ((0)),
	[AccessFailedCount] [int] NOT NULL CONSTRAINT [DF_Users_AccessFailedCount]  DEFAULT ((0)),
	[LastLogin] [datetime] NULL,
	[Avatar] [varbinary](max) NULL,
	[AutoLogin] [bit] NOT NULL CONSTRAINT [DF_Users_AutoLogin]  DEFAULT ((0)),
	[IsAdmin] [bit] NOT NULL CONSTRAINT [DF_Users_IsAdmin]  DEFAULT ((0)),
	[Salt] [nvarchar](50) NOT NULL,
	[DefaultProjectId] [int] NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Address] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

