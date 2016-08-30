USE [BugOutNet]
GO

/****** Object:  Table [dbo].[Roles]    Script Date: 8/30/2016 3:44:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[CreatorId] [int] NOT NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Users]
GO

