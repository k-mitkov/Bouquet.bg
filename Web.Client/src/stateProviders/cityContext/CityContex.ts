import { createContext, useContext } from "react";
import { CityContextType } from "./CityContextType";
import { City } from "@/resources";
import { locationLokeren } from "@/resources/mapConfig";

const defaultCity: City = {
    id: "",
    name: "",
    latitude: 1,
    longitude: 1,
}

export const CityContext = createContext<CityContextType>({
    city: defaultCity,
    location: locationLokeren,
    isCityLoaded: false,
    isLocationLoaded: false,
})

export const useCity = () => useContext(CityContext);