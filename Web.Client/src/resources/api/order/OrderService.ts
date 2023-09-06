import {  MakeOrderType, OrderType, Response, axiosAuthClient } from '../../index'

export const makeOrder = async (data: MakeOrderType): Promise<Response<string>> => {
    return (await axiosAuthClient.post('/Order', data)).data;
};

export const getOrdersByUser = async (userID: string): Promise<Response<OrderType[]>> => {
    return (await axiosAuthClient.get('/Order?userID=' + userID)).data;
};

export const getOrdersByShop = async (shopID: string): Promise<Response<OrderType[]>> => {
    return (await axiosAuthClient.get('/Order?shopID=' + shopID)).data;
};