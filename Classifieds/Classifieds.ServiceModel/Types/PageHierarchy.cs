using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Classifieds.ServiceModel
{
    public class PageHierarchy
    {
        public PageHierarchy(int rank, Link link)
        {
            Rank = rank;
            Link = link;
        }
        public int Rank { get; set; }
        public Link Link { get; set; }
    }
}
