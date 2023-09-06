import { createContext, useContext } from "react";
import { SettingsContextType } from ".";



export const SettingsContext = createContext<SettingsContextType>({
    theme: 'light',
    setApplicationTheme: () => console.log('no theme set')
})

export const useSettings = () => useContext(SettingsContext);