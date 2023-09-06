import { Token } from "../resources";
import LocalStorageService from "./LocalStorageService";
import { ProfilePicture, UserConstants, UserID } from "../utilities/Constants";

export function getUserTokens(): Token {
    const access: string = LocalStorageService.getItemFromLocalStorage(UserConstants.AccessToken) || "";
    const refresh: string = LocalStorageService.getItemFromLocalStorage(UserConstants.RefreshToken) || "";

    const token: Token = {
        accessToken: access,
        refreshToken: refresh
    }
    return token;
}

export function getUserID(): string {
    const id: string = LocalStorageService.getItemFromLocalStorage(UserID) || "";

    return id;
}

export function getProfilePictureUrl(): string {
    const profilePictureUrl: string = LocalStorageService.getItemFromLocalStorage(ProfilePicture) || "";

    return profilePictureUrl;
}

export function setUserAccessToken(accessToken: string): any {
    LocalStorageService.setItemInLocalStorage(UserConstants.AccessToken, accessToken);
}

export function setUserRefreshToken(refreshToken: string): any {

    LocalStorageService.setItemInLocalStorage(UserConstants.RefreshToken, refreshToken);
}

export function setUserTokens(tokens: Token): any {
    setUserAccessToken(tokens.accessToken);
    setUserRefreshToken(tokens.refreshToken);
}

export function setUserID(id: string) {
    LocalStorageService.setItemInLocalStorage(UserID, id);
}

export function setProfilePictureUrl(profilePictureUrl: string) {
    LocalStorageService.setItemInLocalStorage(ProfilePicture, profilePictureUrl);
}

export function removeUserTokens(): any {
    LocalStorageService.removeItemFromLocalStorage(UserConstants.AccessToken);
    LocalStorageService.removeItemFromLocalStorage(UserConstants.RefreshToken);
}

export function removeUserID() {
    LocalStorageService.removeItemFromLocalStorage(UserID);
}

export function removeProfilePictureUrl() {
    LocalStorageService.removeItemFromLocalStorage(ProfilePicture);
}