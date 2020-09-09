IF  EXISTS ( SELECT * FROM sys.objects WHERE object_id = OBJECT_ID( N'[dbo].[messagesWeb]' ) AND type in ( N'U' ) )
    DROP TABLE [dbo].[messagesWeb]
GO

CREATE TABLE [dbo].[messagesWeb](
	[requestID] [uniqueidentifier] NOT NULL,
	[requestDate] [datetime2](7) NOT NULL,
	[firstname] [nvarchar](256) NOT NULL,
	[name] [nvarchar](128) NOT NULL,
	[mail] [nvarchar](256) NOT NULL,
	[costcenter] [nvarchar](12) NOT NULL,
	[reason] [nvarchar](4000) NOT NULL,
	[approved] [bit] NOT NULL,
	[approvalDate] [datetime2](7) NOT NULL,
	[status] [tinyint] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[messagesWeb] ADD PRIMARY KEY CLUSTERED (
	[requestID] ASC
) WITH ( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF ) ON [PRIMARY]
GO
