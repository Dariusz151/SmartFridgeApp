INSERT INTO app."Categories" ("CategoryId", "Name") VALUES 
(1, 'Warzywa'),
(2, 'Owoce'),
(3, 'Nabiał i jaja'), 
(4, 'Słodycze'),
(5, 'Mięso'),
(6, 'Napoje'),
(7, 'Pieczywo'),
(8, 'Makarony, kasza i ryże'),
(9, 'Ryby i owoce morza'),
(10, 'Orzechy i nasiona'),
(11, 'Tłuszcze'),
(12, 'Inne')
ON CONFLICT ("CategoryId") DO NOTHING;

-- Update sequence for Categories
SELECT setval('app."Categories_CategoryId_seq"', (SELECT MAX("CategoryId") FROM app."Categories"));

-- Seed Food Products
INSERT INTO app."FoodProducts" ("FoodProductId", "Name", "CategoryId") VALUES
-- Meat (5)
(1, 'Stek wołowy', 5),
(2, 'Wędlina', 5),
(3, 'Parówki', 5),
(4, 'Tatar z wołowiny', 5),
(5, 'Mięso wołowe', 5),
(6, 'Żeberka', 5),
(7, 'Karkówka', 5),
(8, 'Mięso mielone wieprzowe', 5),
(9, 'Mięso mielone wołowe', 5),
(10, 'Schab', 5),
(11, 'Nogi z kurczaka', 5),
(12, 'Skrzydełka z kurczaka', 5),
(13, 'Udka z kurczaka', 5),
(14, 'Kiełbasa', 5),
(15, 'Pierś z kurczaka', 5),
(16, 'Indyk', 5),
(17, 'Cielęcina', 5),
(18, 'Boczek', 5),
-- Dairy and eggs (3)
(19, 'Jaja', 3),
(20, 'Mozzarella', 3),
(21, 'Cheddar', 3),
(22, 'Mleko', 3),
(23, 'Ser pleśniowy', 3),
(24, 'Ser kozi', 3),
(25, 'Camembert', 3),
(26, 'Parmezan', 3),
(27, 'Ser żółty', 3),
(28, 'Twaróg', 3),
(29, 'Śmietana 30', 3),
(30, 'Śmietana 18', 3),
(31, 'Jogurt naturalny', 3),
-- Vegetables (1)
(32, 'Bazylia', 1),
(33, 'Natka pietruszki', 1),
(34, 'Koperek', 1),
(35, 'Rukola', 1),
(36, 'Roszponka', 1),
(37, 'Pietruszka', 1),
(38, 'Por', 1),
(39, 'Ziemniaki', 1),
(40, 'Szparagi', 1),
(41, 'Szpinak', 1),
(42, 'Sałata', 1),
(43, 'Rzodkiewka', 1),
(44, 'Pomidor', 1),
(45, 'Marchew', 1),
(46, 'Papryka', 1),
(47, 'Oliwki', 1),
(48, 'Ogórek kiszony', 1),
(49, 'Ogórek', 1),
(50, 'Marchewka', 1),
(51, 'Kukurydza', 1),
(52, 'Kapusta', 1),
(53, 'Kalafior', 1),
(54, 'Grzyby', 1),
(55, 'Jarmuż', 1),
(56, 'Groszek', 1),
(57, 'Dynia', 1),
(58, 'Czosnek', 1),
(59, 'Cukinia', 1),
(60, 'Cebula', 1),
(61, 'Burak', 1),
(62, 'Brokuł', 1),
(63, 'Bakłażan', 1),
-- Fruits (2)
(64, 'Mango', 2),
(65, 'Morele', 2),
(66, 'Daktyle', 2),
(67, 'Żurawina', 2),
(68, 'Wiśnie', 2),
(69, 'Śliwki', 2),
(70, 'Mandarynki', 2),
(71, 'Pomarańcze', 2),
(72, 'Rodzynki', 2),
(73, 'Truskawki', 2),
(74, 'Cytryna', 2),
(75, 'Maliny', 2),
(76, 'Kiwi', 2),
(77, 'Gruszka', 2),
(78, 'Jabłko', 2),
(79, 'Banan', 2),
-- Sweets (4)
(80, 'Cukier puder', 4),
(81, 'Cukier', 4),
(82, 'Miód', 4),
(83, 'Masło orzechowe', 4),
(84, 'Chrupki', 4),
(85, 'Chipsy', 4),
(86, 'Czekolada', 4),
(87, 'Lody', 4),
(88, 'Baton', 4),
(89, 'Czekolada', 4),
-- Beverages (6)
(90, 'Kawa', 6),
(91, 'Herbata', 6),
(92, 'Napoje gazowane', 6),
(93, 'Wino', 6),
(94, 'Syrop', 6),
(95, 'Sok', 6),
(96, 'Wódka', 6),
(97, 'Whisky', 6),
(98, 'Piwo', 6),
(99, 'Woda', 6),
-- Bread (7)
(100, 'Wafle', 7),
(101, 'Wafle kukurydziane', 7),
(102, 'Chleb tostowy', 7),
(103, 'Bagietka czosnkowa', 7),
(104, 'Bagietka', 7),
(105, 'Ciabatta', 7),
(106, 'Chleb żytni', 7),
(107, 'Bułki', 7),
(108, 'Chleb razowy', 7),
(109, 'Biały chleb', 7),
-- Pasta and grains (8)
(110, 'Kasza jaglana', 8),
(111, 'Kasza pęczak', 8),
(112, 'Kasza gryczana', 8),
(113, 'Kasza kuskus', 8),
(114, 'Makaron świderki', 8),
(115, 'Makaron do zup', 8),
(116, 'Ryż brązowy', 8),
(117, 'Ryż do sushi', 8),
(118, 'Ryż jaśminowy', 8),
(119, 'Ryż basmati', 8),
(120, 'Ryż biały', 8),
(121, 'Makaron spaghetti', 8),
(122, 'Makaron rurki (penne)', 8),
-- Fish and seafood (9)
(123, 'Paluszki krabowe', 9),
(124, 'Krewetki', 9),
(125, 'Tuńczyk', 9),
(126, 'Pstrąg', 9),
(127, 'Halibut', 9),
(128, 'Morszczuk', 9),
(129, 'Miruna', 9),
(130, 'Paluszki rybne', 9),
(131, 'Śledź', 9),
(132, 'Dorsz', 9),
(133, 'Makrela', 9),
(134, 'Łosoś', 9),
-- Nuts and seeds (10)
(135, 'Pestki dynii', 10),
(136, 'Sezam', 10),
(137, 'Ziarna słonecznika', 10),
(138, 'Orzechy pistacje', 10),
(139, 'Orzechy pekan', 10),
(140, 'Orzechy nerkowca', 10),
(141, 'Orzechy ziemne', 10),
(142, 'Migdały', 10),
(143, 'Orzechy włoskie', 10),
(144, 'Orzechy laskowe', 10),
-- Fats (11)
(145, 'Smalec', 11),
(146, 'Masło klarowane', 11),
(147, 'Olej kokosowy', 11),
(148, 'Margaryna', 11),
(149, 'Masło', 11),
(150, 'Olej', 11),
(151, 'Oliwa z oliwek', 11),
-- Other (12)
(152, 'Tofu', 12),
(153, 'Mąka', 12),
(154, 'Bułka tarta', 12),
(155, 'Sos słodko-kwaśny', 12),
(156, 'Zioła prowansalskie', 12),
(157, 'Ziele angielskie', 12),
(158, 'Imbir', 12),
(159, 'Majeranek', 12),
(160, 'Oregano', 12),
(161, 'Papryka ostra', 12),
(162, 'Papryka słodk', 12),
(163, 'Syrop klonowy', 12),
(164, 'Sól', 12),
(165, 'Pieprz', 12),
(166, 'Ocet', 12),
(167, 'Musztarda', 12),
(168, 'Keczup', 12),
(169, 'Owsianka', 12),
(170, 'Musli', 12),
(171, 'Soczewica', 12),
(172, 'Ciecierzyca', 12),
(173, 'Czekolada gorzka', 4),
(174, 'Kakao', 12),
(175, 'Galaretka cytrynowa', 12),
(176, 'Proszek do pieczenia', 12),
(177, 'Dżem różany', 12),
(178, 'Biszkopty', 12)
ON CONFLICT ("FoodProductId") DO NOTHING;

