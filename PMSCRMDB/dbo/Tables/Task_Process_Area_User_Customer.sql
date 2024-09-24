CREATE TABLE [dbo].[Task_Process_Area_User_Customer] (
    [task_process_area_user_customer_id] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]                         UNIQUEIDENTIFIER NOT NULL,
    [task_process_area_id]               UNIQUEIDENTIFIER NOT NULL,
    [user_id]                            UNIQUEIDENTIFIER NOT NULL,
    [customer_id]                        UNIQUEIDENTIFIER NOT NULL,
    [start_date]                         SMALLDATETIME    NOT NULL,
    [end_date]                           SMALLDATETIME    NOT NULL,
    [status]                             TINYINT          NOT NULL,
    [timestamp]                          SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([task_process_area_user_customer_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer] ([customer_id]),
    FOREIGN KEY ([task_process_area_id]) REFERENCES [dbo].[Task_Process_Area] ([task_process_area_id]),
    CONSTRAINT [FK__Task_Proc__user___76969D2E] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id])
);

