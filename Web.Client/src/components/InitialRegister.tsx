import React from 'react';
import { Controller, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { validationRules, RegisterData, register as registerUser, checkEmail } from '../resources';
import { Switch } from './ui/switch';
import { Button } from './shared/Button';
import { showError } from '@/helpers/ErrorHelper';
import { useToast } from './ui/use-toast';
import { Input } from './shared/input';

interface Props {
  setRegisterNext: React.Dispatch<React.SetStateAction<boolean>>
  setData: React.Dispatch<React.SetStateAction<RegisterData>>
  setIsCompany: React.Dispatch<React.SetStateAction<boolean>>
  isCompany: boolean
}

export const InitialRegister: React.FC<Props> = ({ setRegisterNext, setData, setIsCompany, isCompany }: Props) => {
  const { t } = useTranslation();
  const { toast } = useToast();
  const { handleSubmit, formState: { errors }, watch, control } = useForm<RegisterData>();

  const password = React.useRef({});
  password.current = watch('password', '');

  const onSubmit = async (data: RegisterData) => {
    try {
      await checkEmail(data.email)
      setData(data);
      setRegisterNext(true);
    } catch (error) {
      showError(error, toast, t);
    }
  };

  return (
    <div className='flex flex-col md:grid md:grid-cols-6 mt-10'>
      <div className=' bg-secondary-background dark:bg-secondary-backgroud-dark flex flex-col col-start-1 md:col-start-2 rounded-md items-center space-y-2 md:col-span-4'>
        <h2 className="text-4xl text-main-font dark:text-main-font-dark font-bold">{t('Register')}</h2>
        <form className="flex flex-col justify-center w-72" onSubmit={handleSubmit(onSubmit)}>
          <div className='m-2'>
            <Controller control={control}
              name='email'
              rules={validationRules.email}
              render={({ field: { onChange, onBlur } }) => (
                <Input onChange={onChange} id='email' onBlur={onBlur} placeholder={t('Email')} />

              )} />
            {errors.email && <span className='text-red-500'>{t(errors.email.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='password'
              rules={validationRules.password}
              render={({ field: { onChange, onBlur } }) => (
                <Input className='text-black' onChange={onChange} id='password' type='password' onBlur={onBlur} placeholder={t('Password')} />
              )} />
            {errors.password && <span className='text-red-500'>{t(errors.password.message || '')}</span>}
          </div>
          <div className='m-2'>
            <Controller control={control}
              name='confirmPassword'
              rules={{
                ...validationRules.confirmPassword,
                validate: (value) =>
                  validationRules.confirmPassword.validate(value, watch('password')),
              }}
              render={({ field: { onChange, onBlur } }) => (
                <Input className='text-black' onChange={onChange} id='confirmPassword' type='password' onBlur={onBlur} placeholder={t('Confirm Password')} />
              )} />
            {errors.confirmPassword && <span className='text-red-500'>{t(errors.confirmPassword.message || '')}</span>}
          </div>
          <div className='flex text-main-font dark:text-main-font-dark font-semibold m-2'>
            <label htmlFor='5'>
              {t('Are you a company')}?
            </label>
            <Switch
              className='mx-2'
              id='5'
              checked={isCompany}
              onCheckedChange={(isCompanyChanged) => setIsCompany(isCompanyChanged)} />
          </div>
          <Button className=" w-full" variant={'default'} size={'lg'} type="submit">
            {t('Next')}
          </Button>
        </form>
      </div>

    </div>
  )
}