-- Update sequence for FoodProducts
SELECT setval('app."FoodProducts_FoodProductId_seq"', (SELECT MAX("FoodProductId") FROM app."FoodProducts"));

-- Seed Recipe Categories
INSERT INTO app."RecipeCategories" ("RecipeCategoryId", "Name") VALUES
(1, 'Śniadanie'),
(2, 'Obiad'),
(3, 'Kolacja'),
(4, 'Drink'),
(5, 'Przekąska'),
(6, 'Deser'),
(7, 'Zupa')
ON CONFLICT ("RecipeCategoryId") DO NOTHING;

-- Update sequence for RecipeCategories
SELECT setval('app."RecipeCategories_RecipeCategoryId_seq"', (SELECT MAX("RecipeCategoryId") FROM app."RecipeCategories"));

-- Seed Fridges
INSERT INTO app."Fridges" ("Id", "Name", "Address", "Desc") VALUES
('abfcb234-1d83-4375-a096-04d46aaf1bee', 'Dragana', 'Dragana', 'Dragana'),
('3a3c26b7-22bb-447d-b62e-43b0dcb3b827', 'Ełk', 'Ełk', 'Ełk'),
('08e8ee56-23da-4508-a014-a35f81b05495', 'Solikowskiego', 'Solik', 'Solik')
ON CONFLICT ("Id") DO NOTHING;

