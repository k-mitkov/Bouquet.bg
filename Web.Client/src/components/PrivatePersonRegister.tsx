import { validationRules, RegisterData, register as registerUser, UserInfo, UserCredentials, RegisterInfo } from '../resources';
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import validator from 'validator';
import { useToast } from './ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';
import { Input } from './shared/input';
import { Button } from './shared/Button';

interface Props {
  registerData: RegisterData
}

type FormData = {
  userInfo: UserInfo;
  recaptcha: string; // Add the recaptcha field to the form data type
};

export const PrivatePersonRegister: React.FC<Props> = ({ registerData }: Props) => {
  const { t } = useTranslation();
  const { toast } = useToast();
  const { handleSubmit, formState: { errors }, control } = useForm<FormData>();
  const navigate = useNavigate();

  const onSubmit = async (data: FormData) => {

    try {
      const userCredentials: UserCredentials = { email: registerData.email, password: registerData.password };
      const fullData: RegisterInfo = { userCredentials: userCredentials, userInfo: data.userInfo }

      console.log(userCredentials.email, userCredentials.password);
      console.log(fullData.userInfo.address, fullData.userInfo.city, fullData.userInfo.company,
        fullData.userInfo.country, fullData.userInfo.firstName, fullData.userInfo.lastName, fullData.userInfo.mol, fullData.userInfo.phoneNumber);

      await registerUser(fullData);
      navigate('/login');
    } catch (error) {
      showError(error, toast, t);
    }
  };

  return (
    <div className='flex flex-col md:grid md:grid-cols-6 mt-10'>
      <div className=' bg-secondary-background dark:bg-secondary-backgroud-dark flex flex-col col-start-1 md:col-start-2 rounded-md items-center space-y-2 md:col-span-4'>
        <h2 className="text-4xl text-main-font dark:text-main-font-dark font-bold">{t('Register person')}</h2>
        <form className="flex flex-col justify-center" onSubmit={handleSubmit(onSubmit)}>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.firstName'
              rules={validationRules.firstName}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='firstName' onBlur={onBlur} placeholder={t('First name')} />
              )} />
            {errors.userInfo?.firstName && <span className='text-red-500'>{t(errors.userInfo?.firstName.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='userInfo.lastName'
              rules={validationRules.lastName}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='lastName' onBlur={onBlur} placeholder={t('Last name')} />
              )} />
            {errors.userInfo?.lastName && <span className='text-red-500'>{t(errors.userInfo?.lastName.message || '')}</span>}
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
          <Button className=" w-full" variant={'default'} size={'lg'} type="submit">
            {t('Register')}
          </Button>
        </form>
      </div>
    </div>
  );
};