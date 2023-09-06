import { axiosClient, Response, City } from '../../index'

export const getCities = async (): Promise<Response<City[]>> => {
    return (await axiosClient.get('/City')).data;
};