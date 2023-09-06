import { AddPaymentCard, Card, Response, BaseResponse, axiosAuthClient, Payment } from "@/resources";

export const addCard = async (data: AddPaymentCard): Promise<BaseResponse> => {
    return (await axiosAuthClient.post('/Cards/add', data)).data;
};

export const getCards = async (): Promise<Response<Card[]>> => {
    return (await axiosAuthClient.get('/Cards')).data;
};

export const pay = async (data: Payment): Promise<BaseResponse> => {
    return (await axiosAuthClient.post('/Payments/create/existing-card', data)).data;
};

export const payWithNewCard = async (data: Payment): Promise<BaseResponse> => {
    return (await axiosAuthClient.post('/Payments/create/new-card', data)).data;
};