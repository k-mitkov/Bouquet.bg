import { Bouquet, MakeOrderType, getShop } from '../../resources';
import { useTranslation } from 'react-i18next';
import { useToast } from '../ui/use-toast';
import { useNavigate } from 'react-router-dom';
import { Carousel } from 'react-responsive-carousel';
import { Button } from '../shared/Button';
import { useCart } from '@/stateProviders/cartContext';
import { Flower2Icon, Ruler } from "lucide-react";
import { CartBouquet } from '@/stateProviders/cartContext/CartContextType';
import TimeSpan from '@web-atoms/date-time/src/TimeSpan';
import { showError } from '@/helpers/ErrorHelper';

interface BouquetsProps {
    bouquets: Bouquet[];
}

const Bouquets: React.FC<BouquetsProps> = ({ bouquets }) => {
    const { t } = useTranslation();
    const { toast } = useToast();
    const navigate = useNavigate();
    const { addToCart } = useCart();

    const order = async (bouquet: Bouquet) => {

        try {
            const result = await getShop(bouquet.flowerShopID);

            const shop = result.data;

            const total = bouquet.price < shop.shopConfig!.freeDeliveryAt ? bouquet.price + shop.shopConfig!.price : bouquet.price;

            const cartBouquet: CartBouquet = {
                bouquet: bouquet,
                count: 1
            }

            let newOrder: MakeOrderType = {
                id: '',
                shopId: bouquet.flowerShopID,
                userId: '',
                price: total,
                deliveryFee: shop.shopConfig!.price,
                reciverName: '',
                preferredTime: TimeSpan.fromHours(0),
                reciverPhoneNumber: '',
                address: '',
                description: '',
                hasDelivery: false,
                bouquets: [cartBouquet],
            };

            navigate('/order', { state: { order: newOrder } });
        } catch (error) {
            showError(error, toast, t);
        }


    };

    return (
        <>
            {bouquets != undefined && bouquets.length > 0 ? (
                <div className='space-y-2 w-full'>
                    {bouquets.map(bouquet => (
                        <div className='flex flex-col md:flex-row border-2 md:grid-col-[20%_20%_20%_20%_20%] w-full border-green-500 rounded-lg p-2 text-black dark:text-gray-300'>
                            {bouquet.pictures.length > 0 ? (
                                <Carousel className='h-fit md:col-start-1 md:col-span-1 w-full md:w-2/5' swipeable={true} infiniteLoop={true} autoPlay={true} showThumbs={false} showArrows={true} showIndicators={false}>
                                    {bouquet.pictures?.map((picture) => (
                                        <div className='h-[25vh] md:h-[35vh]'>
                                            <img className='h-[100%] w-fit' src={import.meta.env.VITE_IDENTITY_FILE_URL + picture.pictureDataUrl} alt={bouquet.id} />
                                        </div>
                                    ))}
                                </Carousel>
                            ) : (
                                <div className='h-fit md:col-start-1 md:col-span-1  w-full md:w-2/5'>
                                    <div className='h-[20vh] md:h-[35vh]'>
                                        <Flower2Icon className='h-[100%] w-fit' />
                                        {/* <img className={`h-[100%] w-auto`} src={import.meta.env.VITE_IDENTITY_FILE_URL + 'defouth'} alt={bouquet.id} /> */}
                                    </div>
                                </div>
                            )}
                            <div className='grid md:grid-col-5 w-full'>
                                <div className='grid md:col-span-5 grid-rows-6 md:grid-rows-5 m-5 space-x-1 md:space-y-3 h-full'>
                                    <span className='row-start-1 font-bold'>{bouquet.name}</span>
                                    <span className='row-start-2 row-span-2'>{t('Description') + ': '}  {bouquet.description}</span>
                                    <div className='row-start-4 flex flex-row space-x-5'>
                                        <div className='flex flex-row'>
                                            <Flower2Icon />
                                            <span>{bouquet.flowersCount}</span>
                                        </div>
                                        <div className='flex flex-row'>
                                            <Ruler />
                                            <span>{bouquet.height}</span>
                                        </div>
                                    </div>
                                    <div className='row-start-5 flex flex-row'>
                                        <span className='font-bold'>{t('Price') + ': '} {bouquet.price} {t('lv.')}</span>
                                    </div>
                                </div>
                                <div className='flex justify-between md:justify-end w-full row-start-6 md:col-start-4 md:col-span-2 md:space-x-5'> 
                                    <Button className="md:w-50" variant={'default'} size={'lg'} type="button" onClick={() => addToCart(bouquet)}>
                                        {t('Add to cart')}
                                    </Button>
                                    <Button className="md:w-50" variant={'default'} size={'lg'} type="button" onClick={() => order(bouquet)}>
                                        {t('MakeOrder')}
                                    </Button>
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

export default Bouquets;
