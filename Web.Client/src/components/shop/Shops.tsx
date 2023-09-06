import { useEffect, useState } from 'react';
import { useTitle } from '../../hooks';
import Map from '../map/Map';
import { FlowerShop, getShops, Response, Claims, City, getMyShops } from '../../resources';
import { useTranslation } from 'react-i18next';
import { useToast } from '../ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';
import { useUser } from '../../stateProviders/userContext';
import { Button } from '../shared/Button';
import { useNavigate } from 'react-router-dom';
import { useCities } from '@/stateProviders/cachedDataContext/DataContext';
import CitySelect from '../inputs/CitySelect';
import FlowerShopSelect from '../inputs/FlowerShopSelect';
import { useCity } from '@/stateProviders/cityContext';

const Shops = () => {
    const [shops, setShops] = useState<FlowerShop[]>([]);
    const { t } = useTranslation();
    const { toast } = useToast();
    const { user, isAuthenticated } = useUser();
    const { cities } = useCities();
    const { city } = useCity();
    const [selectedCity, setSelectedCity] = useState<City | undefined>(city);
    const [selectedShop, setSelectedShop] = useState<FlowerShop | undefined>(undefined);
    const navigate = useNavigate();
    useTitle(t('Shops'));

    const hasAddObjectClaim: boolean = (user?.claims !== undefined) && user?.claims.includes(Claims.PERMISSION_SHOP_ADD);

    const fetchFlowerShops = async (ownedShops: boolean) => {
        try {
            let response: Response<FlowerShop[]>;
            if (ownedShops) {
                response = await getMyShops();
            } else {
                response = await getShops();
            }
            const objects: FlowerShop[] = response.data;
            setShops(objects); // Update the state with the fetched stationsObjects
        } catch (error) {
            showError(error, toast, t);
        }
    };

    useEffect(() => {
        fetchFlowerShops(false);
    }, []);

    function handleAllShopsClick(): void {
        fetchFlowerShops(false);
    }

    function handleMyShopsClick(): void {
        fetchFlowerShops(true);
    }

    function handleAddShopClick(): void {
        navigate('/add-flower-shop');
    }

    function handleCitySelect(cityID: string) {
        if (cityID != undefined) {
            const foundCity = cities.find((c) => c.id === cityID)
            setSelectedCity(foundCity)
        } else {
            setSelectedCity(undefined)
        }
    }

    function handleShopSelect(shopID: string) {
        if (shopID != undefined) {
            const foundShop = shops.find((s) => s.id === shopID)
            setSelectedShop(foundShop)
        } else {
            setSelectedShop(undefined)
        }
    }

    return (
        <>
            <div className='flex flex-col items-center justify-center space-y-2 my-2 p-2 bg-secondary-background dark:bg-secondary-backgroud-dark rounded-lg'>
                <div className='flex flex-col space-y-1 md:space-y-0 md:flex-row md:space-x-5 justify-center text-black dark:text-gray-300'>
                    <span className='text-start font-bold'>{(shops != undefined ? shops.length : '?') + ' ' + t('flower shops throughout the country')}</span>
                    <CitySelect
                    defaultCity={city}
                        cities={cities}
                        onChange={(event) => {
                            handleCitySelect(event as string);
                        }}
                        className='z-20'></CitySelect>
                    <FlowerShopSelect
                        shops={shops}
                        onChange={(event) => {
                            handleShopSelect(event as string);
                        }}
                        className='z-10'></FlowerShopSelect>
                    {isAuthenticated() && hasAddObjectClaim &&
                        <div className='space-x-2 md:space-x-5'>
                            <Button type="submit" onClick={() => handleAllShopsClick()}>{t('All shops')}</Button>
                            <Button type="submit" onClick={() => handleMyShopsClick()}>{t('My shops')}</Button>
                            <Button type="submit" onClick={() => handleAddShopClick()}>{t('Add Shop')}</Button>
                        </div>
                    }
                </div>
                {shops != undefined && shops.length > 0 && (
                    <Map flowerShops={selectedShop == undefined ? shops : [selectedShop]} height={'80vh'} width={'100%'} selectedCity={selectedCity} />
                )}
            </div>

        </>
    );
};

export default Shops;

