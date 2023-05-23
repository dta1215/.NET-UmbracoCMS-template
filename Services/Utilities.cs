using System.Drawing.Printing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBoutique.Services
{
    public static class Utilities
    {

        public static Dictionary<string, Action<IPublishedContent, IPublishedElement>> WidgetRenderMethodList = new Dictionary<string, Action<IPublishedContent, IPublishedElement>>();

        public static int TotalPages(int rows, int pageSize)
        {
            return (int)Math.Ceiling(rows / (double)pageSize);
        }
    }
}
