namespace Belt.Models
{
    public class BaseEntity {}
    
    public class User : BaseEntity
    {
        public int id {get;set;}
        public string Username {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Password {get;set;}
        public decimal Wallet {get;set;}
    }
}