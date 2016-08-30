USE [BugOutNet]
GO

/****** Object:  Table [dbo].[Priorities]    Script Date: 8/30/2016 3:43:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Priorities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatorId] [int] NOT NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_Priorities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Priorities]  WITH CHECK ADD  CONSTRAINT [FK_Priorities_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Priorities] CHECK CONSTRAINT [FK_Priorities_Users]
GO

