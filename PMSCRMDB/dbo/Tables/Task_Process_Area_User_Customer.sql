CREATE TABLE [dbo].[Task_Process_Area_User_Customer] (
    [task_process_area_user_customer_id] UNIQUEIDENTIFIER CONSTRAINT [DF__Task_Proc__task___72C60C4A] DEFAULT (newid()) NOT NULL,
    [company_id]                         UNIQUEIDENTIFIER NOT NULL,
    [task_process_area_id]               UNIQUEIDENTIFIER NOT NULL,
    [user_id]                            UNIQUEIDENTIFIER NOT NULL,
    [customer_id]                        UNIQUEIDENTIFIER NOT NULL,
    [start_date]                         SMALLDATETIME    NOT NULL,
    [end_date]                           SMALLDATETIME    NOT NULL,
    [status]                             TINYINT          NOT NULL,
    [timestamp]                          SMALLDATETIME    CONSTRAINT [DF__Task_Proc__times__73BA3083] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Task_Pro__A077543E75EDDF30] PRIMARY KEY CLUSTERED ([task_process_area_user_customer_id] ASC),
    CONSTRAINT [FK__Task_Proc__compa__74AE54BC] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    CONSTRAINT [FK__Task_Proc__custo__778AC167] FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer] ([customer_id]),
    CONSTRAINT [FK__Task_Proc__task___75A278F5] FOREIGN KEY ([task_process_area_id]) REFERENCES [dbo].[Task_Process_Area] ([task_process_area_id]),
    CONSTRAINT [FK__Task_Proc__user___76969D2E] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id])
);

