USE [dariusz151_smartfridge]
GO
SET IDENTITY_INSERT [app].[Categories] ON 
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (1, N'Warzywa')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (2, N'Owoce')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (3, N'Nabiał i jaja')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (4, N'Słodycze')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (5, N'Mięso')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (6, N'Napoje')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (7, N'Pieczywo')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (8, N'Makarony, kasza i ryże')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (9, N'Ryby i owoce morza')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (10, N'Orzechy i nasiona')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (11, N'Tłuszcze')
GO
INSERT [app].[Categories] ([CategoryId], [Name]) VALUES (12, N'Inne')
GO
SET IDENTITY_INSERT [app].[Categories] OFF
GO
SET IDENTITY_INSERT [app].[FoodProducts] ON 
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (1, N'Stek wołowy', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (2, N'Wędlina', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (3, N'Parówki', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (4, N'Tatar z wołowiny', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (5, N'Mięso wołowe', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (6, N'Żeberka', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (7, N'Karkówka', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (8, N'Mięso mielone wieprzowe', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (9, N'Mięso mielone wołowe', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (10, N'Schab', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (11, N'Nogi z kurczaka', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (12, N'Skrzydełka z kurczaka', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (13, N'Udka z kurczaka', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (14, N'Kiełbasa', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (15, N'Pierś z kurczaka', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (16, N'Indyk', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (17, N'Cielęcina', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (18, N'Boczek', 5)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (19, N'Jaja', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (20, N'Mozzarella', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (21, N'Cheddar', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (22, N'Mleko', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (23, N'Ser pleśniowy', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (24, N'Ser kozi', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (25, N'Camembert', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (26, N'Parmezan', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (27, N'Ser żółty', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (28, N'Twaróg', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (29, N'Śmietana 30', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (30, N'Śmietana 18', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (31, N'Jogurt naturalny', 3)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (32, N'Bazylia', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (33, N'Natka pietruszki', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (34, N'Koperek', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (35, N'Rukola', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (36, N'Roszponka', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (37, N'Pietruszka', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (38, N'Por', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (39, N'Ziemniaki', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (40, N'Szparagi', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (41, N'Szpinak', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (42, N'Sałata', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (43, N'Rzodkiewka', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (44, N'Pomidor', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (45, N'Marchew', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (46, N'Papryka', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (47, N'Oliwki', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (48, N'Ogórek kiszony', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (49, N'Ogórek', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (50, N'Marchewka', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (51, N'Kukurydza', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (52, N'Kapusta', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (53, N'Kalafior', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (54, N'Grzyby', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (55, N'Jarmuż', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (56, N'Groszek', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (57, N'Dynia', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (58, N'Czosnek', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (59, N'Cukinia', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (60, N'Cebula', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (61, N'Burak', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (62, N'Brokuł', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (63, N'Bakłażan', 1)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (64, N'Mango', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (65, N'Morele', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (66, N'Daktyle', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (67, N'Żurawina', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (68, N'Wiśnie', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (69, N'Śliwki', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (70, N'Mandarynki', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (71, N'Pomarańcze', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (72, N'Rodzynki', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (73, N'Truskawki', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (74, N'Cytryna', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (75, N'Maliny', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (76, N'Kiwi', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (77, N'Gruszka', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (78, N'Jabłko', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (79, N'Banan', 2)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (80, N'Cukier puder', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (81, N'Cukier', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (82, N'Miód', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (83, N'Masło orzechowe', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (84, N'Chrupki', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (85, N'Chipsy', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (86, N'Czekolada', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (87, N'Lody', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (88, N'Baton', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (89, N'Czekolada', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (90, N'Kawa', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (91, N'Herbata', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (92, N'Napoje gazowane', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (93, N'Wino', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (94, N'Syrop', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (95, N'Sok', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (96, N'Wódka', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (97, N'Whisky', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (98, N'Piwo', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (99, N'Woda', 6)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (100, N'Wafle', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (101, N'Wafle kukurydziane', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (102, N'Chleb tostowy', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (103, N'Bagietka czosnkowa', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (104, N'Bagietka', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (105, N'Ciabatta', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (106, N'Chleb żytni', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (107, N'Bułki', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (108, N'Chleb razowy', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (109, N'Biały chleb', 7)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (110, N'Kasza jaglana', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (111, N'Kasza pęczak', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (112, N'Kasza gryczana', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (113, N'Kasza kuskus', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (114, N'Makaron świderki', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (115, N'Makaron do zup', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (116, N'Ryż brązowy', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (117, N'Ryż do sushi', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (118, N'Ryż jaśminowy', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (119, N'Ryż basmati', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (120, N'Ryż biały', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (121, N'Makaron spaghetti', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (122, N'Makaron rurki (penne)', 8)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (123, N'Paluszki krabowe', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (124, N'Krewetki', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (125, N'Tuńczyk', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (126, N'Pstrąg', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (127, N'Halibut', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (128, N'Morszczuk', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (129, N'Miruna', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (130, N'Paluszki rybne', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (131, N'Śledź', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (132, N'Dorsz', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (133, N'Makrela', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (134, N'Łosoś', 9)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (135, N'Pestki dynii', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (136, N'Sezam', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (137, N'Ziarna słonecznika', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (138, N'Orzechy pistacje', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (139, N'Orzechy pekan', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (140, N'Orzechy nerkowca', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (141, N'Orzechy ziemne', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (142, N'Migdały', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (143, N'Orzechy włoskie', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (144, N'Orzechy laskowe', 10)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (145, N'Smalec', 11)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (146, N'Masło klarowane', 11)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (147, N'Olej kokosowy', 11)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (148, N'Margaryna', 11)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (149, N'Masło', 11)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (150, N'Olej', 11)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (151, N'Oliwa z oliwek', 11)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (152, N'Tofu', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (153, N'Mąka', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (154, N'Bułka tarta', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (155, N'Sos słodko-kwaśny', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (156, N'Zioła prowansalskie', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (157, N'Ziele angielskie', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (158, N'Imbir', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (159, N'Majeranek', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (160, N'Oregano', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (161, N'Papryka ostra', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (162, N'Papryka słodk', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (163, N'Syrop klonowy', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (164, N'Sól', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (165, N'Pieprz', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (166, N'Ocet', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (167, N'Musztarda', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (168, N'Keczup', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (169, N'Owsianka', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (170, N'Musli', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (171, N'Soczewica', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (172, N'Ciecierzyca', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (173, N'Czekolada gorzka', 4)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (174, N'Kakao', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (175, N'Galaretka cytrynowa', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (176, N'Proszek do pieczenia', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (177, N'Dżem różany', 12)
GO
INSERT [app].[FoodProducts] ([FoodProductId], [Name], [CategoryId]) VALUES (178, N'Biszkopty', 12)
GO
SET IDENTITY_INSERT [app].[FoodProducts] OFF
GO
INSERT [app].[Fridges] ([Id], [Name], [Address], [Desc]) VALUES (N'abfcb234-1d83-4375-a096-04d46aaf1bee', N'Dragana', N'Dragana', N'Dragana')
GO
INSERT [app].[Fridges] ([Id], [Name], [Address], [Desc]) VALUES (N'3a3c26b7-22bb-447d-b62e-43b0dcb3b827', N'Ełk', N'Ełk', N'Ełk')
GO
INSERT [app].[Fridges] ([Id], [Name], [Address], [Desc]) VALUES (N'08e8ee56-23da-4508-a014-a35f81b05495', N'Solikowskiego', N'Solik', N'Solik')
GO
INSERT [app].[Users] ([Id], [Name], [Email], [FridgeId], [CreatedAt]) VALUES (N'65521999-83e8-4843-8111-063fed963848', N'Andrzej', N'andrzej@andrzej.pl', N'3a3c26b7-22bb-447d-b62e-43b0dcb3b827', CAST(N'2021-01-27T20:00:00.0000000' AS DateTime2))
GO
INSERT [app].[Users] ([Id], [Name], [Email], [FridgeId], [CreatedAt]) VALUES (N'3ffcfa2d-cf25-4de0-8cc8-27ada6f1db1a', N'Olga', N'olga@olga.pl', N'abfcb234-1d83-4375-a096-04d46aaf1bee', CAST(N'2021-01-28T23:30:00.0000000' AS DateTime2))
GO
INSERT [app].[Users] ([Id], [Name], [Email], [FridgeId], [CreatedAt]) VALUES (N'd3f73dac-5f87-43f0-bd26-558f55e5f6fd', N'Stefan', N'stefan@stefi.pl', N'08e8ee56-23da-4508-a014-a35f81b05495', CAST(N'2021-01-28T23:50:12.9995185' AS DateTime2))
GO
INSERT [app].[Users] ([Id], [Name], [Email], [FridgeId], [CreatedAt]) VALUES (N'9b8bf183-5747-4109-aba4-fde821a0ba94', N'Dariusz', N'dariusz@dariusz.pl', N'abfcb234-1d83-4375-a096-04d46aaf1bee', CAST(N'2021-01-28T23:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [app].[RecipeCategories] ON 
GO
INSERT [app].[RecipeCategories] ([RecipeCategoryId], [Name]) VALUES (1, N'Śniadanie')
GO
INSERT [app].[RecipeCategories] ([RecipeCategoryId], [Name]) VALUES (2, N'Obiad')
GO
INSERT [app].[RecipeCategories] ([RecipeCategoryId], [Name]) VALUES (3, N'Kolacja')
GO
INSERT [app].[RecipeCategories] ([RecipeCategoryId], [Name]) VALUES (4, N'Drink')
GO
INSERT [app].[RecipeCategories] ([RecipeCategoryId], [Name]) VALUES (5, N'Przekąska')
GO
INSERT [app].[RecipeCategories] ([RecipeCategoryId], [Name]) VALUES (6, N'Deser')
GO
INSERT [app].[RecipeCategories] ([RecipeCategoryId], [Name]) VALUES (7, N'Zupa')
GO
SET IDENTITY_INSERT [app].[RecipeCategories] OFF
GO
INSERT [app].[Recipes] ([RecipeId], [Name], [Description], [RequiredTime], [LevelOfDifficulty], [RecipeCategoryId], [FoodProducts]) VALUES (N'8112af41-9dc6-4819-87e2-19ec2c2c7566', N'kurczak z ryzem', N'kiurczak z ryzem', 35, 2, 2, N'<?xml version="1.0" encoding="utf-16"?>
<ArrayOfFoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>120</FoodProductId>
    <FoodProductName>Ryż biały</FoodProductName>
    <AmountValue>
      <Value>100</Value>
      <Unit>Grams</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>15</FoodProductId>
    <FoodProductName>Pierś z kurczaka</FoodProductName>
    <AmountValue>
      <Value>200</Value>
      <Unit>Grams</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
</ArrayOfFoodProductDetails>')
GO
INSERT [app].[Recipes] ([RecipeId], [Name], [Description], [RequiredTime], [LevelOfDifficulty], [RecipeCategoryId], [FoodProducts]) VALUES (N'9d66cb45-4170-474a-9ec3-2b89499e0970', N'ziemniaki i schab', N'Schab i ziemniaki', 36, 2, 2, N'<?xml version="1.0" encoding="utf-16"?>
<ArrayOfFoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>39</FoodProductId>
    <FoodProductName>Ziemniaki</FoodProductName>
    <AmountValue>
      <Value>400</Value>
      <Unit>Grams</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>10</FoodProductId>
    <FoodProductName>Schab</FoodProductName>
    <AmountValue>
      <Value>200</Value>
      <Unit>Grams</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
</ArrayOfFoodProductDetails>')
GO
INSERT [app].[Recipes] ([RecipeId], [Name], [Description], [RequiredTime], [LevelOfDifficulty], [RecipeCategoryId], [FoodProducts]) VALUES (N'd9c2028e-8441-4995-a171-9395b9b3a2c2', N'Kurczak i cebula', N'kurczak i cebula', 15, 1, 1, N'<?xml version="1.0" encoding="utf-16"?>
<ArrayOfFoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>60</FoodProductId>
    <FoodProductName>Cebula</FoodProductName>
    <AmountValue>
      <Value>2</Value>
      <Unit>Pieces</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>15</FoodProductId>
    <FoodProductName>Pierś z kurczaka</FoodProductName>
    <AmountValue>
      <Value>200</Value>
      <Unit>Grams</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
</ArrayOfFoodProductDetails>')
GO
INSERT [app].[Recipes] ([RecipeId], [Name], [Description], [RequiredTime], [LevelOfDifficulty], [RecipeCategoryId], [FoodProducts]) VALUES (N'0bd9c1a8-8ae2-45c3-ae24-abb53efcdea2', N'jajecznica z boczkiem', N'', 2, 1, 1, N'<?xml version="1.0" encoding="utf-16"?>
<ArrayOfFoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>19</FoodProductId>
    <FoodProductName>Jaja</FoodProductName>
    <AmountValue>
      <Value>3</Value>
      <Unit>Pieces</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
  <FoodProductDetails>
    <FoodProductId>18</FoodProductId>
    <FoodProductName>Boczek</FoodProductName>
    <AmountValue>
      <Value>70</Value>
      <Unit>Grams</Unit>
    </AmountValue>
    <IsOptional>false</IsOptional>
  </FoodProductDetails>
</ArrayOfFoodProductDetails>')
GO
SET IDENTITY_INSERT [app].[FridgeItems] ON 
GO
INSERT [app].[FridgeItems] ([Id], [FoodProductId], [Note], [Value], [Unit], [ExpirationDate], [EnteredAt], [IsConsumed], [UserId]) VALUES (1, 60, N'', 22, N'Pieces', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2021-01-28T23:50:40.8603569' AS DateTime2), 0, N'9b8bf183-5747-4109-aba4-fde821a0ba94')
GO
SET IDENTITY_INSERT [app].[FridgeItems] OFF
GO
