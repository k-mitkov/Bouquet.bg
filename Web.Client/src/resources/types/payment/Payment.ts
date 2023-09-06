import AddPaymentCard from "./AddPaymentCard"

export default interface Payment {
    orderId: string,
    amount: string,
    cardId: string,
    newCard: AddPaymentCard | undefined
}