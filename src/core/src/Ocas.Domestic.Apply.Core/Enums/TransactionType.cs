namespace Ocas.Domestic.Apply.Enums
{
    public enum TransactionType
    {
        Payment = 1,
        Deposit = 2,
        ReturnedPayment = 3,
        ReverseFullPayment = 4,
        Refund = 5,
        Transfer = 6,
        ReleaseOverpayment = 7,
        ReleaseFundOnDeposit = 8,
        WriteOffReturnPayment = 9,
        WriteOffReturnPaymentAdminFee = 10,
        WriteOffOverpayment = 11,
        WriteOffBalanceDeposit = 12
    }
}