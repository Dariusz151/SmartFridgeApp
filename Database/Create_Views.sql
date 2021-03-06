USE [dariusz151_smartfridgeapp]
GO
/****** Object:  View [dbo].[v_FoodProducts]    Script Date: 2020-05-14 22:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[v_FoodProducts]
AS
SELECT        fp.FoodProductId, 
			fp.Name, 
			Categories.CategoryId,
			Categories.Name as [Category]
FROM            app.FoodProducts as fp
INNER JOIN [app].Categories on [Categories].CategoryId = fp.CategoryId
GO
/****** Object:  View [dbo].[v_FridgeItems]    Script Date: 2020-05-14 22:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_FridgeItems]
AS
	SELECT        
		fi.Id, 
		fi.UserId, 
		u.FridgeId,
		fi.IsConsumed, 
		fi.EnteredAt, 
		fi.ExpirationDate, 
		fi.Unit, 
		fi.Value, 
		fi.Note,
		fp.FoodProductId,
		fp.Name as [ProductName],
		c.CategoryId as [CategoryId],
		c.Name as [CategoryName]
	FROM            
		app.FridgeItems as fi
		inner join app.FoodProducts as fp
			on fp.FoodProductId = fi.FoodProductId
		inner join app.Categories as c
			on c.CategoryId = fp.CategoryId
		inner join app.Users as u
			on u.Id = fi.UserId
	WHERE fi.IsConsumed = 0

GO
/****** Object:  View [dbo].[v_Fridges]    Script Date: 2020-05-14 22:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_Fridges]
AS
SELECT        Id, Name, Address, [Desc]
FROM            app.Fridges
GO
/****** Object:  View [dbo].[v_FridgeUsers]    Script Date: 2020-05-14 22:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_FridgeUsers]
AS
SELECT        Id, Name, Email, FridgeId, CreatedAt
FROM            app.Users
GO
/****** Object:  View [dbo].[v_Recipes]    Script Date: 2020-05-14 22:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[v_Recipes]
AS
SELECT        r.RecipeId, r.[Name], rc.Name as RecipeCategory, r.Description, r.FoodProducts, r.RequiredTime, r.LevelOfDifficulty
FROM            app.Recipes as r
LEFT JOIN app.RecipeCategories as rc 
	ON rc.RecipeCategoryId = r.RecipeCategoryId
GO



CREATE VIEW [dbo].[v_RecipeCategories]
AS
SELECT        RecipeCategoryId, [Name]
FROM            app.RecipeCategories
GO


CREATE VIEW [dbo].[v_ConsumedFridgeItems]
AS
	SELECT        
		fi.Id, 
		fi.UserId, 
		fi.IsConsumed, 
		fi.EnteredAt, 
		fi.ExpirationDate, 
		fi.Unit, 
		fi.Value, 
		fi.Note,
		fp.FoodProductId,
		fp.Name as [ProductName],
		c.CategoryId as [CategoryId],
		c.Name as [CategoryName]
	FROM            
		app.FridgeItems as fi
		inner join app.FoodProducts as fp
			on fp.FoodProductId = fi.FoodProductId
		inner join app.Categories as c
			on c.CategoryId = fp.CategoryId
	WHERE
		fi.IsConsumed = 1
GO
