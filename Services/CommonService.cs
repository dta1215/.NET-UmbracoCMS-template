using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Common;
using UmbracoBoutique.Services.Interfaces;
using Examine;
using Org.BouncyCastle.Crypto;
using UmbracoBoutique.Services.DTO;
using static Umbraco.Cms.Core.Constants;
using System.Diagnostics;
using Umbraco.Cms.Infrastructure.Examine;
using UmbracoBoutique.Services.ProductOrderStrategy;

namespace UmbracoBoutique.Services
{
    public class CommonService : ICommonService
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private readonly IExamineManager _examineManager;
        private readonly IProductOrderService<SanPham_base> _productOrderService;
        private readonly IHttpContextAccessor _httpContext;

        public CommonService(UmbracoHelper umbracoHelper,
                               IVariationContextAccessor variationContextAccessor,
                               IExamineManager examineManager,
                                IProductOrderService<SanPham_base> productOrderService,
                                IHttpContextAccessor httpContext
                            )
        {
            _umbracoHelper = umbracoHelper;
            _variationContextAccessor = variationContextAccessor;
            _examineManager = examineManager;
            _productOrderService = productOrderService;
            _httpContext = httpContext;
        }
        public IEnumerable<dynamic> GetColorList(string culture = "en-US")
        {
            _variationContextAccessor.VariationContext = new VariationContext(culture);

            var parent = _umbracoHelper.Content(Constants.DocumentContent.Key.ColorList) as QuanLyMauSanPham;
            var items = parent?.Children<MauSanPham>()
                                               .Select(x => new
                                               {
                                                   Id = x.Id,
                                                   Title = x.MauSanPham_content
                                               });
            return items;
        }

        public IEnumerable<dynamic> GetGalleryImages(ProductQueryParameterModel model, string culture = "en-US")
        {
            return default;
        }

        public IEnumerable<dynamic> GetProducts(List<string> Ids, string culture = "en-US")
        {
            _variationContextAccessor.VariationContext = new VariationContext(culture);

            var parent = _umbracoHelper.Content(Constants.DocumentContent.Key.ProductsList) as QuanLySanPham;

            //Check null
            if (Ids?.Count < 0 || parent?.Children?.Any() == false)
            {
                return default;
            }

            var items = parent?.Children<SanPham_base>()
                            ?.Where(x => Ids.Contains(x.Id.ToString()))
                            ?.Select(x => new
                            {
                                Id = x.Id,
                                Title = x.SanPham_tenSanPham,
                                Price = x.SanPham_giaTien,
                                Url = x.Url(),
                                Image = x.SanPham_anhSanPham.Any() == true ? x.SanPham_anhSanPham.Select(a => new
                                {
                                    Id = a.Id,
                                    Url = a.Url()
                                }).FirstOrDefault() : default,
                            });

            return items;
        }

        public IEnumerable<dynamic> GetSizeList(string culture = "en-US")
        {
            _variationContextAccessor.VariationContext = new VariationContext(culture);

            var parent = _umbracoHelper.Content(Constants.DocumentContent.Key.SizeList) as QuanLyKichCoSanPham;
            var items = parent?.Children<KichCoSanPham>()
                                               .Select(x => new
                                               {
                                                   Id = x.Id,
                                                   Title = x.KichCoSanPham_content
                                               });
            return items;
        }

