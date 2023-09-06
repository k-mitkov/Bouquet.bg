import { useTranslation } from 'react-i18next';
import { useTitle } from '../../hooks';
import { ChangeEvent, useEffect, useState } from 'react';
import { axiosAuthClient, validationRules, Response, UserInfo, getUserInfo, updateUserInfo } from '../../resources';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Switch } from '@/components/ui/switch';
import { useForm, Controller } from 'react-hook-form';
import { useUser } from '@/stateProviders/userContext';
import { setProfilePictureUrl } from '@/services/LocalStorageFunctions';
import validator from 'validator';
import { CountrySelect } from '@/components/inputs';
import { Input } from '@/components/shared/input';
import { Button } from '@/components/shared/Button';
import { showError } from '@/helpers/ErrorHelper';
import { useToast } from '@/components/ui/use-toast';

export default function EditProfilePage() {
    const { t } = useTranslation();
    useTitle(t('Edit profile'));
    const { toast } = useToast();

    const [isCompany, setIsCompany] = useState<boolean>(false);

    const { handleSubmit, control, reset, formState: { errors } } = useForm<UserInfo>();

    const { user, setUser } = useUser();

    useEffect(() => {
        async function fetchUserInfo() {
            try {
                const result = await getUserInfo();
                const data = result.data;

                setIsCompany(data.company != null && data.company != '');
                reset(data);
            }
            catch (error) {
                showError(error, toast, t);
            }
        }

        fetchUserInfo();
    }, [])

    async function onSubmit(data: UserInfo) {
        try {
            if (!isCompany) {
                data.address = '';
                data.city = '';
                data.company = '';
                data.taxId = '';
                data.vatId = '';
                reset(data);
            }

            await updateUserInfo(data);
        }
        catch (error) {
            showError(error, toast, t);
        }
    }

    async function onProfilePictureChanged(e: ChangeEvent<HTMLInputElement>) {
        e.preventDefault();

        const formData = new FormData();
        if (e.target.files == null)
            return;

        formData.append('file', e.target.files[0]);
        try {

            const response = await axiosAuthClient.post('/account/upload-profile-picture', formData, { headers: { "Content-Type": "multipart/form-data" } });
            const data = response.data as Response<string>;

            setUser(prev => {
                return {
                    ...prev,
                    userImageUrl: data.data
                }
            });
            setProfilePictureUrl(data.data);
        }
        catch (error) {
            showError(error, toast, t);
        }
    }

    return (
        <div className='flex flex-col md:grid md:grid-cols-5'>
            <div className='flex flex-col items-center space-y-2 md:col-span-2'>
                <Avatar className='h-48 w-48'>
                    <AvatarImage src={user?.userImageUrl} alt="@shadcn" />
                    <AvatarFallback></AvatarFallback>
                </Avatar>

                <label htmlFor="files"
                    className="w-48 border border-black  dark:border-white dark:hover:border-black
                     hover:bg-black hover:text-white rounded-lg cursor-pointer">
                    {t('Select image')}
                </label>
                <input id="files" className='hidden' type="file" accept='.png, .jpg'
                    onChange={(e) => onProfilePictureChanged(e)} />
            </div>
            <div className='md:col-start-3 md:col-span-2 dark:border-white'>
                <form className='space-y-2 mt-2 md:mt-0 mx-2 md:mx-0' onSubmit={handleSubmit(onSubmit)}>
                    <div className='flex space-x-1 justify-center md:justify-start'>
                        <Switch id="company" checked={isCompany} onCheckedChange={(checked) => setIsCompany(checked)} />
                        <label htmlFor='company'>{t('Company')}</label>
                    </div>
                    <div className='grid grid-cols-2 place-items-start'>
                        <label htmlFor='firstName'>{t('First name')}</label>
                        <Controller control={control}
                            name='firstName'
                            rules={validationRules.firstName}
                            render={({ field: { onChange, onBlur } }) => (
                                <Input onChange={onChange} id='firstName' onBlur={onBlur} placeholder={t('First name')} />
                            )} />
                    </div>
                    {errors.firstName && <span>{t(errors.firstName.message || '')}</span>}

                    <div className='grid grid-cols-2 place-items-start'>
                        <label htmlFor='lastName'>{t('Last name')}</label>
                        <Controller control={control}
                            name='lastName'
                            rules={validationRules.lastName}
                            render={({ field: { onChange, onBlur } }) => (
                                <Input onChange={onChange} id='lastName' onBlur={onBlur} placeholder={t('Last name')} />
                            )} />
                    </div>
                    {errors.lastName && <span>{t(errors.lastName.message || '')}</span>}

                    <div className='grid md:grid-cols-2 place-items-end md:place-items-start'>
                        <Controller
                            control={control}
                            name='country'
                            rules={validationRules.country}
                            render={({ field: { onChange, onBlur, value } }) => (
                                <CountrySelect
                                    onChange={onChange}
                                    onBlur={onBlur}
                                    defaultValue={value}
                                    className='md:col-start-2 ' />
                            )} />
                    </div>
                    {errors.country && <span>{t(errors.country.message || '')}</span>}

                    <div className='grid grid-cols-2 place-items-start'>
                        <label htmlFor='phoneNumber'>{t('Phone number')}</label>
                        <Controller control={control}
                            name='phoneNumber'

                            rules={{ ...validationRules.phoneNumber, validate: (value) => validationRules.phoneNumber.validate(validator.isMobilePhone(value, 'bg-BG')) }}
                            render={({ field: { onChange, onBlur, value } }) => (
                                <Input onChange={onChange} value={value} type='tel' id='phoneNumber' onBlur={onBlur} placeholder={t('Phone number')} />
                            )} />
                    </div>
                    {errors.phoneNumber && <span>{t(errors.phoneNumber.message || '')}</span>}
                    {isCompany &&
                        <div className='space-y-2'>
                            <div className='grid grid-cols-2 place-items-start'>
                                <label htmlFor='city'>{t('City')}</label>
                                <Controller control={control}
                                    name='city'
                                    rules={validationRules.city}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} id='city' onBlur={onBlur} placeholder={t('City')} />
                                    )} />
                            </div>
                            {errors.city && <span>{t(errors.city.message || '')}</span>}

                            <div className='grid grid-cols-2 place-items-start'>
                                <label htmlFor='address'>{t('Address')}</label>
                                <Controller control={control}
                                    name='address'
                                    rules={validationRules.city}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} id='address' onBlur={onBlur} placeholder={t('Address')} />
                                    )} />
                            </div>
                            {errors.address && <span>{t(errors.address.message || '')}</span>}

                            <div className='grid grid-cols-2 place-items-start'>
                                <label htmlFor='company'>{t('Company')}</label>
                                <Controller control={control}
                                    name='company'
                                    rules={validationRules.company}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} id='company' onBlur={onBlur} placeholder={t('Company')} />
                                    )} />
                            </div>
                            {errors.company && <span>{t(errors.company.message || '')}</span>}

                            <div className='grid grid-cols-2 place-items-start'>
                                <label htmlFor='taxId'>{t('TaxId')}</label>
                                <Controller control={control}
                                    name='taxId'
                                    rules={validationRules.taxId}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} id='taxId' onBlur={onBlur} placeholder={t('Tax Id')} />
                                    )} />
                            </div>
                            {errors.taxId && <span>{t(errors.taxId.message || '')}</span>}

                            <div className='grid grid-cols-2 place-items-start'>
                                <label htmlFor='vatNumber'>{t('Vat number')}</label>
                                <Controller control={control}
                                    name='vatId'
                                    rules={validationRules.vatNumber}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} id='vatNumber' onBlur={onBlur} placeholder={t('Vat number')} />
                                    )} />
                            </div>
                            {errors.vatId && <span>{t(errors.vatId.message || '')}</span>}
                        </div>
                    }
                    <div>
                        <Button className='w-full' >{t('Save')}</Button>
                    </div>
                </form>
            </div>
        </div>
    )
}