using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Classifieds.ServiceModel
{
    [Route("/categories", "GET")]
    public class GetCategories : IReturn<GetCategoriesResponse>
    {
    }

    public class GetCategoriesResponse
    {
        public List<Category> Result { get; set; }
    }
}
