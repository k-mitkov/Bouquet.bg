import { City, Color, Flower } from "@/resources";

export interface DataContextType {
    cities: City[],
    flowers: Flower[],
    colors: Color[],
    isCitiesLoaded: boolean,
    isFlowersLoaded: boolean,
    isColorsLoaded: boolean,
}