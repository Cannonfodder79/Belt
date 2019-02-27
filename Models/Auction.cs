using System;
namespace Belt.Models
{
    public class Auction : BaseEntity
    {
        public int id {get;set;}
        public string Product {get;set;}
        public string Description {get;set;}
        public decimal TopBid {get;set;}
        public DateTime EndDate {get;set;}
        public int CreatorId {get;set;}
        public int TopBidderId {get;set;}
    }
}