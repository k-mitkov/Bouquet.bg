import ShopConfig from "./ShopConfig";

export default interface FlowerShop {
    id: string;
    name: string;
    latitude: number;
    longitude: number;
    address: string;
    pictureDataUrl: string;
    cityID: string;
    ownerId: string;
    city: string;
    workers: string[];
    shopConfig?: ShopConfig;
}