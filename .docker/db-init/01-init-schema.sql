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
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "Address" VARCHAR(100),
    "Desc" VARCHAR(250)
);

-- Table: Users
CREATE TABLE IF NOT EXISTS app."Users" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(250) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "FridgeId" UUID NOT NULL,
    CONSTRAINT "FK_Users_Fridges" FOREIGN KEY ("FridgeId") 
        REFERENCES app."Fridges"("Id") ON DELETE CASCADE
);

-- Table: FridgeItems
CREATE TABLE IF NOT EXISTS app."FridgeItems" (
    "Id" BIGSERIAL PRIMARY KEY,
    "FoodProductId" SMALLINT NOT NULL,
    "Note" VARCHAR(1000),
    "Value" NUMERIC NOT NULL,
    "Unit" VARCHAR(50) NOT NULL,
    "ExpirationDate" TIMESTAMP NOT NULL,
    "EnteredAt" TIMESTAMP NOT NULL,
    "IsConsumed" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsWasted" BOOLEAN NOT NULL DEFAULT FALSE,
    "WastedAt" TIMESTAMP,
    "WasteReason" VARCHAR(500),
    "UserId" UUID NOT NULL,
    CONSTRAINT "FK_FridgeItems_FoodProducts" FOREIGN KEY ("FoodProductId") 
        REFERENCES app."FoodProducts"("FoodProductId") ON DELETE CASCADE,
    CONSTRAINT "FK_FridgeItems_Users" FOREIGN KEY ("UserId") 
        REFERENCES app."Users"("Id") ON DELETE CASCADE
);

-- Table: Recipes
CREATE TABLE IF NOT EXISTS app."Recipes" (
    "RecipeId" UUID PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Description" VARCHAR(5000),
    "RequiredTime" INTEGER NOT NULL DEFAULT -1,
    "LevelOfDifficulty" SMALLINT NOT NULL DEFAULT 0,
    "RecipeCategoryId" SMALLINT,
    "FoodProducts" TEXT NOT NULL,
    CONSTRAINT "FK_Recipes_RecipeCategories" FOREIGN KEY ("RecipeCategoryId") 
        REFERENCES app."RecipeCategories"("RecipeCategoryId") ON DELETE SET NULL
);

-- ============================================
-- Internal Schema Tables
-- ============================================

-- Table: OutboxMessages
CREATE TABLE IF NOT EXISTS internal."OutboxMessages" (
    "Id" UUID PRIMARY KEY,
    "OccurredOn" TIMESTAMP NOT NULL,
    "Type" VARCHAR(255) NOT NULL,
    "Data" TEXT NOT NULL,
    "ProcessedDate" TIMESTAMP
);

-- ============================================
-- Indexes for Performance
-- ============================================

CREATE INDEX IF NOT EXISTS "IX_Users_FridgeId" ON app."Users"("FridgeId");
CREATE INDEX IF NOT EXISTS "IX_FridgeItems_UserId" ON app."FridgeItems"("UserId");
CREATE INDEX IF NOT EXISTS "IX_FridgeItems_FoodProductId" ON app."FridgeItems"("FoodProductId");
CREATE INDEX IF NOT EXISTS "IX_FridgeItems_IsConsumed" ON app."FridgeItems"("IsConsumed");
CREATE INDEX IF NOT EXISTS "IX_FoodProducts_CategoryId" ON app."FoodProducts"("CategoryId");
CREATE INDEX IF NOT EXISTS "IX_Recipes_RecipeCategoryId" ON app."Recipes"("RecipeCategoryId");
CREATE INDEX IF NOT EXISTS "IX_OutboxMessages_ProcessedDate" ON internal."OutboxMessages"("ProcessedDate") WHERE "ProcessedDate" IS NULL;
