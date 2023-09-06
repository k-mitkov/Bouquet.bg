import React, { useState, ChangeEvent } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { axiosAuthClient, validationRules, Bouquet, addBouquet } from '../../resources';
import { useLocation, useNavigate } from 'react-router-dom';
import { useToast } from '../ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';
import { Input } from '../shared/input';
import { Button } from '../shared/Button';
import FlowerSelect from '../inputs/FlowerSelector';
import { useCities } from '@/stateProviders/cachedDataContext/DataContext';
import ColorSelect from '../inputs/ColorsSelector';

const AddBouquet: React.FC = () => {
    const location = useLocation();
    const shopID: string = location.state?.shopID;
    const { flowers, colors } = useCities();
    const { t } = useTranslation();
    const { toast } = useToast()
    const { control, handleSubmit, formState: { errors }, setValue } = useForm<Bouquet>();
    const navigate = useNavigate();
    const [formData, setFormData] = useState<FormData>();
    const [previewUrl, setPreviewUrl] = useState<string[]>([]);

    const onSubmit = async (data: Bouquet) => {
        try {
            data.flowerShopID = shopID;
            const result = await addBouquet(data);
            uploadPictures(result.data);
            navigate('/');
        } catch (error) {
            showError(error, toast, t);
        }
    };

    const uploadPictures = async (bouquetID: string) => {
        if (formData == null) {
            return;
        }
        try {
            const urlWithQueryParam = '/Bouquet/upload-bouquet-picture?bouquetID=' + bouquetID;
            await axiosAuthClient.post(urlWithQueryParam, formData, { headers: { "Content-Type": "multipart/form-data" } });
        }
        catch (error) {
            showError(error, toast, t);
        }
    }

    async function onPictureChanged(e: ChangeEvent<HTMLInputElement>) {
        e.preventDefault();

        if (e.target.files == null)
            return;

        let newformData: FormData
        if (formData == undefined) {
            newformData = new FormData();
        } else {
            newformData = formData;
        }

        newformData!.append('file', e.target.files[0]);

        setFormData(newformData);

        setPreviewUrl((prev) =>
            prev.concat(
                Array.from(e.target.files ?? []).map((f) =>
                    window.URL.createObjectURL(f)
                ))
        );
    }

    return (
        <div className="flex overflow-hidden min-h-[500px] justify-center mt-5" >
            <div className='grid grid-cols-1 w-[100%] rounded-lg space-x-5 p-5 bg-secondary-background dark:bg-secondary-backgroud-dark'>
                <div className="flex flex-col min-h-[300px] h-screen md:h-auto text-black dark:text-gray-300">
                    <div className='space-y-2'>
                        <h2 className='text-center'>{t('Add bouquet')}</h2>
                        <form className='space-y-5 space-x-5 grid grid-cols-1 md:grid-cols-2 p-2' onSubmit={handleSubmit(onSubmit)}>
                            <div className='flex flex-col md:col-span-2 place-items-start items-start space-x-2'>
                                <div className='flex flex-row space-x-2 overflow-x-scroll'>
                                    {previewUrl.map((url) => (
                                        <img className='h-48 w-48' src={url} />
                                    ))}
                                </div>
                                <label htmlFor="files" className="w-48 col-start-2 border border-slate-300 rounded-lg hover:bg-black hover:text-white cursor-pointer text-center">{formData == undefined || formData?.getAll.length == 0 ? t('Select image') : t('Add image')}</label>
                                <input id="files" className='hidden' type="file" accept='.png, .jpg'
                                    onChange={(e) => onPictureChanged(e)} />
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('Flowers')}:</label>
                                <Controller
                                    control={control}
                                    name='flowers'
                                    rules={validationRules.type}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <FlowerSelect
                                            flowers={flowers}
                                            onChange={(event) => {
                                                onChange(event);
                                            }}
                                            onBlur={onBlur}
                                            className='col-start-2'></FlowerSelect>
                                    )} />
                                {errors.flowers && <span className="error-message col-span-2">{t(errors.flowers.message || '')}</span>}
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('Colors')}:</label>
                                <Controller
                                    control={control}
                                    name='colors'
                                    rules={validationRules.type}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <ColorSelect
                                            colors={colors}
                                            onChange={(event) => {
                                                onChange(event);
                                            }}
                                            onBlur={onBlur}
                                            className='col-start-2'></ColorSelect>
                                    )} />
                                {errors.colors && <span className="error-message col-span-2">{t(errors.colors.message || '')}</span>}
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
                                <label className='text-left'>{t('Price')}:</label>
                                <Controller control={control}
                                    name='price'
                                    rules={validationRules.price}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Price')} />
                                    )} />
                                {errors.price && <span className="error-message col-span-2">{t(errors.price.message || '')}</span>}
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('Flowers count')}:</label>
                                <Controller control={control}
                                    name='flowersCount'
                                    rules={validationRules.price}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Flowers count')} />
                                    )} />
                                {errors.flowersCount && <span className="error-message col-span-2">{t(errors.flowersCount.message || '')}</span>}
                            </div>
                            <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                                <label className='text-left'>{t('Height')}:</label>
                                <Controller control={control}
                                    name='height'
                                    rules={validationRules.price}
                                    render={({ field: { onChange, onBlur } }) => (
                                        <Input onChange={onChange} onBlur={onBlur} placeholder={t('Height')} />
                                    )} />
                                {errors.height && <span className="error-message col-span-2">{t(errors.height.message || '')}</span>}
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
                            <Button className='w-full' type="submit">{t('Submit')}</Button>
                        </form>
                    </div>
                </div>
            </div>
        </div >

    );
};

export default AddBouquet