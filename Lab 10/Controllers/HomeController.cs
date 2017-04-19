using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_10.RestaurantReviewService;
using System.Threading.Tasks;
using Lab_10.AsyncLock;
namespace Lab_10.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {
        private readonly AsyncLock.AsyncLock m_lock = new AsyncLock.AsyncLock();
        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Reviews()
        {
            HttpContext.Server.ScriptTimeout = 300;
            using (var releaser = await m_lock.LockAsync())
            {
                var reviewer = new RestaurantReviewServiceClient();
                var restaurants = await reviewer.GetRestaurantsByRatingAsync(0);
                var jsonRestaurants = restaurants.Select(resto => new Models.RestaurantInfo(resto));
                //Need to cleanse NovaScotia and BritishColumbia
                
                return Json(jsonRestaurants, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> UpdateReview(string restInfo)
        {
            if (!string.IsNullOrWhiteSpace(restInfo))
            {
                var restaurantInfo = System.Web.Helpers.Json.Decode<Models.RestaurantInfo>(restInfo);
                var info = new RestaurantInfo()
                {
                    Name = restaurantInfo.Name,
                    Summary = restaurantInfo.Summary,
                    Rating = restaurantInfo.Rating,
                    Location = new Address()
                    {
                        City = restaurantInfo.Location.City,
                        Street = restaurantInfo.Location.Street,
                        PostalCode = restaurantInfo.Location.PostalCode,
                        Province = restaurantInfo.Location.Province                   
                    }
                };
                var reviewer = new RestaurantReviewServiceClient();
                if (await reviewer.SaveRestaurantAsync(info))
                {
                    return Json("Update successful.");
                }
            }
            return Json("No data received.");
        }
    }
}