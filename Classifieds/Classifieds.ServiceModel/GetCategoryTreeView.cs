using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Classifieds.ServiceModel
{
    [Route("/categories/tree", "GET")]
    public class GetCategoryTreeView : IReturn<GetCategoryTreeViewResponse>
    {
    }

    public class GetCategoryTreeViewResponse
    {
        public List<CategoryTreeView> Result { get; set; }
    }
}
