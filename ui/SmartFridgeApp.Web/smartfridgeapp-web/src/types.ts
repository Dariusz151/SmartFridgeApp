/* ── Domain types ── */

export interface FoodProduct {
  foodProductId: number;
  foodProductName: string;
  foodProductCategory?: string;
}

export interface FoodProductCategory {
  categoryId: number;
  name: string;
}

export interface Kitchen {
  id: string;
  name: string;
  address: string;
  desc?: string;
}

export interface FridgeUser {
  id: string;
  name: string;
  email?: string;
}

export interface AmountValue {
  value: number;
  unit: Unit;
}

export type Unit = "Grams" | "Pieces" | "Mililiter" | "NotAssigned";

export type StorageLocation = "Fridge" | "Freezer" | "Pantry" | "SpiceRack" | "Counter" | "Cellar";

export type ItemTag =
  | "Organic"
  | "GlutenFree"
  | "Vegan"
  | "DairyFree"
  | "LowSugar"
  | "HighProtein"
  | "Homemade"
  | "Leftover"
  | "ForParty"
  | "QuickMeal";

export const STORAGE_LOCATIONS: { value: StorageLocation; label: string; icon: string }[] = [
  { value: "Fridge", label: "Fridge", icon: "\u{1F9CA}" },
  { value: "Freezer", label: "Freezer", icon: "\u{2744}\u{FE0F}" },
  { value: "Pantry", label: "Pantry", icon: "\u{1F3E0}" },
  { value: "SpiceRack", label: "Spice Rack", icon: "\u{1F336}\u{FE0F}" },
  { value: "Counter", label: "Counter", icon: "\u{1F372}" },
  { value: "Cellar", label: "Cellar", icon: "\u{1F377}" },
];

export const ITEM_TAGS: { value: ItemTag; label: string }[] = [
  { value: "Organic", label: "Organic" },
  { value: "GlutenFree", label: "Gluten Free" },
  { value: "Vegan", label: "Vegan" },
  { value: "DairyFree", label: "Dairy Free" },
  { value: "LowSugar", label: "Low Sugar" },
  { value: "HighProtein", label: "High Protein" },
  { value: "Homemade", label: "Homemade" },
  { value: "Leftover", label: "Leftover" },
  { value: "ForParty", label: "For Party" },
  { value: "QuickMeal", label: "Quick Meal" },
];

export interface FridgeItem {
  stockItemId: string;
  foodProductId: number;
  memberId: number;
  amount: number;
  unit: Unit;
  expirationDate: string;
  note?: string;
  location: StorageLocation;
  tags: ItemTag[];
  stockedAt: string;
}

export interface KitchenMember {
  id: number;
  kitchenId: string;
  email: string;
  name: string;
  memberRole: string;
  status: string;
  color: string;
}

export interface KitchenInvite {
  id: number;
  kitchenId: string;
  kitchenName: string;
  inviterEmail: string;
  inviterName: string;
  invitedAt: string;
}

export interface ExpiringItem {
  stockItemId: string;
  foodProductId: number;
  amount: number;
  unit: string;
  expirationDate: string;
  daysUntilExpiry: number;
  memberId: number;
}

export interface KitchenScore {
  kitchenId: string;
  wasteScore: number;
  rank: string;
}

export interface ShoppingStatus {
  kitchenId: string;
  activeItemCount: number;
  averageItemCount: number;
  isShoppingNeeded: boolean;
}

export interface RecipeFoodProduct {
  foodProductId: number;
  foodProductName: string;
  amountValue: AmountValue;
  optional?: boolean;
}

export interface Recipe {
  recipeId?: string;
  name: string;
  recipeName?: string;
  description: string;
  recipeCategory: number | string;
  requiredTime: number | string;
  levelOfDifficulty: number | string;
  foodProducts: RecipeFoodProduct[] | string;
}

export interface RecipeCreatePayload {
  name: string;
  description: string;
  recipeCategory: number;
  requiredTime: number;
  levelOfDifficulty: number;
  products: {
    foodProductId: number;
    amountValue: { value: number; unit: string };
    optional: boolean;
  }[];
}

/* ── Auth ── */

export interface AuthState {
  isAdmin: boolean;
  token: string | null;
  role: string | null;
  name: string | null;
  email: string | null;
}

export type AuthAction =
  | { type: "LOGIN_ADMIN"; payload: { token: string; role: string; name?: string; email?: string } }
  | { type: "LOGOUT_ADMIN" };
