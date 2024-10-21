CREATE TABLE [dbo].[Process] (
    [process_id]  UNIQUEIDENTIFIER CONSTRAINT [DF__Process__process__4D94879B] DEFAULT (newid()) NOT NULL,
    [company_id]  UNIQUEIDENTIFIER NOT NULL,
    [area_id]     UNIQUEIDENTIFIER NOT NULL,
    [name]        NVARCHAR (50)    NOT NULL,
    [description] NVARCHAR (255)   NULL,
    [duration]    TINYINT          NOT NULL,
    [timestamp]   SMALLDATETIME    CONSTRAINT [DF__Process__timesta__4E88ABD4] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Process__9446C3E11DFED589] PRIMARY KEY CLUSTERED ([process_id] ASC),
    CONSTRAINT [FK__Process__company__4F7CD00D] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    CONSTRAINT [FK_Area_Process] FOREIGN KEY ([area_id]) REFERENCES [dbo].[Area] ([area_id]) ON DELETE CASCADE ON UPDATE CASCADE
);

