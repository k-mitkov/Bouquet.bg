import TimeSpan from "@web-atoms/date-time/src/TimeSpan";
import { CartBouquet } from "@/stateProviders/cartContext/CartContextType";

export interface MakeOrderType {
    id: string;
    price: number;
    deliveryFee: number;
    shopId: string;
    userId: string;
    reciverName: string;
    reciverPhoneNumber: string;
    hasDelivery: boolean;
    address: string;
    description: string
    preferredTime: TimeSpan;
    bouquets: CartBouquet[];
}

export interface OrderType {
    id: string;
    price: number;
    description: string;
    userId: string;
    name: string;
    phoneNumber: string;
    reciverName: string;
    reciverPhoneNumber: string;
    status: number;
    type: number;
    address: string;
    preferredTime: TimeSpan;
    bouquets: CartBouquet[];
}