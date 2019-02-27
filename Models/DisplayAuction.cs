namespace Belt.Models
{
    public class DisplayAuction : BaseEntity
    {
        public int id {get;set;}
        public string Product {get;set;}
        public string Description {get;set;}
        public string Seller {get;set;}
        public int SellerId {get;set;}
        public decimal TopBid {get;set;}
        public string TopBidder {get;set;}
        public int TopBidderId {get;set;}
        public string TimeRemaining {get;set;}
    }
}