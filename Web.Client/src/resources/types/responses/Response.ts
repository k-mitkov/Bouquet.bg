import BaseResponse from "./BaseResponse";

export default interface Response<T> extends BaseResponse {
    data: T
}