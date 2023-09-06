import { useTitle } from "@/hooks";
import { useTranslation } from "react-i18next"

export default function NotFoundPage() {
    const { t } = useTranslation();
    useTitle(t('Page not found'));


    return (
        <div className="flex w-full h-screen items-center justify-center">
            <h1 className="text-black dark:text-white">{t('Page not found').toLocaleUpperCase()}</h1>
        </div>
    )
}