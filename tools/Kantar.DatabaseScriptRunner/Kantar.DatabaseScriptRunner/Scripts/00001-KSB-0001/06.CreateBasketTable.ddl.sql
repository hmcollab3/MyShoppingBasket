IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Basket'
)
BEGIN
    CREATE TABLE [Kantar].[Basket] (
        Id INT IDENTITY(1,1),
        ClientId INT,
        Status NVARCHAR(32),
        CountryId INT,
        CreationDateUtc DATETIME NOT NULL CONSTRAINT [DF_Basket_CreationDateUtc] DEFAULT (SYSDATETIMEOFFSET()),
        CONSTRAINT [PK_Basket] PRIMARY KEY([Id]),
        FOREIGN KEY(ClientId) REFERENCES [Kantar].[Client]([Id]),
        FOREIGN KEY(CountryId) REFERENCES [Kantar].[Country]([Id]),
    );
END