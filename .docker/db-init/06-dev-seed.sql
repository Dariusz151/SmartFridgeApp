-- Dev seed: 3 test users, 3 fridges, members, and fridge items
-- Idempotent (ON CONFLICT DO NOTHING).
-- Topology:
--   Jan  -> Creator of "Lodówka Jana" (sole member)
--   Anna -> Creator of "Lodówka Anny" (+ Dariusz as Member)
--   Dariusz -> Member of Anna's fridge, Creator of "Lodówka Dariusza"
--   Admin -> sees all fridges via role, NOT an explicit member

-- ============================================
-- 1. Create 3 test users (password: "admin" for all)
-- ============================================
INSERT INTO app."AppUsers" ("Email", "PasswordHash", "Name", "Role")
VALUES
    ('jan@test.com',     '$2a$11$ME3Q9eYEI3IBhokEgLICeeqEQt66mdV.eqwW9rq5GMLYme4cd6VS.', 'Jan Kowalski',     'User'),
    ('anna@test.com',    '$2a$11$ME3Q9eYEI3IBhokEgLICeeqEQt66mdV.eqwW9rq5GMLYme4cd6VS.', 'Anna Nowak',        'User'),
    ('dariusz@test.com', '$2a$11$ME3Q9eYEI3IBhokEgLICeeqEQt66mdV.eqwW9rq5GMLYme4cd6VS.', 'Dariusz Wiśniewski','User')
ON CONFLICT ("Email") DO NOTHING;

-- ============================================
-- 2. Fridge 1: "Lodówka Jana" — Creator: Jan (sole member)
-- ============================================
INSERT INTO app."Fridges" ("Id", "Name", "Address", "Desc")
VALUES ('a1b2c3d4-0000-0000-0000-000000000001', 'Lodówka Jana', 'ul. Testowa 1', 'Prywatna lodówka Jana')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO app."FridgeMembers" ("FridgeId", "Email", "MemberRole", "Status", "Color")
VALUES ('a1b2c3d4-0000-0000-0000-000000000001', 'jan@test.com', 'Creator', 'Accepted', '#1565c0')
ON CONFLICT ("FridgeId", "Email") DO NOTHING;

-- ============================================
-- 3. Fridge 2: "Lodówka Anny" — Creator: Anna, Member: Dariusz
-- ============================================
INSERT INTO app."Fridges" ("Id", "Name", "Address", "Desc")
VALUES ('b2c3d4e5-0000-0000-0000-000000000002', 'Lodówka Anny', 'ul. Wspólna 10', 'Lodówka Anny i Dariusza')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO app."FridgeMembers" ("FridgeId", "Email", "MemberRole", "Status", "Color")
VALUES
    ('b2c3d4e5-0000-0000-0000-000000000002', 'anna@test.com',    'Creator', 'Accepted', '#ad1457'),
    ('b2c3d4e5-0000-0000-0000-000000000002', 'dariusz@test.com', 'Member',  'Accepted', '#e65100')
ON CONFLICT ("FridgeId", "Email") DO NOTHING;

-- ============================================
-- 4. Fridge 3: "Lodówka Dariusza" — Creator: Dariusz (sole member)
-- ============================================
INSERT INTO app."Fridges" ("Id", "Name", "Address", "Desc")
VALUES ('c3d4e5f6-0000-0000-0000-000000000003', 'Lodówka Dariusza', 'ul. Polna 5', 'Prywatna lodówka Dariusza')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO app."FridgeMembers" ("FridgeId", "Email", "MemberRole", "Status", "Color")
VALUES ('c3d4e5f6-0000-0000-0000-000000000003', 'dariusz@test.com', 'Creator', 'Accepted', '#4527a0')
ON CONFLICT ("FridgeId", "Email") DO NOTHING;

-- ============================================
-- 5. Seed fridge items (dynamic MemberId lookup)
-- ============================================
DO $$
DECLARE
    v_jan_f1      INTEGER;
    v_anna_f2     INTEGER;
    v_dariusz_f2  INTEGER;
    v_dariusz_f3  INTEGER;
