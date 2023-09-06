import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Controller, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { UserCredentials, validationRules, login } from '../resources'
import { User, useUser } from '../stateProviders/userContext';
import { setProfilePictureUrl, setUserID, setUserTokens } from '../services/LocalStorageFunctions';
import { useTitle } from '../hooks';
import { Button } from './shared/Button';
import { Input } from './shared/input';
import { useToast } from './ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';

const Login: React.FC = () => {
  const { t } = useTranslation();
  useTitle(t('Login'));

  const { setUser } = useUser();
  const navigate = useNavigate();
  const { toast } = useToast()

  const { handleSubmit, control, formState: { errors } } = useForm<UserCredentials>();

  async function onSubmit(data: UserCredentials) {
    try {

      const response = await login(data);
      const userData: User = {
        id: response.data.id,
        tokens: {
          accessToken: response.data.accessToken,
          refreshToken: response.data.refreshToken
        },
        claims: response.data.claims,
        userImageUrl: response.data.profilePictureUrl
      };

      setUserTokens(userData.tokens);
      setProfilePictureUrl(userData.userImageUrl);
      setUserID(userData.id)

      setUser(userData);
      navigate('/');

      console.log(response);
    } catch (error) {
      showError(error, toast, t);
    }
  }

  return (

    <div className="flex flex-col md:grid md:grid-cols-6 mt-10">
      <div className=' bg-secondary-background dark:bg-secondary-backgroud-dark flex flex-col col-start-1 md:col-start-2 rounded-md items-center space-y-2 md:col-span-4' >
        <h2 className="text-4xl text-main-font mt-10 dark:text-main-font-dark font-bold">{t('Login')}</h2>
        <form className="flex flex-col justify-center" onSubmit={handleSubmit(onSubmit)}>
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

          <Button className=" w-full" variant={'default'} size={'lg'} type="submit">
            {t('Submit')}
          </Button>
        </form>
        <div className="flex flex-col md:flex-row pt-32 pb-3 md:space-x-2">
          <span className='text-main-font dark:text-main-font-dark font-semibold'>{t('Don\'t have an account?')} </span>
          <Link className='text-main-font dark:text-main-font-dark font-semibold' to="/register" >
            {t('Register')}
          </Link>
          <span className='collapse md:visible'> | </span>
          <Link className='text-main-font dark:text-main-font-dark font-semibold' to="/forgot-password">
            {t('Forgot Password')}
          </Link>
        </div>
      </div >
    </div >

  );
};

export default Login;
