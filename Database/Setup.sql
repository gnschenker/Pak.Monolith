CREATE DATABASE [EventStore]
GO

USE [EventStore]
GO

CREATE TABLE Events(
  Id uniqueidentifier not null,
  [Index] [bigint] IDENTITY(1,1) NOT NULL,
  Created DATETIME not null,
  AggregateType nvarchar(100) not null,
  AggregateId uniqueidentifier not null,
  Version int not null,
  EventName nvarchar(200) not null,
  Event nvarchar(MAX) not null,
  MetaData nvarchar(MAX) null,
  Dispatched bit not null default(0),
  Constraint PKEvents PRIMARY KEY(ID)
)
GO

CREATE INDEX Idx_Events_AggregateId
ON Events(AggregateId)
GO

CREATE INDEX Idx_Events_Dispatched
ON Events(Dispatched)
GO

-- ***************************************

CREATE DATABASE [Projections]
GO

USE [Projections]
GO

CREATE TABLE OrdersView(
  Id uniqueidentifier not null,
  [Version] int not null,
  CreatedOn datetime not null,
  UpdatedOn datetime not null,
  [Status] int not null,
  CustomerId uniqueidentifier not null,
  SenderId uniqueidentifier not null,
  PackageIds NVARCHAR(MAX) null,
  Constraint PKOrdersView PRIMARY KEY(Id)
)
GO