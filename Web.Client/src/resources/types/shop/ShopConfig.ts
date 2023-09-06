import TimeSpan from "@web-atoms/date-time/src/TimeSpan"

export default interface ShopConfig {
    id: string;
    price: number;
    freeDeliveryAt: number;
    openAt: TimeSpan;
    closeAt: TimeSpan;
    sameDayTillHour: TimeSpan;
}