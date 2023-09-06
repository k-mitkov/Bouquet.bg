
import UserCredentials from "../UserCredentials";
import UserInfo from "./UserInfo";

export default interface RegisterInfo {
    userInfo: UserInfo,
    userCredentials: UserCredentials
}