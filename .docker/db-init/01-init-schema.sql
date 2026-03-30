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
    "InsertedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt"  TIMESTAMP,
    CONSTRAINT "FK_FoodProducts_Categories" FOREIGN KEY ("CategoryId")
        REFERENCES app."Categories"("CategoryId") ON DELETE CASCADE
);

-- Table: RecipeCategories
CREATE TABLE IF NOT EXISTS app."RecipeCategories" (
    "RecipeCategoryId" SMALLSERIAL PRIMARY KEY,
    "Name" VARCHAR(25) NOT NULL
);

-- Table: Kitchens
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

-- Table: AppUsers (authentication accounts — email/password and Google OAuth)
CREATE TABLE IF NOT EXISTS app."AppUsers" (
    "Email"        VARCHAR(250) PRIMARY KEY,
    "PasswordHash" VARCHAR(500) NULL,
    "Name"         VARCHAR(100) NULL,
    "Role"         VARCHAR(50)  NOT NULL DEFAULT 'User',
    "CreatedAt"    TIMESTAMP    NOT NULL DEFAULT NOW(),
    "UpdatedAt"    TIMESTAMP
);

-- Table: ProductVariants
CREATE TABLE IF NOT EXISTS app."ProductVariants" (
    "VariantId"     SERIAL       PRIMARY KEY,
    "FoodProductId" SMALLINT     NOT NULL,
    "Name"          VARCHAR(80)  NOT NULL,
    "Barcode"       VARCHAR(50)  NULL,
    CONSTRAINT "FK_ProductVariants_FoodProducts" FOREIGN KEY ("FoodProductId")
        REFERENCES app."FoodProducts"("FoodProductId") ON DELETE CASCADE
);
CREATE UNIQUE INDEX IF NOT EXISTS "UX_ProductVariants_Barcode"
    ON app."ProductVariants"("Barcode") WHERE "Barcode" IS NOT NULL;

-- Table: KitchenMembers (links AppUsers to Kitchens, handles invites)
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
    CONSTRAINT "FK_KitchenMembers_Kitchens"   FOREIGN KEY ("KitchenId") REFERENCES app."Kitchens"("Id")    ON DELETE CASCADE,
    CONSTRAINT "FK_KitchenMembers_AppUsers"  FOREIGN KEY ("Email")    REFERENCES app."AppUsers"("Email") ON DELETE CASCADE,
    CONSTRAINT "UQ_KitchenMembers"           UNIQUE ("KitchenId", "Email")
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
    "InsertedAt"        TIMESTAMP    NOT NULL DEFAULT NOW(),
    "UpdatedAt"         TIMESTAMP,
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

CREATE INDEX IF NOT EXISTS "IX_KitchenMembers_Email"       ON app."KitchenMembers"("Email");
CREATE INDEX IF NOT EXISTS "IX_KitchenMembers_kitchenId"    ON app."KitchenMembers"("KitchenId");
CREATE INDEX IF NOT EXISTS "IX_KitchenMembers_Status"      ON app."KitchenMembers"("Status");
CREATE INDEX IF NOT EXISTS "IX_FoodProducts_CategoryId"   ON app."FoodProducts"("CategoryId");
CREATE INDEX IF NOT EXISTS "IX_Recipes_RecipeCategoryId"  ON app."Recipes"("RecipeCategoryId");
CREATE INDEX IF NOT EXISTS "IX_OutboxMessages_ProcessedDate" ON internal."OutboxMessages"("ProcessedDate") WHERE "ProcessedDate" IS NULL;
