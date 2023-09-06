import { ChangeEvent, useState } from "react"
import { Popover } from "../ui/popover"
import { PopoverContent, PopoverTrigger } from "@radix-ui/react-popover"
import { Check, ChevronsUpDown } from "lucide-react"
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem } from "../ui/command"
import { cn } from "@/lib/utils"
import { Noop } from "react-hook-form"
import { useTranslation } from "react-i18next"
import { Button } from "../shared/Button"
import { Flower } from "../../resources"

interface Props {
    className?: string
    onChange?: (event: Flower[] | ChangeEvent<Element>) => void,
    onBlur?: Noop,
    flowers: Flower[]
}

const initialFlowers: Flower[] = [];

export default function FlowerSelect({ className, onChange, onBlur, flowers }: Props) {
    const { t } = useTranslation();

    const [open, setOpen] = useState<boolean>(false)
    const [value, setValue] = useState<Flower[]>(initialFlowers)


    function onValueChanged(newValue: Flower) {
        let selectedFlowers = value;
        if (selectedFlowers.find(t => t.id === newValue.id)) {
            selectedFlowers = selectedFlowers.filter((t) => t.id !== newValue.id);
        } else {
            selectedFlowers = selectedFlowers.concat(newValue);
        }
        setValue(selectedFlowers);
        onChange?.(selectedFlowers);
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
                        {value == initialFlowers || value.length == 0 ? t('Filter by flowers') : t(value.map(t => t.name).join(", "))}
                        <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                    </Button>
                </PopoverTrigger>

                <PopoverContent>
                    <Command>
                        <CommandInput placeholder={t('Search flower')} />
                        <CommandEmpty>{t('No flower found')}</CommandEmpty>
                        <CommandGroup>
                            {flowers.map((flower, index) => (
                                <CommandItem key={index} onSelect={() => {
                                    onValueChanged(flower)
                                    setOpen(false);
                                }}>
                                    <Check className={cn('mr-2 h-4 w-4', value.find(t=> t.id === flower.id) ? 'opacity-100' : 'opacity-0')} />
                                    {t(flower.name)}
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </Command>
                </PopoverContent>
            </Popover>
        </div>
    )
}
