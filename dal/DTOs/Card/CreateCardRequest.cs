using System.ComponentModel.DataAnnotations;
using BankApi.dal.Models.Enums;

namespace BankApi.dal.DTOs.Card;

public class CreateCardRequest
{
    [Required]
    public CardProduct Product { get; set; }

    [Required]
    public int AccountId { get; set; }
}