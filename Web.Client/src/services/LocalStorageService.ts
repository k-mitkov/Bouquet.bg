import { Cart } from "@/stateProviders/cartContext";

class LocalStorageService {

    getItemFromLocalStorage(itemName: string): string | null {
        const itemValue = localStorage.getItem(itemName);
        return itemValue;
    }

    setItemInLocalStorage(itemName: string, itemValue: string): void {

        localStorage.setItem(itemName, itemValue);
    }

    removeItemFromLocalStorage(itemName: string): void {
        localStorage.removeItem(itemName);
    }

    getCartFromLocalStorage(): Cart | null {
        const cartJson = this.getItemFromLocalStorage('cart');
        if (cartJson) {
            return JSON.parse(cartJson) as Cart;
        }
        return null;
    }

    setCartInLocalStorage(cartValue: Cart): void {
        const cartJson = JSON.stringify(cartValue);
        this.setItemInLocalStorage('cart', cartJson);
    }

    removeCartFromLocalStorage(): void {
        this.removeItemFromLocalStorage('cart');
    }

}

export default new LocalStorageService();