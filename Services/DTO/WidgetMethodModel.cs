using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbracoBoutique.Services.DTO
{
    public class WidgetMethodModel
    {
        public IPublishedContent Document { get; set; }
        public BlockListModel DanhSachWidgets { get; set; }
    }
}
