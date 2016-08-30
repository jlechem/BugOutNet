USE [BugOutNet]
GO

/****** Object:  Table [dbo].[Tokens]    Script Date: 8/30/2016 3:44:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Tokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [varchar](max) NOT NULL,
	[UserId] [int] NOT NULL,
	[Expired] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[LastChecked] [datetime] NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Tokens]  WITH CHECK ADD  CONSTRAINT [FK_Tokens_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Tokens] CHECK CONSTRAINT [FK_Tokens_Users]
GO

