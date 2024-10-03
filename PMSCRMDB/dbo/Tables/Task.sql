CREATE TABLE [dbo].[Task] (
    [task_id]     UNIQUEIDENTIFIER CONSTRAINT [DF__Task__task_id__52593CB8] DEFAULT (newid()) NOT NULL,
    [company_id]  UNIQUEIDENTIFIER NOT NULL,
    [name]        NVARCHAR (50)    NOT NULL,
    [description] NVARCHAR (255)   NULL,
    [duration]    TINYINT          NOT NULL,
    [timestamp]   SMALLDATETIME    CONSTRAINT [DF__Task__timestamp__534D60F1] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Task__0492148D2DD46EA4] PRIMARY KEY CLUSTERED ([task_id] ASC),
    CONSTRAINT [FK__Task__company_id__5441852A] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id])
);

