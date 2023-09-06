import { axiosClient, Response, Color } from '../../index'

export const getColors = async (): Promise<Response<Color[]>> => {
    return (await axiosClient.get('/Colors')).data;
};