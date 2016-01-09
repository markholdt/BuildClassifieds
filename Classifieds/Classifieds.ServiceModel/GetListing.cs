using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Classifieds.ServiceModel
{
    [Route("/listings/{Id}", "GET")]
    public class GetListing : IReturn<GetListingResponse>
    {
        public int Id { get; set; }
    }

    public class GetListingResponse
    {
        public Listing Result { get; set; }
       
        public List<PageHierarchy> PageHierarchy { get; set; }

    }
}
