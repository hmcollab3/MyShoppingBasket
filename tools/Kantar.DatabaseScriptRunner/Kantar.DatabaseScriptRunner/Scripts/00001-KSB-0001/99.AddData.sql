IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Product'
)
BEGIN
    INSERT INTO [Kantar].[Product] (Name, Description)
    SELECT 'Soup', 'Warm and hearty soup'
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Product] WHERE Name = 'Soup'
    );

    INSERT INTO [Kantar].[Product] (Name, Description)
    SELECT 'Bread', 'Freshly baked bread'
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Product] WHERE Name = 'Bread'
    );

    INSERT INTO [Kantar].[Product] (Name, Description)
    SELECT 'Milk', 'Whole milk, 1L carton'
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Product] WHERE Name = 'Milk'
    );

    INSERT INTO [Kantar].[Product] (Name, Description)
    SELECT 'Apples', 'Fresh green apples'
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Product] WHERE Name = 'Apples'
    );
END

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Country'
)
BEGIN
    INSERT INTO [Kantar].[Country] (Name, ISO3166)
    SELECT 'Portugal', 'PT'
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Country] WHERE Name = 'Portugal'
    );

    INSERT INTO [Kantar].[Country] (Name, ISO3166)
    SELECT 'GreatBritain', 'GB'
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Country] WHERE Name = 'GreatBritain'
    );

    INSERT INTO [Kantar].[Country] (Name, ISO3166)
    SELECT 'USA', 'US'
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Country] WHERE Name = 'USA'
    );
END

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Price'
)
BEGIN
    INSERT INTO [Kantar].[Price] (Currency, BasePrice, ProductId, CountryId)
    SELECT 'EUR', 0.65, 1, 1
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Price] WHERE ProductId = 1 AND CountryId = 1
    );

    INSERT INTO [Kantar].[Price] (Currency, BasePrice, ProductId, CountryId)
    SELECT 'EUR', 0.8, 2, 1
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Price] WHERE ProductId = 2 AND CountryId = 1
    );

    INSERT INTO [Kantar].[Price] (Currency, BasePrice, ProductId, CountryId)
    SELECT 'EUR', 1.3, 3, 1
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Price] WHERE ProductId = 3 AND CountryId = 1
    );

    INSERT INTO [Kantar].[Price] (Currency, BasePrice, ProductId, CountryId)
    SELECT 'EUR', 1, 4, 1
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Price] WHERE ProductId = 4 AND CountryId = 1
    );
END

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'Discount'
)
BEGIN
    INSERT INTO [Kantar].[Discount] (
        StartDateUtc,
        EndDateUtc,
        AffectedProductId,
        DiscountFactor
    )
    SELECT 
        '2025-09-01T00:00:00', 
        '2025-09-30T23:59:59', 
        4,
        0.10
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[Discount] 
        WHERE AffectedProductId = 4 
          AND DiscountFactor = 0.10
          AND StartDateUtc = '2025-09-01T00:00:00'
          AND EndDateUtc = '2025-09-30T23:59:59'
    );
END

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'Kantar' 
      AND TABLE_NAME = 'MultiBuyDiscount'
)
BEGIN
    INSERT INTO [Kantar].[MultiBuyDiscount] (
        StartDateUtc,
        EndDateUtc,
        AffectedProductId,
        DiscountFactor,
        TriggeringProductId,
        TriggerQuantity
    )
    SELECT 
        '2025-09-01T00:00:00', 
        '2025-09-30T23:59:59', 
        2,
        0.50,
        1,
        2
    WHERE NOT EXISTS (
        SELECT 1 FROM [Kantar].[MultiBuyDiscount]
        WHERE AffectedProductId = 2
          AND DiscountFactor = 0.50
          AND TriggeringProductId = 1
          AND TriggerQuantity = 2
          AND StartDateUtc = '2025-09-01T00:00:00'
          AND EndDateUtc = '2025-09-30T23:59:59'
    );
END