import { useTranslation } from 'react-i18next';
import { useTitle } from '../../hooks';
import { useForm } from 'react-hook-form';
import { ChangePassword, axiosAuthClient, validationRules } from '../../resources';
import { showError } from '@/helpers/ErrorHelper';
import { useToast } from '@/components/ui/use-toast';

export default function ChangePasswordPage() {
    const { t } = useTranslation();
    useTitle(t('Change password'));
    const { toast } = useToast();

    const { handleSubmit, register, formState: { errors }, watch } = useForm<ChangePassword>();

    async function onSubmit(data: ChangePassword) {
        try {
            await axiosAuthClient.put('/account/change-password', { oldPassword: data.oldPassword, newPassword: data.newPassword });
        }
        catch (error) {
            showError(error, toast, t);
        }
    }

    return (
        <div className='grid h-fit md:h-full place-items-center'>
            <form onSubmit={handleSubmit(onSubmit)} className='flex flex-col w-[300px] items-center justify-center h-full'>
                <div className='w-full'>
                    <div className='flex flex-col'>
                        <label>{t('Old password')}</label>
                        <input type='password'
                            className='px-1 bg-transparent border border-black dark:border-white rounded-lg'
                            {...register('oldPassword', validationRules.password)} />
                        {errors.oldPassword && <span>{t(errors.oldPassword.message || '')}</span>}
                    </div>
                    <div className='flex flex-col'>
                        <label>{t('New password')}</label>
                        <input type='password'
                            className='px-1 bg-transparent border border-black dark:border-white rounded-lg'
                            {...register('newPassword', validationRules.password)} />
                        {errors.newPassword && <span>{t(errors.newPassword.message || '')}</span>}
                    </div>
                    <div className='flex flex-col'>
                        <label>{t('Confirm new password')}</label>
                        <input type='password'
                            className='px-1 bg-transparent border border-black dark:border-white rounded-lg'
                            {...register('confirmNewPassword', {
                                ...validationRules.password,
                                validate: (value) => validationRules.confirmPassword.validate(value, watch('newPassword'))
                            })} />
                        {errors.confirmNewPassword && <span>{t(errors.confirmNewPassword.message || '')}</span>}
                    </div>
                </div>
                <button className='w-full bg-black hover:bg-white text-white hover:text-black border border-black rounded-lg mt-5'>{t('Change password')}</button>
            </form>
        </div>
    )
}