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

export interface Fridge {
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

export interface FridgeItem {
  fridgeItemId: string;
  foodProductId: number;
  productName: string;
  categoryName: string;
  value: number;
  unit: Unit;
  expirationDate: string;
  userName?: string;
  userEmail?: string;
  userColor?: string;
}

export interface FridgeMember {
  id: number;
  fridgeId: string;
  email: string;
  name: string;
  memberRole: string;
  status: string;
  color: string;
}

export interface FridgeInvite {
  id: number;
  fridgeId: string;
  fridgeName: string;
  inviterEmail: string;
  inviterName: string;
  invitedAt: string;
}

export interface ExpiringItem {
  fridgeItemId: number;
  productName: string;
  categoryName: string;
  value: number;
  unit: string;
  expirationDate: string;
  daysUntilExpiry: number;
  userName: string;
  userEmail: string;
}

export interface FridgeScore {
  fridgeId: string;
  wasteScore: number;
  rank: string;
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
