import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

// Import your translation resources
import translationEN from './locales/en/translation.json';
import translationBG from './locales/bg/translation.json';

// Set up i18next
i18n.use(initReactI18next).init({
  resources: {
    en: {
      translation: translationEN,
    },
    bg: {
      translation: translationBG,
    },
  },
  lng: 'en', // Set the default language here
  fallbackLng: 'en',
  interpolation: {
    escapeValue: false,
  },
});

export default i18n;