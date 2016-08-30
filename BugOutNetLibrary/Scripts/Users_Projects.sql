USE [BugOutNet]
GO

/****** Object:  Table [dbo].[Users_Projects]    Script Date: 8/30/2016 3:44:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users_Projects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Users_Projects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Users_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Users_Projects_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO

ALTER TABLE [dbo].[Users_Projects] CHECK CONSTRAINT [FK_Users_Projects_Projects]
GO

ALTER TABLE [dbo].[Users_Projects]  WITH CHECK ADD  CONSTRAINT [FK_Users_Projects_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Users_Projects] CHECK CONSTRAINT [FK_Users_Projects_Users]
GO

