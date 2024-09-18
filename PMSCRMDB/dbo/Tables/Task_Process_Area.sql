CREATE TABLE [dbo].[Task_Process_Area] (
    [task_process_area_id] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]           UNIQUEIDENTIFIER NOT NULL,
    [task_id]              UNIQUEIDENTIFIER NOT NULL,
    [process_id]           UNIQUEIDENTIFIER NOT NULL,
    [area_id]              UNIQUEIDENTIFIER NOT NULL,
    [timestamp]            SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([task_process_area_id] ASC),
    FOREIGN KEY ([area_id]) REFERENCES [dbo].[Area] ([area_id]),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    FOREIGN KEY ([process_id]) REFERENCES [dbo].[Process] ([process_id]),
    FOREIGN KEY ([task_id]) REFERENCES [dbo].[Task] ([task_id])
);

