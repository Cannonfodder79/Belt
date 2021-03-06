using System;
using System.ComponentModel.DataAnnotations;
namespace Belt.Models
{
    public class AuctionValidator : BaseEntity
    {
        [Required(ErrorMessage = "Product name is required.")]
        [MinLength(3, ErrorMessage = "Product name must be at least 3 characters.")]
        public string Product {get;set;}
        [Required(ErrorMessage = "Description is required.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters.")]
        public string Description {get;set;}
        [Required(ErrorMessage = "Starting bid is required.")]
        [DataType(DataType.Currency, ErrorMessage = "Invalid starting bid format.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "The starting bid cannot be $0.00.")]
        public string StartingBid {get;set;}
        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid end date format.")]
        public DateTime EndDate {get;set;}
    }
}