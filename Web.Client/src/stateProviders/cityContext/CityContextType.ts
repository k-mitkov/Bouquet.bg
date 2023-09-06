import { City } from "@/resources";
import { Location } from "@/types/MapTypes";

export interface CityContextType {
    city: City,
    location: Location,
    isCityLoaded: boolean,
    isLocationLoaded: boolean,
}