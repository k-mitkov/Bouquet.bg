import React, { useState, ChangeEvent } from 'react';
import Map from '../map/Map';
import { Controller, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { axiosAuthClient, validationRules, FlowerShop, addShop, City, AddShopType } from '../../resources';
import { useNavigate } from 'react-router-dom';
import { useToast } from '../ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';
import { Input } from '../shared/input';
import { Button } from '../shared/Button';
import { useCities } from '@/stateProviders/cachedDataContext/DataContext';
import CitySelect from '../inputs/CitySelect';
import { useCity } from '@/stateProviders/cityContext';

const AddShop: React.FC = () => {
    const { t } = useTranslation();
    const { cities } = useCities();
    const [selectedCity, setSelectedCity] = useState<City | undefined>(undefined);
    const { toast } = useToast()
    const { control, handleSubmit, formState: { errors }, setValue } = useForm<AddShopType>();
    const navigate = useNavigate();
    const [formData, setFormData] = useState<FormData>();
    const [previewUrl, setPreviewUrl] = useState<string[]>([]);
    const { city } = useCity();

    const defaultCity: City = {
        id: "default",
        name: t('Аnother'),
        latitude: 42.695144, 
        longitude: 23.329273,
    }

    const citiesForSelect = [...cities, defaultCity];

    const onSubmit = async (data: AddShopType) => {
        try {
            const result = await addShop(data);
            uploadPicture(result.data);
            navigate('/');
        } catch (error) {
            showError(error, toast, t);
        }
    };

    const uploadPicture = async (shopID: string) => {
        if (formData == null) {
            return;
        }
        try {
            const urlWithQueryParam = '/FlowerShop/upload-shop-picture?shopID=' + shopID;
            await axiosAuthClient.post(urlWithQueryParam, formData, { headers: { "Content-Type": "multipart/form-data" } });
        }
        catch (error) {
            showError(error, toast, t);
        }
    }

    const handleShopAdded = (newShop: FlowerShop) => {
        const { latitude, longitude } = newShop;
        setValue('latitude', latitude);
        setValue('longitude', longitude);
    };

    async function onPictureChanged(e: ChangeEvent<HTMLInputElement>) {
        e.preventDefault();

        if (e.target.files == null)
            return;

        let newformData = new FormData();

        newformData!.append('file', e.target.files[0]);

        setFormData(newformData);

        setPreviewUrl(() =>
            Array.from(e.target.files ?? []).map((f) =>
                window.URL.createObjectURL(f)
            ));
    }

    function handleCitySelect(cityID: string) {
        if (cityID != undefined) {
            const foundCity = citiesForSelect.find((c) => c.id === cityID)

            if (foundCity != undefined) setSelectedCity(foundCity)
        }
    }

    return (
        <div className="flex overflow-hidden min-h-[700px] justify-center mt-5" >
            <div className='grid grid-cols-1 md:grid-cols-3 rounded-lg space-x-5 p-5 bg-secondary-background dark:bg-secondary-backgroud-dark'>
                <div className="flex flex-col 
             min-h-[500px] h-screen md:h-auto  text-black dark:text-gray-300">
                    <div className='space-y-2'>
                        <h2 className='text-center'>{t('Add shop')}</h2>
                        <form className='space-y-5' onSubmit={handleSubmit(onSubmit)}>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('City')}:</label>
                                <Controller
                                    control={control}
                                    name='cityID'
                                    rules={validationRules.object}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <CitySelect
                                            defaultCity={city}
                                            cities={citiesForSelect}
                                            onChange={(event) => {
                                                onChange(event);
                                                handleCitySelect(event as string);
                                            }}
                                            onBlur={onBlur}
                                            className='col-start-2'></CitySelect>
                                    )} />
                                {errors.cityID && <span className="error-message col-span-2">{t(errors.cityID.message || '')}</span>}
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('Name')}:</label>
                                <Controller control={control}
                                    name='name'
                                    rules={validationRules.firstName}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Name')} />
                                    )} />
                                {errors.name && <span className="error-message col-span-2">{t(errors.name.message || '')}</span>}
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
                                <label className='text-left'>{t('Delivery price')}:</label>
                                <Controller control={control}
                                    name='price'
                                    rules={validationRules.price}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Delivery price')} />
                                    )} />
                                {errors.price && <span className="error-message col-span-2">{t(errors.price.message || '')}</span>}
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('Free shipping over')}:</label>
                                <Controller control={control}
                                    name='freeDeliveryAt'
                                    rules={validationRules.price}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Free shipping over')} />
                                    )} />
                                {errors.price && <span className="error-message col-span-2">{t(errors.price.message || '')}</span>}
                            </div>
                            <div className='grid grid-rows-2 place-items-start items-center'>
                                <label className='text-left'>{t('Work time')}:</label>
                                <div className='grid grid-cols-2 space-x-2'>
                                <Controller control={control}
                                    name='openAt'
                                    rules={validationRules.hour}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Open at')} />
                                    )} />
                                <Controller control={control}
                                    name='closeAt'
                                    rules={validationRules.hour}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Close at')} />
                                    )} />
                                {errors.openAt && <span className="error-message col-span-2">{t(errors.openAt.message || '')}</span> || errors.closeAt && <span className="error-message col-span-2">{t(errors.closeAt.message || '')}</span>}
                                </div>
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('Same day delivery till')}:</label>
                                <Controller control={control}
                                    name='sameDayTillHour'
                                    rules={validationRules.hour}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Same day delivery till')} />
                                    )} />
                                {errors.sameDayTillHour && <span className="error-message col-span-2">{t(errors.sameDayTillHour.message || '')}</span>}
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <div className='flex flex-row space-x-2'>
                                    {previewUrl.map((url) => (
                                        <img className='h-auto w-full' src={url} />
                                    ))}
                                </div>
                                <label htmlFor="files" className="w-full col-start-2 border border-slate-300 rounded-lg hover:bg-black hover:text-white cursor-pointer text-center">{formData == undefined || formData?.getAll.length == 0 ? t('Select image') : t('Change image')}</label>
                                <input id="files" className='hidden' type="file" accept='.png, .jpg'
                                    onChange={(e) => onPictureChanged(e)} />
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center'>
                                <div className='hidden'>
                                    <label className='text-left'>{t('Latitude')}:</label>
                                    <Controller control={control}
                                        name='latitude'
                                        rules={validationRules.latitude}
                                        render={({ field: { onChange, onBlur } }) => (
                                            <Input onChange={onChange} disabled name='latitude' onBlur={onBlur} placeholder={t('Latitude')} />
                                        )} />
                                    {errors.latitude && <span className="error-message col-span-2">{t(errors.latitude.message || '')}</span>}
                                </div>
                                <div className='hidden'>
                                    <label className='text-left'>{t('Longitude')}:</label>
                                    <Controller control={control}
                                        name='longitude'
                                        rules={validationRules.longitude}
                                        render={({ field: { onChange, onBlur } }) => (
                                            <Input onChange={onChange} disabled name='longitude' onBlur={onBlur} placeholder={t('Longitude')} />
                                        )} />
                                    {errors.longitude && <span className="error-message col-span-2">{t(errors.longitude.message || '')}</span>}
                                </div>
                                {errors.latitude && <span className="error-message col-span-2">{t(errors.latitude.message || '')}</span> || errors.longitude && <span className="error-message col-span-2">{t(errors.longitude.message || '')}</span>}
                            </div>
                            <Button className='w-full' type="submit">{t('Submit')}</Button>
                        </form>
                    </div>
                </div>

                <div className="col-start-2 col-span-2 hidden md:block">
                    <Map flowerShops={[]} height='100%' width='100%' allowPointSelection onObjectAdded={handleShopAdded} selectedCity={selectedCity} />
                </div>
            </div>
        </div >

    );
};

export default AddShop