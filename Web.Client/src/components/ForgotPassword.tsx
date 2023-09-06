import React from 'react';
import { useForm } from 'react-hook-form';
import './ForgotPassword.css';
import { useNavigate } from 'react-router-dom';
import { validationRules, forgotPassword } from '../resources';
import { useTranslation } from 'react-i18next';
import { useTitle } from '../hooks';
import { useToast } from './ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';

interface FormData {
  email: string;
}

const ForgotPassword: React.FC = () => {
  const { t } = useTranslation();
  const { toast } = useToast();
  useTitle(t('ForgotPassword'));

  const { register, handleSubmit, formState: { errors } } = useForm<FormData>();
  const navigate = useNavigate();

  const onSubmit = async (data: FormData) => {
    try {
      await forgotPassword(data.email);
      navigate('/');
    } catch (error) {
      showError(error, toast, t);
    }
  };

  return (
    <div className="forgot-password-container">
      <h2 className="forgot-password-heading">{t('Forgot Password')}</h2>
      <form className="forgot-password-form" onSubmit={handleSubmit(onSubmit)}>
        <label className="forgot-password-label">
          {t('Email')}:
          <input
            className="forgot-password-input"
            type="email"
            {...register('email', validationRules.email)}
          />
          {errors.email && <span className="error-message">{t(errors.email.message || '')}</span>}
        </label>
        <button className="forgot-password-button" type="submit">
          {t('Reset Password')}
        </button>
      </form>
    </div>
  );
};

export default ForgotPassword;
