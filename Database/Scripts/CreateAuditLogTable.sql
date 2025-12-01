-- Create AuditLogs table for Audit.NET integration
-- This table stores audit events from Audit.NET in a format compatible with the existing AuditLogEntry model

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AuditLogs' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[AuditLogs] (
        [AuditLogId] INT IDENTITY(1,1) NOT NULL,
        [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [Timestamp] DATETIME2(7) NOT NULL,
        [UserName] NVARCHAR(255) NULL,
        [EntityType] NVARCHAR(255) NULL,
        [EntityId] NVARCHAR(255) NULL,
        [ActionType] NVARCHAR(100) NULL,
        [PreviousState] NVARCHAR(MAX) NULL,
        [CurrentState] NVARCHAR(MAX) NULL,
        [ApplicationName] NVARCHAR(255) NULL,
        [UserId] NVARCHAR(255) NULL,
        [TenantId] INT NULL,
        [TenantName] NVARCHAR(255) NULL,
        [ExecutionDuration] INT NOT NULL DEFAULT 0,
        [ClientIpAddress] NVARCHAR(255) NULL,
        [CorrelationId] NVARCHAR(255) NULL,
        [BrowserInfo] NVARCHAR(500) NULL,
        [HttpMethod] NVARCHAR(10) NULL,
        [HttpStatusCode] INT NULL,
        [Url] NVARCHAR(2000) NULL,
        [ExceptionDetails] NVARCHAR(MAX) NULL,
        [Comments] NVARCHAR(MAX) NULL,
        [AuditData] NVARCHAR(MAX) NULL,
        [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([AuditLogId] ASC)
    );
    
    -- Create indexes for better query performance
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_Timestamp] ON [dbo].[AuditLogs] ([Timestamp] DESC);
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_EntityType] ON [dbo].[AuditLogs] ([EntityType]);
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_EntityId] ON [dbo].[AuditLogs] ([EntityId]);
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_UserName] ON [dbo].[AuditLogs] ([UserName]);
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_ActionType] ON [dbo].[AuditLogs] ([ActionType]);
    CREATE NONCLUSTERED INDEX [IX_AuditLogs_TenantId] ON [dbo].[AuditLogs] ([TenantId]);
    
    PRINT 'AuditLogs table created successfully with indexes.';
END
ELSE
BEGIN
    PRINT 'AuditLogs table already exists.';
END

-- Grant permissions (adjust as needed for your security model)
-- GRANT SELECT, INSERT ON [dbo].[AuditLogs] TO [CalibrationSaaSUser];
