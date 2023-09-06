import { axiosClient, Response, Flower } from '../../index'

export const getFlowers = async (): Promise<Response<Flower[]>> => {
    return (await axiosClient.get('/Flower')).data;
};