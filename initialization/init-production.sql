-- ============================================================
-- SmartFridgeApp — Production Database Initialization Script
-- PostgreSQL
--
-- Run this against a fresh PostgreSQL instance before starting
-- the application for the first time.
--
-- What this script sets up:
--   1. Schemas (app, internal)
--   2. All application tables
--   3. Indexes
--   4. Database views (for debugging / reporting)
--   5. Reference / seed data (categories, food products,
--      product variants, recipe categories)
--   6. Admin user account
--
-- NOTE: Marten event store tables (used for inventory tracking)
--       are created automatically by the application at startup.
--       No manual steps required for those tables.
-- ============================================================


-- ============================================================
-- 1. Schemas
-- ============================================================

CREATE SCHEMA IF NOT EXISTS app;
CREATE SCHEMA IF NOT EXISTS internal;


-- ============================================================
-- 2. Tables
-- ============================================================

CREATE TABLE IF NOT EXISTS app."Categories" (
    "CategoryId" SMALLSERIAL PRIMARY KEY,
    "Name" VARCHAR(25) NOT NULL
);

CREATE TABLE IF NOT EXISTS app."FoodProducts" (
    "FoodProductId" SMALLSERIAL PRIMARY KEY,
    "Name" VARCHAR(40) NOT NULL,
    "CategoryId" SMALLINT NOT NULL,
    "InsertedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt"  TIMESTAMP,
    CONSTRAINT "FK_FoodProducts_Categories" FOREIGN KEY ("CategoryId")
        REFERENCES app."Categories"("CategoryId") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS app."RecipeCategories" (
    "RecipeCategoryId" SMALLSERIAL PRIMARY KEY,
    "Name" VARCHAR(25) NOT NULL
);

CREATE TABLE IF NOT EXISTS app."Kitchens" (
    "Id"                   UUID         PRIMARY KEY,
    "Name"                 VARCHAR(50)  NOT NULL,
    "Address"              VARCHAR(100),
    "Desc"                 VARCHAR(250),
    "WasteScore"           INTEGER      NOT NULL DEFAULT 1000,
    "ActiveItemCount"      INTEGER      NOT NULL DEFAULT 0,
    "AverageItemCount"     FLOAT        NOT NULL DEFAULT 0,
    "InventorySampleCount" INTEGER      NOT NULL DEFAULT 0,
    "CreatedAt"            TIMESTAMP    NOT NULL DEFAULT NOW(),
    "UpdatedAt"            TIMESTAMP
);

CREATE TABLE IF NOT EXISTS app."AppUsers" (
    "Email"        VARCHAR(250) PRIMARY KEY,
    "PasswordHash" VARCHAR(500) NULL,
    "Name"         VARCHAR(100) NULL,
    "Role"         VARCHAR(50)  NOT NULL DEFAULT 'User',
    "CreatedAt"    TIMESTAMP    NOT NULL DEFAULT NOW(),
    "UpdatedAt"    TIMESTAMP
);

CREATE TABLE IF NOT EXISTS app."ProductVariants" (
    "VariantId"     SERIAL      PRIMARY KEY,
    "FoodProductId" SMALLINT    NOT NULL,
    "Name"          VARCHAR(80) NOT NULL,
    "Barcode"       VARCHAR(50) NULL,
    CONSTRAINT "FK_ProductVariants_FoodProducts" FOREIGN KEY ("FoodProductId")
        REFERENCES app."FoodProducts"("FoodProductId") ON DELETE CASCADE
);
CREATE UNIQUE INDEX IF NOT EXISTS "UX_ProductVariants_Barcode"
    ON app."ProductVariants"("Barcode") WHERE "Barcode" IS NOT NULL;

-- MemberRole: 'Creator' | 'Member'
-- Status:     'Accepted' | 'Pending'
CREATE TABLE IF NOT EXISTS app."KitchenMembers" (
    "Id"         SERIAL       PRIMARY KEY,
    "KitchenId"  UUID         NOT NULL,
    "Email"      VARCHAR(250) NOT NULL,
    "MemberRole" VARCHAR(50)  NOT NULL DEFAULT 'Member',
    "Status"     VARCHAR(50)  NOT NULL DEFAULT 'Pending',
    "Color"      VARCHAR(7)   NOT NULL DEFAULT '#000000',
    "InvitedAt"  TIMESTAMP    NOT NULL DEFAULT NOW(),
    "UpdatedAt"  TIMESTAMP,
    CONSTRAINT "FK_KitchenMembers_Kitchens"  FOREIGN KEY ("KitchenId") REFERENCES app."Kitchens"("Id")    ON DELETE CASCADE,
    CONSTRAINT "FK_KitchenMembers_AppUsers"  FOREIGN KEY ("Email")     REFERENCES app."AppUsers"("Email") ON DELETE CASCADE,
    CONSTRAINT "UQ_KitchenMembers"           UNIQUE ("KitchenId", "Email")
);

CREATE TABLE IF NOT EXISTS app."Recipes" (
    "RecipeId"          UUID         PRIMARY KEY,
    "Name"              VARCHAR(100) NOT NULL,
    "Description"       VARCHAR(5000),
    "RequiredTime"      INTEGER      NOT NULL DEFAULT -1,
    "LevelOfDifficulty" SMALLINT     NOT NULL DEFAULT 0,
    "RecipeCategoryId"  SMALLINT,
    "FoodProducts"      TEXT         NOT NULL,
    "InsertedAt"        TIMESTAMP    NOT NULL DEFAULT NOW(),
    "UpdatedAt"         TIMESTAMP,
    CONSTRAINT "FK_Recipes_RecipeCategories" FOREIGN KEY ("RecipeCategoryId")
        REFERENCES app."RecipeCategories"("RecipeCategoryId") ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS internal."OutboxMessages" (
    "Id"            UUID         PRIMARY KEY,
    "OccurredOn"    TIMESTAMP    NOT NULL,
    "Type"          VARCHAR(255) NOT NULL,
    "Data"          TEXT         NOT NULL,
    "ProcessedDate" TIMESTAMP
);


-- ============================================================
-- 3. Indexes
-- ============================================================

CREATE INDEX IF NOT EXISTS "IX_KitchenMembers_Email"         ON app."KitchenMembers"("Email");
CREATE INDEX IF NOT EXISTS "IX_KitchenMembers_KitchenId"     ON app."KitchenMembers"("KitchenId");
CREATE INDEX IF NOT EXISTS "IX_KitchenMembers_Status"        ON app."KitchenMembers"("Status");
CREATE INDEX IF NOT EXISTS "IX_FoodProducts_CategoryId"      ON app."FoodProducts"("CategoryId");
CREATE INDEX IF NOT EXISTS "IX_Recipes_RecipeCategoryId"     ON app."Recipes"("RecipeCategoryId");
CREATE INDEX IF NOT EXISTS "IX_OutboxMessages_ProcessedDate" ON internal."OutboxMessages"("ProcessedDate") WHERE "ProcessedDate" IS NULL;


-- ============================================================
-- 4. Views  (for manual debugging / reporting only — not used
--             by application queries)
-- ============================================================

DROP VIEW IF EXISTS app.v_foodproducts;
CREATE VIEW app.v_foodproducts AS
SELECT fp."FoodProductId", fp."Name", c."CategoryId", c."Name" AS "Category",
       fp."InsertedAt", fp."UpdatedAt"
FROM app."FoodProducts" AS fp
INNER JOIN app."Categories" c ON c."CategoryId" = fp."CategoryId";

DROP VIEW IF EXISTS app.v_kitchens;
CREATE VIEW app.v_kitchens AS
SELECT "Id", "Name", "Address", "Desc", "CreatedAt"
FROM app."Kitchens";

DROP VIEW IF EXISTS app.v_member_kitchens;
CREATE VIEW app.v_member_kitchens AS
SELECT f."Id", f."Name", f."Address", f."Desc", f."CreatedAt", f."UpdatedAt",
       fm."Email"
FROM app."Kitchens" f
INNER JOIN app."KitchenMembers" fm ON fm."KitchenId" = f."Id"
WHERE fm."Status" = 'Accepted';

DROP VIEW IF EXISTS app.v_KitchenMembers;
CREATE VIEW app.v_KitchenMembers AS
SELECT fm."Id", fm."KitchenId", fm."Email", au."Name", fm."MemberRole", fm."Color", fm."InvitedAt", fm."UpdatedAt"
FROM app."KitchenMembers" fm
LEFT JOIN app."AppUsers" au ON au."Email" = fm."Email"
WHERE fm."Status" = 'Accepted';

DROP VIEW IF EXISTS app.v_kitchen_members_detail;
CREATE VIEW app.v_kitchen_members_detail AS
SELECT fm."Id", fm."KitchenId", fm."Email",
       COALESCE(au."Name", fm."Email") AS "Name",
       fm."MemberRole", fm."Status", fm."Color"
FROM app."KitchenMembers" fm
LEFT JOIN app."AppUsers" au ON au."Email" = fm."Email";

DROP VIEW IF EXISTS app.v_pending_invites;
CREATE VIEW app.v_pending_invites AS
SELECT fm."Id", fm."KitchenId", fm."Email",
       f."Name" AS "kitchenName",
       creator."Email" AS "InviterEmail",
       COALESCE(au."Name", creator."Email") AS "InviterName",
       fm."InvitedAt"
FROM app."KitchenMembers" fm
INNER JOIN app."Kitchens" f ON f."Id" = fm."KitchenId"
INNER JOIN app."KitchenMembers" creator
    ON creator."KitchenId" = fm."KitchenId" AND creator."MemberRole" = 'Creator'
LEFT JOIN app."AppUsers" au ON au."Email" = creator."Email"
WHERE fm."Status" = 'Pending';

DROP VIEW IF EXISTS app.v_recipes;
CREATE VIEW app.v_recipes AS
SELECT r."RecipeId", r."Name", rc."Name" AS "RecipeCategory",
       r."Description", r."FoodProducts", r."RequiredTime", r."LevelOfDifficulty",
       r."InsertedAt", r."UpdatedAt"
FROM app."Recipes" AS r
LEFT JOIN app."RecipeCategories" AS rc ON rc."RecipeCategoryId" = r."RecipeCategoryId";

DROP VIEW IF EXISTS app.v_recipecategories;
CREATE VIEW app.v_recipecategories AS
SELECT "RecipeCategoryId", "Name"
FROM app."RecipeCategories";


-- ============================================================
-- 5. Reference / seed data
-- ============================================================

-- Categories
INSERT INTO app."Categories" ("CategoryId", "Name") VALUES
(1,  'Warzywa'),
(2,  'Owoce'),
(3,  'Nabiał i jaja'),
(4,  'Słodycze'),
(5,  'Mięso'),
(6,  'Napoje'),
(7,  'Pieczywo'),
(8,  'Makarony, kasza i ryże'),
(9,  'Ryby i owoce morza'),
(10, 'Orzechy i nasiona'),
(11, 'Tłuszcze'),
(12, 'Inne'),
(13, 'Przyprawy'),
(14, 'Sosy i dodatki'),
(15, 'Strączki')
ON CONFLICT ("CategoryId") DO NOTHING;

SELECT setval('app."Categories_CategoryId_seq"', (SELECT MAX("CategoryId") FROM app."Categories"));

-- Food Products
INSERT INTO app."FoodProducts" ("FoodProductId", "Name", "CategoryId") VALUES
-- Meat (5)
(1,  'Stek wołowy',             5),
(2,  'Wędlina',                 5),
(3,  'Parówki',                 5),
(4,  'Tatar z wołowiny',        5),
(5,  'Mięso wołowe',            5),
(6,  'Żeberka',                 5),
(7,  'Karkówka',                5),
(8,  'Mięso mielone wieprzowe', 5),
(9,  'Mięso mielone wołowe',    5),
(10, 'Schab',                   5),
(11, 'Nogi z kurczaka',         5),
(12, 'Skrzydełka z kurczaka',   5),
(13, 'Udka z kurczaka',         5),
(14, 'Kiełbasa',                5),
(15, 'Pierś z kurczaka',        5),
(16, 'Indyk',                   5),
(17, 'Cielęcina',               5),
(18, 'Boczek',                  5),
-- Dairy and eggs (3)
(19, 'Jaja',                    3),
(20, 'Mozzarella',              3),
(21, 'Cheddar',                 3),
(22, 'Mleko',                   3),
(23, 'Ser pleśniowy',           3),
(24, 'Ser kozi',                3),
(25, 'Camembert',               3),
(26, 'Parmezan',                3),
(27, 'Ser żółty',               3),
(28, 'Twaróg',                  3),
(29, 'Śmietana 30',             3),
(30, 'Śmietana 18',             3),
(31, 'Jogurt naturalny',        3),
-- Vegetables (1)
(32, 'Bazylia',                 1),
(33, 'Natka pietruszki',        1),
(34, 'Koperek',                 1),
(35, 'Rukola',                  1),
(36, 'Roszponka',               1),
(37, 'Pietruszka',              1),
(38, 'Por',                     1),
(39, 'Ziemniaki',               1),
(40, 'Szparagi',                1),
(41, 'Szpinak',                 1),
(42, 'Sałata',                  1),
(43, 'Rzodkiewka',              1),
(44, 'Pomidor',                 1),
(45, 'Marchew',                 1),
(46, 'Papryka',                 1),
(47, 'Oliwki',                  1),
(48, 'Ogórek kiszony',          1),
(49, 'Ogórek',                  1),
(50, 'Marchewka',               1),
(51, 'Kukurydza',               1),
(52, 'Kapusta',                 1),
(53, 'Kalafior',                1),
(54, 'Grzyby',                  1),
(55, 'Jarmuż',                  1),
(56, 'Groszek',                 1),
(57, 'Dynia',                   1),
(58, 'Czosnek',                 1),
(59, 'Cukinia',                 1),
(60, 'Cebula',                  1),
(61, 'Burak',                   1),
(62, 'Brokuł',                  1),
(63, 'Bakłażan',                1),
-- Fruits (2)
(64, 'Mango',                   2),
(65, 'Morele',                  2),
(66, 'Daktyle',                 2),
(67, 'Żurawina',                2),
(68, 'Wiśnie',                  2),
(69, 'Śliwki',                  2),
(70, 'Mandarynki',              2),
(71, 'Pomarańcze',              2),
(72, 'Rodzynki',                2),
(73, 'Truskawki',               2),
(74, 'Cytryna',                 2),
(75, 'Maliny',                  2),
(76, 'Kiwi',                    2),
(77, 'Gruszka',                 2),
(78, 'Jabłko',                  2),
(79, 'Banan',                   2),
-- Sweets (4)
(80, 'Cukier puder',            4),
(81, 'Cukier',                  4),
(82, 'Miód',                    4),
(83, 'Masło orzechowe',         4),
(84, 'Chrupki',                 4),
(85, 'Chipsy',                  4),
(86, 'Czekolada',               4),
(87, 'Lody',                    4),
(88, 'Baton',                   4),
(89, 'Ciastka',                 4),
-- Beverages (6)
(90, 'Kawa',                    6),
(91, 'Herbata',                 6),
(92, 'Napoje gazowane',         6),
(93, 'Wino',                    6),
(94, 'Syrop',                   6),
(95, 'Sok',                     6),
(96, 'Wódka',                   6),
(97, 'Whisky',                  6),
(98, 'Piwo',                    6),
(99, 'Woda',                    6),
-- Bread (7)
(100, 'Wafle',                  7),
(101, 'Wafle kukurydziane',     7),
(102, 'Chleb tostowy',          7),
(103, 'Bagietka czosnkowa',     7),
(104, 'Bagietka',               7),
(105, 'Ciabatta',               7),
(106, 'Chleb żytni',            7),
(107, 'Bułki',                  7),
(108, 'Chleb razowy',           7),
(109, 'Biały chleb',            7),
-- Pasta and grains (8)
(110, 'Kasza jaglana',          8),
(111, 'Kasza pęczak',           8),
(112, 'Kasza gryczana',         8),
(113, 'Kasza kuskus',           8),
(114, 'Makaron świderki',       8),
(115, 'Makaron do zup',         8),
(116, 'Ryż brązowy',            8),
(117, 'Ryż do sushi',           8),
(118, 'Ryż jaśminowy',          8),
(119, 'Ryż basmati',            8),
(120, 'Ryż biały',              8),
(121, 'Makaron spaghetti',      8),
(122, 'Makaron rurki (penne)',  8),
-- Fish and seafood (9)
(123, 'Paluszki krabowe',       9),
(124, 'Krewetki',               9),
(125, 'Tuńczyk',                9),
(126, 'Pstrąg',                 9),
(127, 'Halibut',                9),
(128, 'Morszczuk',              9),
(129, 'Miruna',                 9),
(130, 'Paluszki rybne',         9),
(131, 'Śledź',                  9),
(132, 'Dorsz',                  9),
(133, 'Makrela',                9),
(134, 'Łosoś',                  9),
-- Nuts and seeds (10)
(135, 'Pestki dynii',           10),
(136, 'Sezam',                  10),
(137, 'Ziarna słonecznika',     10),
(138, 'Orzechy pistacje',       10),
(139, 'Orzechy pekan',          10),
(140, 'Orzechy nerkowca',       10),
(141, 'Orzechy ziemne',         10),
(142, 'Migdały',                10),
(143, 'Orzechy włoskie',        10),
(144, 'Orzechy laskowe',        10),
-- Fats (11)
(145, 'Smalec',                 11),
(146, 'Masło klarowane',        11),
(147, 'Olej kokosowy',          11),
(148, 'Margaryna',              11),
(149, 'Masło',                  11),
(150, 'Olej',                   11),
(151, 'Oliwa z oliwek',         11),
-- Other (12)
(152, 'Tofu',                   12),
(153, 'Mąka',                   12),
(154, 'Bułka tarta',            12),
(155, 'Sos słodko-kwaśny',      12),
(156, 'Zioła prowansalskie',    12),
(157, 'Ziele angielskie',       12),
(158, 'Imbir',                  12),
(159, 'Majeranek',              12),
(160, 'Oregano',                12),
(161, 'Papryka ostra',          12),
(162, 'Papryka słodk',          12),
(163, 'Syrop klonowy',          12),
(164, 'Sól',                    12),
(165, 'Pieprz',                 12),
(166, 'Ocet',                   12),
(167, 'Musztarda',              12),
(168, 'Keczup',                 12),
(169, 'Owsianka',               12),
(170, 'Musli',                  12),
(171, 'Soczewica',              12),
(172, 'Ciecierzyca',            12),
(173, 'Czekolada gorzka',        4),
(174, 'Kakao',                  12),
(175, 'Galaretka cytrynowa',    12),
(176, 'Proszek do pieczenia',   12),
(177, 'Dżem różany',            12),
(178, 'Biszkopty',              12),
-- Dairy (3) – extra
(179, 'Kefir',                  3),
(180, 'Maślanka',               3),
(181, 'Ricotta',                3),
(182, 'Mascarpone',             3),
(183, 'Skyr',                   3),
(184, 'Śmietanka kremówka',     3),
(185, 'Cream cheese',           3),
-- Fruits (2) – extra
(186, 'Arbuz',                  2),
(187, 'Ananas',                 2),
(188, 'Grejpfrut',              2),
(189, 'Winogrona',              2),
(190, 'Awokado',                2),
(191, 'Figi',                   2),
(192, 'Brzoskwinie',            2),
(193, 'Agrest',                 2),
-- Meat (5) – extra
(194, 'Polędwica wieprzowa',    5),
(195, 'Golonka',                5),
(196, 'Kaczka',                 5),
(197, 'Gęś',                    5),
(198, 'Rostbef',                5),
(199, 'Wątróbka',               5),
(200, 'Szynka',                 5),
-- Beverages (6) – extra
(201, 'Kompot',                 6),
(202, 'Lemoniada',              6),
(203, 'Napoje energetyczne',    6),
(204, 'Rum',                    6),
(205, 'Cola',                   6),
-- Bread (7) – extra
(206, 'Grahamka',               7),
(207, 'Chleb wieloziarnisty',   7),
(208, 'Pumpernikiel',           7),
(209, 'Rogalik',                7),
(210, 'Tortilla',               7),
(211, 'Pita',                   7),
-- Pasta and grains (8) – extra
(212, 'Makaron tagliatelle',    8),
(213, 'Lasagne płaty',          8),
(214, 'Komosa ryżowa',          8),
(215, 'Ryż parboiled',          8),
(216, 'Orkisz',                 8),
(217, 'Makaron ryżowy',         8),
-- Fish and seafood (9) – extra
(218, 'Tilapia',                9),
(219, 'Sardynki',               9),
(220, 'Kałamarnica',            9),
(221, 'Flądra',                 9),
(222, 'Karp',                   9),
-- Nuts and seeds (10) – extra
(223, 'Siemię lniane',         10),
(224, 'Chia',                  10),
(225, 'Orzechy brazylijskie',  10),
(226, 'Mak',                   10),
-- Fats (11) – extra
(227, 'Olej słonecznikowy',    11),
(228, 'Olej rzepakowy',        11),
(229, 'Ghee',                  11),
(230, 'Olej sezamowy',         11),
-- Sweets (4) – extra
(231, 'Nutella',                4),
(232, 'Żelki',                  4),
(233, 'Wafelki czekoladowe',    4),
(234, 'Karmel',                 4),
(235, 'Dżem truskawkowy',       4),
(236, 'Dżem morelowy',          4),
(237, 'Pierniczki',             4),
-- Spices (13)
(238, 'Cynamon',               13),
(239, 'Kurkuma',               13),
(240, 'Kminek',                13),
(241, 'Curry',                 13),
(242, 'Chili mielone',         13),
(243, 'Tymianek',              13),
(244, 'Rozmaryn',              13),
(245, 'Liść laurowy',          13),
(246, 'Kardamon',              13),
(247, 'Gałka muszkatołowa',    13),
(248, 'Kmin rzymski',          13),
(249, 'Kolendra mielona',      13),
(250, 'Wanilia',               13),
(251, 'Szafran',               13),
(252, 'Anyż',                  13),
-- Sauces and condiments (14)
(253, 'Majonez',               14),
(254, 'Sos sojowy',            14),
(255, 'Sos Worcestershire',    14),
(256, 'Tabasco',               14),
(257, 'Sos pesto',             14),
(258, 'Miso',                  14),
(259, 'Ocet balsamiczny',      14),
(260, 'Sos teriyaki',          14),
(261, 'Harissa',               14),
(262, 'Sambal oelek',          14),
(263, 'Aioli',                 14),
(264, 'Tahini',                14),
-- Legumes (15)
(265, 'Fasola biała',          15),
(266, 'Fasola czerwona',       15),
(267, 'Fasola czarna',         15),
(268, 'Bób',                   15),
(269, 'Groch',                 15),
(270, 'Edamame',               15),
(271, 'Soczewica zielona',     15),
(272, 'Soczewica czerwona',    15),
(273, 'Ciecierzyca suszona',   15),
(274, 'Fasola kidney',         15)
ON CONFLICT ("FoodProductId") DO NOTHING;

SELECT setval('app."FoodProducts_FoodProductId_seq"', (SELECT MAX("FoodProductId") FROM app."FoodProducts"));

-- Product Variants
INSERT INTO app."ProductVariants" ("VariantId", "FoodProductId", "Name", "Barcode") VALUES
-- Mleko (22)
(1,  22,  'Łaciate 3,2% 1L',               '5900820000001'),
(2,  22,  'Łaciate 2% 1L',                 '5900820000002'),
(3,  22,  'Łaciate UHT 3,2% 1L',           '5900820000003'),
(4,  22,  'Piątnica Pełne 3,8% 1L',        '5900199000001'),
-- Jogurt naturalny (31)
(5,  31,  'Danone Naturalny 400g',          '3033490004001'),
(6,  31,  'Piątnica Naturalny 400g',        '5900199010001'),
-- Masło (149)
(7,  149, 'Łaciate 200g',                  '5900820010001'),
(8,  149, 'Piątnica 200g',                 '5900199020001'),
(9,  149, 'Kerrygold 200g',                '5099387000001'),
-- Czekolada (86)
(10, 86,  'Milka Mleczna 100g',            '4025700000001'),
(11, 86,  'Wedel Gorzka 100g',             '5900124000001'),
(12, 86,  'Lindt 70% 100g',               '3046920022017'),
-- Ser żółty (27)
(13, 27,  'Gouda Holenderska 200g',        '9000000000001'),
(14, 27,  'Edam 200g',                     '9000000000002'),
-- Wędlina (2)
(15, 2,   'Szynka Konserwowa 200g',        '5900000010001'),
(16, 2,   'Polędwica Sopocka 150g',        '5900000010002'),
-- Kiełbasa (14)
(17, 14,  'Kiełbasa Śląska 1kg',           '5900000020001'),
(18, 14,  'Kabanosy 200g',                 '5900000020002'),
-- Jaja (19)
(19, 19,  'Jaja M 10szt.',                 '5900000030001'),
(20, 19,  'Jaja L 10szt.',                 '5900000030002'),
(21, 19,  'Jaja XL wolny wybieg 6szt.',    '5900000030003'),
-- Pierś z kurczaka (15)
(22, 15,  'Pierś z kurczaka Cedrob 500g',  '5900000040001'),
(23, 15,  'Filet z kurczaka Indykpol 600g','5900000040002'),
-- Chleb razowy (108)
(24, 108, 'Chleb razowy żytni 500g',       '5900000050001'),
(25, 108, 'Chleb razowy orkiszowy 400g',   '5900000050002'),
-- Biały chleb (109)
(26, 109, 'Chleb pszenny krojony 500g',    '5900000050003'),
-- Piwo (98)
(27, 98,  'Żywiec Jasne 500ml',            '5900000060001'),
(28, 98,  'Tyskie Gronie 500ml',           '5900000060002'),
(29, 98,  'Lech Premium 500ml',            '5900000060003'),
(30, 98,  'Książęce Złote 500ml',          '5900000060004'),
-- Kawa (90)
(31, 90,  'Lavazza Qualità Rossa 250g',    '8000070036529'),
(32, 90,  'Jacobs Krönung 500g',           '8711000530092'),
(33, 90,  'Illy Classico 250g',            '8003753900667'),
-- Sok (95)
(34, 95,  'Cappy Pomarańczowy 1L',         '5449000000001'),
(35, 95,  'Tymbark Jabłkowy 1L',           '5900000070001'),
(36, 95,  'Hortex Multiwitamina 1L',       '5900000070002'),
-- Ryż basmati (119)
(37, 119, 'Basmati Britta 1kg',            '5900000080001'),
(38, 119, 'Basmati Tesco 500g',            '5900000080002'),
-- Makaron spaghetti (121)
(39, 121, 'Lubella Spaghetti 500g',        '5900000090001'),
(40, 121, 'Barilla Spaghetti No.5 500g',   '8076800195057'),
-- Łosoś (134)
(41, 134, 'Łosoś atlantycki filet 400g',   '5900000100001'),
(42, 134, 'Łosoś wędzony plastry 200g',    '5900000100002'),
-- Śmietana 30% (29)
(43, 29,  'Łaciata 30% 200ml',             '5900820030001'),
(44, 29,  'Piątnica 30% 200ml',            '5900199030001'),
-- Mozzarella (20)
(45, 20,  'Galbani Mozzarella 125g',       '8000430130003'),
(46, 20,  'Zott Mozzarella 125g',          '4014500513010'),
-- Parmezan (26)
(47, 26,  'Parmezan tarty 100g',           '5900000110001'),
(48, 26,  'Parmigiano Reggiano 200g',      '5900000110002'),
-- Stek wołowy (1)
(49, 1,   'Ribeye 300g',                   '5900000120001'),
(50, 1,   'New York Strip 250g',           '5900000120002')
ON CONFLICT ("VariantId") DO NOTHING;

SELECT setval('app."ProductVariants_VariantId_seq"', (SELECT MAX("VariantId") FROM app."ProductVariants"));

-- Recipe Categories
INSERT INTO app."RecipeCategories" ("RecipeCategoryId", "Name") VALUES
(1, 'Śniadanie'),
(2, 'Obiad'),
(3, 'Kolacja'),
(4, 'Drink'),
(5, 'Przekąska'),
(6, 'Deser'),
(7, 'Zupa')
ON CONFLICT ("RecipeCategoryId") DO NOTHING;

SELECT setval('app."RecipeCategories_RecipeCategoryId_seq"', (SELECT MAX("RecipeCategoryId") FROM app."RecipeCategories"));


-- ============================================================
-- 6. Admin user
--    Password: "admin"  (bcrypt, cost 11)
--    IMPORTANT: Change this password after first login.
-- ============================================================

INSERT INTO app."AppUsers" ("Email", "PasswordHash", "Name", "Role")
VALUES (
    'admin@admin.com',
    '$2a$11$ME3Q9eYEI3IBhokEgLICeeqEQt66mdV.eqwW9rq5GMLYme4cd6VS.',
    'Admin',
    'Admin'
)
ON CONFLICT ("Email") DO NOTHING;
