namespace BankApi.Common;

public static class ErrorCodes
{
    public const string AccountNotFound = "ACCOUNT_NOT_FOUND";
    public const string AccountClosed = "ACCOUNT_CLOSED";
    public const string AccountNotBelongToUser = "ACCOUNT_NOT_BELONG_TO_USER";
    public const string AccountHasBalance = "ACCOUNT_HAS_BALANCE";
    public const string AccountAlreadyHasActiveCard = "ACCOUNT_ALREADY_HAS_ACTIVE_CARD";
    public const string AccountHasBalanceNoOtherAccounts = "ACCOUNT_HAS_BALANCE_NO_OTHER_ACCOUNTS";
    public const string AccountHasBalanceChooseTarget = "ACCOUNT_HAS_BALANCE_CHOOSE_TARGET";
    public const string InvalidTargetAccountForTransfer = "INVALID_TARGET_ACCOUNT_FOR_TRANSFER";
    public const string CurrencyMismatchForCloseTransfer = "CURRENCY_MISMATCH_FOR_CLOSE_TRANSFER";
    public const string AccountClosedSuccess = "ACCOUNT_CLOSED_SUCCESS";
    public const string AccountClosedWithTransfer = "ACCOUNT_CLOSED_WITH_TRANSFER";
    public const string AccountAlreadyActive = "ACCOUNT_ALREADY_ACTIVE";
    
    public const string CardNotFound = "CARD_NOT_FOUND";
    public const string CardBlocked = "CARD_BLOCKED";
    public const string CardNotActive = "CARD_NOT_ACTIVE";
    public const string CardCreatedSuccess = "CARD_CREATED_SUCCESS";
    public const string CardBlockedSuccess = "CARD_BLOCKED_SUCCESS";
    public const string CardUnblockedSuccess = "CARD_UNBLOCKED_SUCCESS";
    public const string CardNotBelongToUser = "CARD_NOT_BELONG_TO_USER";
    public const string CardReissuedSuccess = "CARD_REISSUED_SUCCESS";
    
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
    public const string UserAlreadyActive = "USER_ALREADY_ACTIVE";
    public const string UserDeactivatedUseRestoreEndpoint = "USER_DEACTIVATED_USE_RESTORE_ENDPOINT";
    public const string UserDeactivatedSuccess = "USER_DEACTIVATED_SUCCESS";
    public const string UserAlreadyDeactivated = "USER_ALREADY_DEACTIVATED";

    public const string UnsupportedCurrencyConversion = "UNSUPPORTED_CURRENCY_CONVERSION";
    
    public const string InsufficientFunds = "INSUFFICIENT_FUNDS";
    public const string TransferSameAccount = "TRANSFER_SAME_ACCOUNT";
    public const string TransferSuccess = "TRANSFER_SUCCESS";

    public const string CannotDeleteLastPhoneNumber = "CANNOT_DELETE_LAST_PHONE_NUMBER";
    public const string PhoneNotFound = "PHONE_NOT_FOUND";
    public const string PhoneAlreadyExists = "PHONE_ALREADY_EXISTS";
    public const string PhoneNotBelongToUser = "PHONE_NOT_BELONG_TO_USER";
    
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    
}