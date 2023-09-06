import { useTranslation } from 'react-i18next';
import { useTitle } from '../../hooks';
import { useEffect, useState } from 'react';
import { MakeOrderType, OrderType, getOrdersByUser } from '@/resources';
import { useUser } from '@/stateProviders/userContext';
import { useToast } from '@/components/ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';
import { Button } from '@/components/shared/Button';
import { Carousel } from 'react-responsive-carousel';
import { Flower2Icon } from 'lucide-react';
import TimeSpan from '@web-atoms/date-time/src/TimeSpan';
import { useNavigate } from 'react-router-dom';

export default function OrdersHistoryPage() {
    const { t } = useTranslation();
    useTitle(t('Charge history'));
    const { user } = useUser();
    const { toast } = useToast();
    const [orders, setOrders] = useState<OrderType[]>([]);
    const navigate = useNavigate();

    const fetchOrders = async () => {
        try {
            const response = await getOrdersByUser(user!.id);

            setOrders(response.data)
        } catch (error) {
            showError(error, toast, t);
        }
    };

    useEffect(() => {
        fetchOrders();
    }, []);

    const pay = (order: OrderType) => {
        let makdeOrder: MakeOrderType = {
            id: order.id,
            shopId: "",
            userId: '',
            description: "",
            price: order.price,
            deliveryFee: 0,
            reciverName: '',
            preferredTime: TimeSpan.fromHours(0),
            reciverPhoneNumber: '',
            address: '',
            hasDelivery: false,
            bouquets: [],
        };
        navigate('/payment-cards', { state: { order: makdeOrder } });
    };

    function getStatusString(value: number): string {
        switch (value) {
            case 0:
                return "Pending payment";
            case 1:
                return "Paid/Received";
            case 2:
                return "Received";
            case 3:
                return "Accepted";
            case 4:
                return "Completed";
            default:
                return "Unknown";
        }
    }

    return (
        <>
            {orders.filter(o => o.status == 0).map(order => (
                <div className='flex flex-col w-[100%] border-2 border-green-500 rounded-lg items-center justify-center space-y-2 my-2 p-2 bg-secondary-background dark:bg-secondary-backgroud-dark'>
                    <div className='flex flex-row space-x-5 justify-start text-black dark:text-gray-300'>
                        <span className='text-start font-bold'>{t('Order') + ": " + order.id}</span>
                    </div>
                    {order.bouquets.map(bouquet => (
                        <div className='flex flex-row border-2 border-green-500 rounded-lg p-2 w-full text-black dark:text-gray-300'>
                            <div className='grid grid-col-[20%_20%_20%_20%_20%] grid-rows-2 m-2 w-full'>
                                <div className='row-span-2 md:row-span-3 col-start-1 col-span-1 w-16'>
                                    {bouquet.bouquet.pictures.length > 0 ? (
                                        <Carousel className='w-fit' swipeable={true} infiniteLoop={true} autoPlay={true} showThumbs={false} showArrows={false} showStatus={false} showIndicators={false}>
                                            {bouquet.bouquet.pictures?.map((picture) => (
                                                <div className='h-[7vh]'>
                                                    <img className={`h-[100%] w-fit`} src={import.meta.env.VITE_IDENTITY_FILE_URL + picture.pictureDataUrl} alt={bouquet.bouquet.id} />
                                                </div>
                                            ))}
                                        </Carousel>
                                    ) : (
                                        <div className='h-fit w-12'>
                                            <div className='h-[5vh]'>
                                                <Flower2Icon className='h-[100%] w-fit' />
                                            </div>
                                        </div>
                                    )}
                                </div>
                                <span className='row-start-1 col-start-2 col-span-4 font-bold'>{bouquet.bouquet.name}</span>
                                <div className='row-start-2 col-start-1 md:col-start-2 col-span-4 flex flex-row'>
                                    <span className='font-bold'>{t('Price') + ': '} {bouquet.bouquet.price} {t('lv.')}</span>
                                </div>
                                <div className='row-start-2  col-start-2  md:col-start-3 col-span-3 flex flex-row'>
                                    <span className='font-bold'>{t('Count') + ': ' + bouquet.count}</span>
                                </div>
                                <div className='row-start-2 col-start-5 md:col-start-5 md:col-span-2 col-span-5 flex flex-row'>
                                    <span className='font-bold'>{t('Total') + ': '} {bouquet.bouquet.price * bouquet.count} {t('lv.')}</span>
                                </div>
                            </div>
                        </div>
                    ))}
                    <div className='flex flex-row w-full'>
                        <div className='flex justify-center w-full'>
                            <span className='text-start font-bold'>{t('Total') + ": " + order.price} {t('lv.')}</span>
                        </div>
                        <div className='flex justify-center w-full'>
                            <span className='text-start font-bold'>{t('Status') + ": " + getStatusString(order.status)}</span>
                        </div>
                        {order.status == 0 ?
                            (
                                <div className='flex items-center justify-center w-full'>
                                    <Button className="w-1/5" variant={'default'} size={'lg'} type="button" onClick={() => pay(order)}>
                                        {t('Pay')}
                                    </Button>
                                </div>
                            ) :
                            (
                                <></>
                            )
                        }
                    </div>
                </div>
            ))}
        </>
    )
}