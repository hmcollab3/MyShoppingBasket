IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Price'
)
BEGIN
    CREATE TABLE [Kantar].[Price] (
        Currency NVARCHAR(3) NOT NULL,
        BasePrice DECIMAL(18,2) NOT NULL,
        ProductId INT NOT NULL,
        CountryId INT NOT NULL,
        CreationDateUtc DATETIME NOT NULL CONSTRAINT [DF_Price_CreationDateUtc] DEFAULT (SYSDATETIMEOFFSET()),
        CONSTRAINT [PK_Price] PRIMARY KEY([ProductId],[CountryId]),
        FOREIGN KEY(ProductId) REFERENCES [Kantar].[Product]([Id]),
        FOREIGN KEY(CountryId) REFERENCES [Kantar].[Country]([Id])
    );
END