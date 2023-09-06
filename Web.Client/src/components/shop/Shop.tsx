import { Bouquet, Claims, FlowerShop, Response, getBouquetsByShop } from '../../resources';
import { useLocation, useNavigate } from 'react-router-dom';
import Map from '../map/Map';
import "react-responsive-carousel/lib/styles/carousel.min.css"; // requires a loader
import { useEffect, useState } from 'react';
import { showError } from '@/helpers/ErrorHelper';
import { useTranslation } from 'react-i18next';
import { useToast } from '../ui/use-toast';
import Bouquets from '../bouquet/Bouquets';
import { useUser } from '@/stateProviders/userContext';
import { Button } from '../shared/Button';
import Workers from '../profile/Workers';
import Orders from '../cart/Orders';

const Shop = () => {
  const location = useLocation();
  const shop: FlowerShop = location.state?.sObject;
  const [bouquets, setBouquets] = useState<Bouquet[]>([]);
  const [showWorkers, SetShowWorkers] = useState<boolean>(false);
  const [showOrders, SetShowOrders] = useState<boolean>(false);
  const { t } = useTranslation();
  const { toast } = useToast();
  const { user, isAuthenticated } = useUser();
  const navigate = useNavigate();

  const hasAddBouquetClaim: boolean = (user?.claims !== undefined) && user?.claims.includes(Claims.PERMISSION_BOUQUET_ADD) && (user.id == shop.ownerId || shop.workers.find(id => id == user.id) != undefined);
  const hasAddWorkerClaim: boolean = (user?.claims !== undefined) && user?.claims.includes(Claims.PERMISSION_WORKER_ADD) && (user.id == shop.ownerId);

  const fetchBouquets = async () => {
    try {
      let response: Response<Bouquet[]>;
      response = await getBouquetsByShop(shop.id);
      const bouquets: Bouquet[] = response.data;
      setBouquets(bouquets); // Update the state with the fetched stationsObjects
    } catch (error) {
      showError(error, toast, t);
      setBouquets([]);
    }
  };

  useEffect(() => {
    fetchBouquets();
  }, []);

  if (!shop) {
    // Handle case when object is not available in state
    return null;
  }

  function handleAddBouquetClick(): void {
    const shopID = shop.id;
    navigate('/add-bouquet', { state: { shopID } });
  }

  function changeShownElements(type: number): void {
    switch (type) {
      case 1:
        SetShowWorkers(false);
        SetShowOrders(false);
        return;
      case 2:
        SetShowWorkers(true);
        SetShowOrders(false);
        return;
      case 3:
        SetShowWorkers(false);
        SetShowOrders(true);
        return;
      default:
        SetShowWorkers(false);
        SetShowOrders(false);
        return;
    }
  }

  // Render object details and map
  return (
    <div className="flex justify-center mt-5">
      <div className='h-[50vh] w-screen grid grid-cols-1 md:grid-cols-3'>
        <div className='flex flex-col p-5 bg-secondary-background dark:bg-secondary-backgroud-dark
             min-h-[500px] h-auto rounded-lg text-black dark:text-gray-300'>
          <h2 className='text-black dark:text-gray-300 text-center'>{shop.name}</h2>
          <img className={`h-[35vh] w-auto`} src={import.meta.env.VITE_IDENTITY_FILE_URL + shop.pictureDataUrl} alt={shop.name} />
          <div>
            <ObjectRow label={t('City')} value={t(shop.city == "default" ? "Unknown" : shop.city)} />
            <ObjectRow label={t('Address')} value={shop.address} />
            <ObjectRow label={t('Open at')} value={shop.shopConfig!.openAt.toString()} />
            <ObjectRow label={t('Close at')} value={shop.shopConfig!.closeAt.toString()} />
            <ObjectRow label={t('Same day delivery till')} value={shop.shopConfig!.sameDayTillHour.toString()} />
            <ObjectRow label={t('Free shipping over')} value={shop.shopConfig!.freeDeliveryAt.toString()} />
            <ObjectRow label={t('Delivery price')} value={shop.shopConfig!.price.toString()} />
          </div>
        </div>
        <div className="col-start-2 col-span-2 hidden md:block ml-5">
          <Map flowerShops={[shop]} height={'100%'} width={'100%'} />
        </div>
        <div className='md:col-span-3 flex flex-col w-[100%] h-auto items-center justify-center space-y-2 my-2 p-2 bg-secondary-background dark:bg-secondary-backgroud-dark rounded-lg'>
          <div className='flex flex-row space-x-5'>
            {isAuthenticated() && hasAddBouquetClaim &&
              <div className='space-x-5'>
                <Button type="submit" onClick={() => handleAddBouquetClick()}>{t('Add Bouquet')}</Button>
                <Button type="submit" onClick={() => changeShownElements(1)}>{t('Bouquets')}</Button>
                <Button type="submit" onClick={() => changeShownElements(3)}>{t('Orders')}</Button>
              </div>
            }
            {isAuthenticated() && hasAddWorkerClaim &&
              <div className='space-x-5'>
                <Button type="submit" onClick={() => changeShownElements(2)}>{t('Workers')}</Button>
                <Button type="submit" onClick={() => changeShownElements(2)}>{t('Edit shop')}</Button>
              </div>
            }
          </div>
          {showWorkers ? (
            <Workers shopId={shop.id}></Workers>
          ) : showOrders ? (
            <Orders shopID={shop.id}></Orders>
          ) : (
            <Bouquets bouquets={bouquets}></Bouquets>
          )}
        </div>
      </div>
    </div>
  );
};

interface ObjectRowProps {
  label: string,
  value: string
  hidden?: boolean
}

function ObjectRow({ label, value, hidden }: ObjectRowProps) {

  return (
    <>
      {!hidden &&
        <div className='flex flex-rol justify-between w-full'>
          <span className='w-fit'>{label} :</span>
          <span className=' text-right'>{value}</span>
        </div>
      }
    </>
  )
}

export default Shop;
