import * as React from 'react'

import { cn } from '@/lib/utils'

export interface InputProps
    extends React.InputHTMLAttributes<HTMLInputElement> { }

const Input = React.forwardRef<HTMLInputElement, InputProps>(


    ({ className, ...props }, ref) => {
        return (
            <input
                className={cn(
                    'flex h-10 w-full rounded-md border text-main-font dark:text-main-background-dark border-slate-300 bg-transparent py-2 px-3 text-sm placeholder:main-font focus:outline-none  focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 dark:border-main-font-dark dark:text-main-font-dark dark:focus:ring-slate-400 dark:focus:ring-offset-slate-900',
                    className
                )}
                ref={ref}
                {...props}
            />
        )
    }
)
Input.displayName = 'Input'

export { Input }