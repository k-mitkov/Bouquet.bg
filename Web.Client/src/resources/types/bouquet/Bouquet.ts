import Picture from "../Picture";
import Color from "./Color";
import Flower from "./Flower";

export default interface Bouquet {
    id: string;
    name: string;
    height: number;
    width: number;
    flowersCount: number;
    description: string;
    price: number;
    flowerShopID: string;
    pictures: Picture [];
    flowers: Flower[];
    colors: Color[];
}