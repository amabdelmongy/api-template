USE [db];
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Events' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[Events](
			[Id]               [bigint] IDENTITY(1, 1) NOT NULL,
			[AggregateId]      [uniqueidentifier] NOT NULL,
			[EventData]        [nvarchar](max) NOT NULL,
			[Version]		   [int] not null default 0,
			[CreatedOn]        [datetime2] DEFAULT GETDATE() NOT NULL,	
			[Type]			   [varchar](max) not null
		PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)
	)
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Dispatchs' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[Dispatchs](
		[Id]		[bigint] IDENTITY(1,1) NOT NULL,
		[EventData] [nvarchar](max) NOT NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[Type]		[varchar](max) not null
	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PaymentProjections' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[PaymentProjections](
		[PaymentId]	  [uniqueidentifier] NOT NULL,
		[CardNumber]	  [nvarchar](16) NOT NULL,
		[CardExpiry]	  [nvarchar](10) NOT NULL,
		[CardCvv]		  [nvarchar](3) NOT NULL,
		[LastUpdatedDate] [datetime2](7) NOT NULL,
		[FailedDetails]	  [nvarchar](max) null,
		PRIMARY KEY CLUSTERED
		(
			[PaymentId] ASC
		)
	)
END
