import { axiosClient, Response, axiosAuthClient, FlowerShop, AddShopType, BaseResponse, AddWorker, Worker } from '../../index'

export const getShops = async (): Promise<Response<FlowerShop[]>> => {
    return (await axiosClient.get('/FlowerShop')).data;
};

export const getShop = async (shopID: string): Promise<Response<FlowerShop>> => {
    return (await axiosClient.get('/FlowerShop?shopID=' + shopID)).data;
};


export const getMyShops = async (): Promise<Response<FlowerShop[]>> => {
    return (await axiosAuthClient.get('/FlowerShop/my-shops')).data;
};

export const addShop = async (data: AddShopType): Promise<Response<string>> => {
    return (await axiosAuthClient.post('/FlowerShop', data)).data;
};


export const getWorkers = async (shopID: string): Promise<Response<Worker[]>> => {
    return (await axiosAuthClient.get('/FlowerShop/workers?shopID=' + shopID)).data;
};

export const addWorker = async (data: AddWorker): Promise<BaseResponse> => {
    return (await axiosAuthClient.post('/FlowerShop/add-worker', data)).data;
};

export const removeWorker = async (objectID: string, workerId: string): Promise<BaseResponse> => {
    return (await axiosAuthClient.delete('/FlowerShop/remove-worker?objectID=' + objectID + '&workerId=' + workerId)).data;
};