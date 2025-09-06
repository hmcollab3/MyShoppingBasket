IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Product'
)
BEGIN
    CREATE TABLE [Kantar].[Product] (
        Id INT IDENTITY(1,1),
        Name NVARCHAR(64) NOT NULL,
        Description NVARCHAR(150),
        CreationDateUtc DATETIME NOT NULL CONSTRAINT [DF_Product_CreationDateUtc] DEFAULT (SYSDATETIMEOFFSET()),
        CONSTRAINT [PK_Product] PRIMARY KEY([Id]),
    );
END