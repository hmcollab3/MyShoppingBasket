IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'BasketItem'
)
BEGIN
    CREATE TABLE [Kantar].[BasketItem] (
        Id INT IDENTITY(1,1) NOT NULL,
        BasketId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL,
        Currency NVARCHAR(3) NOT NULL,
        OriginalPrice DECIMAL(18,2) NOT NULL,
        PriceAtCheckout DECIMAL(18,2),
        CreationDateUtc DATETIME NOT NULL CONSTRAINT [DF_BasketItem_CreationDateUtc] DEFAULT (SYSDATETIMEOFFSET()),
        CONSTRAINT [PK_BasketItem] PRIMARY KEY([Id]),
        FOREIGN KEY(BasketId) REFERENCES [Kantar].[Basket]([Id]),
        FOREIGN KEY(ProductId) REFERENCES [Kantar].[Product]([Id]),
    );
END