using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Classifieds.ServiceModel
{
    [Route("/listings", "GET")]
    public class GetListings : IReturn<GetListingsResponse>
    {
        public int? PageNr { get; set; }
        public int? CategoryId { get; set; }
        public int? Id { get; set; }

        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
    }

    public class GetListingsResponse
    {
        public List<Listing> Result { get; set; }
        public int TotalPageCount { get; set; }
        public int CurrentPageNr { get; set; }
        public int TotalRecords { get; set; }
        public List<PageHierarchy> PageHierarchy { get; set; }
        

    }
}
