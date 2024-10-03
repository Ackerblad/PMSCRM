CREATE TABLE [dbo].[Area] (
    [area_id]     UNIQUEIDENTIFIER CONSTRAINT [DF__Area__area_id__571DF1D5] DEFAULT (newid()) NOT NULL,
    [company_id]  UNIQUEIDENTIFIER NOT NULL,
    [name]        NVARCHAR (50)    NOT NULL,
    [description] NVARCHAR (255)   NULL,
    [timestamp]   SMALLDATETIME    CONSTRAINT [DF__Area__timestamp__5812160E] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Area__985D6D6BE6F5CCBB] PRIMARY KEY CLUSTERED ([area_id] ASC),
    CONSTRAINT [FK__Area__company_id__59063A47] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id])
);

