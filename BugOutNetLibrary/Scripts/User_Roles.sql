USE [BugOutNet]
GO

/****** Object:  Table [dbo].[Users_Roles]    Script Date: 8/30/2016 3:44:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users_Roles](
	[Id] [int] NOT NULL,
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Users_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Users_Roles]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Users_Roles] CHECK CONSTRAINT [FK_Users_Roles_Users]
GO

ALTER TABLE [dbo].[Users_Roles]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles_Users_Roles1] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO

ALTER TABLE [dbo].[Users_Roles] CHECK CONSTRAINT [FK_Users_Roles_Users_Roles1]
GO

