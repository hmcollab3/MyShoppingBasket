IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'ReceiptItem'
)
BEGIN
    CREATE TABLE [Kantar].[ReceiptItem] (
        Id INT IDENTITY(1,1) NOT NULL,
        ReceiptId INT NOT NULL,
        Name NVARCHAR(128) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        Currency NVARCHAR(3) NOT NULL,
        Quantity INT NOT NULL,
        Discount DECIMAL(18,2) NOT NULL,
        MultiBuyTotalSavings DECIMAL(18,2) NOT NULL,
        MultiBuyDiscountedCount INT NOT NULL,
        ItemTotalCost DECIMAL(18,2) NOT NULL,
        CONSTRAINT [PK_ReceiptItem] PRIMARY KEY([Id]),
        FOREIGN KEY(ReceiptId) REFERENCES [Kantar].[Receipt]([Id])
    );
END