INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (NEWID()
           ,N'Apple'
           ,'M1'
           ,'apple'
           ,1
           ,1
           ,1
           ,'US'
           ,'{"exampleKey": "exampleValue"}'
           ,'fb43bd5b4c1d4fcd99b43ab38df2220e'
           ,GETDATE()
           ,null)
INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (NEWID()
           ,N'Microsoft'
           ,'M2'
           ,'microsoft'
           ,1
           ,1
           ,1
           ,'US'
           ,'{"exampleKey": "exampleValue"}'
           ,'fb43bd5b4c1d4fcd99b43ab38df2220e'
           ,GETDATE()
           ,null)