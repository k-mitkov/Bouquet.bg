import React, { useState } from 'react';
import './Register.css';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';
import { useTitle } from '../hooks';
import { InitialRegister } from './InitialRegister';
import { RegisterData } from '../resources';
import { DetailedRegister } from './DetailedRegister';

const Register: React.FC = () => {
  const { t } = useTranslation();
  useTitle(t('Register'));

  const [registerNext, setRegisterNext] = useState<boolean>(false);

  const [isCompany, setIsCompany] = useState<boolean>(false);

  const [data, setData] = useState<RegisterData>({
    username: "",
    password: "",
    confirmPassword: "",
    email: "",
  });

  return (
    <div className='bg-secondary-background dark:bg-secondary-backgroud-dark mt-10 flex flex-col rounded-md items-center space-y-2'>
      {!registerNext ?
        <InitialRegister setData={setData} setRegisterNext={setRegisterNext}
          setIsCompany={setIsCompany} isCompany={isCompany} />
        :
        <DetailedRegister data={data} isCompany={isCompany} />}

      <div className="flex pt-12 pb-3 space-x-2">
        <span className='text-main-font dark:text-main-font-dark font-semibold'>{t('Already have an account?')}</span>
        <Link className='text-main-font dark:text-main-font-dark font-semibold' to="/login">
          {t('Login')}
        </Link>
      </div>
    </div>

  );
};

export default Register;
