import { useTranslation } from 'react-i18next';
import { useTitle } from '../../hooks';
import creditCard from '../../assets/credit-card.png'
import { useForm } from 'react-hook-form';
import { AddPaymentCard, MakeOrderType, Payment, addCard, payWithNewCard, validationRules } from '../../resources';
import { Image } from '@/components/utilities';
import { Button } from '@/components/shared/Button';
import { useLocation, useNavigate } from 'react-router-dom';
import { showError } from '@/helpers/ErrorHelper';
import { useToast } from '@/components/ui/use-toast';
import { Listbox, Transition } from "@headlessui/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Fragment, useState } from 'react';

const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
const years = ['2023', '2024', '2025', '2026', '2027', '2028', '2029', '2030', '2031', '2032']

type cardType = 'None' | 'JCB' | 'American Express' | 'Diners Club' | 'Visa' | 'MasterCard' | 'Discover'

const initialCard: AddPaymentCard = {
    cardNumber: '',
    cardholderName: '',
    month: 'January',
    year: '2023',
    cardType: 'None',
    cvv: ''
}

export default function AddNewPaymentCard() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const { toast } = useToast();
    const [card, setCard] = useState<AddPaymentCard>(initialCard);
    useTitle(t('Add new payment card'));
    const location = useLocation();
    const order: MakeOrderType | undefined = location.state?.order;
    const { handleSubmit, register, formState: { errors } } = useForm<AddPaymentCard>();

    function onSelectedMonthChanged(month: string) {
        setCard(prev => { return { ...prev, month: month } });
    }

    function onSelectedYearChanged(year: string) {
        setCard(prev => { return { ...prev, year: year } });
    }

    function checkCardType(cardNumber: string): cardType {
        if (cardNumber.match(/^(?:2131|1800|35)[0-9]{0,}$/)) {
            return 'JCB';
        }

        if (cardNumber.match(/^3[47][0-9]{0,}$/)) {
            return 'American Express';
        }

        if (cardNumber.match(/^3(?:0[0-59]{1}|[689])[0-9]{0,}$/)) {
            return 'Diners Club';
        }

        if (cardNumber.match(/^4[0-9]{0,}$/)) {
            return 'Visa';
        }

        if (cardNumber.match(/^(5[1-5]|222[1-9]|22[3-9]|2[3-6]|27[01]|2720)[0-9]{0,}$/)) {
            return 'MasterCard';
        }

        if (cardNumber.match(/^(6011|65|64[4-9]|62212[6-9]|6221[3-9]|622[2-8]|6229[01]|62292[0-5])[0-9]{0,}$/)) {
            return 'Discover';
        }

        return 'None';
    }


    async function onSubmit(data: AddPaymentCard) {
        try {
            data.month = card.month;
            data.year = card.year;
            data.cardType = checkCardType(data.cardNumber);

            if (order != undefined) {
                let payment: Payment = {
                    cardId: "",
                    amount: order.price.toString(),
                    orderId: order!.id,
                    newCard: data
                }
                await payWithNewCard(payment);
                navigate('/');
            } else {
                await addCard(data);
                navigate('/payment-cards');
            }
        } catch (error) {
            showError(error, toast, t);
        }
    }

    return (
        <div className='h-full flex flex-col md:grid md:grid-cols-2 md:place-items-center items-center mx-2 md:mx-0 space-y-2 md:space-y-0'>
            <Image imageSrc={creditCard}
                imageClassName='sm:w-96 justify-self-end'
                skeletonClassName='w-96 h-60 justify-self-end' />

            <form className='space-y-2 flex flex-col w-full mt-2 md:mt-0 md:mx-0 justify-self-start md:ml-2' onSubmit={handleSubmit(onSubmit)}>
                <input id="cardNumber" type='text' {...register('cardNumber', validationRules.cardNumber)}
                    className='px-1 py-2 bg-transparent border border-black dark:border-white rounded-lg' placeholder={t('Card number')} />
                {errors.cardNumber && <span>{t(errors.cardNumber.message || '')}</span>}
                <input id="cardholderName" type='text' {...register('cardholderName', validationRules.cardholderName)}
                    className='px-1 py-2 bg-transparent border border-black dark:border-white rounded-lg' placeholder={t('Cardholder name')} />
                {errors.cardholderName && <span>{t(errors.cardholderName.message || '')}</span>}
                <div className='grid grid-cols-2 space-x-2'>
                    <div className='flex flex-col'>
                        <input id="cvv" type='text' {...register('cvv', validationRules.cardCVV)}
                            className='px-1 py-2 bg-transparent border border-black dark:border-white rounded-lg' placeholder='CVV'
                        />
                        {errors.cvv && <span>{t(errors.cvv.message || '')}</span>}
                    </div>
                    <div className="w-full flex flex-row sm:space-x-1 justify-center">
                        <div>
                            <Listbox value={card.month} onChange={onSelectedMonthChanged}>
                                {({ open }) => (
                                    <div className="relative w-36 ">
                                        <Listbox.Button className=" w-full
                                                                 text-black dark:text-white border border-black dark:border-white px-1 py-2 rounded-lg">
                                            <div className="flex w-full items-center justify-between">
                                                <span>{card.month}</span>
                                                {open == true ?
                                                    <FontAwesomeIcon icon={['fas', 'chevron-up']} /> :
                                                    <FontAwesomeIcon icon={['fas', 'chevron-down']} />}
                                            </div>
                                        </Listbox.Button>

                                        <Transition
                                            show={open}
                                            as={Fragment}
                                            enter="transition ease-out duration-200"
                                            enterFrom="opacity-0 translate-y-1"
                                            enterTo="opacity-100 translate-y-0"
                                            leave="transition ease-in duration-150"
                                            leaveFrom="opacity-100 translate-y-0"
                                            leaveTo="opacity-0 translate-y-1">
                                            <Listbox.Options className="overflow-auto h-32 absolute z-10 w-full bg-white dark:bg-main-background-dark shadow-lg rounded-lg">
                                                {months.map((value, index) => {
                                                    return (
                                                        <Listbox.Option value={value} key={index} className="w-full text-center text-black py-1 dark:text-white
                                                            hover:text-orange-500 hover:dark:text-orange-500 cursor-pointer">
                                                            {value}
                                                        </Listbox.Option>
                                                    )
                                                })}
                                            </Listbox.Options>
                                        </Transition>
                                    </div>
                                )}
                            </Listbox>
                        </div>
                        <div>
                            <Listbox value={card.year} onChange={onSelectedYearChanged}>
                                {({ open }) => (
                                    <div className="relative w-36">
                                        <Listbox.Button className="w-full 
                                                                 text-black dark:text-white border border-black dark:border-white px-1 py-2 rounded-lg">
                                            <div className="flex w-full items-center justify-between">
                                                <span>{card.year}</span>
                                                {open == true ?
                                                    <FontAwesomeIcon icon={['fas', 'chevron-up']} /> :
                                                    <FontAwesomeIcon icon={['fas', 'chevron-down']} />}
                                            </div>
                                        </Listbox.Button>

                                        <Transition
                                            show={open}
                                            as={Fragment}
                                            enter="transition ease-out duration-200"
                                            enterFrom="opacity-0 translate-y-1"
                                            enterTo="opacity-100 translate-y-0"
                                            leave="transition ease-in duration-150"
                                            leaveFrom="opacity-100 translate-y-0"
                                            leaveTo="opacity-0 translate-y-1">
                                            <Listbox.Options className="overflow-auto h-32 absolute z-10 w-full bg-white dark:bg-main-background-dark shadow-lg rounded-lg">
                                                {years.map((value, index) => {
                                                    return (
                                                        <Listbox.Option value={value} key={index} className="w-full text-center text-black py-1 dark:text-white
                                                            hover:text-orange-500 hover:dark:text-orange-500 cursor-pointer">
                                                            {value}
                                                        </Listbox.Option>
                                                    )
                                                })}
                                            </Listbox.Options>
                                        </Transition>
                                    </div>
                                )}
                            </Listbox>
                        </div>
                    </div>
                </div>
                <Button className="w-full" variant={'default'} size={'lg'} type="submit">
                    {order == undefined ? t('Save') : t('Pay')}
                </Button>
            </form>
        </div>
    )
}