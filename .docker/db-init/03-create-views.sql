-- ============================================
-- Views (app schema) — for manual debugging / reporting.
-- These are NOT used by application queries.
-- ============================================

-- View: FoodProducts with Category
DROP VIEW IF EXISTS app.v_foodproducts;
CREATE VIEW app.v_foodproducts AS
SELECT 
    fp."FoodProductId", 
    fp."Name", 
    c."CategoryId",
    c."Name" as "Category",
    fp."InsertedAt",
    fp."UpdatedAt"
FROM app."FoodProducts" as fp
INNER JOIN app."Categories" c ON c."CategoryId" = fp."CategoryId";

-- View: Kitchens
DROP VIEW IF EXISTS app.v_kitchens;
CREATE VIEW app.v_kitchens AS
SELECT "Id", "Name", "Address", "Desc", "CreatedAt"
FROM app."Kitchens";

-- View: Member Kitchens — Kitchens accessible to a member (for GetMyKitchensAsync)
DROP VIEW IF EXISTS app.v_member_kitchens;
CREATE VIEW app.v_member_kitchens AS
SELECT f."Id", f."Name", f."Address", f."Desc", f."CreatedAt", f."UpdatedAt",
       fm."Email"
FROM app."Kitchens" f
INNER JOIN app."KitchenMembers" fm ON fm."KitchenId" = f."Id"
WHERE fm."Status" = 'Accepted';

-- View: Kitchen Members (accepted only — for backwards compat / reporting)
DROP VIEW IF EXISTS app.v_KitchenMembers;
CREATE VIEW app.v_KitchenMembers AS
SELECT fm."Id", fm."KitchenId", fm."Email", au."Name", fm."MemberRole", fm."Color", fm."InvitedAt", fm."UpdatedAt"
FROM app."KitchenMembers" fm
LEFT JOIN app."AppUsers" au ON au."Email" = fm."Email"
WHERE fm."Status" = 'Accepted';

-- View: Kitchen Members Detail — all statuses with resolved name (for GetMembersAsync)
DROP VIEW IF EXISTS app.v_kitchen_members_detail;
CREATE VIEW app.v_kitchen_members_detail AS
SELECT fm."Id", fm."KitchenId", fm."Email",
       COALESCE(au."Name", fm."Email") AS "Name",
       fm."MemberRole", fm."Status", fm."Color"
FROM app."KitchenMembers" fm
LEFT JOIN app."AppUsers" au ON au."Email" = fm."Email";

-- View: Pending Invites — for GetPendingInvitesAsync
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

-- View: Recipes
DROP VIEW IF EXISTS app.v_recipes;
CREATE VIEW app.v_recipes AS
SELECT 
    r."RecipeId", 
    r."Name", 
    rc."Name" as "RecipeCategory", 
    r."Description", 
    r."FoodProducts", 
    r."RequiredTime", 
    r."LevelOfDifficulty",
    r."InsertedAt",
    r."UpdatedAt"
FROM app."Recipes" as r
LEFT JOIN app."RecipeCategories" as rc ON rc."RecipeCategoryId" = r."RecipeCategoryId";

-- View: Recipe Categories
DROP VIEW IF EXISTS app.v_recipecategories;
CREATE VIEW app.v_recipecategories AS
SELECT "RecipeCategoryId", "Name"
FROM app."RecipeCategories";
