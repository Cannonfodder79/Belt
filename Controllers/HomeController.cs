using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Belt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Belt.Controllers
{
    public class HomeController : Controller
    {
        private AuctionContext _context;
 
        public HomeController(AuctionContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegValidator model)
        {
            if(ModelState.IsValid)
            {
                List<User> users = _context.Users.ToList();
                User existing = users.SingleOrDefault(u => u.Username == model.Username);
                if(existing == null)
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    User newUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Username = model.Username,
                        Wallet = 1000
                    };
                    newUser.Password = hasher.HashPassword(newUser, model.Password);
                    _context.Add(newUser);
                    _context.SaveChanges();
                    users = _context.Users.ToList();
                    User justCreated = users.Single(u => u.Username == newUser.Username);
                    HttpContext.Session.SetInt32("id", justCreated.id);
                    HttpContext.Session.SetString("name", justCreated.FirstName);
                    return RedirectToAction("Main");
                }
                ModelState.AddModelError("Username", "This Username is already in use.");              
            }
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginValidator model)
        {
            if(ModelState.IsValid)
            {
                List<User> users = _context.Users.ToList();
                User attemptedLogin = users.SingleOrDefault(u => u.Username == model.LogUsername);
                if(attemptedLogin == null) 
                {
                    ModelState.AddModelError("LogUsername", "This Username does not exist.");
                }
                else
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    if(0 != hasher.VerifyHashedPassword(attemptedLogin, attemptedLogin.Password, model.LogPassword))
                    {
                        HttpContext.Session.SetInt32("id", attemptedLogin.id);
                        HttpContext.Session.SetString("name", attemptedLogin.FirstName);
                        return RedirectToAction("Main");
                    }
                    else ModelState.AddModelError("LogPassword", "Incorrect Password.");
                }
            }
            return View("Index");
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public bool IsNotLogged()
        {
            if(HttpContext.Session.GetInt32("id") != null) return false;
            return true;
        }

        [Route("main")]
        public IActionResult Main()
        {
            if(IsNotLogged()) return RedirectToAction("Index");
            ResolveAllAuctions();
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.id = HttpContext.Session.GetInt32("id");
            List<Auction> auctions = _context.Auctions.ToList();
            auctions = auctions.OrderBy(a => a.EndDate).ToList();
            List<User> users = _context.Users.ToList();
            List<DisplayAuction> displayAuctions = new List<DisplayAuction>();
            foreach(Auction a in auctions)
            {
                int days = (a.EndDate - DateTime.Now).Days;
                int hours = (a.EndDate - DateTime.Now).Hours;
                int minutes = (a.EndDate - DateTime.Now).Minutes;
                string TimeRemaining = $"{days} days, {hours} hours, {minutes} minutes";
                displayAuctions.Add(new DisplayAuction{
                    id = a.id,
                    Product = a.Product,
                    Seller = users.SingleOrDefault(u => u.id == a.CreatorId).FirstName,
                    SellerId = a.CreatorId,
                    TopBid = a.TopBid,
                    TimeRemaining = TimeRemaining
                });
            }
            ViewBag.Wallet = _context.Users.SingleOrDefault(u => u.id == HttpContext.Session.GetInt32("id")).Wallet;
            return View(displayAuctions);
        }

        [Route("create_auction")]
        public IActionResult NewAuction()
        {
            if(IsNotLogged()) return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        [Route("create_action/submit")]
        public IActionResult Create(AuctionValidator model)
        {
            if(IsNotLogged()) return RedirectToAction("Index");
            if(ModelState.IsValid)
            {
                if(model.EndDate <= DateTime.Today) ModelState.AddModelError("EndDate", "End date must be in the future.");
                if(Double.Parse(model.StartingBid) < 1.00) ModelState.AddModelError("StartingBid", "Starting bid must be at least $1.");
                if(ModelState.IsValid)
                {
                    Auction newAuction = new Auction
                    {
                        Product = model.Product,
                        Description = model.Description,
                        EndDate = model.EndDate,
                        TopBid = decimal.Parse(model.StartingBid),
                        CreatorId = (int)HttpContext.Session.GetInt32("id"),
                    };
                    _context.Add(newAuction);
                    _context.SaveChanges();
                    return RedirectToAction("Main");
                }
            }
            return View("NewAuction");
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            Auction target = _context.Auctions.SingleOrDefault(a => a.id == id);
            if(target == null) return RedirectToAction("Main");
            if(target.CreatorId != HttpContext.Session.GetInt32("id")) return RedirectToAction("Main");
            if(target.TopBidderId != 0)
            {
                User topBidder = _context.Users.SingleOrDefault(u => u.id == target.TopBidderId);
                topBidder.Wallet += target.TopBid;
                _context.Update(topBidder);
            }
            _context.Remove(target);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }
        
        [Route("auction/{id}")]
        public IActionResult DisplayAuction(int id)
        {
            if(IsNotLogged()) return RedirectToAction("Index");
            ResolveAuction(id);
            Auction auction = _context.Auctions.SingleOrDefault(a => a.id == id);
            if(auction == null) return RedirectToAction("Main");
            List<User> users = _context.Users.ToList();
            User Seller = users.SingleOrDefault(u => u.id == auction.CreatorId);
            string Seller_name = $"{Seller.FirstName} {Seller.LastName}";
            int days = (auction.EndDate - DateTime.Now).Days;
            int hours = (auction.EndDate - DateTime.Now).Hours;
            int minutes = (auction.EndDate - DateTime.Now).Minutes;
            string TimeRemaining = $"{days} days, {hours} hours, {minutes} minutes";
            DisplayAuction displayAuction = new DisplayAuction
            {
                id = auction.id,
                Product = auction.Product,
                Description = auction.Description,
                Seller = Seller_name,
                SellerId = auction.CreatorId,
                TopBid = auction.TopBid,
                TopBidderId = auction.TopBidderId,
                TimeRemaining = TimeRemaining
            };
            User TopBidder = users.SingleOrDefault(u => u.id == auction.TopBidderId);
            if(TopBidder != null)
            {
                string TopBidder_name = $"{TopBidder.FirstName} {TopBidder.LastName}";
                displayAuction.TopBidder = TopBidder_name;
            }
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.Wallet = _context.Users.SingleOrDefault(u => u.id == HttpContext.Session.GetInt32("id")).Wallet;
            return View(displayAuction);
        }

        [HttpPost]
        [Route("auction/{id}/bid")]
        public IActionResult Bid(int id, string bid)
        {
            if(IsNotLogged()) return RedirectToAction("Index");
            ResolveAuction(id);
            Auction auction = _context.Auctions.SingleOrDefault(a => a.id == id);
            if(auction == null) return RedirectToAction("Main");
            decimal validBid = 0;
            if(decimal.TryParse(bid, out validBid) == false) return RedirectToAction("DisplayAuction");
            User user = _context.Users.SingleOrDefault(u => u.id == HttpContext.Session.GetInt32("id"));
            if(validBid > auction.TopBid && validBid <= user.Wallet)
            {
                if(auction.TopBidderId != 0)
                {
                    User previousTopBidder = _context.Users.SingleOrDefault(u => u.id == auction.TopBidderId);
                    previousTopBidder.Wallet += auction.TopBid;
                    _context.Update(previousTopBidder);
                }
                auction.TopBid = validBid;
                user.Wallet -= validBid;
                auction.TopBidderId = user.id;
                _context.Update(auction);
                _context.Update(user);
                _context.SaveChanges();
            }
            return RedirectToAction("DisplayAuction", id);
        }

        public void ResolveAllAuctions()
        {
            List<Auction> auctions = _context.Auctions.ToList();
            foreach(Auction a in auctions)
            {
                if(a.EndDate.Date <= DateTime.Today)
                {
                    User winner = _context.Users.SingleOrDefault(u => u.id == a.TopBidderId);
                    User Seller = _context.Users.SingleOrDefault(u => u.id == a.CreatorId);
                    // System.Console.WriteLine($"The auction for {a.Product} has ended! The winning bid of {a.TopBid} was made by {winner.FirstName} {winner.LastName}.");
                    Seller.Wallet += a.TopBid;
                    _context.Update(Seller);
                    _context.Remove(a);
                }
            }
            _context.SaveChanges();
        }

        public void ResolveAuction(int id)
        {
            Auction auction = _context.Auctions.SingleOrDefault(a => a.id == id);
            if(auction.EndDate.Date <= DateTime.Today)
            {
                User winner = _context.Users.SingleOrDefault(u => u.id == auction.TopBidderId);
                User Seller = _context.Users.SingleOrDefault(u => u.id == auction.CreatorId);
                Seller.Wallet += auction.TopBid;
                // System.Console.WriteLine($"The auction for {auction.Product} has ended! The winning bid of {auction.TopBid} was made by {winner.FirstName} {winner.LastName}.");
                _context.Remove(auction);
                _context.Update(Seller);
                _context.SaveChanges();
            }
        }
    }
}
