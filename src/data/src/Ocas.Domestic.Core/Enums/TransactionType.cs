namespace Ocas.Domestic.Enums
{
    // CRM OptionSet: ocaslr_financialtransaction_ocaslr_transactiontype
    public enum TransactionType
    {
        Deposit = 2,
        Payment = 1,
        Refund = 5,
        ReleaseFundonDeposit = 8,
        ReleaseOverpayment = 7,
        ReturnedPayment = 3,
        ReverseFullPayment = 4,
        Transfer = 6,
        WriteoffBalance_Deposit = 12,
        WriteoffOverpayment = 11,
        WriteoffReturnPayment = 9,
        WriteoffReturnPaymentAdminFee = 10,
    }
}
