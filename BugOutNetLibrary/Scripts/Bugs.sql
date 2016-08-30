USE [BugOutNet]
GO

/****** Object:  Table [dbo].[Bugs]    Script Date: 8/30/2016 3:42:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bugs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatorId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[PriorityId] [int] NOT NULL,
	[AssignedToId] [int] NULL,
	[Created] [datetime] NULL,
	[LatUpdated] [datetime] NULL,
	[LastUpdatedId] [int] NULL,
 CONSTRAINT [PK_Bugs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Bugs]  WITH CHECK ADD  CONSTRAINT [FK_Bugs_AssignedTo] FOREIGN KEY([AssignedToId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Bugs] CHECK CONSTRAINT [FK_Bugs_AssignedTo]
GO

ALTER TABLE [dbo].[Bugs]  WITH CHECK ADD  CONSTRAINT [FK_Bugs_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO

ALTER TABLE [dbo].[Bugs] CHECK CONSTRAINT [FK_Bugs_Categories]
GO

ALTER TABLE [dbo].[Bugs]  WITH CHECK ADD  CONSTRAINT [FK_Bugs_Priority] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Priorities] ([Id])
GO

ALTER TABLE [dbo].[Bugs] CHECK CONSTRAINT [FK_Bugs_Priority]
GO

ALTER TABLE [dbo].[Bugs]  WITH CHECK ADD  CONSTRAINT [FK_Bugs_Projects] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Projects] ([Id])
GO

ALTER TABLE [dbo].[Bugs] CHECK CONSTRAINT [FK_Bugs_Projects]
GO

ALTER TABLE [dbo].[Bugs]  WITH CHECK ADD  CONSTRAINT [FK_Bugs_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Statuses] ([Id])
GO

ALTER TABLE [dbo].[Bugs] CHECK CONSTRAINT [FK_Bugs_Status]
GO

ALTER TABLE [dbo].[Bugs]  WITH CHECK ADD  CONSTRAINT [FK_Bugs_Users] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Bugs] CHECK CONSTRAINT [FK_Bugs_Users]
GO

