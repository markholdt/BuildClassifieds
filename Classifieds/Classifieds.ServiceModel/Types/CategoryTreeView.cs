using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Classifieds.ServiceModel
{
    public class CategoryTreeView : Category
    {
        public CategoryTreeView()
        {
            SubCategories = new List<Category>();
        }


        [Ignore]
        public List<Category> SubCategories { get; set; }


    }
}
