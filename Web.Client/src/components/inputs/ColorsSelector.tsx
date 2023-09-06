import { ChangeEvent, useState } from "react"
import { Popover } from "../ui/popover"
import { PopoverContent, PopoverTrigger } from "@radix-ui/react-popover"
import { Check, ChevronsUpDown } from "lucide-react"
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem } from "../ui/command"
import { cn } from "@/lib/utils"
import { Noop } from "react-hook-form"
import { useTranslation } from "react-i18next"
import { Button } from "../shared/Button"
import { Color } from "../../resources"

interface Props {
    className?: string
    onChange?: (event: Color[] | ChangeEvent<Element>) => void,
    onBlur?: Noop,
    colors: Color[]
}

const initialColors: Color[] = [];

export default function ColorSelect({ className, onChange, onBlur, colors }: Props) {
    const { t } = useTranslation();

    const [open, setOpen] = useState<boolean>(false)
    const [value, setValue] = useState<Color[]>(initialColors)


    function onValueChanged(newValue: Color) {
        let selectedTypes = value;
        if (selectedTypes.find(t => t.id === newValue.id)) {
            selectedTypes = selectedTypes.filter((t) => t.id !== newValue.id);
        } else {
            selectedTypes = selectedTypes.concat(newValue);
        }
        setValue(selectedTypes);
        onChange?.(selectedTypes);
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
                        className="w-[200px] justify-between flex items-center border border-slate-300  rounded-lg px-1">
                        {value == initialColors || value.length == 0 ? t('Filter by colors') : t(value.map(t => t.name).join(", "))}
                        <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                    </Button>
                </PopoverTrigger>

                <PopoverContent>
                    <Command>
                        <CommandInput placeholder={t('Search color')} />
                        <CommandEmpty>{t('No color found')}</CommandEmpty>
                        <CommandGroup>
                            {colors.map((color, index) => (
                                <CommandItem key={index} onSelect={() => {
                                    onValueChanged(color)
                                    setOpen(false);
                                }}>
                                    <Check className={cn('mr-2 h-4 w-4', value.find(t=> t.id === color.id) ? 'opacity-100' : 'opacity-0')} />
                                    {t(color.name)}
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </Command>
                </PopoverContent>
            </Popover>
        </div>
    )
}
