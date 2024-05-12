INSERT INTO [dbo].[AppProductCategories]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[SortOrder]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[ParentId]
           ,[SeoMetaDescription]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (NEWID()
           ,N'Điện Thoại'
           ,'C1'
           ,'dien-thoai'
           ,1
           ,1
           ,1
           ,1
           ,null
           ,N'Danh Mục Điện Thoại'
           ,'test'
           ,'test'
           ,GETDATE()
           ,null)


		   INSERT INTO [dbo].[AppProductCategories]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[SortOrder]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[ParentId]
           ,[SeoMetaDescription]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (NEWID()
           ,N'Laptop'
           ,'C2'
           ,'laptop'
           ,1
           ,1
           ,1
           ,1
           ,null
           ,N'Máy tính xách tay'
           ,'test'
           ,'test'
           ,GETDATE()
           ,null)