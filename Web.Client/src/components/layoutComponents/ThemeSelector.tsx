import { ThemeType, useSettings } from "@/stateProviders/settingsContext";
import { DropdownMenu, DropdownMenuContent, DropdownMenuRadioGroup, DropdownMenuRadioItem, DropdownMenuTrigger } from "../ui/dropdown-menu";
import { Palette } from "lucide-react";
import { useTranslation } from 'react-i18next';

export default function ThemeSelector() {
    const { setApplicationTheme } = useSettings()
    const { t } = useTranslation();

    const handleThemeChange = (theme: string) => {
        setApplicationTheme(theme as ThemeType);
    }

    return (
        <DropdownMenu  >
            <DropdownMenuTrigger className="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-black rounded-lg  hover:bg-gray-100 focus:outline-none focus:ring-2  dark:text-white dark:hover:bg-gray-700 dark:focus:ring-gray-600">
                <Palette />
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-56">
                <DropdownMenuRadioGroup className="w-56" onValueChange={handleThemeChange}>
                    <DropdownMenuRadioItem value={'dark'} >{t('Dark')}</DropdownMenuRadioItem >
                    <DropdownMenuRadioItem value={'light'} >{t('Light')}</DropdownMenuRadioItem >
                    <DropdownMenuRadioItem value={'system'} >{t('System')}</DropdownMenuRadioItem >
                </DropdownMenuRadioGroup >
            </DropdownMenuContent>
        </DropdownMenu>
    )
}
