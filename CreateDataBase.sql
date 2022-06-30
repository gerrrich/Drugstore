﻿USE [Drugstore]
GO

CREATE SCHEMA [Drugstore]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Drugstore].[Product](
	[ProductId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Drugstore].[Store](
	[StoreId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Address] [varchar](500) NULL,
	[Phone] [varchar](11) NULL,
PRIMARY KEY CLUSTERED 
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [Drugstore].[Storage](
	[StorageId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Store] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [Drugstore].[Storage]  WITH CHECK ADD FOREIGN KEY([Store])
REFERENCES [Drugstore].[Store] ([StoreId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Drugstore].[Batch](
	[BatchId] [uniqueidentifier] NOT NULL,
	[Product] [uniqueidentifier] NOT NULL,
	[Storage] [uniqueidentifier] NOT NULL,
	[Count] [smallint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Drugstore].[Batch]  WITH CHECK ADD FOREIGN KEY([Product])
REFERENCES [Drugstore].[Product] ([ProductId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [Drugstore].[Batch]  WITH CHECK ADD FOREIGN KEY([Storage])
REFERENCES [Drugstore].[Storage] ([StorageId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


