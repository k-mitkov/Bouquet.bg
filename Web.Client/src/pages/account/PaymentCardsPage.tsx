import { useTranslation } from 'react-i18next';
import { useTitle } from '../../hooks';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { showError } from '@/helpers/ErrorHelper';
import { useEffect, useState } from 'react';
import { Card, MakeOrderType, Payment, getCards, pay } from '@/resources';
import { useToast } from '@/components/ui/use-toast';
import React from 'react';
import { Skeleton } from '@/components/ui/skeleton';
import { Trash2Icon } from "lucide-react";
import { Button } from '@/components/shared/Button';

export default function PaymentCardsPage() {
    const { t } = useTranslation();
    const { toast } = useToast();
    const [cards, setCards] = useState<Card[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const location = useLocation();
    const navigate = useNavigate();
    const order: MakeOrderType | undefined= location.state?.order;
    useTitle(order == undefined ? t('Payment cards') : t('Select card'));


    async function fetchCards() {
        try {
            setIsLoading(true);
            var response = await getCards();
            if(response.data.length == 0 && order != undefined){
                navigate('/payment-cards/add', { state: {order: order} });
            }
            setCards(response.data);
        }
        catch (error) {
            showError(error, toast, t);
        }
        setIsLoading(false);
    }

    useEffect(() => {
        fetchCards();
    }, [])

    async function deleteCard(card: Card) {
        try {
            //
            setCards(prev => {
                var newCards = prev.filter(c => c.id != card.id);
                return newCards;
            })
        }
        catch (error) {
            showError(error, toast, t);
        }
    }

    async function MakePayment(card: Card) {
        try {
            let payment : Payment ={
                cardId: card.id,
                amount: order!.price.toString(),
                orderId: order!.id,
                newCard: undefined
            }
            await pay(payment);
            navigate('/');
        }
        catch (error) {
            showError(error, toast, t);
        }
    }

    function navigateToAddCart() {
        navigate('/payment-cards/add', { state: {order: order} });
    }

    return (
        <section className="flex md:flex-row flex-col justify-center md:justify-start md:space-x-2 md:space-y-0 space-y-2">
            <div onClick={() => navigateToAddCart()}
                className='w-72 h-40 py-16 border border-black dark:border-white dark:hover:border-black 
                 hover:bg-black hover:text-white rounded-lg shadow-lg'>
                <div className='flex flex-col'>
                    <span className='text-center'>+</span>
                    <span className='text-center'>{t('Add a new card')}</span>
                </div>
            </div>
            {isLoading ?
                <>
                    {new Array(3).fill(null).map((_, index) => {
                        return (
                            <React.Fragment key={index}>
                                <Skeleton className="w-72 h-[180px]" />
                            </React.Fragment>
                        )
                    })}
                </>
                :
                <>
                    {cards.map((card, index) => {
                        return (
                            <div key={index}>
                                <div className='flex flex-col justify-center w-72 h-40 py-16 border border-black dark:border-white dark:hover:border-black hover:bg-black hover:text-white rounded-lg shadow-lg'>
                                    <span className="text-2xl text-center">{card.last4.padStart(16, '*')}</span>
                                    {order != undefined ?
                                        <Button className="w-full" variant={'default'} size={'lg'} type="button" onClick={() => MakePayment(card)}>
                                            {t('Pay')}
                                        </Button>
                                        :
                                        <></>
                                    }
                                    <div className="flex justify-center">
                                        <Trash2Icon
                                            className="hover:text-red-600 cursor-pointer"
                                            onClick={() => deleteCard(card)} />
                                    </div>
                                </div>
                            </div>
                        )
                    })}
                </>
            }
        </section>
    )
}