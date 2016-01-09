using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Classifieds.ServiceModel
{
    [Route("/images/upload")]
    public class UploadImage : IReturn<UploadImageResponse>
    {
    }
    public class UploadImageResponse
    {
        public string Url { get; set; }
    }

    [Route("/images")]
    public class GetImages { }

    [Route("/images/resize/{Id}")]
    public class ResizeImage
    {
        public string Id { get; set; }
        public string Size { get; set; }
    }

    [Route("/images/reset")]
    public class ResetImage { }
}
