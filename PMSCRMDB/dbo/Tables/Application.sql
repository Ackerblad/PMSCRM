CREATE TABLE [dbo].[Application] (
    [application_id] UNIQUEIDENTIFIER CONSTRAINT [DF_Application_application_id] DEFAULT (newid()) NOT NULL,
    [data]           NVARCHAR (MAX)   NOT NULL
);

