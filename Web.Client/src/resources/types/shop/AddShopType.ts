import TimeSpan from "@web-atoms/date-time/src/TimeSpan";

export default interface AddShopType {
    name: string;
    latitude: number;
    longitude: number;
    address: string;
    pictureDataUrl: string;
    cityID: string;
    price: number;
    freeDeliveryAt: number;
    openAt: TimeSpan;
    closeAt: TimeSpan;
    sameDayTillHour: TimeSpan;
}