BEGIN
    -- Fridge 1: Jan
    SELECT "Id" INTO v_jan_f1
    FROM app."FridgeMembers"
    WHERE "FridgeId" = 'a1b2c3d4-0000-0000-0000-000000000001' AND "Email" = 'jan@test.com';

    -- Fridge 2: Anna + Dariusz
    SELECT "Id" INTO v_anna_f2
    FROM app."FridgeMembers"
    WHERE "FridgeId" = 'b2c3d4e5-0000-0000-0000-000000000002' AND "Email" = 'anna@test.com';

    SELECT "Id" INTO v_dariusz_f2
    FROM app."FridgeMembers"
    WHERE "FridgeId" = 'b2c3d4e5-0000-0000-0000-000000000002' AND "Email" = 'dariusz@test.com';

    -- Fridge 3: Dariusz
    SELECT "Id" INTO v_dariusz_f3
    FROM app."FridgeMembers"
    WHERE "FridgeId" = 'c3d4e5f6-0000-0000-0000-000000000003' AND "Email" = 'dariusz@test.com';

    -- ── Fridge 1: Jan's items (5) ──────────────────
    IF v_jan_f1 IS NOT NULL THEN
        INSERT INTO app."FridgeItems" ("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        SELECT val.* FROM (VALUES
            (22::smallint, 'Mleko 2%',         3,    'Pieces',    NOW() + INTERVAL '5 days',  NOW(), false, false, v_jan_f1),
            (19::smallint, '',                  10,   'Pieces',    NOW() + INTERVAL '14 days', NOW(), false, false, v_jan_f1),
            (15::smallint, 'Na obiad',          500,  'Grams',     NOW() + INTERVAL '3 days',  NOW(), false, false, v_jan_f1),
            (120::smallint,'',                  1000, 'Grams',     NOW() + INTERVAL '90 days', NOW(), false, false, v_jan_f1),
            (30::smallint, 'Sok pomarańczowy',  1000, 'Mililiter', NOW() + INTERVAL '7 days',  NOW(), false, false, v_jan_f1)
        ) AS val("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        WHERE NOT EXISTS (
            SELECT 1 FROM app."FridgeItems" WHERE "MemberId" = v_jan_f1 AND "IsConsumed" = false AND "IsWasted" = false LIMIT 1
        );
    END IF;

    -- ── Fridge 2: Anna's items (5) ─────────────────
    IF v_anna_f2 IS NOT NULL THEN
        INSERT INTO app."FridgeItems" ("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        SELECT val.* FROM (VALUES
            (22::smallint, 'Mleko sojowe',      1000, 'Mililiter', NOW() + INTERVAL '7 days',  NOW(), false, false, v_anna_f2),
            (45::smallint, 'Jabłka',            5,    'Pieces',    NOW() + INTERVAL '14 days', NOW(), false, false, v_anna_f2),
            (10::smallint, 'Pomidory',          300,  'Grams',     NOW() + INTERVAL '5 days',  NOW(), false, false, v_anna_f2),
            (3::smallint,  'Parówki',           200,  'Grams',     NOW() + INTERVAL '4 days',  NOW(), false, false, v_anna_f2),
            (65::smallint, 'Makaron spaghetti', 500,  'Grams',     NOW() + INTERVAL '365 days',NOW(), false, false, v_anna_f2)
        ) AS val("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        WHERE NOT EXISTS (
            SELECT 1 FROM app."FridgeItems" WHERE "MemberId" = v_anna_f2 AND "IsConsumed" = false AND "IsWasted" = false LIMIT 1
        );
    END IF;

    -- ── Fridge 2: Dariusz's items (5) ──────────────
    IF v_dariusz_f2 IS NOT NULL THEN
        INSERT INTO app."FridgeItems" ("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        SELECT val.* FROM (VALUES
            (1::smallint,  'Stek na weekend',   400,  'Grams',     NOW() + INTERVAL '2 days',  NOW(), false, false, v_dariusz_f2),
            (23::smallint, 'Masło',             250,  'Grams',     NOW() + INTERVAL '30 days', NOW(), false, false, v_dariusz_f2),
            (19::smallint, 'Jajka',             6,    'Pieces',    NOW() + INTERVAL '10 days', NOW(), false, false, v_dariusz_f2),
            (86::smallint, 'Czekolada',         100,  'Grams',     NOW() + INTERVAL '180 days',NOW(), false, false, v_dariusz_f2),
            (72::smallint, 'Chleb pszenny',     1,    'Pieces',    NOW() + INTERVAL '3 days',  NOW(), false, false, v_dariusz_f2)
        ) AS val("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        WHERE NOT EXISTS (
            SELECT 1 FROM app."FridgeItems" WHERE "MemberId" = v_dariusz_f2 AND "IsConsumed" = false AND "IsWasted" = false LIMIT 1
        );
    END IF;

    -- ── Fridge 3: Dariusz's items (5) ──────────────
    IF v_dariusz_f3 IS NOT NULL THEN
        INSERT INTO app."FridgeItems" ("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        SELECT val.* FROM (VALUES
            (48::smallint, 'Banany',            4,    'Pieces',    NOW() + INTERVAL '4 days',  NOW(), false, false, v_dariusz_f3),
            (52::smallint, 'Pomarańcze',        3,    'Pieces',    NOW() + INTERVAL '10 days', NOW(), false, false, v_dariusz_f3),
            (5::smallint,  'Kurczak',           600,  'Grams',     NOW() + INTERVAL '2 days',  NOW(), false, false, v_dariusz_f3),
            (24::smallint, 'Ser żółty',         200,  'Grams',     NOW() + INTERVAL '14 days', NOW(), false, false, v_dariusz_f3),
            (33::smallint, 'Woda mineralna',    1500, 'Mililiter', NOW() + INTERVAL '180 days',NOW(), false, false, v_dariusz_f3)
        ) AS val("FoodProductId", "Note", "Value", "Unit", "ExpirationDate", "EnteredAt", "IsConsumed", "IsWasted", "MemberId")
        WHERE NOT EXISTS (
            SELECT 1 FROM app."FridgeItems" WHERE "MemberId" = v_dariusz_f3 AND "IsConsumed" = false AND "IsWasted" = false LIMIT 1
        );
    END IF;

END $$;
