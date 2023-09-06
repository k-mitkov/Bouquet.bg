import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import LocalStorageService from '../services/LocalStorageService';
import { useUser } from '../stateProviders/userContext';
import ProfileAvatar from './profile/ProfileAvatar';
import HamburgerMenu from './mobileComponents/HamburgerMenu';
import ThemeSelector from './layoutComponents/ThemeSelector';
import LanguageSelect from './inputs/LanguageSelect';
import { useCart } from '@/stateProviders/cartContext';
import { Flower2Icon } from "lucide-react";
import bgFlowersImage from '../resources/images/logo.png';

const Navigation: React.FC = () => {
  const { isAuthenticated } = useUser();
  const { t, i18n } = useTranslation();
  const { bouquetsCount } = useCart();


  useEffect(() => {
    const selectedLanguage = LocalStorageService.getItemFromLocalStorage('selectedLanguage');
    if (selectedLanguage) {
      i18n.changeLanguage(selectedLanguage);
    }
  }, [i18n]);

  return (
    <nav className="bg-white border-gray-200 dark:bg-green-950">
      <div className='max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-2'>
        <Link to="/" className="flex items-center">
          <img src={bgFlowersImage} className=" h-12 mr-3" alt="Flowbite Logo" />
          <span className="self-center text-2xl font-semibold whitespace-nowrap dark:text-main-font-dark  text-main-font">Bouquet.bg</span>
        </Link>
        <HamburgerMenu />
        <div className="items-center justify-between hidden w-full md:flex md:w-auto md:order-1" id="navbar-user">
          <ul className="flex flex-col font-medium p-4 md:p-0 mt-4 border border-gray-100 rounded-lg bg-gray-50 md:flex-row md:space-x-8 md:mt-0 md:border-0 md:bg-white dark:bg-green-950 dark:border-gray-700">
            <li >
              <Link to="/" className="block py-2 pl-3 pr-4 mt-2 text-main-font rounded md:bg-transparent md:text-main-font md:p-0 md:dark:text-main-font-dark">
                {t('Home')}
              </Link>
            </li>
            <li >
              <Link className="block py-2 pl-3 pr-4 mt-2 text-main-font rounded md:bg-transparent md:text-main-font md:p-0 md:dark:text-main-font-dark" to="/shops">
                {t('Shops')}
              </Link>
            </li>
            <LanguageSelect />
            <ThemeSelector />
            <li>
              {bouquetsCount > 0 ? (
                <Link className='block py-2 pl-3 pr-4 mt-2 text-main-font rounded md:bg-transparent md:text-main-font md:p-0 md:dark:text-main-font-dark' to="/cart">
                  <Flower2Icon />
                  <div className="text-center transform translate-x-1/2 -translate-y-2/3 md:text-main-font md:p-0 md:dark:text-main-font-dark font-bold bg-red-600 rounded-xl">
                    {bouquetsCount}
                  </div>
                </Link>
              ) : (
                <div className='block py-2 pl-3 pr-4 mt-2 text-main-font rounded md:bg-transparent md:text-main-font md:p-0 md:dark:text-main-font-dark'>
                  <Flower2Icon />
                  <div className="text-center transform translate-x-1/2 -translate-y-2/3 md:text-main-font md:p-0 md:dark:text-main-font-dark font-bold bg-red-600 rounded-xl">
                    {bouquetsCount}
                  </div>
                </div>
              )}
            </li>
            {isAuthenticated() ? (
              <li className="ml-auto">
                <ProfileAvatar />
              </li>
            ) : (
              <li>
                <Link className="block py-2 pl-3 pr-4 mt-2 text-main-font rounded md:bg-transparent md:text-main-font md:p-0 md:dark:text-main-font-dark" to="/login">
                  {t('Login')}
                </Link>
              </li>
            )}
          </ul>
        </div>

      </div>
    </nav >
  );
};

export default Navigation;
