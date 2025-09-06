IF NOT EXISTS (
    SELECT name
    FROM sys.schemas
    WHERE name = N'Kantar'
)
BEGIN
    EXEC('CREATE SCHEMA [Kantar]');
END