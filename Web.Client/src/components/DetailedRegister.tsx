import { RegisterData } from "../resources"
import { CompanyRegister } from "./CompanyRegister";
import { PrivatePersonRegister } from "./PrivatePersonRegister";

interface Props {
    data: RegisterData
    isCompany: boolean
}

export const DetailedRegister: React.FC<Props> = ({data, isCompany} : Props) =>
{

    return (
            <div>
                {isCompany ?
                <CompanyRegister registerData={data} />
                :
                <PrivatePersonRegister registerData={data} />}
            </div>
    );
};