import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from "react-router-dom";

export default function ProfileTabs() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const location = useLocation();

    const [defaultValue, setDefaultValue] = useState<string>('');

    useEffect(() => {
        setDefaultValue(location.pathname.split('/')[1]);
    }, [])

    if (defaultValue == '') {
        return (
            <>
            </>
        )
    }

    return (
        <Tabs defaultValue={defaultValue} className="">
            <TabsList className="flex-wrap md:grid md:grid-cols-5 h-fit">
                <TabsTrigger value="profile" className="w-full"
                    onClick={() => navigate('/profile')}>
                    {t('Profile')}
                </TabsTrigger>
                <TabsTrigger value="edit-profile" className="w-full"
                    onClick={() => navigate('/edit-profile')}>
                    {t('Edit profile')}
                </TabsTrigger>
                <TabsTrigger value="payment-cards" className="w-full"
                    onClick={() => navigate('/payment-cards')}>
                    {t('Payment cards')}
                </TabsTrigger>
                <TabsTrigger value="change-password" className="w-full"
                    onClick={() => navigate('/change-password')}>
                    {t('Change password')}
                </TabsTrigger>
                <TabsTrigger value="orders-history" className="w-full"
                    onClick={() => navigate('/orders-history')}>
                    {t('Orders history')}
                </TabsTrigger>
            </TabsList>
        </Tabs>
    )
}