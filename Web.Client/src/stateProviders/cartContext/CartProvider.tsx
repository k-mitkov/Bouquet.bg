import { useEffect, useState } from "react";
import { Cart, CartBouquet } from "./CartContextType";
import { Bouquet } from "@/resources";
import { CartContext } from "./CartContect";
import LocalStorageService from "@/services/LocalStorageService";

const emptyCart: Cart = {
    bouquets: [],
    amount: 0
}

export default function CartProvider(props: any) {
    const [cart, setCart] = useState<Cart>(emptyCart);
    const [bouquetsCount, setBouquetsCount] = useState<number>(0);

    useEffect(() => {
        const cart = LocalStorageService.getCartFromLocalStorage() ?? emptyCart;
        setCart(cart);
        let count =  0;
        cart.bouquets.forEach(b => count += b.count)
        setBouquetsCount(count)
    }, [])

    function addToCart(bouquet: Bouquet): void {

        const existingBouquet = cart.bouquets.find(b => b.bouquet.id === bouquet.id);

        if (existingBouquet) {
            cart.bouquets.find(b => b.bouquet.id === bouquet.id)!.count++;
        }else{
            const cartBouquet: CartBouquet = {
                bouquet: bouquet,
                count: 1
            }
            cart.bouquets.push(cartBouquet);
        }
        cart.amount += bouquet.price;
        setBouquetsCount(bouquetsCount + 1);

        LocalStorageService.setCartInLocalStorage(cart);
    }

    function addCartBoquetToCart(bouquet: CartBouquet): void {

        const existingBouquet = cart.bouquets.find(b => b.bouquet.id === bouquet.bouquet.id);

        if (existingBouquet) {
            cart.bouquets.find(b => b.bouquet.id === bouquet.bouquet.id)!.count++;
        }else{
            bouquet.count = 1;
            cart.bouquets.push(bouquet);
        }
        cart.amount += bouquet.bouquet.price;
        setBouquetsCount(bouquetsCount + 1);

        LocalStorageService.setCartInLocalStorage(cart);
    }

    function clearCart(): void {
        setCart(emptyCart);
        setBouquetsCount(0);
        LocalStorageService.removeCartFromLocalStorage();
    }

    function removeFromCart(bouquet: Bouquet): void {

        const existingBouquet = cart.bouquets.find(b => b.bouquet.id == bouquet.id);

        if (existingBouquet) {
            if(cart.bouquets.find(b => b.bouquet.id == bouquet.id)!.count > 1){
                cart.bouquets.find(b => b.bouquet.id == bouquet.id)!.count--;
            }else{
                cart.bouquets = cart.bouquets.filter(b => b.bouquet.id != bouquet.id)
            }
            cart.amount -= bouquet.price;
            setCart(cart)
            setBouquetsCount(bouquetsCount - 1);
        }

        LocalStorageService.setCartInLocalStorage(cart);
    }

    function removeGrope(shopId: string): void {

        const filteredBouquets = cart.bouquets.filter(b => b.bouquet.flowerShopID != shopId);
        cart.bouquets = filteredBouquets;
        setCart(cart);
        let count =  0;
        cart.bouquets.forEach(b => count += b.count)
        setBouquetsCount(count)

        LocalStorageService.setCartInLocalStorage(cart);
    }

    return (
        <CartContext.Provider value={{ cart: cart, bouquetsCount: bouquetsCount, addToCart: addToCart, addCartBoquetToCart: addCartBoquetToCart, clearCart: clearCart, removeFromCart: removeFromCart , removeGroupe: removeGrope}}>
            {props.children}
        </CartContext.Provider>
    )

}