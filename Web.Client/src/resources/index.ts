import translationEN from './localization/locales/en/translation.json';
import translationBG from './localization/locales/bg/translation.json';
import i18n from './localization/i18n'
import { validationRules } from './validation/Validation';
import { axiosClient, axiosAuthClient } from './api/axiosConfig'
import { login, register, forgotPassword, resetPassword, getUserClaims
  , getUserInfo, updateUserInfo, checkEmail, requestRefreshToken } from './api/UserService'
import { getShops, addShop, getMyShops, getShop, addWorker, getWorkers, removeWorker } from './api/shop/FlowerShopService'
import RegisterData from './types/RegisterData';
import ResetPasswordData from './types/ResetPasswordData';
import Token from './types/Token';
import Picture from './types/Picture';
import UserCredentials from './types/UserCredentials';
import { Response, BaseResponse } from './types/responses';
import FlowerShop from './types/shop/FlowerShop'
import ShopConfig from './types/shop/ShopConfig'
import LoginModel from './types/LoginModel';
import UserInfo from './types/user/UserInfo';
import { StatusEnum } from './types/enums/StatusEnum';
import ChangePassword from './types/ChangePassword';
import Claims from './api/claims/Claims'
import RegisterInfo from './types/user/RegisterInfo';
import City from './types/shop/City';
import { getCities } from './api/shop/CityService';
import Color from './types/bouquet/Color';
import Flower from './types/bouquet/Flower';
import { getFlowers } from './api/bouquet/FlowersService';
import { getColors } from './api/bouquet/ColorsService';
import AddShopType from './types/shop/AddShopType';
import Bouquet from './types/bouquet/Bouquet';
import { addBouquet, getBouquets, getBouquetsByShop } from './api/bouquet/BouquetService';
import { MakeOrderType, OrderType }  from './types/order/Order';
import { getOrdersByShop, getOrdersByUser, makeOrder } from './api/order/OrderService';
import AddPaymentCard from './types/payment/AddPaymentCard';
import { addCard, getCards, pay, payWithNewCard } from './api/payment/PaymentService';
import Card from './types/payment/Card';
import Payment from './types/payment/Payment';
import AddWorker from './types/shop/AddWorker';
import Worker from './types/user/Worker';

const translationResources = {
  en: translationEN,
  bg: translationBG,
};

export {
  i18n, translationResources, validationRules, axiosClient, axiosAuthClient,
  login, register, forgotPassword, resetPassword, getUserClaims, getShop, addCard, getCards, pay, payWithNewCard
  , getUserInfo, updateUserInfo, Claims, checkEmail, requestRefreshToken,  getShops, addShop, getCities, getWorkers
  , getFlowers, getColors, getMyShops, getBouquets, getBouquetsByShop, addBouquet, makeOrder, addWorker, removeWorker
  , getOrdersByUser, getOrdersByShop
};
export type {
  RegisterData, ResetPasswordData, Token, UserCredentials, AddPaymentCard, Card, Payment, AddWorker,
  BaseResponse, Response, LoginModel, StatusEnum, UserInfo, ChangePassword, RegisterInfo, Worker,
  Picture, FlowerShop, ShopConfig, City, Color, Flower, AddShopType, Bouquet, MakeOrderType, OrderType
}