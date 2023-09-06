import { createContext, useContext } from "react";
import { Cart, CartContextType } from "./CartContextType";

const emptyCart: Cart = {
    bouquets: [],
    amount: 0
}

export const CartContext = createContext<CartContextType>({
    cart: emptyCart,
    bouquetsCount: 0,
    addToCart: () => console.log('cart is empty'),
    addCartBoquetToCart: () => console.log('cart is empty'),
    clearCart: () => console.log('cart is empty'),
    removeFromCart: () => console.log('cart is empty'),
    removeGroupe: () => console.log('cart is empty'),
})

export const useCart = () => useContext(CartContext);