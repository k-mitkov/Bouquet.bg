import LocalStorageService from "../services/LocalStorageService"
import { Token } from '../resources';

function saveAuthInfoToLocalStorage(xd: Token): void {

    LocalStorageService.setItemInLocalStorage('Bearer', xd.accessToken);
    LocalStorageService.setItemInLocalStorage('RefreshToken', xd.refreshToken);
    //localStorageService.setItemInLocalStorage('Expiration', authInfo.expiration);
}

function getAuthInfoFromLocalStorage(): Token {
    const bearer: string = LocalStorageService.getItemFromLocalStorage('Bearer') || "";
    const refreshToken: string = LocalStorageService.getItemFromLocalStorage('RefreshToken') || "";
    const refresh: Token = {
        accessToken: bearer,
        refreshToken: refreshToken
    }
    return refresh;
}

export { saveAuthInfoToLocalStorage, getAuthInfoFromLocalStorage };