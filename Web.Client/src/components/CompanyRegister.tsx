import { validationRules, RegisterData, register as registerUser, UserInfo, RegisterInfo, UserCredentials } from '../resources';
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import validator from 'validator';
import { showError } from '@/helpers/ErrorHelper';
import { useToast } from './ui/use-toast';
import { Input } from './shared/input';
import { Button } from './shared/Button';

interface Props {
  registerData: RegisterData
}

type FormData = {
  userInfo: UserInfo;
  recaptcha: string; // Add the recaptcha field to the form data type
};

export const CompanyRegister: React.FC<Props> = ({ registerData }: Props) => {
  const { t } = useTranslation();
  const { toast } = useToast();

  const { handleSubmit, formState: { errors }, control } = useForm<FormData>();
  const navigate = useNavigate();

  const onSubmit = async (data: FormData) => {
    try {
      const userCredentials: UserCredentials = { email: registerData.email, password: registerData.password };
      const fullData: RegisterInfo = { userCredentials: userCredentials, userInfo: data.userInfo }

      await registerUser(fullData);
      navigate('/login');
    } catch (error) {
      showError(error, toast, t);
    }
  };

  return (
    <div className='flex flex-col md:grid md:grid-cols-6 mt-10'>
      <div className=' bg-secondary-background dark:bg-secondary-backgroud-dark flex flex-col col-start-1 md:col-start-2 rounded-md items-center space-y-2 md:col-span-4'>
        <h2 className="text-4xl text-main-font dark:text-main-font-dark font-bold">{t('Register company')}</h2>
        <form className="flex flex-col justify-center" onSubmit={handleSubmit(onSubmit)}>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.company'
              rules={validationRules.company}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='company' onBlur={onBlur} placeholder={t('Company')} />
              )} />
            {errors.userInfo?.company && <span className='text-red-500'>{t(errors.userInfo?.company.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.mol'
              rules={validationRules.MOL}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='mol' onBlur={onBlur} placeholder={t('MOL')} />
              )} />
            {errors.userInfo?.mol && <span className='text-red-500'>{t(errors.userInfo?.mol.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.country'
              rules={validationRules.country}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='country' onBlur={onBlur} placeholder={t('Country')} />
              )} />
            {errors.userInfo?.country && <span className='text-red-500'>{t(errors.userInfo?.country.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.city'
              rules={validationRules.city}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='city' onBlur={onBlur} placeholder={t('City')} />
              )} />
            {errors.userInfo?.city && <span className='text-red-500'>{t(errors.userInfo?.city.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.address'
              rules={validationRules.address}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='address' onBlur={onBlur} placeholder={t('Address')} />
              )} />
            {errors.userInfo?.address && <span className='text-red-500'>{t(errors.userInfo?.address.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.phoneNumber'
              rules={{
                ...validationRules.phoneNumber,
                validate: (value) =>
                  validationRules.phoneNumber.validate(validator.isMobilePhone(value, 'bg-BG')),
              }}
              render={({ field: { onChange, onBlur } }) => (
                <Input className='text-black' onChange={onChange} id='phoneNumber' type='tel' onBlur={onBlur} placeholder={t('Phone number')} />
              )} />
            {errors.userInfo?.phoneNumber && <span className='text-red-500'>{t(errors.userInfo?.phoneNumber.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.taxId'
              rules={validationRules.taxId}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='taxId' onBlur={onBlur} placeholder={t('TaxId')} />
              )} />
            {errors.userInfo?.taxId && <span className='text-red-500'>{t(errors.userInfo?.taxId.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.vatId'
              rules={validationRules.vatNumber}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='vatId' onBlur={onBlur} placeholder={t('Vat number')} />
              )} />
            {errors.userInfo?.vatId && <span className='text-red-500'>{t(errors.userInfo?.vatId.message || '')}</span>}
          </div>
          <Button className=" w-full" variant={'default'} size={'lg'} type="submit">
            {t('Register')}
          </Button>
        </form>
      </div>
    </div>
  );
};