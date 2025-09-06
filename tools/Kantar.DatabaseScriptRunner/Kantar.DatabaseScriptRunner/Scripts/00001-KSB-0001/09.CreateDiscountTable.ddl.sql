IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'Kantar'
        AND TABLE_NAME = 'Discount'
)
BEGIN
    CREATE TABLE [Kantar].[Discount] (
        Id INT IDENTITY(1,1) NOT NULL,
        StartDateUtc DATETIME NOT NULL,
        EndDateUtc DATETIME NOT NULL,
        AffectedProductId INT NOT NULL,
        DiscountFactor DECIMAL(3,2) NOT NULL,
        CreationDateUtc DATETIME NOT NULL CONSTRAINT [DF_Discount_CreationDateUtc] DEFAULT (SYSDATETIMEOFFSET()),
        CONSTRAINT [PK_Discount] PRIMARY KEY (Id),
        FOREIGN KEY (AffectedProductId) REFERENCES [Kantar].[Product]([Id])
    );
END