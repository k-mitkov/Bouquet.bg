import { Bouquet } from "@/resources";

export interface CartBouquet{
    bouquet: Bouquet,
    count: number
}

export interface Cart{
    bouquets: CartBouquet[],
    amount: number
}

export interface CartContextType {

    cart: Cart,
    bouquetsCount: number,
    addToCart(bouquet: Bouquet): void;
    addCartBoquetToCart(bouquet: CartBouquet): void;
    clearCart(): void;
    removeFromCart(bouquet: Bouquet): void,
    removeGroupe(shopId: string): void
}