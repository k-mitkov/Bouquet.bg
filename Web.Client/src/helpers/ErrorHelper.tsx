import axios, { AxiosError } from "axios";
import { BaseResponse } from "../resources";
import { TFunction } from "react-i18next";

export function showError(error: unknown, toast: ({ ...props }: any) => void, t: TFunction<"translation", undefined>) {
  if (axios.isAxiosError(error)) {
    const axiosError = (error as AxiosError);
    const errorMessage = (axiosError.response?.data as BaseResponse).message

    toast({
      description: t(errorMessage) || 'An error occurred.',
      variant: 'info'
    })
  } else {
    console.log(error)
  }
}