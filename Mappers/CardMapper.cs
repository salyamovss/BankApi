using BankApi.Common;
using BankApi.dal.DTOs.Card;
using BankApi.dal.Models;
using BankApi.dal.Models.Enums;

namespace BankApi.Mappers;

public class CardMapper
{
    public CardResponseMessage ToCreateResponse(Card card, string message)
    {
        string detailedMessage = $"{message} {card.HolderName}, {card.Product} ({BankHelper.MaskPan(card.CardNumber)}).";
        return new CardResponseMessage { Message = detailedMessage, Card = ToResponse(card) };
    }

    public CardResponseMessage ToBlockResponse(Card card, string message)
    {
        string detailedMessage = $"{message} (Окончание номера: {card.CardNumber[^4..]}).";
        return new CardResponseMessage { Message = detailedMessage, Card = ToResponse(card) };
    }

    public CardResponseMessage ToUnblockResponse(Card card, string message)
    {
        string detailedMessage = $"{message} (Окончание номера: {card.CardNumber[^4..]}).";
        return new CardResponseMessage { Message = detailedMessage, Card = ToResponse(card) };
    }

    public CardResponse ToResponse(Card card) => new()
    {
        Id = card.Id,
        CardNumber = BankHelper.MaskPan(card.CardNumber),
        Product = card.Product.ToString(),
        PaymentSystem = card.PaymentSystem.ToString(),
        HolderName = card.HolderName,
        Status = card.Status.ToString(),
        ExpiryDate = card.ExpiryDate,
        CreatedAt = card.CreatedAt,
        AccountId = card.AccountId
    };

    public Card ToEntity(CreateCardRequest request, string cardNumber, PaymentSystem paymentSystem, string holderName) => new()
    {
        CardNumber = cardNumber,
        Product = request.Product,
        PaymentSystem = paymentSystem,
        HolderName = holderName,
        ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5)),
        AccountId = request.AccountId,
        Status = CardStatus.Active,
        CreatedAt = DateTime.UtcNow
    };
    
    public Card ToReissueEntity(Card oldCard) => new()
    {
        CardNumber = BankHelper.GenerateCardNumber(oldCard.PaymentSystem),
        Product = oldCard.Product,
        PaymentSystem = oldCard.PaymentSystem,
        HolderName = oldCard.HolderName,
        ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5)),
        AccountId = oldCard.AccountId,
        Status = CardStatus.Active,
        CreatedAt = DateTime.UtcNow
    };
    
}