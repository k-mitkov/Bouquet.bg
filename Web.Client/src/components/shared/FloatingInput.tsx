import { HTMLInputTypeAttribute } from "react";

interface Props {
    inputId: string,
    placeholder: string,
    type: HTMLInputTypeAttribute,
    validate?: (value: string) => boolean,
    validateOnLostFocus?: boolean,
    immediateValdation?: boolean,
    validationMessage?: string,
    isPassword?: boolean,
    initialValue?: string
}