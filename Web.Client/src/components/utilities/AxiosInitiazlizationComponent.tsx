import { useEffect } from "react";
import { Token, axiosAuthClient, axiosClient, requestRefreshToken } from "../../resources";
import { AxiosRequestConfig } from "axios";
import { useUser } from "../../stateProviders/userContext/UserContext";
import { useNavigate } from "react-router-dom";
import { getUserTokens, removeUserTokens, setUserTokens } from "../../services/LocalStorageFunctions";

export default function AxiosInitiazlizationComponent() {
    const { setUser } = useUser();
    const navigate = useNavigate();
    let isRefreshing = false;
    let refreshSubscribers: ((user: Token) => void)[] = [];

    function setAxiosClientsBaseURL() {
        axiosAuthClient.defaults.baseURL = import.meta.env.VITE_IDENTITY_API_URL;
        axiosClient.defaults.baseURL = import.meta.env.VITE_IDENTITY_API_URL;
    }

    async function initAuthAxios() {

        axiosAuthClient.interceptors.request.use(async (config) => {

            var tokens = getUserTokens();
            config.headers['Authorization'] = 'Bearer ' + tokens.accessToken;
            config.headers['RefreshToken'] = tokens.refreshToken;

            return config;
        })

        axiosAuthClient.interceptors.response.use(
            function (response) {
                return response;
            },
            async function (error) {
                const config: AxiosRequestConfig = error.config;
                if (error.response?.status === 401) {
                    if (isRefreshing) {
                        try {
                            const userTokens = await new Promise<Token>((resolve) => {
                                refreshSubscribers.push(user => resolve(user));
                            });
                            config.headers!['Authorization'] = 'Bearer ' + userTokens.accessToken;
                            config.headers!['RefreshToken'] = userTokens.refreshToken;
                            return axiosClient.request(config);
                        } catch (e) {
                            return Promise.reject(e);
                        }
                    }

                    isRefreshing = true;
                    var tokens = getUserTokens();
                    try {

                        var response = await requestRefreshToken(tokens);

                        var data = response.data as Token;
                        setUser(prev => {
                            return {
                                ...prev,
                                tokens: {
                                    refreshToken: data.refreshToken,
                                    accessToken: data.accessToken
                                }
                            }
                        });
                        setUserTokens(data);
                        config.headers!['Authorization'] = 'Bearer ' + data.accessToken;
                        config.headers!['RefreshToken'] = data.refreshToken;

                        refreshSubscribers.forEach((subscriber) => subscriber(data));
                        refreshSubscribers = [];

                        return axiosClient.request(config);
                    }
                    catch (err) {
                        removeUserTokens();
                        navigate('/login');
                    }
                    finally {
                        isRefreshing = false;
                    }
                }
                return Promise.reject(error);
            }
        );
    }

    useEffect(() => {
        setAxiosClientsBaseURL();
        initAuthAxios();
    }, [])

    return (
        <>
        </>
    )
}