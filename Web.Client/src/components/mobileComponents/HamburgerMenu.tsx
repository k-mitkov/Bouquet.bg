import { useTranslation } from 'react-i18next';
import { DropdownMenu, DropdownMenuContent, DropdownMenuTrigger } from "../ui/dropdown-menu";
import { Link } from "react-router-dom";
import { useUser } from "@/stateProviders/userContext";
import { Claims } from '../../resources';

export interface Claims {
    hasAddStationClaim: boolean
}

export default function HamburgerMenu() {
    const { t } = useTranslation();
    const { user, isAuthenticated, logout } = useUser();

    return (
        <DropdownMenu >
            <DropdownMenuTrigger className="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600">
                <svg className="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 17 14">
                    <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M1 1h15M1 7h15M1 13h15" />
                </svg>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-56">
                <Link to="/" className="block py-2 pl-3 pr-4  rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500">
                    {t('Home')}
                </Link>
                <Link to="/shops" className="block py-2 pl-3 pr-4  rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500">
                    {t('Shops')}
                </Link>
                {isAuthenticated() ? (
                    <>
                    <Link to="/profile" className="block py-2 pl-3 pr-4  rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500">
                        {t('Profile')}
                    </Link>
                    <button
                            className="text-black dark:text-white py-2 pl-3 pr-4 bg-transparent border-none border-0 
                                        rounded hover:bg-custom-gray hover:text-blue-600 dark:hover:text-blue-600"
                            onClick={() => logout()}>
                            {t('Logout')}
                        </button>
                    </>
                ) : (
                    <Link className="block py-2 pl-3 pr-4  rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500" to="/login">
                        {t('Login')}
                    </Link>

                )}
            </DropdownMenuContent>
        </DropdownMenu>
    )
}