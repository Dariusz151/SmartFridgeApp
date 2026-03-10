-- SmartFridgeApp Database Initialization Script
-- PostgreSQL version

-- Create schemas
CREATE SCHEMA IF NOT EXISTS app;
CREATE SCHEMA IF NOT EXISTS internal;

-- ============================================
-- Application Schema Tables
-- ============================================

-- Table: Categories
CREATE TABLE IF NOT EXISTS app."Categories" (
    "CategoryId" SMALLSERIAL PRIMARY KEY,
    "Name" VARCHAR(25) NOT NULL
);

-- Table: FoodProducts
CREATE TABLE IF NOT EXISTS app."FoodProducts" (
    "FoodProductId" SMALLSERIAL PRIMARY KEY,
    "Name" VARCHAR(40) NOT NULL,
    "CategoryId" SMALLINT NOT NULL,
    CONSTRAINT "FK_FoodProducts_Categories" FOREIGN KEY ("CategoryId")
        REFERENCES app."Categories"("CategoryId") ON DELETE CASCADE
);

-- Table: RecipeCategories
CREATE TABLE IF NOT EXISTS app."RecipeCategories" (
    "RecipeCategoryId" SMALLSERIAL PRIMARY KEY,
    "Name" VARCHAR(25) NOT NULL
);

-- Table: Fridges
CREATE TABLE IF NOT EXISTS app."Fridges" (
    "Id"                   UUID         PRIMARY KEY,
    "Name"                 VARCHAR(50)  NOT NULL,
    "Address"              VARCHAR(100),
    "Desc"                 VARCHAR(250),
    "WasteScore"           INTEGER      NOT NULL DEFAULT 1000,
    "ActiveItemCount"      INTEGER      NOT NULL DEFAULT 0,
    "AverageItemCount"     FLOAT        NOT NULL DEFAULT 0,
    "InventorySampleCount" INTEGER      NOT NULL DEFAULT 0,
    "CreatedAt"            TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- Table: AppUsers (authentication accounts — email/password and Google OAuth)
CREATE TABLE IF NOT EXISTS app."AppUsers" (
    "Email"        VARCHAR(250) PRIMARY KEY,
    "PasswordHash" VARCHAR(500) NULL,
    "Name"         VARCHAR(100) NULL,
    "Role"         VARCHAR(50)  NOT NULL DEFAULT 'User',
    "CreatedAt"    TIMESTAMP    NOT NULL DEFAULT NOW()
);

-- Table: FridgeMembers (links AppUsers to Fridges, handles invites)
-- MemberRole: 'Creator' | 'Member'
-- Status:     'Accepted' | 'Pending'
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

-- Table: FridgeItems (owned by a FridgeMember)
CREATE TABLE IF NOT EXISTS app."FridgeItems" (
    "Id"             BIGSERIAL    PRIMARY KEY,
    "FoodProductId"  SMALLINT     NOT NULL,
    "Note"           VARCHAR(1000),
    "Value"          NUMERIC      NOT NULL,
    "Unit"           VARCHAR(50)  NOT NULL,
    "ExpirationDate" TIMESTAMP    NOT NULL,
    "EnteredAt"      TIMESTAMP    NOT NULL,
    "IsConsumed"     BOOLEAN      NOT NULL DEFAULT FALSE,
    "IsWasted"       BOOLEAN      NOT NULL DEFAULT FALSE,
    "WastedAt"       TIMESTAMP,
    "WasteReason"    VARCHAR(500),
    "MemberId"       INTEGER      NOT NULL,
    CONSTRAINT "FK_FridgeItems_FoodProducts"  FOREIGN KEY ("FoodProductId") REFERENCES app."FoodProducts"("FoodProductId") ON DELETE CASCADE,
    CONSTRAINT "FK_FridgeItems_FridgeMembers" FOREIGN KEY ("MemberId")      REFERENCES app."FridgeMembers"("Id")           ON DELETE CASCADE
);

-- Table: Recipes
CREATE TABLE IF NOT EXISTS app."Recipes" (
    "RecipeId"          UUID         PRIMARY KEY,
    "Name"              VARCHAR(100) NOT NULL,
    "Description"       VARCHAR(5000),
    "RequiredTime"      INTEGER      NOT NULL DEFAULT -1,
    "LevelOfDifficulty" SMALLINT     NOT NULL DEFAULT 0,
    "RecipeCategoryId"  SMALLINT,
    "FoodProducts"      TEXT         NOT NULL,
    CONSTRAINT "FK_Recipes_RecipeCategories" FOREIGN KEY ("RecipeCategoryId")
        REFERENCES app."RecipeCategories"("RecipeCategoryId") ON DELETE SET NULL
);

-- ============================================
-- Internal Schema Tables
-- ============================================

-- Table: OutboxMessages
CREATE TABLE IF NOT EXISTS internal."OutboxMessages" (
    "Id"            UUID         PRIMARY KEY,
    "OccurredOn"    TIMESTAMP    NOT NULL,
    "Type"          VARCHAR(255) NOT NULL,
    "Data"          TEXT         NOT NULL,
    "ProcessedDate" TIMESTAMP
);

-- ============================================
-- Indexes for Performance
-- ============================================

CREATE INDEX IF NOT EXISTS "IX_FridgeMembers_Email"       ON app."FridgeMembers"("Email");
CREATE INDEX IF NOT EXISTS "IX_FridgeMembers_FridgeId"    ON app."FridgeMembers"("FridgeId");
CREATE INDEX IF NOT EXISTS "IX_FridgeMembers_Status"      ON app."FridgeMembers"("Status");
CREATE INDEX IF NOT EXISTS "IX_FridgeItems_MemberId_Active" ON app."FridgeItems"("MemberId", "IsConsumed", "IsWasted");
CREATE INDEX IF NOT EXISTS "IX_FridgeItems_FoodProductId" ON app."FridgeItems"("FoodProductId");
CREATE INDEX IF NOT EXISTS "IX_FridgeItems_WastedAt"      ON app."FridgeItems"("WastedAt") WHERE "IsWasted" = true;
CREATE INDEX IF NOT EXISTS "IX_FoodProducts_CategoryId"   ON app."FoodProducts"("CategoryId");
CREATE INDEX IF NOT EXISTS "IX_Recipes_RecipeCategoryId"  ON app."Recipes"("RecipeCategoryId");
CREATE INDEX IF NOT EXISTS "IX_OutboxMessages_ProcessedDate" ON internal."OutboxMessages"("ProcessedDate") WHERE "ProcessedDate" IS NULL;
