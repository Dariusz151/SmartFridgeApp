USE [dariusz151_smartfridgeapp]
GO

INSERT INTO [app].[Fridges]
           ([Name]
           ,[Address]
           ,[Desc])
     VALUES
           ('Dariusz',
           'Solikowskiego',
           'Lod�wka Solikowskiego')
GO


INSERT INTO [app].[Users]
           ([Name]
           ,[Email]
           ,[FridgeId]
           ,[CreatedAt])
     VALUES
           ('Dariusz',
           'dariusz@gmail.com',
           1,
           '2020-05-05 20:00:00')
GO



INSERT INTO [app].[Categories]
           ([Name])
     VALUES
           ('Mi�so'), 
		   ('Warzywa'), 
		   ('Owoce'), 
		   ('S�odycze'),
		   ('Nabia�'),
		   ('Makarony'),
		   ('Ry�e'),
		   ('Inne')

GO

INSERT INTO [app].[RecipeCategories]
           ([Name])
     VALUES
           ('Obiad'), 
		   ('�niadanie'), 
		   ('Kolacja'), 
		   ('Przystawka'),
		   ('Inne'),
		   ('Deser')

GO

INSERT INTO [app].[FoodProducts]
           ([Name]
           ,[CategoryId])
     VALUES
			('Kurczak',1),
			('Mi�so mielone',1),
			('Mi�so wo�owe',1),
			('Cebula',2),
			('Ziemniaki',2),
			('Marchew',2),
			('Og�rek',2),
			('Pomidor',2),
			('Sa�ata',2),
			('Kapusta',2),
			('Banan',3),
			('Jab�ko',3),
			('Cytryna',3),
			('Gruszka',3),
			('Mleko',5),
			('Ser',5),
			('Twarog',5),
			('Makaron spaghetti',6),
			('Ry� basmati',7),
			('Ry� bia�y',7),
			('Piwo',8),
			('W�dka',8)
GO


INSERT INTO [app].[FridgeItems]
           ([FoodProductId]
           ,[Note]
           ,[Value]
           ,[Unit]
           ,[ExpirationDate]
           ,[EnteredAt]
           ,[IsConsumed]
           ,[UserId])
     VALUES
           (1,NULL,1 ,'kg' ,'2020-05-05 20:00:00','2020-05-05 20:00:00',0,1),
		   (2,NULL,1 ,'kg' ,'2020-05-05 20:00:00','2020-05-05 20:00:00',0,1),
		   (3,NULL,1 ,'kg' ,'2020-05-05 20:00:00','2020-05-05 20:00:00',0,1),
		   (4,NULL,1 ,'kg' ,'2020-05-05 20:00:00','2020-05-05 20:00:00',0,1),
		   (5,NULL,1 ,'kg' ,'2020-05-05 20:00:00','2020-05-05 20:00:00',0,1)

GO




