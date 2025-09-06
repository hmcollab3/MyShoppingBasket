IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Receipt'
)
BEGIN
    CREATE TABLE [Kantar].[Receipt] (
        Id INT IDENTITY(1,1) NOT NULL,
        BasketId INT NOT NULL UNIQUE,
        TotalCost DECIMAL(18,2) NOT NULL,
        Currency NVARCHAR(3) NOT NULL,
        CreationDateUtc DATETIME2 NOT NULL,
        CONSTRAINT [PK_Receipt] PRIMARY KEY([Id]),
        FOREIGN KEY(BasketId) REFERENCES [Kantar].[Basket]([Id])
    );
END