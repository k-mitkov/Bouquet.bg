
const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
const years = ['2023', '2024', '2025', '2026', '2027', '2028', '2029', '2030', '2031', '2032']

type MonthsType = typeof months[number]
type YearsType = typeof years[number]

type cardType = 'None' | 'JCB' | 'American Express' | 'Diners Club' | 'Visa' | 'MasterCard' | 'Discover'

export default interface AddPaymentCard {
    cardNumber: string,
    cardholderName: string,
    month: MonthsType,
    year: YearsType,
    cardType: cardType,
    cvv: string
}