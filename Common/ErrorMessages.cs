using System.Globalization;

namespace BankApi.Common;

public static class ErrorMessages
{
    private static readonly Dictionary<string, Dictionary<string, string>> Messages = new()
    {
        {
            ErrorCodes.UserNotFound, new() {
                { "ru", "Пользователь не найден." },
                { "en", "User not found." },
                { "ky", "Колдонуучу табылган жок." }
            }
        },
        {
            ErrorCodes.UserAlreadyExists, new() {
                { "ru", "Пользователь с таким Email уже зарегистрирован." },
                { "en", "User with this email is already registered." },
                { "ky", "Мындай Email менен колдонуучу мурунтан катталган." }
            }
        },
        {
            ErrorCodes.AccountNotFound, new() {
                { "ru", "Счёт не найден." },
                { "en", "Account not found." },
                { "ky", "Эсеп табылган жок." }
            }
        },
        {
            ErrorCodes.AccountHasBalance, new() {
                { "ru", "Нельзя закрыть счёт с ненулевым балансом. Текущий баланс: {0}." },
                { "en", "Cannot close an account with a non-zero balance. Current balance: {0}." },
                { "ky", "Нөлдүк эмес балансы бар эсепти жабууга болбойт. Учурдагы баланс: {0}." }
            }
        },
        {
            ErrorCodes.AccountHasBalanceNoOtherAccounts, new() {
                { "ru", "Нельзя закрыть счёт. На балансе осталось {0}, и у вас нет других счетов. Пожалуйста, снимите деньги в кассе или банкомате." },
                { "en", "Cannot close the account. There is {0} left, and you have no other accounts. Please withdraw funds via ATM or branch." },
                { "ky", "Эсепти жабууга болбойт. Баланста {0} калды жана башка эсебиңиз жок. Сураныч, акчаны кассадан же банкоматтан чыгарып алыңыз." }
            }
        },
        {
            ErrorCodes.AccountHasBalanceChooseTarget, new() {
                { "ru", "На счёте осталось {0}. Пожалуйста, выберите другой ваш счёт из списка, чтобы мы автоматически перевели остаток средств." },
                { "en", "There is {0} left on the account. Please select another account from the list so we can automatically transfer the remaining funds." },
                { "ky", "Эсепте {0} калды. Калган каражатты автоматтык түрдө которуу үчүн, тизмеден башка эсебиңизди тандаңыз." }
            }
        },
        {
            ErrorCodes.InvalidTargetAccountForTransfer, new() {
                { "ru", "Выбранный счёт для перевода не найден, закрыт или не принадлежит вам." },
                { "en", "The selected account for transfer was not found, is closed, or does not belong to you." },
                { "ky", "Которуу үчүн тандалган эсеп табылган жок, жабык же сизге таандык эмес." }
            }
        },
        {
            ErrorCodes.CurrencyMismatchForCloseTransfer, new() {
                { "ru", "Валюта закрываемого счёта и счёта для перевода не совпадают. Автоматический перевод невозможен." },
                { "en", "The currency of the closing account and the target account do not match. Automatic transfer is not possible." },
                { "ky", "Жабылып жаткан эсептин жана которула турган эсептин валютасы дал келбейт. Автоматтык которуу мүмкүн эмес." }
            }
        },
        {
            ErrorCodes.CardNotFound, new() {
                { "ru", "Карта не найдена." },
                { "en", "Card not found." },
                { "ky", "Карта табылган жок." }
            }
        },
        {
            ErrorCodes.CardBlocked, new() {
                { "ru", "Карта заблокирована." },
                { "en", "Card is blocked." },
                { "ky", "Карта бөгөттөлгөн." }
            }
        },
        {
            ErrorCodes.CardNotActive, new() {
                { "ru", "Карта неактивна." },
                { "en", "Card is not active." },
                { "ky", "Карта активдүү эмес." }
            }
        },
        {
            ErrorCodes.AccountClosed, new() {
                { "ru", "Счёт уже закрыт." },
                { "en", "Account is already closed." },
                { "ky", "Эсеп мурунтан жабылган." }
            }
        },
        {
            ErrorCodes.AccountAlreadyHasActiveCard, new() {
                { "ru", "У данного счета уже есть одна активная карта. Нельзя выпустить вторую активную карту." },
                { "en", "This account already has an active card. Cannot issue a second active card." },
                { "ky", "Бул эсепте активдүү карта бар. Экинчи активдүү картаны чыгарууга болбойт." }
            } 
        },
        {
            ErrorCodes.AccountClosedSuccess, new() {
                { "ru", "Счет успешно закрыт." },
                { "en", "Account has been successfully closed." },
                { "ky", "Эсеп ийгиликтүү жабылды." }
            }
        },
        {
            ErrorCodes.AccountClosedWithTransfer, new() {
                { "ru", "Счет успешно закрыт. Остаток средств в размере {0} был сконвертирован и переведен на счет №{1}." },
                { "en", "Account closed successfully. The remaining balance of {0} was converted and transferred to account No. {1}." },
                { "ky", "Эсеп ийгиликтүү жабылды. {0} өлчөмүндөгү калган каражат конвертацияланып, №{1} эсепке которулду." }
            }
        },
        {
            ErrorCodes.UserAlreadyActive, new() {
                { "ru", "Пользователь уже активен и не нуждается в восстановлении." },
                { "en", "User is already active and does not need to be restored." },
                { "ky", "Колдонуучу мурунтан эле активдүү, калыбына келтирүүнүн кереги жок." }
            }
        },
        {
            ErrorCodes.UserDeactivatedUseRestoreEndpoint, new() {
                { "ru", "Этот аккаунт ранее был деактивирован. Пожалуйста, используйте специальный эндпоинт восстановления." },
                { "en", "This account has been deactivated. Please use the specialized restore endpoint to reactivate it." },
                { "ky", "Бул аккаунт деактивацияланган. Аны калыбына келтирүү үчүн атайын баракчаны колдонуңуз." }
            }
        },
        {
            ErrorCodes.CardCreatedSuccess, new() {
                { "ru", "Карта успешно выпущена." },
                { "en", "Card has been successfully issued." },
                { "ky", "Карта ийгиликтүү чыгарылды." }
            }
        },
        {
            ErrorCodes.CardReissuedSuccess, new() {
                { "ru", "Карта успешно перевыпущена." },
                { "en", "Card has been successfully reissued." },
                { "ky", "Карта ийгиликтүү кайра чыгарылды." }
            }
        },
        {
            ErrorCodes.CardBlockedSuccess, new() {
                { "ru", "Карта успешно заблокирована." },
                { "en", "Card has been successfully blocked." },
                { "ky", "Карта ийгиликтүү бөгөттөлдү." }
            }
        },
        {
            ErrorCodes.CardUnblockedSuccess, new() {
                { "ru", "Карта успешно разблокирована." },
                { "en", "Card has been successfully unblocked." },
                { "ky", "Карта ийгиликтүү бөгөттөн чыгарылды." }
            }
        },
        {
            ErrorCodes.InternalServerError, new() {
                { "ru", "Произошла внутренняя ошибка сервера. Пожалуйста, попробуйте позже." },
                { "en", "An internal server error occurred. Please try again later." },
                { "ky", "Ички сервер катасы кетти. Сураныч, кийинчерээк кайра аракет кылыңыз." }
            }
        },
        {
            ErrorCodes.UnsupportedCurrencyConversion, new() {
                { "ru", "Конвертация между указанными валютами не поддерживается." },
                { "en", "Conversion between the specified currencies is not supported." },
                { "ky", "Көрсөтүлгөн валюталардын ортосунда конвертация колдоого алынбайт." }
            }
        },
        {
            ErrorCodes.UserDeactivatedSuccess, new() {
                { "ru", "Пользователь успешно деактивирован." },
                { "en", "User has been successfully deactivated." },
                { "ky", "Колдонуучу ийгиликтүү деактивацияланды." }
            }
        },
        {
        ErrorCodes.CardNotBelongToUser, new() {
            { "ru", "Карта не принадлежит данному пользователю." },
            { "en", "This card does not belong to the current user." },
            { "ky", "Бул карта учурдагы колдонуучуга таандык эмес." }
        }
    },
        {
            ErrorCodes.InsufficientFunds, new() {
                { "ru", "Недостаточно средств на счёте." },
                { "en", "Insufficient funds on the account." },
                { "ky", "Эсепте каражат жетишсиз." }
            }
        },
        {
            ErrorCodes.TransferSameAccount, new() {
                { "ru", "Нельзя переводить средства на тот же счёт." },
                { "en", "Cannot transfer funds to the same account." },
                { "ky", "Каражатты ошол эле эсепке которууга болбойт." }
            }
        },
        {
            ErrorCodes.TransferSuccess, new() {
                { "ru", "Перевод выполнен успешно." },
                { "en", "Transfer completed successfully." },
                { "ky", "Которуу ийгиликтүү аяктады." }
            }
        },
        {
            ErrorCodes.PhoneNotFound, new() {
                { "ru", "Телефон не найден." },
                { "en", "Phone not found." },
                { "ky", "Телефон табылган жок." }
            }
        },
        {
            ErrorCodes.PhoneAlreadyExists, new() {
                { "ru", "Этот номер телефона уже добавлен." },
                { "en", "This phone number already exists." },
                { "ky", "Бул телефон номери мурунтан кошулган." }
            }
        },
        {
            ErrorCodes.PhoneNotBelongToUser, new() {
                { "ru", "Этот телефон не принадлежит вам." },
                { "en", "This phone does not belong to you." },
                { "ky", "Бул телефон сизге таандык эмес." }
            }
        },
        {
            ErrorCodes.UserAlreadyDeactivated, new() {
                { "ru", "Пользователь уже деактивирован." },
                { "en", "User is already deactivated." },
                { "ky", "Колдонуучу мурунтан эле деактивацияланган." }
            }
        },
        {
            ErrorCodes.AccountAlreadyActive, new() {
                { "ru", "Этот счёт уже активен." },
                { "en", "This account is already active." },
                { "ky", "Бул эсеп мурунтан эле активдүү." }
            }
        },
        {
            ErrorCodes.CannotDeleteLastPhoneNumber, new() {
                { "ru", "Невозможно удалить номер телефона. У клиента должен оставаться как минимум один активный номер." },
                { "en", "Cannot delete phone number. The client must keep at least one active phone number." },
                { "ky", "Телефон номерин өчүрүүгө мүмкүн эмес. Кардардын кеминде бир активдүү номери калышы керек." }
            }
        }
    };

    public static string Get(string errorCode, string lang, params object[]? args)
    {
        if (!Messages.TryGetValue(errorCode, out var translations))
            return $"Error code: {errorCode}";

        if (!translations.TryGetValue(lang, out var message))
            message = translations["ru"]; // Дефолт на русский

        if (args == null || args.Length == 0)
            return message;

        return string.Format(message, args);
    }
    
    public static string Get(string errorCode, params object[]? args)
    {
        string autoLang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
    
        return Get(errorCode, autoLang, args);
    }
}