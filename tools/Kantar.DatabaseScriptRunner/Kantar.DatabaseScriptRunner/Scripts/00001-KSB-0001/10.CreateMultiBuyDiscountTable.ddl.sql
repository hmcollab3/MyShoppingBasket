IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'Kantar'
        AND TABLE_NAME = 'MultiBuyDiscount'
)
BEGIN
    CREATE TABLE [Kantar].[MultiBuyDiscount] (
        Id INT IDENTITY(1,1) NOT NULL,
        StartDateUtc DATETIME NOT NULL,
        EndDateUtc DATETIME NOT NULL,
        AffectedProductId INT NOT NULL,
        DiscountFactor DECIMAL(3,2) NOT NULL,
        TriggeringProductId INT NOT NULL,
        TriggerQuantity INT NOT NULL,
        CreationDateUtc DATETIME NOT NULL CONSTRAINT [DF_MultiBuyDiscount_CreationDateUtc] DEFAULT (SYSDATETIMEOFFSET()),
        CONSTRAINT [PK_MultiBuyDiscount] PRIMARY KEY (Id),
        FOREIGN KEY (AffectedProductId) REFERENCES [Kantar].[Product]([Id]),
        FOREIGN KEY (TriggeringProductId) REFERENCES [Kantar].[Product]([Id])
    );
END