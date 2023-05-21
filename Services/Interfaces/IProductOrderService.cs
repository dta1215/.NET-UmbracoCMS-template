using Umbraco.Cms.Web.Common.PublishedModels;

namespace UmbracoBoutique.Services.Interfaces
{
    public interface IProductOrderService<T>
    {
        IEnumerable<T> Items { get; set; }
        IEnumerable<T> Order(IProductOrderStrategy<T> orderType);
    }
}
