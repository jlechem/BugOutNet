USE [BugOutNet]
GO

/****** Object:  Table [dbo].[BugAttachments]    Script Date: 8/30/2016 3:41:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BugAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BugId] [int] NOT NULL,
	[Attachment] [varbinary](max) NOT NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[UploadedById] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_BugAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[BugAttachments]  WITH CHECK ADD  CONSTRAINT [FK_BugAttachments_Bugs] FOREIGN KEY([BugId])
REFERENCES [dbo].[Bugs] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BugAttachments] CHECK CONSTRAINT [FK_BugAttachments_Bugs]
GO

ALTER TABLE [dbo].[BugAttachments]  WITH CHECK ADD  CONSTRAINT [FK_BugAttachments_Users] FOREIGN KEY([UploadedById])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[BugAttachments] CHECK CONSTRAINT [FK_BugAttachments_Users]
GO

