using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Classifieds.ServiceModel
{
    [Route("/listings", "POST")]
    public class CreateListing : IReturn<CreateListingResponse>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string Files { get; set; }
        public string EmailAddress { get; set; }
        public string City { get; set; }

        public int? CategoryId { get; set; }
    }

    public class CreateListingResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
