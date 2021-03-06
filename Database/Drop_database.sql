USE [dariusz151_smartfridge]
GO
ALTER TABLE [app].[Users] DROP CONSTRAINT [FK_Users_Fridges_FridgeId]
GO
ALTER TABLE [app].[FridgeItems] DROP CONSTRAINT [FK_FridgeItems_Users_UserId]
GO
ALTER TABLE [app].[FridgeItems] DROP CONSTRAINT [FK_FridgeItems_FoodProducts_FoodProductId]
GO
ALTER TABLE [app].[FoodProducts] DROP CONSTRAINT [FK_FoodProducts_Categories_CategoryId]

GO
/****** Object:  Table [app].[Users]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE [app].[Users]
GO
/****** Object:  Table [app].[Recipes]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE [app].[Recipes]
GO
/****** Object:  Table [app].[OutboxMessages]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE internal.[OutboxMessages]

GO
/****** Object:  Table [app].[Fridges]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE [app].[Fridges]
GO
/****** Object:  Table [app].[FridgeItems]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE [app].[FridgeItems]
GO
/****** Object:  Table [app].[FoodProducts]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE [app].[FoodProducts]
GO
/****** Object:  Table [app].[Categories]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE [app].[Categories]
GO
/****** Object:  Table [app].[Categories]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE [app].[RecipeCategories]
GO
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 24.08.2020 20:43:37 ******/
DROP TABLE dariusz151_Administrator.[__EFMigrationsHistory]
