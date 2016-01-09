using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Classifieds.ServiceModel
{
    public class Category
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }


    }
}
