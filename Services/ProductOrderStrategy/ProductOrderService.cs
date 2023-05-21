using Umbraco.Cms.Web.Common.PublishedModels;
using UmbracoBoutique.Services.Interfaces;

namespace UmbracoBoutique.Services.ProductOrderStrategy
{
    public class ProductOrderService<T> : IProductOrderService<T> where T: class
    {
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<T> Order(IProductOrderStrategy<T> orderService)
        {
            return orderService.Order(Items);
        }
    }

    public class OrderByNameAscending : IProductOrderStrategy<SanPham_base>
    {
        public IEnumerable<SanPham_base> Order(IEnumerable<SanPham_base> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
    public class OrderByNameDescending : IProductOrderStrategy<SanPham_base>
    {
        public IEnumerable<SanPham_base> Order(IEnumerable<SanPham_base> query)
        {
            return query.OrderByDescending(x => x.Name);
        }
    }
    public class OrderByPriceAscending : IProductOrderStrategy<SanPham_base>
    {
        public IEnumerable<SanPham_base> Order(IEnumerable<SanPham_base> query)
        {
            return query.OrderBy(x => x.SanPham_giaTien);
        }
    }
    public class OrderByPriceDescending : IProductOrderStrategy<SanPham_base>
    {
        public IEnumerable<SanPham_base> Order(IEnumerable<SanPham_base> query)
        {
            return query.OrderByDescending(x => x.SanPham_giaTien);
        }
    }
    public class OrderByDateAscending : IProductOrderStrategy<SanPham_base>
    {
        public IEnumerable<SanPham_base> Order(IEnumerable<SanPham_base> query)
        {
            return query.OrderBy(x => x.CreateDate);
        }
    }
    public class OrderByDateDescending : IProductOrderStrategy<SanPham_base>
    {
        public IEnumerable<SanPham_base> Order(IEnumerable<SanPham_base> query)
        {
            return query.OrderByDescending(x => x.CreateDate);
        }
    }
}
