using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Classifieds.ServiceModel
{
    public class Listing
    {

        public Listing()
        {
            LastModified = DateTime.Now;
            ImageUrls = new List<ImageModel>();
        }

        [AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string EmailAddress { get; set; }
        public string City { get; set; }

        public int? CategoryId { get; set; }
        public int Price { get; set; }
        public DateTime LastModified { get; set; }

      public List<ImageModel> ImageUrls { get; set; }

        // helpers
        public string PrimaryImageUrl()
        {
            if (this.ImageUrls.Any())
                return this.ImageUrls.First().Url;
            else
                return "noAd.png";
        }

        public string PriceText()
        {
            if (this.Price != null)
            {
                return "N$ " + this.Price.ToString("N00");
            }
            return "";
        }
        public string ShortDescription()
        {
            if (this.Description != null)
            {
                if (this.Description.Length > 250)
                    return this.Description.Remove(0, 250) + " ...";
                else
                    return this.Description;
            }
            return "";
        }
        [Ignore]
        public Category Category { get; set; }
        [Ignore]
        public bool IsFeatured { get; set; } = false;

        
    }
}
