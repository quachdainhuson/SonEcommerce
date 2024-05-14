BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProducts]') AND [c].[name] = N'ThumbnailPicture');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AppProducts] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AppProducts] ALTER COLUMN [ThumbnailPicture] nvarchar(250) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProducts]') AND [c].[name] = N'SortOrder');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AppProducts] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AppProducts] ALTER COLUMN [SortOrder] int NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProducts]') AND [c].[name] = N'SeoMetaDescription');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AppProducts] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AppProducts] ALTER COLUMN [SeoMetaDescription] nvarchar(250) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProducts]') AND [c].[name] = N'SellPrice');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AppProducts] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AppProducts] ALTER COLUMN [SellPrice] float NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProducts]') AND [c].[name] = N'Description');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AppProducts] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AppProducts] ALTER COLUMN [Description] nvarchar(max) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProductCategories]') AND [c].[name] = N'SortOrder');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [AppProductCategories] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [AppProductCategories] ALTER COLUMN [SortOrder] int NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProductCategories]') AND [c].[name] = N'SeoMetaDescription');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [AppProductCategories] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [AppProductCategories] ALTER COLUMN [SeoMetaDescription] nvarchar(250) NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppProductCategories]') AND [c].[name] = N'CoverPicture');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [AppProductCategories] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [AppProductCategories] ALTER COLUMN [CoverPicture] nvarchar(250) NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppManufacturers]') AND [c].[name] = N'CoverPicture');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [AppManufacturers] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [AppManufacturers] ALTER COLUMN [CoverPicture] nvarchar(250) NULL;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppInventoryTicketItems]') AND [c].[name] = N'BatchNumber');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [AppInventoryTicketItems] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [AppInventoryTicketItems] ALTER COLUMN [BatchNumber] varchar(50) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240513082000_AddNull', N'8.0.4');
GO

COMMIT;
GO

