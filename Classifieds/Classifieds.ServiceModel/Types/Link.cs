using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Classifieds.ServiceModel
{
    public class Link
    {
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
