import { useTranslation } from 'react-i18next';
import { useToast } from '../ui/use-toast';
import { useNavigate } from 'react-router-dom';
import { Carousel } from 'react-responsive-carousel';
import { useCart } from '@/stateProviders/cartContext';
import { CartBouquet } from '@/stateProviders/cartContext/CartContextType';
import { Flower2Icon, Ruler } from 'lucide-react';
import React from 'react';

interface CartBouquetsProps {
    bouquets: CartBouquet[];
    refreshTotal: (shopId: string) => void;
}

function useForceUpdate() {
    const [, setTick] = React.useState(0);
    const update = React.useCallback(() => {
      setTick(tick => tick + 1);
    }, []);
    return update;
  }

const CartBouquets: React.FC<CartBouquetsProps> = ({ bouquets, refreshTotal }) => {
    const { t } = useTranslation();
    const { toast } = useToast();
    const navigate = useNavigate();
    const { addCartBoquetToCart, removeFromCart } = useCart();
    const forceUpdate = useForceUpdate();

    const addQtty = (bouquet: CartBouquet) => {
        addCartBoquetToCart(bouquet);
        forceUpdate();
        refreshTotal(bouquet.bouquet.flowerShopID);
    };

    const reduceQtty = (bouquet: CartBouquet) => {
        if(bouquet.count > 0){
            if(bouquet.count == 1){
                bouquet.count = 0;
            }
            removeFromCart(bouquet.bouquet)
            forceUpdate();
            refreshTotal(bouquet.bouquet.flowerShopID);
        }
    };

    return (
        <>
            {bouquets != undefined && bouquets.length > 0 ? (
                <div className='space-y-2 w-full'>
                    {bouquets.map(bouquet => (
                        <div className='flex flex-row border-2 border-green-500 rounded-lg p-2  text-black dark:text-gray-300'>
                            <div className='grid grid-col-[20%_20%_20%_20%_20%] grid-rows-3 m-2 w-full'>
                                <div className='row-span-2 md:row-span-3 col-start-1 col-span-1 w-32'>
                                    {bouquet.bouquet.pictures.length > 0 ? (
                                        <Carousel className='w-fit' swipeable={true} infiniteLoop={true} autoPlay={true} showThumbs={false} showArrows={false} showIndicators={false}>
                                            {bouquet.bouquet.pictures?.map((picture) => (
                                                <div className='h-[15vh]'>
                                                    <img className={`h-[100%] w-fit`} src={import.meta.env.VITE_IDENTITY_FILE_URL + picture.pictureDataUrl} alt={bouquet.bouquet.id} />
                                                </div>
                                            ))}
                                        </Carousel>
                                    ) : (
                                        <div className='h-fit w-32'>
                                            <div className='h-[15vh]'>
                                                <Flower2Icon className='h-[100%] w-fit' />
                                                {/* <img className={`h-[100%] w-auto`} src={import.meta.env.VITE_IDENTITY_FILE_URL + 'defouth'} alt={bouquet.id} /> */}
                                            </div>
                                        </div>
                                    )}
                                </div>
                                <span className='row-start-1 col-start-2 col-span-4 font-bold'>{bouquet.bouquet.name}</span>
                                <div className='row-start-2 col-start-2 col-span-4 flex flex-row space-x-5'>
                                    <div className='flex flex-row'>
                                        <Flower2Icon />
                                        <span>{bouquet.bouquet.flowersCount}</span>
                                    </div>
                                    <div className='flex flex-row'>
                                        <Ruler />
                                        <span>{bouquet.bouquet.height}</span>
                                    </div>
                                </div>
                                <div className='row-start-3 col-start-1 md:col-start-2 col-span-4 flex flex-row'>
                                    <span className='font-bold'>{t('Price') + ': '} {bouquet.bouquet.price} {t('lv.')}</span>
                                </div>
                                <div className='row-start-3  col-start-2  md:col-start-3 col-span-3 flex flex-row'>
                                    <span className='font-bold'>{t('Count') + ': '}</span>
                                    <button className="rounded-lg h-5 w-5" type="button" onClick={() => reduceQtty(bouquet)} >
                                        -
                                    </button>
                                    <span className='font-bold'> {bouquet.count}</span>
                                    <button className="rounded-lg h-5 w-5" type="button" onClick={() => addQtty(bouquet)}>
                                        +
                                    </button>
                                </div>
                                <div className='row-start-3 col-start-5 md:col-start-5 md:col-span-2 col-span-5 flex flex-row'>
                                    <span className='font-bold'>{t('Total') + ': '} {bouquet.bouquet.price * bouquet.count} {t('lv.')}</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <div>{t('No bouquets available.')}</div>
            )}
        </>
    );
};

export default CartBouquets;
