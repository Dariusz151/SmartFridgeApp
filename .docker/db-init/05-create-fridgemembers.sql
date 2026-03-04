-- FridgeMembers: links AppUsers (email-based auth accounts) to Fridges
-- MemberRole: 'Creator' or 'Member'
-- Status: 'Accepted' or 'Pending'
CREATE TABLE IF NOT EXISTS app."FridgeMembers" (
    "Id"         SERIAL       PRIMARY KEY,
    "FridgeId"   UUID         NOT NULL,
    "Email"      VARCHAR(250) NOT NULL,
    "MemberRole" VARCHAR(50)  NOT NULL DEFAULT 'Member',
    "Status"     VARCHAR(50)  NOT NULL DEFAULT 'Pending',
    "Color"      VARCHAR(7)   NOT NULL DEFAULT '#000000',
    "InvitedAt"  TIMESTAMP    NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_FridgeMembers_Fridges"   FOREIGN KEY ("FridgeId") REFERENCES app."Fridges"("Id")    ON DELETE CASCADE,
    CONSTRAINT "FK_FridgeMembers_AppUsers"  FOREIGN KEY ("Email")    REFERENCES app."AppUsers"("Email") ON DELETE CASCADE,
    CONSTRAINT "UQ_FridgeMembers"           UNIQUE ("FridgeId", "Email")
);

CREATE INDEX IF NOT EXISTS "IX_FridgeMembers_Email"    ON app."FridgeMembers"("Email");
CREATE INDEX IF NOT EXISTS "IX_FridgeMembers_FridgeId" ON app."FridgeMembers"("FridgeId");
CREATE INDEX IF NOT EXISTS "IX_FridgeMembers_Status"   ON app."FridgeMembers"("Status");

-- Link seed data: assign existing fridges to admin
-- The 3 seeded fridges from 02-seed-data.sql get admin as Creator
INSERT INTO app."FridgeMembers" ("FridgeId", "Email", "MemberRole", "Status", "Color")
SELECT f."Id", 'admin@admin.com', 'Creator', 'Accepted', '#00695c'
FROM app."Fridges" f
ON CONFLICT ("FridgeId", "Email") DO NOTHING;

-- Also create domain Users for admin in each fridge so the user selector works
INSERT INTO app."Users" ("Id", "Name", "Email", "FridgeId", "CreatedAt")
SELECT gen_random_uuid(), 'Admin', 'admin@admin.com', f."Id", NOW()
FROM app."Fridges" f
WHERE NOT EXISTS (
    SELECT 1 FROM app."Users" u WHERE u."Email" = 'admin@admin.com' AND u."FridgeId" = f."Id"
);
