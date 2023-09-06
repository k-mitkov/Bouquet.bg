import { useState } from "react"
import { Skeleton } from "../ui/skeleton"
import { cn } from "@/lib/utils";

interface Props {
    imageSrc: string,
    imageClassName?: string,
    skeletonClassName?: string
}

export default function Image({ imageSrc, imageClassName, skeletonClassName }: Props) {
    const [isImageLoaded, setIsImageLoaded] = useState<boolean>(false);

    return (
        <div>
            <img src={imageSrc} className={cn(`${!isImageLoaded ? 'hidden' : ''}`, imageClassName)} onLoad={() => setIsImageLoaded(true)} />
            <Skeleton className={skeletonClassName} hidden={isImageLoaded} />
        </div>
    )
}