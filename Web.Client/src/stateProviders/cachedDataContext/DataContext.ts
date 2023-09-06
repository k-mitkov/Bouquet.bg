import { DataContextType } from "./DataContextType";
import { createContext, useContext } from "react";

export const DataContext = createContext<DataContextType>({
    cities: [],
    flowers: [],
    colors: [],
    isCitiesLoaded: false,
    isFlowersLoaded: false,
    isColorsLoaded: false
})

export const useCities = () => useContext(DataContext);