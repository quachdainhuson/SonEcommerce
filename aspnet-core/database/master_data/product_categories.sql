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
       ,'path/to/cover/picture.jpg' -- Thay 'path/to/cover/picture.jpg' bằng đường dẫn thích hợp hoặc giá trị hình ảnh
       ,1
       ,1
       ,null -- Thay null bằng khóa của danh mục cha (nếu có)
       ,N'Danh Mục Điện Thoại'
       ,'{"exampleKey": "exampleValue"}'
       ,'fb43bd5b4c1d4fcd99b43ab38df2220e'
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
       ,'C1'
       ,'laptop'
       ,1
       ,'path/to/cover/picture.jpg' -- Thay 'path/to/cover/picture.jpg' bằng đường dẫn thích hợp hoặc giá trị hình ảnh
       ,1
       ,1
       ,null -- Thay null bằng khóa của danh mục cha (nếu có)
       ,N'Máy tính xách tay'
       ,'{"exampleKey": "exampleValue"}'
       ,'fb43bd5b4c1d4fcd99b43ab38df2220e'
       ,GETDATE()
       ,null)