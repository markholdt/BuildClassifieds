using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Classifieds.ServiceModel
{
    public class ImageModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsPrimary { get; set; }
    }
}
