import { StatusEnum } from "../enums/StatusEnum";

export default interface BaseResponse {
    status: StatusEnum,
    message: string
}