-- Seed Users
INSERT INTO app."Users" ("Id", "Name", "Email", "FridgeId", "CreatedAt") VALUES
('65521999-83e8-4843-8111-063fed963848', 'Andrzej', 'andrzej@andrzej.pl', '3a3c26b7-22bb-447d-b62e-43b0dcb3b827', '2021-01-27T20:00:00'::timestamp),
('3ffcfa2d-cf25-4de0-8cc8-27ada6f1db1a', 'Olga', 'olga@olga.pl', 'abfcb234-1d83-4375-a096-04d46aaf1bee', '2021-01-28T23:30:00'::timestamp),
('d3f73dac-5f87-43f0-bd26-558f55e5f6fd', 'Stefan', 'stefan@stefi.pl', '08e8ee56-23da-4508-a014-a35f81b05495', '2021-01-28T23:50:12.9995185'::timestamp),
('9b8bf183-5747-4109-aba4-fde821a0ba94', 'Dariusz', 'dariusz@dariusz.pl', 'abfcb234-1d83-4375-a096-04d46aaf1bee', '2021-01-28T23:00:00'::timestamp)
ON CONFLICT ("Id") DO NOTHING;

-- Seed Recipes
INSERT INTO app."Recipes" ("RecipeId", "Name", "Description", "RequiredTime", "LevelOfDifficulty", "RecipeCategoryId", "FoodProducts") VALUES
('8112af41-9dc6-4819-87e2-19ec2c2c7566', 'kurczak z ryzem', 'kiurczak z ryzem', 35, 2, 2, '<?xml version="1.0" encoding="utf-16"?>
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
</ArrayOfFoodProductDetails>'),
('9d66cb45-4170-474a-9ec3-2b89499e0970', 'ziemniaki i schab', 'Schab i ziemniaki', 36, 2, 2, '<?xml version="1.0" encoding="utf-16"?>
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
</ArrayOfFoodProductDetails>'),
('d9c2028e-8441-4995-a171-9395b9b3a2c2', 'Kurczak i cebula', 'kurczak i cebula', 15, 1, 1, '<?xml version="1.0" encoding="utf-16"?>
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
</ArrayOfFoodProductDetails>'),
('0bd9c1a8-8ae2-45c3-ae24-abb53efcdea2', 'jajecznica z boczkiem', '', 2, 1, 1, '<?xml version="1.0" encoding="utf-16"?>
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
ON CONFLICT ("RecipeId") DO NOTHING;

-- Seed Sample Fridge Item
INSERT INTO app."FridgeItems" ("Id", "FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "UserId") VALUES
(1, 60, '', 22, 'Pieces', '0001-01-01T00:00:00'::timestamp, '2021-01-28T23:50:40.8603569'::timestamp, false, '9b8bf183-5747-4109-aba4-fde821a0ba94')
ON CONFLICT ("Id") DO NOTHING;

-- Update sequence for FridgeItems
SELECT setval('app."FridgeItems_Id_seq"', (SELECT MAX("Id") FROM app."FridgeItems"));
