BEGIN TRANSACTION;
GO

ALTER TABLE [AppProducts] ADD [SellPrice] float NOT NULL DEFAULT 0.0E0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240512125546_AddShellPriceToProduct', N'8.0.4');
GO

COMMIT;
GO

