-- Dev seed: 3 test users, 3 Kitchens, members, and Kitchen items
-- Idempotent (ON CONFLICT DO NOTHING).
-- Topology:
--   Jan  -> Creator of "Lodówka Jana" (sole member)
--   Anna -> Creator of "Lodówka Anny" (+ Dariusz as Member)
--   Dariusz -> Member of Anna's Kitchen, Creator of "Lodówka Dariusza"
--   Admin -> sees all Kitchens via role, NOT an explicit member

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
-- 2. Kitchen 1: "Lodówka Jana" — Creator: Jan (sole member)
-- ============================================
INSERT INTO app."Kitchens" ("Id", "Name", "Address", "Desc")
VALUES ('a1b2c3d4-0000-0000-0000-000000000001', 'Lodówka Jana', 'ul. Testowa 1', 'Prywatna lodówka Jana')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO app."KitchenMembers" ("kitchenId", "Email", "MemberRole", "Status", "Color")
VALUES ('a1b2c3d4-0000-0000-0000-000000000001', 'jan@test.com', 'Creator', 'Accepted', '#1565c0')
ON CONFLICT ("kitchenId", "Email") DO NOTHING;

-- ============================================
-- 3. Kitchen 2: "Lodówka Anny" — Creator: Anna, Member: Dariusz
-- ============================================
INSERT INTO app."Kitchens" ("Id", "Name", "Address", "Desc")
VALUES ('b2c3d4e5-0000-0000-0000-000000000002', 'Lodówka Anny', 'ul. Wspólna 10', 'Lodówka Anny i Dariusza')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO app."KitchenMembers" ("kitchenId", "Email", "MemberRole", "Status", "Color")
VALUES
    ('b2c3d4e5-0000-0000-0000-000000000002', 'anna@test.com',    'Creator', 'Accepted', '#ad1457'),
    ('b2c3d4e5-0000-0000-0000-000000000002', 'dariusz@test.com', 'Member',  'Accepted', '#e65100')
ON CONFLICT ("kitchenId", "Email") DO NOTHING;

-- ============================================
-- 4. Kitchen 3: "Lodówka Dariusza" — Creator: Dariusz (sole member)
-- ============================================
INSERT INTO app."Kitchens" ("Id", "Name", "Address", "Desc")
VALUES ('c3d4e5f6-0000-0000-0000-000000000003', 'Lodówka Dariusza', 'ul. Polna 5', 'Prywatna lodówka Dariusza')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO app."KitchenMembers" ("kitchenId", "Email", "MemberRole", "Status", "Color")
VALUES ('c3d4e5f6-0000-0000-0000-000000000003', 'dariusz@test.com', 'Creator', 'Accepted', '#4527a0')
ON CONFLICT ("kitchenId", "Email") DO NOTHING;
