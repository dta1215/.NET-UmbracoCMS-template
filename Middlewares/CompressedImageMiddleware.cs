using ImageMagick;
using Microsoft.AspNetCore.Http.Extensions;

namespace UmbracoBoutique.Middlewares
{
    public class CompressedImageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CompressedImageMiddleware> _logger;

        public CompressedImageMiddleware(RequestDelegate next,
                                        IHttpContextAccessor httpContextAccessor,
                                        IWebHostEnvironment env,
                                        ILogger<CompressedImageMiddleware> logger
                                        )
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _logger = logger;

        }
        public async Task Invoke(HttpContext context)
        {
            const int _KB = 1024;
            var path = context.Request.Path;

            if (path.Value.StartsWith("/media/"))
            {
                string imagePath = path.Value;

                string imageExtension = Path.GetExtension(imagePath).ToLower();

                if (imageExtension == ".jpg" || imageExtension == ".jpeg")
                {
                    // Get the original image file stream
                    var originalFileStream = File.OpenRead(Path.Join(_env.WebRootPath, imagePath));

                    var isNeededCompress = originalFileStream.Length / _KB > 200;

                    //Neu file image nho hon 200KB thi bo qua
                    if (!isNeededCompress)
                    {
                        await _next(context);
                        return;
                    }

                    // Convert the original image to WebP format
                    using (var image = new MagickImage(originalFileStream))
                    {
                        image.Format = MagickFormat.WebP;
                        image.Quality = 50;

                        var webpFileStream = new MemoryStream();
                        image.Write(webpFileStream);
                        webpFileStream.Seek(0, SeekOrigin.Begin);

                        // Set the content type to WebP image format
                        context.Response.ContentType = "image/webp";
                        // Copy the WebP file stream to the response stream
                        webpFileStream.CopyTo(context.Response.Body);
                        return;
                    }
                }
            }
            await _next(context);
        }
    }

    public static class CompressedImageMiddlewareExtension
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CompressedImageMiddleware>();
        }
    }

}
