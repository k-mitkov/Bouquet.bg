import { useEffect, useState } from "react";
import { ThemeType } from ".";
import { SettingsContext } from ".";
import LocalStorageService from "@/services/LocalStorageService";


export default function SettingsProvider(props: any) {
    const [theme, setTheme] = useState<ThemeType>('system');

    useEffect(() => {
        const theme = (LocalStorageService.getItemFromLocalStorage('theme') as ThemeType) ?? 'system';
        setApplicationTheme(theme as ThemeType);
    }, [])

    function setApplicationTheme(themeType: ThemeType): void {

        if (themeType === 'system') {
            if (window.matchMedia('(prefers-color-scheme:dark)').matches) {
                saveTheme('dark')
            }
            else {
                saveTheme('light')
            }
        }

        if (themeType === 'dark') {
            saveTheme('dark');
        }
        if (themeType === 'light') {
            saveTheme('light');
        }

    }

    function saveTheme(theme: ThemeType) {
        if (theme === 'dark') {
            document.documentElement.classList.add('dark');
            LocalStorageService.setItemInLocalStorage('theme', theme);
            setTheme(theme);
        }
        if (theme === 'light') {
            document.documentElement.classList.remove('dark');
            LocalStorageService.setItemInLocalStorage('theme', theme);
            setTheme(theme);
        }
    }

    return (
        <SettingsContext.Provider value={{ theme: theme, setApplicationTheme: setApplicationTheme }}>
            {props.children}
        </SettingsContext.Provider>
    )

}