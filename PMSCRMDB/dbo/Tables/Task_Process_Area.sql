CREATE TABLE [dbo].[Task_Process_Area] (
    [task_process_area_id] UNIQUEIDENTIFIER CONSTRAINT [DF__Task_Proc__task___5BE2A6F2] DEFAULT (newid()) NOT NULL,
    [company_id]           UNIQUEIDENTIFIER NOT NULL,
    [task_id]              UNIQUEIDENTIFIER NOT NULL,
    [process_id]           UNIQUEIDENTIFIER NOT NULL,
    [area_id]              UNIQUEIDENTIFIER NOT NULL,
    [timestamp]            SMALLDATETIME    CONSTRAINT [DF__Task_Proc__times__5CD6CB2B] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Task_Pro__42ABA6518838CF42] PRIMARY KEY CLUSTERED ([task_process_area_id] ASC),
    CONSTRAINT [FK__Task_Proc__area___5FB337D6] FOREIGN KEY ([area_id]) REFERENCES [dbo].[Area] ([area_id]),
    CONSTRAINT [FK__Task_Proc__compa__60A75C0F] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    CONSTRAINT [FK__Task_Proc__proce__5DCAEF64] FOREIGN KEY ([process_id]) REFERENCES [dbo].[Process] ([process_id]),
    CONSTRAINT [FK__Task_Proc__task___5EBF139D] FOREIGN KEY ([task_id]) REFERENCES [dbo].[Task] ([task_id])
);

