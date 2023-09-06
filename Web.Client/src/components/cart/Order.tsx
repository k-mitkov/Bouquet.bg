import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { validationRules, MakeOrderType, makeOrder } from '../../resources';
import { useLocation, useNavigate } from 'react-router-dom';
import { useToast } from '../ui/use-toast';
import { Input } from '../shared/input';
import { Button } from '../shared/Button';
import validator from 'validator';
import { useUser } from '@/stateProviders/userContext';
import { showError } from '@/helpers/ErrorHelper';
import { useCart } from '@/stateProviders/cartContext';

const Order: React.FC = () => {
    const location = useLocation();
    const order: MakeOrderType = location.state?.order;
    const [amount, setAmount] = useState<number>(0)
    const [hasDelivery, setHasDelivery] = useState<boolean>(true);
    const { t } = useTranslation();
    const { clearCart, removeGroupe } = useCart();
    const { toast } = useToast()
    const { control, handleSubmit, formState: { errors } } = useForm<MakeOrderType>();
    const navigate = useNavigate();
    const { user } = useUser();

    const onSubmit = async (data: MakeOrderType) => {
        try {
            data.userId = user?.id || "";
            data.shopId = order.shopId;
            data.bouquets = order.bouquets;
            data.hasDelivery = hasDelivery;
            data.price = amount;
            const result = await makeOrder(data);
            if (hasDelivery) {
                data.price = order.price + order.deliveryFee;
                data.id = result.data;
                navigate('/payment-cards', { state: { order: data } });
            } else {
                navigate('/');
            }
            removeGroupe(order.shopId);
        } catch (error) {
            showError(error, toast, t);
        }
    };

    useEffect(() => {
        if (hasDelivery) {
            const total = order.price + order.deliveryFee;
            setAmount(total);
        } else {
            setAmount(order.price);
        }
    }, [hasDelivery]);

    return (
        <div className="flex overflow-hidden min-h-[500px] justify-center mt-5" >
            <div className='grid grid-cols-1 w-[100%] rounded-lg space-x-5 p-5 bg-secondary-background dark:bg-secondary-backgroud-dark'>
                <div className="flex flex-col min-h-[300px] h-screen md:h-auto text-black dark:text-gray-300">
                    <div className='space-y-2'>
                        <h2 className='text-center'>{t('Order')}</h2>
                        <label className="relative inline-flex items-center cursor-pointer ml-5">
                            <input type="checkbox" className="sr-only peer" checked={hasDelivery} onChange={() => setHasDelivery(!hasDelivery)} />
                            <div className="w-11 h-6 bg-gray-200 rounded-full peer peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-0.5 after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600"></div>
                            <span className="ml-3 text-sm font-medium text-gray-900 dark:text-gray-300">{hasDelivery ? t('With delivery') : t('Get it from the store')}</span>
                        </label>
                        <form className='space-y-5 space-x-5 grid grid-cols-1 md:grid-cols-2' onSubmit={handleSubmit(onSubmit)}>
                            {hasDelivery ? (
                                <>
                                    <div className='m-5 space-y-5'>
                                        <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                            <label className='text-left'>{t('City')}:</label>
                                            <label className='text-left'>{t('Sofia')}</label>
                                        </div>
                                        <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                            <label className='text-left'>{t('Address')}:</label>
                                            <Controller control={control}
                                                name='address'
                                                rules={validationRules.address}
                                                render={({ field: { onChange, onBlur } }) => (
                                                    <Input onChange={onChange} onBlur={onBlur} placeholder={t('Address')} />
                                                )} />
                                            {errors.address && <span className="error-message col-span-2">{t(errors.address.message || '')}</span>}
                                        </div>
                                        <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                            <label className='text-left'>{t('Preferred delivery hour')}:</label>
                                            <Controller control={control}
                                                name='preferredTime'
                                                rules={validationRules.hour}
                                                render={({ field: { onChange, onBlur } }) => (
                                                    <Input onChange={onChange} onBlur={onBlur} placeholder={t('Preferred delivery hour')} />
                                                )} />
                                            {errors.preferredTime && <span className="error-message col-span-2">{t(errors.preferredTime.message || '')}</span>}
                                        </div>
                                    </div>
                                    <div className='m-5 space-y-5'>
                                        <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                            <label className='text-left'>{t('Reciver name')}:</label>
                                            <Controller control={control}
                                                name='reciverName'
                                                rules={validationRules.firstName}
                                                render={({ field: { onChange, onBlur } }) => (
                                                    <Input onChange={onChange} onBlur={onBlur} placeholder={t('Reciver name')} />
                                                )} />
                                            {errors.reciverName && <span className="error-message col-span-2">{t(errors.reciverName.message || '')}</span>}
                                        </div>
                                        <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                            <label className='text-left'>{t('Reciver phone number')}:</label>
                                            <Controller control={control}
                                                name='reciverPhoneNumber'
                                                rules={{ ...validationRules.phoneNumber, validate: (value) => validationRules.phoneNumber.validate(validator.isMobilePhone(value, 'bg-BG')) }}
                                                render={({ field: { onChange, onBlur } }) => (
                                                    <Input onChange={onChange} onBlur={onBlur} placeholder={t('Reciver phone number')} />
                                                )} />
                                            {errors.reciverPhoneNumber && <span className="error-message col-span-2">{t(errors.reciverPhoneNumber.message || '')}</span>}
                                        </div>
                                        <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                            <label className='text-left'>{t('Description')}:</label>
                                            <Controller control={control}
                                                name='description'
                                                rules={validationRules.description}
                                                render={({ field: { onChange, onBlur } }) => (
                                                    <Input onChange={onChange} onBlur={onBlur} placeholder={t('Description')} />
                                                )} />
                                            {errors.description && <span className="error-message col-span-2">{t(errors.description.message || '')}</span>}
                                        </div>
                                    </div>
                                </>
                            ) : (
                                <></>
                            )
                            }
                            <div className='space-x-5'>
                                {hasDelivery ? (
                                    <span className='text-start font-bold'>{t('Delivery fee') + ": " + order.deliveryFee}</span>
                                ) : (
                                    <></>
                                )
                                }
                                <span className='text-start font-bold'>{t('Total') + ": " + amount}</span>
                                <Button className='m-5 w-full' type="submit">{t('MakeOrder')}</Button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div >
    );
};

export default Order