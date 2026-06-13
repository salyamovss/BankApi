using System.Net;
using BankApi.Common;
using BankApi.dal.DTOs.Card;
using BankApi.Mappers;
using BankApi.dal.Models.Enums;
using BankApi.dal.Repositories; 
using Microsoft.Extensions.Caching.Memory;

namespace BankApi.Services;

public class CardService(IMemoryCache cache, CardMapper mapper, ICardRepository cardRepository)
{
    public async Task<CardResponseMessage> Create(int userId, CreateCardRequest request)
    {
        var account = await cardRepository.GetAccountWithUserAsync(request.AccountId)
            ?? throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

        if (account.UserId != userId)
            throw new AppException(ErrorCodes.AccountNotBelongToUser, HttpStatusCode.Forbidden);

        if (account.Status == AccountStatus.Closed)
            throw new AppException(ErrorCodes.AccountClosed, HttpStatusCode.Conflict);

        if (await cardRepository.HasActiveCardAsync(request.AccountId))
            throw new AppException(ErrorCodes.AccountAlreadyHasActiveCard, HttpStatusCode.Conflict);

        PaymentSystem paymentSystem = request.Product switch
        {
            CardProduct.VisaGold or CardProduct.VisaPlatinum or CardProduct.VirtualVisa => PaymentSystem.Visa,
            _ => PaymentSystem.Mastercard
        };

        string generatedCardNumber = BankHelper.GenerateCardNumber(paymentSystem);
        string holderName = $"{account.User.FirstName} {account.User.LastName}".ToUpper();

        var card = mapper.ToEntity(request, generatedCardNumber, paymentSystem, holderName);
        await cardRepository.AddAsync(card);
        await cardRepository.SaveChangesAsync();

        InvalidateCache(request.AccountId);
        return mapper.ToCreateResponse(card, ErrorMessages.Get(ErrorCodes.CardCreatedSuccess));
    }

    public async Task<CardResponseMessage> Block(int id, int userId)
    {
        var card = await cardRepository.GetByIdWithAccountAsync(id)
            ?? throw new AppException(ErrorCodes.CardNotFound, HttpStatusCode.NotFound);

        if (card.Account.UserId != userId)
            throw new AppException(ErrorCodes.CardNotBelongToUser, HttpStatusCode.Forbidden);

        if (card.Status == CardStatus.Blocked)
            throw new AppException(ErrorCodes.CardBlocked, HttpStatusCode.Conflict);

        card.Status = CardStatus.Blocked;
        await cardRepository.SaveChangesAsync();

        InvalidateCache(card.AccountId);
        return mapper.ToBlockResponse(card, ErrorMessages.Get(ErrorCodes.CardBlockedSuccess));
    }

    public async Task<CardResponseMessage> Unblock(int id, int userId)
    {
        var card = await cardRepository.GetByIdWithAccountAsync(id)
            ?? throw new AppException(ErrorCodes.CardNotFound, HttpStatusCode.NotFound);

        if (card.Account.UserId != userId)
            throw new AppException(ErrorCodes.CardNotBelongToUser, HttpStatusCode.Forbidden);

        if (card.Status != CardStatus.Blocked)
            throw new AppException(ErrorCodes.CardNotActive, HttpStatusCode.Conflict);

        card.Status = CardStatus.Active;
        await cardRepository.SaveChangesAsync();

        InvalidateCache(card.AccountId);
        return mapper.ToUnblockResponse(card, ErrorMessages.Get(ErrorCodes.CardUnblockedSuccess));
    }

    public async Task<CardResponseMessage> Reissue(int id, int userId)
    {
        var oldCard = await cardRepository.GetByIdWithAccountAndUserAsync(id)
            ?? throw new AppException(ErrorCodes.CardNotFound, HttpStatusCode.NotFound);

        if (oldCard.Account.UserId != userId)
            throw new AppException(ErrorCodes.CardNotBelongToUser, HttpStatusCode.Forbidden);

        if (oldCard.Account.Status == AccountStatus.Closed)
            throw new AppException(ErrorCodes.AccountClosed, HttpStatusCode.Conflict);

        oldCard.Status = CardStatus.Closed;

        var newCard = mapper.ToReissueEntity(oldCard);
        await cardRepository.AddAsync(newCard);
        await cardRepository.SaveChangesAsync();

        InvalidateCache(oldCard.AccountId);
        return mapper.ToCreateResponse(newCard, ErrorMessages.Get(ErrorCodes.CardReissuedSuccess));
    }

    public async Task<List<CardResponse>> GetByAccountId(int accountId)
    {
        if (!await cardRepository.AccountExistsAsync(accountId))
            throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

        var cacheKey = $"cards_account_{accountId}";

        if (!cache.TryGetValue(cacheKey, out List<CardResponse>? cards))
        {
            var cardEntities = await cardRepository.GetByAccountIdAsync(accountId);
            cards = cardEntities.Select(c => mapper.ToResponse(c)).ToList();
            cache.Set(cacheKey, cards, TimeSpan.FromMinutes(5));
        }

        return cards!;
    }

    public async Task<CardResponse> GetById(int id)
    {
        var card = await cardRepository.GetByIdWithAccountAsync(id)
            ?? throw new AppException(ErrorCodes.CardNotFound, HttpStatusCode.NotFound);

        return mapper.ToResponse(card);
    }

    /// <summary>
    /// Сбрасывает кэш карт счёта после любого изменения (выпуск, блокировка, перевыпуск).
    /// </summary>
    private void InvalidateCache(int accountId) => cache.Remove($"cards_account_{accountId}");
}