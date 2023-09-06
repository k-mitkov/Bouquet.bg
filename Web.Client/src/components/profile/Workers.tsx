import { AddWorker, Response, addWorker, getWorkers, validationRules, Worker, removeWorker } from '../../resources';
import { useTranslation } from 'react-i18next';
import { useToast } from '../ui/use-toast';
import { Button } from '../shared/Button';
import { useEffect, useState } from 'react';
import { showError } from '@/helpers/ErrorHelper';
import { Controller, useForm } from 'react-hook-form';
import { Input } from '../shared/input';
import { Avatar, AvatarFallback, AvatarImage } from '../ui/avatar';
import { Trash2Icon } from "lucide-react";

interface WorkersProps {
    shopId: string;
}

const Workers: React.FC<WorkersProps> = ({ shopId }) => {
    const { t } = useTranslation();
    const { toast } = useToast();
    const [workers, setWorkers] = useState<Worker[]>([]);
    const { control, handleSubmit, formState: { errors } } = useForm<AddWorker>();

    const fetchWorkers = async () => {
        try {
            let response: Response<Worker[]>;
            response = await getWorkers(shopId);
            setWorkers(response.data);
        } catch (error) {
            showError(error, toast, t);
        }
    };

    useEffect(() => {
        fetchWorkers();
    }, []);

    const deleteWorker = async (worker: Worker) => {
        try {
            await removeWorker(shopId, worker.id);
            fetchWorkers();
        } catch (error) {
            showError(error, toast, t);
        }
    };

    const onSubmit = async (data: AddWorker) => {
        try {
            data.flowerShopID = shopId;
            await addWorker(data);
            fetchWorkers();
        } catch (error) {
            showError(error, toast, t);
        }
    };

    return (
        <div className='flex flex-col w-full space-y-1'>
            <div className='flex flex-col justify-center border-2 border-green-500 rounded-lg p-2 text-black dark:text-gray-300'>
                <h2 className='text-center'>{t('Add worker')}</h2>
                <div className='flex flex-row justify-center'>
                    <form className='space-y-5 justify-center' onSubmit={handleSubmit(onSubmit)}>
                        <div className='grid grid-cols-2 place-items-start items-center space-x-2'>
                            <label className='text-left'>{t('User unique number')}:</label>
                            <Controller control={control}
                                name='userUniqueNumber'
                                rules={validationRules.firstName}
                                render={({ field: { onChange, onBlur } }) => (
                                    <Input onChange={onChange} onBlur={onBlur} placeholder={t('User unique number')} />
                                )} />
                            {errors.userUniqueNumber && <span className="error-message col-span-2">{t(errors.userUniqueNumber.message || '')}</span>}
                        </div>
                        <Button className='w-full' type="submit">{t('Add worker')}</Button>
                    </form>
                </div>
            </div>
            {workers != undefined && workers.length > 0 ? (
                <div className='space-y-2 w-full'>
                    {workers.map(worker => (
                        <div className='flex flex-row border-2 border-green-500 rounded-lg p-2 text-black dark:text-gray-300'>
                            <div className='flex flex-col m-2'>
                                <Avatar className=''>
                                    <AvatarImage src={import.meta.env.VITE_IDENTITY_FILE_URL + worker.userImageUrl} alt="@shadcn" />
                                    <AvatarFallback></AvatarFallback>
                                </Avatar>
                            </div>
                            <div className='m-2 space-x-3'>
                                <span className='font-bold'>{worker.firstName}</span>
                                <span className='font-bold'>{worker.lastName}</span>
                            </div>
                            <div className="flex justify-center m-2 ">
                                <Trash2Icon
                                    className="hover:text-red-600 cursor-pointer"
                                    onClick={() => deleteWorker(worker)} />
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <div>{t('There are no workers')}</div>
            )}
        </div>
    );
};

export default Workers;
