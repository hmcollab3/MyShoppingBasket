IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Client'
)
BEGIN
    CREATE TABLE [Kantar].[Client] (
        Id INT IDENTITY(1,1),
        Name VARCHAR(64) NOT NULL,
        Email VARCHAR(64) NOT NULL UNIQUE,
        Role VARCHAR(64) NOT NULL,
        PasswordHash VARCHAR(255) NOT NULL,
        CreationDateUtc DATETIME NOT NULL CONSTRAINT [DF_Client_CreationDateUtc] DEFAULT (SYSDATETIMEOFFSET()),
        CONSTRAINT [PK_Client] PRIMARY KEY([Id]),
    );
END