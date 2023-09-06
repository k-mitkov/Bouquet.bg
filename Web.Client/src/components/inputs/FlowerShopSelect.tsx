import { ChangeEvent, useState } from "react"
import { Popover } from "../ui/popover"
import { PopoverContent, PopoverTrigger } from "@radix-ui/react-popover"
import { Check, ChevronsUpDown } from "lucide-react"
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem } from "../ui/command"
import { cn } from "@/lib/utils"
import { Noop } from "react-hook-form"
import { useTranslation } from "react-i18next"
import { Button } from "../shared/Button"
import { FlowerShop } from "../../resources"

interface Props {
    className?: string
    onChange?: (event: string | ChangeEvent<Element>) => void,
    onBlur?: Noop,
    shops: FlowerShop[]
}

const initialShop: FlowerShop = {
    address: '',
    latitude: 0,
    longitude: 0,
    name: '',
    id: '',
    pictureDataUrl: '',
    cityID: '',
    ownerId: '',
    workers: []
}

export default function FlowerShopSelect({ className, onChange, onBlur, shops }: Props) {
    const { t } = useTranslation();

    const [open, setOpen] = useState<boolean>(false)
    const [value, setValue] = useState<FlowerShop | undefined>(initialShop)


    function onValueChanged(newValue: FlowerShop) {
        if (newValue === value){
            setValue(undefined);
            onChange?.("");
        }else{
            setValue(newValue)
            onChange?.(newValue.id);
        }
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
                        {value == initialShop || value == undefined ? t('Select shop') : t(value?.name)}
                        <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                    </Button>
                </PopoverTrigger>

                <PopoverContent>
                    <Command>
                        <CommandInput placeholder={t('Search shop')} />
                        <CommandEmpty>{t('No shop found')}</CommandEmpty>
                        <CommandGroup>
                            {shops != undefined && shops.map((shop, index) => (
                                <CommandItem key={index} onSelect={() => {
                                    onValueChanged(shop)
                                    setOpen(false);
                                }}>
                                    <Check className={cn('mr-2 h-4 w-4', value === shop ? 'opacity-100' : 'opacity-0')} />
                                    {t(shop.name)}
                                </CommandItem>
                            ))}
                        </CommandGroup>
                    </Command>
                </PopoverContent>
            </Popover>
        </div>
    )
}
