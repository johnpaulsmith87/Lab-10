using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_10.Models
{
    public class RestaurantInfo
    {
        public RestaurantInfo()
        {

        }
        public RestaurantInfo(RestaurantReviewService.RestaurantInfo restaurant)
        {
            Name = restaurant.Name;

            Location = new Address()
            {
                Street = restaurant.Location.Street,
                City = restaurant.Location.City,
                PostalCode = restaurant.Location.PostalCode,
                Province = GetClientSideString(restaurant.Location.Province)
            };
            Summary = restaurant.Summary;
            Rating = restaurant.Rating;

        }
        private string GetClientSideString(string province)
        {
            string result = province;
            if (string.Equals(province, "BritishColumbia"))
            {
                result = "British Columbia";
            }
            else if (string.Equals(province, "NovaScotia"))
            {
                result = "Nova Scotia";
            }
            return result;
        }
        public string Name { get; set; }
        public Address Location { get; set; }
        public string Summary { get; set; }
        public int Rating { get; set; }
    }
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
    }



}