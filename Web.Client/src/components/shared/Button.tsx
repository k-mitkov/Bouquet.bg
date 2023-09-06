import { ButtonHTMLAttributes, forwardRef } from 'react';
import { VariantProps, cva } from 'class-variance-authority'
import { cn } from '@/helpers/utils';

interface Props extends ButtonHTMLAttributes<HTMLButtonElement>, VariantProps<typeof buttonVariants> { }

const buttonVariants = cva(
    'inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-slate-400 focus:ring-offset-2 dark:hover:bg-green-800 dark:hover:text-slate-100 disabled:opacity-50 dark:focus:ring-slate-400 disabled:pointer-events-none dark:focus:ring-offset-green-900 data-[state=open]:slate-300 dark:data-[state=open]:bg-green-900',
    {
        variants: {
            variant: {
                default:
                    'bg-button  text-white dark:hover:bg-button-hover hover:bg-button-hover dark:text-slate-100',
                outline:
                    'bg-transparent  dark:text-button-color border border-button hover:bg-transparent',
                link:
                    'block py-2 pl-3 pr-4 mt-2 text-white bg-blue-700 hover:transparent rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500'

            },
            size: {
                default: 'py-2 px-4',
                sm: 'py-1 px-2 rounded-md',
                lg: 'py-2 px-8 rounded-md'
            },
        },
        defaultVariants: {
            variant: 'default',
            size: 'default'
        }
    }
)

const Button = forwardRef<HTMLButtonElement, Props>(({ className, size, variant, ...props }: Props, ref) => {
    return <button ref={ref} className={cn(buttonVariants({ variant, size, className }))} {...props}></button>
})

export { Button, buttonVariants }
