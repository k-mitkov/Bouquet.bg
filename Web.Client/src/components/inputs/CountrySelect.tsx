import { ChangeEvent, useEffect, useState } from "react"
import { Popover } from "../ui/popover"
import { PopoverContent, PopoverTrigger } from "@radix-ui/react-popover"
import { Check, ChevronsUpDown } from "lucide-react"
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem } from "../ui/command"
import { cn } from "@/lib/utils"
import { Noop } from "react-hook-form"
import { useTranslation } from "react-i18next"
import { Button } from "../shared/Button"

const countries = ['Bulgaria', 'Serbia', 'Romania', 'Greece', 'Macedonia', 'Turkey']

interface Props {
    className?: string
    onChange?: (event: string | ChangeEvent<Element>) => void,
    onBlur?: Noop,
    defaultValue?: string
}

export default function CountrySelect({ className, onChange, onBlur, defaultValue }: Props) {
    const { t } = useTranslation();

    const [open, setOpen] = useState<boolean>(false)
    const [value, setValue] = useState("")

    useEffect(() => {
        setValue(defaultValue || '');
        onChange?.(defaultValue || '');
    })

    function onValueChanged(newValue: string) {
        var valueToSet = newValue === value ? "" : newValue;
        setValue(valueToSet)
        onChange?.(valueToSet);
    }

    return (
        <div className={className}>
            <Popover>
                <PopoverTrigger asChild>
                    <Button
                        variant={'outline'}
                        size={'sm'}
                        aria-expanded={open}
                        onBlur={onBlur}
                        className="w-[200px] justify-between flex items-center border border-black dark:border-white rounded-lg px-1">
                        {value == '' ? t('Select country') : t(value)}
                        <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                    </Button>
                </PopoverTrigger>

                <PopoverContent>
                    <Command>
                        <CommandInput placeholder={t('Search country')} />
                        <CommandEmpty>{t('No country found')}</CommandEmpty>
                        <CommandGroup>
                            {countries.map((country, index) => (
                                <CommandItem key={index} onSelect={() => {
                                    onValueChanged(country)
                                    setOpen(false);
                                }}>
                                    <Check className={cn('mr-2 h-4 w-4', value === country ? 'opacity-100' : 'opacity-0')} />
                                    {t(country)}
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </Command>
                </PopoverContent>
            </Popover>
        </div>
    )
}