        public DataSet<object> ProductByCategory(ProductQueryParameterModel model)
        {
            _variationContextAccessor.VariationContext = new VariationContext("en-US");

            var productList = _umbracoHelper.Content(Constants.DocumentContent.Key.ProductsList) as QuanLySanPham;

            //Truong hop null
            if (productList?.Children()?.Any() == false) return default;

            var query = productList.Children<SanPham_base>()
                        .OrderByDescending(x => x.CreateDate)
                        .Where(x => x.SanPham_danhMuc?.Any(c => c.Id == model.CategoryId) == true);

            //Filter theo kich co
            if (!string.IsNullOrEmpty(model.ProductSize))
            {
                var Ids = model.ProductSize.Split(",");

                query = query.Where(x => x?.SanPhamBase_danhSachLoaiSanPham?.Any(y =>
                {
                    var convertedItem = y as SanPham_loaiHang;
                    if (convertedItem is null) return false;

                    return Ids.Contains(convertedItem?.SanPham_loaiHang_kichCo?.Id.ToString()) == true;
                }) == true);
            }
            //Filter theo màu sắc
            if (!string.IsNullOrEmpty(model.ProductColor))
            {
                var Ids = model.ProductColor.Split(",");

                query = query.Where(x => x?.SanPhamBase_danhSachLoaiSanPham?.Any(y =>
                {
                    var convertedItem = y as SanPham_loaiHang;
                    if (convertedItem is null) return false;

                    return Ids.Contains(convertedItem?.SanPham_loaiHang_mauSac?.Id.ToString()) == true;
                }) == true);
            }

            //Neu co order by thi 
            if (!string.IsNullOrEmpty(model.OrderBy))
            {
                query = Order(query, model.OrderBy);
            }

            //Check null
            if (query is null) return default;

            var result = query.Select(x => new
            {
                Id = x.Id,
                Title = x.SanPham_tenSanPham,
                Price = x.SanPham_giaTien,
                Url = x.Url(),
                Images = x.SanPham_anhSanPham.Select(i => new
                {
                    Id = i.Id,
                    Name = i.Name,
                    Src = i.Url()
                }),
                Sizes = x.SanPhamBase_danhSachLoaiSanPham?.Select(y =>
                {
                    var convertedItem = y as SanPham_loaiHang;
                    return new
                    {
                        Size = convertedItem?.SanPham_loaiHang_kichCo?.Value(nameof(KichCoSanPham.KichCoSanPham_content))
                    };
                }),
                Colors = x.SanPhamBase_danhSachLoaiSanPham?.Select(y =>
                {
                    var convertedItem = y as SanPham_loaiHang;
                    return new
                    {
                        Size = convertedItem?.SanPham_loaiHang_mauSac?.Value(nameof(MauSanPham.MauSanPham_content))
                    };
                })
            }).Skip((model.CurrentPage - 1) * model.PageSize).Take(model.PageSize);

            //Truong hop null
            if (result?.Any() == false) return default;

            var totalRows = query.Count();

            DataSet<object> dataSet = new DataSet<object>();
            dataSet.Items = result;
            dataSet.TotalRows = totalRows;
            dataSet.CurrentPage = model.CurrentPage;
            dataSet.PageSize = model.PageSize;

            return dataSet;
        }

