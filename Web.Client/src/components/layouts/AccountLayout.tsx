import { ProfileTabs } from "../profile"

export default function AccountLayout(props: any) {

    return (
        <div className="flex overflow-hidden justify-center md:mt-5">
            <div className="flex flex-col w-screen md:p-5 bg-secondary-background dark:bg-secondary-backgroud-dark
             min-h-[500px] h-screen md:h-auto rounded-lg text-black dark:text-gray-300">
                <ProfileTabs />
                <div className="h-full mt-2">
                    {props.children}
                </div>
            </div>
        </div>
    )
}