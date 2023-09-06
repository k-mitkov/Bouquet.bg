import { axiosClient, ResetPasswordData, UserCredentials, Token, Response, BaseResponse, LoginModel, axiosAuthClient, UserInfo, RegisterInfo } from '../index'

export const login = async (data: UserCredentials): Promise<Response<LoginModel>> => {
        return (await axiosClient.post('/authentication/login', data)).data;
};

export const register = async (data: RegisterInfo): Promise<BaseResponse> => {
        return (await axiosClient.post('/authentication/register', data)).data;
};

export const resetPassword = async (data: ResetPasswordData): Promise<BaseResponse> => {
        return (await axiosClient.post('/user/reset-password', data)).data;
};

export const forgotPassword = async (email: string): Promise<BaseResponse> => {
        return (await axiosClient.post('/user/forgot-password', null, {
                params: { email: email },
        })).data;
};

export const requestRefreshToken = async (tokens: Token): Promise<Response<Token>> => {
        return (await axiosClient.post(`/authentication/refresh-token`, tokens)).data;
}

export const getUserClaims = async (): Promise<Response<string[]>> => {
        return (await axiosAuthClient.get('/permissions')).data;
}

export const getUserInfo = async (): Promise<Response<UserInfo>> => {
        return (await axiosAuthClient.get('/account/user-info')).data;
}

export const updateUserInfo = async (userInfo: UserInfo): Promise<Response<UserInfo>> => {
        return (await axiosAuthClient.put('/account/user-info', userInfo)).data;
}

export const checkEmail = async (email: string): Promise<BaseResponse> => {
        return (await axiosClient.get('/account/check-email?email=' + email)).data;
}