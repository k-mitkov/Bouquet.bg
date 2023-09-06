import { useState } from "react";
import { Link } from "react-router-dom";
import { Avatar, AvatarFallback, AvatarImage } from "../ui/avatar";
import { Popover, PopoverContent, PopoverTrigger } from "../ui/popover";
import { useTranslation } from 'react-i18next';
import { useUser } from "@/stateProviders/userContext";

export default function ProfileAvatar() {
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const { user, logout } = useUser();
    const { t } = useTranslation();

    return (
        <Popover open={isOpen} onOpenChange={() => setIsOpen(open => !open)}>
            <PopoverTrigger asChild>
                <Avatar className="cursor-pointer">
                    <AvatarImage src={user?.userImageUrl} alt="@shadcn" />
                    <AvatarFallback></AvatarFallback>
                </Avatar>
            </PopoverTrigger>
            <PopoverContent className="w-48" align="end" onOpenAutoFocus={(e) => e.preventDefault()}>
                <div className="grid gap-4">
                    <div className="flex flex-col space-y-2 items-center">
                        <Link to='/profile'
                            className="text-black dark:text-white w-full py-1.5 text-center 
                            rounded hover:bg-custom-gray hover:text-blue-600 dark:hover:text-blue-600"
                            onClick={() => setIsOpen(false)}>
                            {t('Profile')}
                        </Link>
                        <button
                            className="text-black dark:text-white w-full bg-transparent border-none border-0 
                                        rounded py-1.5 hover:bg-custom-gray hover:text-blue-600 dark:hover:text-blue-600"
                            onClick={() => logout()}>
                            {t('Logout')}
                        </button>
                    </div>
                </div>
            </PopoverContent>
        </Popover>
    )
}