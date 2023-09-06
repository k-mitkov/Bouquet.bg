import UserProvider from "./userContext/UserProvider";
import { I18nextProvider } from 'react-i18next';
import { i18n } from '../resources';
import { SettingsProvider } from "./settingsContext";
import { Toaster } from "@/components/ui/toaster"
import DataProvider from "./cachedDataContext/DataProvider";
import CityProvider from "./cityContext/CityProvider";
import { CartProvider } from "./cartContext";


export default function ContextProvider(props: any) {

    return (
        <UserProvider>
            <I18nextProvider i18n={i18n}>
                <SettingsProvider>
                    <DataProvider>
                        <CityProvider>
                            <CartProvider>
                                {props.children}
                            </CartProvider>
                        </CityProvider>
                    </DataProvider>
                    <Toaster />
                </SettingsProvider>
            </I18nextProvider>
        </UserProvider>
    )
}