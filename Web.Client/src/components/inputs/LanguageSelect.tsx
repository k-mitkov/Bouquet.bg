import { useTranslation } from 'react-i18next';
import { DropdownMenu, DropdownMenuContent, DropdownMenuRadioGroup, DropdownMenuRadioItem, DropdownMenuTrigger } from "../ui/dropdown-menu";
import LocalStorageService from '@/services/LocalStorageService';
import { Globe } from 'lucide-react';

export default function LanguageSelect() {
    const { t, i18n } = useTranslation();


    const handleLanguageChange = (lng: string) => {
        i18n.changeLanguage(lng);
        LocalStorageService.setItemInLocalStorage('selectedLanguage', lng);
    };
    return (
        <DropdownMenu >
            <DropdownMenuTrigger className="inline-flex items-center p-2 w-10 h-10 justify-center text-sm">
                <Globe className='text-black dark:text-white' />
            </DropdownMenuTrigger>
            <DropdownMenuContent>
                <DropdownMenuRadioGroup className="w-56" onValueChange={handleLanguageChange}>
                    <DropdownMenuRadioItem value={'en'} >English</DropdownMenuRadioItem >
                    <DropdownMenuRadioItem value={'bg'} >Български</DropdownMenuRadioItem >
                </DropdownMenuRadioGroup >
            </DropdownMenuContent>
        </DropdownMenu>
    )
}