        public SearchResultModel Search(string key = "")
        {
            var searchDocument = _umbracoHelper.Content(Constants.DocumentContent.Key.SearchPage);
            var searchPageUrl = searchDocument?.Url();

            IEnumerable<QueryResultModel> query = Enumerable.Empty<QueryResultModel>();
            SearchResultModel searchResultModel = new SearchResultModel();

            //Nếu key < 3 thì tìm tiêu đề của sản phẩm
            if (key.Length < 3)
            {
                var itemList = _umbracoHelper.Content(Constants.DocumentContent.Key.ProductsList);
                if(itemList?.Children<SanPham_base>()?.Any() == true)
                {
                    query = itemList.Children<SanPham_base>()
                                    .Where(x => x.SanPham_tenSanPham.ToLower().Contains(key.ToLower())
                                    || int.TryParse(key, out int number) ? x.SanPham_giaTien == int.Parse(key) : false
                                    || x.CreateDate.ToString("dd/MM/yyyy") == key)
                                    .Select(x =>
                                    {
                                        var sanphamBase = x as SanPham_base;
                                        return new QueryResultModel
                                        {
                                            Id = sanphamBase.Id,
                                            Name = sanphamBase.Name,
                                            Url = sanphamBase.Url(),
                                            Price = sanphamBase.SanPham_giaTien,
                                            CreateDate = sanphamBase.CreateDate.ToString(),
                                            Image = sanphamBase.SanPham_anhSanPham?.FirstOrDefault()?.Url()
                                        };
                                    }).Take(8);

                    searchResultModel.TotalRows = itemList.Children<SanPham_base>()
                                                    .Where(x => x.SanPham_tenSanPham.ToLower().Contains(key.ToLower())
                                                            || int.TryParse(key, out int number) ? x.SanPham_giaTien == int.Parse(key) : false
                                                            || x.CreateDate.ToString("dd/MM/yyyy") == key)
                                                    .Count();
                }
            }
            //Nếu key > 3 thì mới dùng được Lucene
            else
            {
                if (!_examineManager.TryGetIndex(UmbracoIndexes.InternalIndexName, out var index) || !(index is IUmbracoIndex umbIndex))
                {
                    return null;
                }

                var searcher = index.Searcher;
                // Add time ticks (excution time counter)
                Stopwatch watch = new Stopwatch();
                watch.Start();
                var result = searcher.CreateQuery(IndexTypes.Content).ManagedQuery(key).Execute();
                watch.Stop();

                //Check null
                if (!result.Any())
                {
                    return null;
                }

                query = _umbracoHelper.Content(result.Select(x => x.Id))
                                            .Where(x => x.ContentType.CompositionAliases?.Any(a => a == SanPham_base.ModelTypeAlias) == true)
                                            .Select(x =>
                                            {
                                                var sanphamBase = x as SanPham_base;
                                                return new QueryResultModel
                                                {
                                                    Id = sanphamBase.Id,
                                                    Name = sanphamBase.Name,
                                                    Url = sanphamBase.Url(),
                                                    Price = sanphamBase.SanPham_giaTien,
                                                    CreateDate = sanphamBase.CreateDate.ToString(),
                                                    Image = sanphamBase.SanPham_anhSanPham?.FirstOrDefault()?.Url()
                                                };
                                            }).Take(8);

                searchResultModel.TotalRows = _umbracoHelper.Content(result.Select(x => x.Id))
                                        .Where(x => x.ContentType.CompositionAliases?.Any(a => a == SanPham_base.ModelTypeAlias) == true)
                                        .Count();
            }
            

            //Check null
            if (!query.Any())
            {
                return null;
            }

            searchResultModel.Items = query;
            searchResultModel.CurrentPage = 1;
            searchResultModel.PageSize = 8;
            searchResultModel.TotalPages = Utilities.TotalPages(searchResultModel.TotalRows, searchResultModel.PageSize);
            searchResultModel.SearchValue = key;
            searchResultModel.Other = _httpContext?.HttpContext?.Request?.QueryString.Value;

            return searchResultModel;
        }

        private IEnumerable<SanPham_base> Order(IEnumerable<SanPham_base> query, string orderValue)
        {
            var splitOrderValue = orderValue.Split("_");
            string orderColumn = splitOrderValue[0];
            string direction = splitOrderValue[1];

            _productOrderService.Items = query;
            switch (orderValue)
            {
                case Constants.OrderProduct.Name_ASC:
                    query = _productOrderService.Order(new OrderByNameAscending());
                    break;
                case Constants.OrderProduct.Name_DESC:
                    query = _productOrderService.Order(new OrderByNameDescending());
                    break;
                case Constants.OrderProduct.Price_ASC:
                    query = _productOrderService.Order(new OrderByPriceAscending());
                    break;
                case Constants.OrderProduct.Price_DESC:
                    query = _productOrderService.Order(new OrderByPriceDescending());
                    break;
                case Constants.OrderProduct.Date_ASC:
                    query = _productOrderService.Order(new OrderByDateAscending());
                    break;
                case Constants.OrderProduct.Date_DESC:
                    query = _productOrderService.Order(new OrderByDateDescending());
                    break;
            }
            return query;
        }
    }
}
