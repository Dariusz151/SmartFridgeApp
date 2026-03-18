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

-- View: Fridge Items (Active — not consumed, not wasted)
DROP VIEW IF EXISTS app.v_fridgeitems;
CREATE VIEW app.v_fridgeitems AS
SELECT        
    fi."Id", 
    fi."MemberId",
    fm."FridgeId",
    fi."IsConsumed", 
    fi."EnteredAt", 
    fi."ExpirationDate", 
    fi."Unit", 
    fi."Value", 
    fi."Note",
    fp."FoodProductId",
    fp."Name" as "ProductName",
    c."CategoryId" as "CategoryId",
    c."Name" as "CategoryName"
FROM app."FridgeItems" as fi
INNER JOIN app."FridgeMembers" as fm ON fm."Id" = fi."MemberId"
INNER JOIN app."FoodProducts" as fp ON fp."FoodProductId" = fi."FoodProductId"
INNER JOIN app."Categories" as c ON c."CategoryId" = fp."CategoryId"
WHERE fi."IsConsumed" = false AND fi."IsWasted" = false;

-- View: Fridges (includes WasteScore and CreatedAt)
DROP VIEW IF EXISTS app.v_fridges;
CREATE VIEW app.v_fridges AS
SELECT "Id", "Name", "Address", "Desc", "WasteScore", "CreatedAt", "UpdatedAt"
FROM app."Fridges";

-- View: Member Fridges — fridges accessible to a member (for GetMyFridgesAsync)
DROP VIEW IF EXISTS app.v_member_fridges;
CREATE VIEW app.v_member_fridges AS
SELECT f."Id", f."Name", f."Address", f."Desc", f."WasteScore", f."CreatedAt", f."UpdatedAt",
       fm."Email"
FROM app."Fridges" f
INNER JOIN app."FridgeMembers" fm ON fm."FridgeId" = f."Id"
WHERE fm."Status" = 'Accepted';

-- View: Fridge Members (accepted only — for backwards compat / reporting)
DROP VIEW IF EXISTS app.v_fridgemembers;
CREATE VIEW app.v_fridgemembers AS
SELECT fm."Id", fm."FridgeId", fm."Email", au."Name", fm."MemberRole", fm."Color", fm."InvitedAt", fm."UpdatedAt"
FROM app."FridgeMembers" fm
LEFT JOIN app."AppUsers" au ON au."Email" = fm."Email"
WHERE fm."Status" = 'Accepted';

-- View: Fridge Members Detail — all statuses with resolved name (for GetMembersAsync)
DROP VIEW IF EXISTS app.v_fridge_members_detail;
CREATE VIEW app.v_fridge_members_detail AS
SELECT fm."Id", fm."FridgeId", fm."Email",
       COALESCE(au."Name", fm."Email") AS "Name",
       fm."MemberRole", fm."Status", fm."Color"
FROM app."FridgeMembers" fm
LEFT JOIN app."AppUsers" au ON au."Email" = fm."Email";

-- View: Pending Invites — for GetPendingInvitesAsync
DROP VIEW IF EXISTS app.v_pending_invites;
CREATE VIEW app.v_pending_invites AS
SELECT fm."Id", fm."FridgeId", fm."Email",
       f."Name" AS "FridgeName",
       creator."Email" AS "InviterEmail",
       COALESCE(au."Name", creator."Email") AS "InviterName",
       fm."InvitedAt"
FROM app."FridgeMembers" fm
INNER JOIN app."Fridges" f ON f."Id" = fm."FridgeId"
INNER JOIN app."FridgeMembers" creator
    ON creator."FridgeId" = fm."FridgeId" AND creator."MemberRole" = 'Creator'
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

-- View: Consumed Fridge Items
DROP VIEW IF EXISTS app.v_consumedfridgeitems;
CREATE VIEW app.v_consumedfridgeitems AS
SELECT        
    fi."Id", 
    fi."MemberId",
    fm."FridgeId",
    fi."IsConsumed", 
    fi."EnteredAt", 
    fi."ExpirationDate", 
    fi."Unit", 
    fi."Value", 
    fi."Note",
    fp."FoodProductId",
    fp."Name" as "ProductName",
    c."CategoryId" as "CategoryId",
    c."Name" as "CategoryName"
FROM app."FridgeItems" as fi
INNER JOIN app."FridgeMembers" as fm ON fm."Id" = fi."MemberId"
INNER JOIN app."FoodProducts" as fp ON fp."FoodProductId" = fi."FoodProductId"
INNER JOIN app."Categories" as c ON c."CategoryId" = fp."CategoryId"
WHERE fi."IsConsumed" = true;

-- View: Wasted Fridge Items
DROP VIEW IF EXISTS app.v_wastedfridgeitems;
CREATE VIEW app.v_wastedfridgeitems AS
SELECT        
    fi."Id", 
    fi."MemberId",
    fm."FridgeId",
    fi."IsWasted",
    fi."WastedAt",
    fi."WasteReason",
    fi."EnteredAt", 
    fi."ExpirationDate", 
    fi."Unit", 
    fi."Value", 
    fi."Note",
    fp."FoodProductId",
    fp."Name" as "ProductName",
    c."CategoryId" as "CategoryId",
    c."Name" as "CategoryName"
FROM app."FridgeItems" as fi
INNER JOIN app."FridgeMembers" as fm ON fm."Id" = fi."MemberId"
INNER JOIN app."FoodProducts" as fp ON fp."FoodProductId" = fi."FoodProductId"
INNER JOIN app."Categories" as c ON c."CategoryId" = fp."CategoryId"
WHERE fi."IsWasted" = true;
