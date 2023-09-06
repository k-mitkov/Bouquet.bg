import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { showError } from '@/helpers/ErrorHelper';
import { useNavigate } from 'react-router-dom';
import { useCart } from '@/stateProviders/cartContext';
import CartBouquets from '../bouquet/CartBouquets';
import { Spinner } from '../utilities';
import { FlowerShop, MakeOrderType, getShop } from '@/resources';
import { toast } from '../ui/use-toast';
import { Button } from '../shared/Button';
import TimeSpan from '@web-atoms/date-time/src/TimeSpan';
import React from 'react';

function useForceUpdate() {
    const [, setTick] = React.useState(0);
    const update = React.useCallback(() => {
      setTick(tick => tick + 1);
    }, []);
    return update;
  }

const Cart = () => {
    const { t } = useTranslation();
    const { cart } = useCart();
    const [isObjectLoaded, setIsObjectLoaded] = useState<boolean>(false);
    const [shops, setShops] = useState<FlowerShop[]>([]);
    const [orders, setOrders] = useState<MakeOrderType[]>([]);
    const navigate = useNavigate();
    const forceUpdate = useForceUpdate();

    const fetchObjects = async () => {
        const uniqueShopIds = [...new Set(cart.bouquets.map((b) => b.bouquet.flowerShopID))];
        let orderByShops: MakeOrderType[] = [];
        let uniqueShops: FlowerShop[] = [];
        await Promise.all(
            uniqueShopIds.map(async (shopId) => {
                try {
                    const response = await getShop(shopId);
                    const object: FlowerShop = response.data;
                    uniqueShops.push(object);

                    const bouquetsByShop = cart.bouquets.filter(b => b.bouquet.flowerShopID == shopId);

                    let newOrder: MakeOrderType = {
                        id: '',
                        shopId: shopId,
                        userId: '',
                        price: 0,
                        deliveryFee: 0,
                        reciverName: '',
                        preferredTime: TimeSpan.fromHours(0),
                        reciverPhoneNumber: '',
                        address: '',
                        description: '',
                        hasDelivery: false,
                        bouquets: bouquetsByShop,
                    };

                    bouquetsByShop.forEach(b => newOrder.price += b.count * b.bouquet.price)

                    if(newOrder.price < object.shopConfig!.freeDeliveryAt){
                        newOrder.deliveryFee = object.shopConfig!.price;
                    }

                    orderByShops.push(newOrder);

                } catch (error) {
                    showError(error, toast, t);
                }
            })
        );
        setOrders(orderByShops);
        setShops(uniqueShops);
        setIsObjectLoaded(true);
    };

    const refreshTotal = (shopId: string) =>{
        let orderToUpdate = orders.find(o => o.shopId == shopId) 
        const bouquets = orderToUpdate!.bouquets;
        let newPrice = 0;
        bouquets.forEach(b => newPrice += b.count * b.bouquet.price)

        const shop = shops.find(s => s.id == shopId);

        if(newPrice < shop!.shopConfig!.freeDeliveryAt){
            orderToUpdate!.deliveryFee =  shop!.shopConfig!.price;
        }else{
            orderToUpdate!.deliveryFee = 0;
        }

        orderToUpdate!.price = newPrice;
        forceUpdate();
    }

    useEffect(() => {
        fetchObjects();
    }, []);

    const order = (shop: FlowerShop) => {
        navigate('/order', { state: { order: orders.filter(o => o.shopId == shop.id)[0] } });
    };

    if (!isObjectLoaded) {
        return (
            <>
                <div className='flex w-full overflow-hidden items-center justify-center'>
                    <Spinner />
                </div>
            </>
        )
    }

    return (
        <>
            {shops.map(shop => (
                <div className='flex flex-col w-[100%] items-center justify-center space-y-2 my-2 p-2 bg-secondary-background dark:bg-secondary-backgroud-dark rounded-lg'>
                    <div className='flex flex-row space-x-5 justify-center text-black dark:text-gray-300'>
                        <span className='text-start font-bold'>{shop.name}</span>
                    </div>
                    <CartBouquets bouquets={orders.filter(o => o.shopId == shop.id)[0].bouquets} refreshTotal={refreshTotal}></CartBouquets>
                    <div className='flex flex-row w-full'>
                        <div className='flex justify-start w-full'>
                            <span className='text-start font-bold'>{t('Free deliver over') + ": " + shop.shopConfig?.freeDeliveryAt} {t('lv.')}</span>
                        </div>
                        <div className='flex justify-center w-full'>
                            <span className='text-start font-bold'>{t('Total') + ": " + orders.filter(o => o.shopId == shop.id)[0].price} {t('lv.')}</span>
                        </div>
                        <div className='flex justify-start w-full'>
                            <span className='text-start font-bold'>{t('Total with delivery') + ": " + (orders.filter(o => o.shopId == shop.id)[0].price + orders.filter(o => o.shopId == shop.id)[0].deliveryFee)} {t('lv.')}</span>
                        </div>
                        <div className='flex items-center justify-center w-full'>
                            <Button className="w-1/5" variant={'default'} size={'lg'} type="button" onClick={() => order(shop)}>
                                {t('MakeOrder')}
                            </Button>
                        </div>
                    </div>
                </div>
            ))}
        </>
    );
};

export default Cart;
