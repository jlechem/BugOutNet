USE [BugOutNet]
GO

/****** Object:  Table [dbo].[BugComments]    Script Date: 8/30/2016 3:42:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BugComments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[BugId] [int] NOT NULL,
	[CreatorId] [int] NOT NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [PK_BugComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[BugComments]  WITH CHECK ADD  CONSTRAINT [FK_BugComments_Bugs] FOREIGN KEY([BugId])
REFERENCES [dbo].[Bugs] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BugComments] CHECK CONSTRAINT [FK_BugComments_Bugs]
GO

