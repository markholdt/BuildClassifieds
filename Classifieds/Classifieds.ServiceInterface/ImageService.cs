using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Classifieds.ServiceModel;

namespace Classifieds.ServiceInterface
{
    public class ImageService : Service
    {
        const int ThumbnailSize = 100;
        readonly string UploadsDir = "~/uploads".MapHostAbsolutePath();
        readonly string ThumbnailsDir = "~/uploads/thumbnails".MapHostAbsolutePath();
        readonly List<string> ImageSizes = new[] { "320x480" }.ToList(); //{ "320x480", "640x960", "640x1136", "768x1024", "1536x2048" }.ToList()

        public object Get(GetImages request)
        {
            return Directory.GetFiles(UploadsDir).Map(x => x.SplitOnLast(Path.DirectorySeparatorChar).Last());
        }

        public object Post(UploadImage request)
        {
         
            string urls = "";
            foreach (var uploadedFile in Request.Files.Where(uploadedFile => uploadedFile.ContentLength > 0))
            {
                var ms = new MemoryStream();
                {
                    uploadedFile.WriteTo(ms);
                    urls =WriteImage(ms);
                     
                }
            }
            return new UploadImageResponse {Url = urls};
        }

        private string WriteImage(Stream ms)
        {
            var hash = RandomString(25);

            ms.Position = 0;
            var fileName = hash + ".png";
            using (var img = Image.FromStream(ms))
            {
                img.Save(UploadsDir.CombineWith(fileName));
                var stream = Resize(img, ThumbnailSize, ThumbnailSize);
                File.WriteAllBytes(ThumbnailsDir.CombineWith(fileName), stream.ReadFully());

                ImageSizes.ForEach(x => File.WriteAllBytes(
                    AssertDir(UploadsDir.CombineWith(x)).CombineWith(hash + ".png"),
                    Get(new ResizeImage { Id = hash, Size = x }).ReadFully()));
            }
            return fileName;
        }

        [AddHeader(ContentType = "image/png")]
        public Stream Get(ResizeImage request)
        {
            var imagePath = UploadsDir.CombineWith(request.Id + ".png");
            if (request.Id == null || !File.Exists(imagePath))
                throw HttpError.NotFound(request.Id + " was not found");

            using (var stream = File.OpenRead(imagePath))
            using (var img = Image.FromStream(stream))
            {
                var parts = request.Size == null ? null : request.Size.Split('x');
                int width = img.Width;
                int height = img.Height;

                if (parts != null && parts.Length > 0)
                    int.TryParse(parts[0], out width);

                if (parts != null && parts.Length > 1)
                    int.TryParse(parts[1], out height);

                return Resize(img, width, height);
            }
        }
        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static Stream Resize(Image img, int newWidth, int newHeight)
        {
            if (newWidth != img.Width || newHeight != img.Height)
            {
                var ratioX = (double)newWidth / img.Width;
                var ratioY = (double)newHeight / img.Height;
                var ratio = Math.Max(ratioX, ratioY);
                var width = (int)(img.Width * ratio);
                var height = (int)(img.Height * ratio);

                var newImage = new Bitmap(width, height);
                Graphics.FromImage(newImage).DrawImage(img, 0, 0, width, height);
                img = newImage;

                if (img.Width != newWidth || img.Height != newHeight)
                {
                    var startX = (Math.Max(img.Width, newWidth) - Math.Min(img.Width, newWidth)) / 2;
                    var startY = (Math.Max(img.Height, newHeight) - Math.Min(img.Height, newHeight)) / 2;
                    img = Crop(img, newWidth, newHeight, startX, startY);
                }
            }

            var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return ms;
        }

        public static Image Crop(Image Image, int newWidth, int newHeight, int startX = 0, int startY = 0)
        {
            if (Image.Height < newHeight)
                newHeight = Image.Height;

            if (Image.Width < newWidth)
                newWidth = Image.Width;

            using (var bmp = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb))
            {
                bmp.SetResolution(72, 72);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(Image, new Rectangle(0, 0, newWidth, newHeight), startX, startY, newWidth, newHeight, GraphicsUnit.Pixel);

                    var ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Png);
                    Image.Dispose();
                    var outimage = Image.FromStream(ms);
                    return outimage;
                }
            }
        }

        public object Any(ResetImage request)
        {
            Directory.GetFiles(AssertDir(UploadsDir)).ToList().ForEach(File.Delete);
            Directory.GetFiles(AssertDir(ThumbnailsDir)).ToList().ForEach(File.Delete);
            ImageSizes.ForEach(x =>
                Directory.GetFiles(AssertDir(UploadsDir.CombineWith(x))).ToList().ForEach(File.Delete));
            File.ReadAllLines("~/preset-urls.txt".MapHostAbsolutePath()).ToList()
                .ForEach(url => WriteImage(new MemoryStream(url.Trim().GetBytesFromUrl())));

            return HttpResult.Redirect("/");
        }

        private static string AssertDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            return dirPath;
        }
    }
}
