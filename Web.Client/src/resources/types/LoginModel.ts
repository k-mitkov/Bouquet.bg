import Token from "./Token";

export default interface LoginModel extends Token {
    id: string,
    profilePictureUrl: string,
    claims: string[]
}