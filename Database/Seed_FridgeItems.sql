

INSERT [app].[Fridges] ([Id], [Name], [Address], [Desc], [IsWelcomed]) VALUES (N'a3be7ef8-9a86-4705-b87d-a16990dda55f', N'Dragana', N'Dragana', N'Dragana', 0)
GO
INSERT [app].[Users] ([Id], [Name], [Email], [FridgeId], [CreatedAt]) VALUES (N'3a7a9300-638a-45a6-8f7a-21ef0c5156ea', N'User_example2', N'user2@email.com', N'a3be7ef8-9a86-4705-b87d-a16990dda55f', CAST(N'2020-12-03T20:34:15.2321908' AS DateTime2))
GO
INSERT [app].[Users] ([Id], [Name], [Email], [FridgeId], [CreatedAt]) VALUES (N'bb55b8af-0ff1-414d-9a6a-af1a6cd115d0', N'User_example1', N'user1@email.com', N'a3be7ef8-9a86-4705-b87d-a16990dda55f', CAST(N'2020-12-03T20:32:08.6222776' AS DateTime2))
GO
INSERT [app].[Users] ([Id], [Name], [Email], [FridgeId], [CreatedAt]) VALUES (N'35ef6934-8301-4589-9e3f-e6c438261b8e', N'User_example3', N'user3@email.com', N'a3be7ef8-9a86-4705-b87d-a16990dda55f', CAST(N'2020-12-03T20:35:08.9447953' AS DateTime2))
GO
SET IDENTITY_INSERT [app].[FridgeItems] ON 
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50010, 1, N'ExampleFridgeItem1', 10, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:33:28.8586305' AS DateTime2), 0, N'bb55b8af-0ff1-414d-9a6a-af1a6cd115d0')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50011, 3, N'ExampleFridgeItem2', 20, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:33:34.7079891' AS DateTime2), 0, N'bb55b8af-0ff1-414d-9a6a-af1a6cd115d0')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50012, 6, N'ExampleFridgeItem3', 30, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:33:42.5174029' AS DateTime2), 0, N'bb55b8af-0ff1-414d-9a6a-af1a6cd115d0')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50013, 9, N'ExampleFridgeItem4', 3, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:33:49.8951251' AS DateTime2), 0, N'bb55b8af-0ff1-414d-9a6a-af1a6cd115d0')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50014, 11, N'ExampleFridgeItem5', 11, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:33:56.0398824' AS DateTime2), 0, N'bb55b8af-0ff1-414d-9a6a-af1a6cd115d0')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50015, 13, N'ExampleFridgeItem1', 111, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:34:41.8265016' AS DateTime2), 0, N'3a7a9300-638a-45a6-8f7a-21ef0c5156ea')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50016, 14, N'ExampleFridgeItem2', 34, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:34:50.1043882' AS DateTime2), 0, N'3a7a9300-638a-45a6-8f7a-21ef0c5156ea')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50017, 16, N'ExampleFridgeItem3', 7, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:34:56.8033372' AS DateTime2), 0, N'3a7a9300-638a-45a6-8f7a-21ef0c5156ea')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50018, 18, N'ExampleFridgeItem1', 1, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:35:28.9002067' AS DateTime2), 0, N'35ef6934-8301-4589-9e3f-e6c438261b8e')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50019, 2, N'ExampleFridgeItem2', 13, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:35:35.5195749' AS DateTime2), 0, N'35ef6934-8301-4589-9e3f-e6c438261b8e')
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (50020, 8, N'ExampleFridgeItem3', 133, N'NotAssigned', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-12-03T20:35:43.6962783' AS DateTime2), 0, N'35ef6934-8301-4589-9e3f-e6c438261b8e')
GO
SET IDENTITY_INSERT [app].[FridgeItems] OFF
GO
