CREATE OR REPLACE VIEW public.v_foodproducts AS
SELECT 
    fp."FoodProductId", 
    fp."Name", 
    c."CategoryId",
    c."Name" as "Category"
FROM app."FoodProducts" as fp
INNER JOIN app."Categories" c ON c."CategoryId" = fp."CategoryId";

-- View: Fridge Items (Active)
CREATE OR REPLACE VIEW public.v_fridgeitems AS
SELECT        
    fi."Id", 
    fi."UserId", 
    u."FridgeId",
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
INNER JOIN app."FoodProducts" as fp ON fp."FoodProductId" = fi."FoodProductId"
INNER JOIN app."Categories" as c ON c."CategoryId" = fp."CategoryId"
INNER JOIN app."Users" as u ON u."Id" = fi."UserId"
WHERE fi."IsConsumed" = false;

-- View: Fridges
CREATE OR REPLACE VIEW public.v_fridges AS
SELECT "Id", "Name", "Address", "Desc"
FROM app."Fridges";

-- View: Fridge Users
CREATE OR REPLACE VIEW public.v_fridgeusers AS
SELECT "Id", "Name", "Email", "FridgeId", "CreatedAt"
FROM app."Users";

-- View: Recipes
CREATE OR REPLACE VIEW public.v_recipes AS
SELECT 
    r."RecipeId", 
    r."Name", 
    rc."Name" as "RecipeCategory", 
    r."Description", 
    r."FoodProducts", 
    r."RequiredTime", 
    r."LevelOfDifficulty"
FROM app."Recipes" as r
LEFT JOIN app."RecipeCategories" as rc ON rc."RecipeCategoryId" = r."RecipeCategoryId";

-- View: Recipe Categories
CREATE OR REPLACE VIEW public.v_recipecategories AS
SELECT "RecipeCategoryId", "Name"
FROM app."RecipeCategories";

-- View: Consumed Fridge Items
CREATE OR REPLACE VIEW public.v_consumedfridgeitems AS
SELECT        
    fi."Id", 
    fi."UserId", 
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
INNER JOIN app."FoodProducts" as fp ON fp."FoodProductId" = fi."FoodProductId"
INNER JOIN app."Categories" as c ON c."CategoryId" = fp."CategoryId"
WHERE fi."IsConsumed" = true;
