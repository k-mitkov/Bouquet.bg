import { useTranslation } from 'react-i18next';
import { useTitle } from '../../hooks';
import { useEffect, useState } from 'react';
import { axiosAuthClient, Response, UserInfo } from '../../resources';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { useUser } from '@/stateProviders/userContext';
import { showError } from '@/helpers/ErrorHelper';
import { useToast } from '@/components/ui/use-toast';

const initUserInfo: UserInfo = {
    firstName: '',
    lastName: '',
    uniqueNumber: '',
    country: '',
    city: '',
    address: '',
    phoneNumber: '',
    taxId: '',
    vatId: '',
    company: '',
    mol: ''
}

export default function ProfilePage() {
    const { t } = useTranslation();
    useTitle(t('Profile'));
    const { toast } = useToast();

    const [userInfo, setUserInfo] = useState<UserInfo>(initUserInfo);

    const { user } = useUser();

    useEffect(() => {
        async function fetchUserInfo() {
            try {
                const result = await axiosAuthClient.get('/account/user-info')
                const data = result.data as Response<UserInfo>;

                setUserInfo(data.data);
            }
            catch (error) {
                showError(error, toast, t);
            }
        }

        fetchUserInfo();
    }, [])

    return (
        <div className='md:grid md:grid-cols-5'>
            <div className='flex flex-col items-center md:col-span-2'>
                <Avatar className='h-48 w-48'>
                    <AvatarImage src={user?.userImageUrl} alt="@shadcn" />
                    <AvatarFallback></AvatarFallback>
                </Avatar>
            </div>
            <div className='space-y-4 mt-2 md:mt-0 md:col-start-3 md:col-span-2'>
                <ProfileRow label={t('First name')} value={userInfo.firstName} />
                <ProfileRow label={t('Last name')} value={userInfo.lastName} />
                <ProfileRow label={t('User unique number')} value={userInfo.uniqueNumber} />
                <ProfileRow label={t('Country')} value={userInfo.country} />
                <ProfileRow label={t('Phone number')} value={userInfo.phoneNumber} />
                <ProfileRow label={t('City')} value={userInfo.city} hidden={userInfo.company == ''} />
                <ProfileRow label={t('Address')} value={userInfo.address} hidden={userInfo.company == ''} />
                <ProfileRow label={t('Company')} value={userInfo.company} hidden={userInfo.company == ''} />
                <ProfileRow label={t('TaxId')} value={userInfo.taxId} hidden={userInfo.company == ''} />
                <ProfileRow label={t('Vat number')} value={userInfo.vatId} hidden={userInfo.company == ''} />
            </div>
        </div>
    )
}

interface ProfileRowProps {
    label: string,
    value: string
    hidden?: boolean
}

function ProfileRow({ label, value, hidden }: ProfileRowProps) {

    return (
        <>
            {!hidden &&
                <div className='grid grid-cols-2 place-items-center md:place-items-start'>
                    <span>{label} :</span>
                    <span>{value}</span>
                </div>
            }
        </>
    )
}