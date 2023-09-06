import { axiosClient, Response, Bouquet, axiosAuthClient } from '../../index'

export const getBouquets = async (cityID: string): Promise<Response<Bouquet[]>> => {
    return (await axiosClient.get('/Bouquet?cityID=' + cityID)).data;
};

export const getBouquetsByShop = async (shopID: string): Promise<Response<Bouquet[]>> => {
    return (await axiosClient.get('/Bouquet?shopID=' + shopID)).data;
};

export const addBouquet = async (data: Bouquet): Promise<Response<string>> => {
    return (await axiosAuthClient.post('/Bouquet', data)).data;
};