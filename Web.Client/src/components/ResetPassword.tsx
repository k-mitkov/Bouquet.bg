import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './ResetPassword.css';
import { base64UrlDecode } from '../helpers/Decoder';
import { validationRules, ResetPasswordData, resetPassword } from '../resources';
import { useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { useTitle } from '../hooks';
import { useToast } from './ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';


interface ResetPassword {
  password: string;
  confirmPassword: string;
}

const ResetPassword: React.FC = () => {
  const { t } = useTranslation();
  const { toast } = useToast();
  useTitle(t('ResetPassword'));

  const location = useLocation();
  const navigate = useNavigate();
  const { register, handleSubmit, formState: { errors }, watch } = useForm<ResetPassword>();

  const getSearchParams = () => {
    const searchParams = new URLSearchParams(location.search);
    const email = base64UrlDecode(searchParams.get('email') || '');
    const token = base64UrlDecode(searchParams.get('token') || '');

    return { email, token };
  };

  const resetPasswordArgs: ResetPasswordData = {
    ...getSearchParams(),
    password: '',
  };

  const onSubmit = async (data: ResetPassword) => {
    resetPasswordArgs.password = data.password;

    try {
      // Send the reset password request to the server
      await resetPassword(resetPasswordArgs);
      navigate('/');
    } catch (error) {
      showError(error, toast, t);
    }
  };

  return (
    <div className="reset-password-container">
      <h2 className="reset-password-heading">{t('Reset Password')}</h2>
      <form className="reset-password-form" onSubmit={handleSubmit(onSubmit)}>
        <label className="reset-password-label">
          {t('New Password')}:
          <input
            className={`register-input ${errors.password ? 'invalid-field' : ''}`}
            type="password"
            {...register('password', validationRules.password)}
          />
          {errors.password && <span className="error-message">{t(errors.password.message || '')}</span>}
        </label>
        <label className="reset-password-label">
          {t('Confirm Password')}:
          <input
            className={`register-input ${errors.confirmPassword ? 'invalid-field' : ''}`}
            type="password"
            {...register('confirmPassword', {
              ...validationRules.confirmPassword,
              validate: (value) => validationRules.confirmPassword.validate(value, watch('password'))
            })}
          />
          {errors.confirmPassword && <span className="error-message">{t(errors.confirmPassword.message || '')}</span>}
        </label>
        <button className="reset-password-button" type="submit">
          {t('Reset Password')}
        </button>
      </form>
    </div>
  );
};

export default ResetPassword;
