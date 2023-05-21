using Examine;
using Lucene.Net.Analysis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Examine;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;
using UmbracoBoutique.Services;
using UmbracoBoutique.Services.DTO;
using UmbracoBoutique.Services.Interfaces;
using UmbracoBoutique.Services.ProductOrderStrategy;
using static Umbraco.Cms.Core.Constants;
using static Umbraco.Cms.Core.Constants.HttpContext;

namespace UmbracoBoutique.Controller
{
    [Route("home")]
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private readonly IExamineManager _examineManager;
        private readonly IProductOrderService<SanPham_base> _productOrderService;
        private readonly ICommonService _commonService;
        private readonly IContentService _contentService;
        private readonly IPublishedContentQuery _publishedContentQuery;

        public HomeController(UmbracoHelper umbracoHelper,
                               IVariationContextAccessor variationContextAccessor,
                               IExamineManager examineManager,
                                ICommonService commonService,
                                IContentService contentService,
                                IPublishedContentQuery publishedContentQuery,
                                IContentTypeService contentTypeService
                                )
        {
            _umbracoHelper = umbracoHelper;
            _variationContextAccessor = variationContextAccessor;
            _examineManager = examineManager;
            _commonService = commonService;
            _contentService = contentService;
            _publishedContentQuery = publishedContentQuery;
        }
        

        [HttpGet("product_by_category")]
        public IActionResult ProductByCategory(ProductQueryParameterModel model)
        {
            var dataSet = _commonService.ProductByCategory(model);

            //Check null
            if (dataSet is null) return NotFound();

            return Ok(dataSet);
        }

        [HttpGet("color_list")]
        public IActionResult ColorList(string culture = "en-US")
        {
            var items = _commonService.GetColorList(culture);
            //Check null
            if (items?.Any() == false) return NotFound();

            return Ok(items);
        }

        [HttpGet("size_list")]
        public IActionResult SizeList(string culture = "en-US")
        {
            var items = _commonService.GetSizeList(culture);
            //Check null
            if (items?.Any() == false) return NotFound();

            return Ok(items);
        }

        [HttpPost("products")]
        public IActionResult Products(List<string> Ids, string culture = "en-US")
        {
            var items = _commonService.GetProducts(Ids, culture);
            //Check null
            if (items?.Any() == false) return NotFound();

            return Ok(items);
        }

        [HttpGet("/search")]
        public IActionResult Search(string key = "")
        {
            var searchResultModel = _commonService.Search(key);

            //Check null
            if(searchResultModel is null)
            {
                TempData["SearchResultModel"] = null;
                return View("~/Views/Search.cshtml");
            }

            TempData["SearchResultModel"] = JsonConvert.SerializeObject(searchResultModel);
            return View("~/Views/Search.cshtml");
        }

        [HttpGet("widget_gallery")]
        public IActionResult WidgetGalleryImages()
        {
            return Ok();
        }
    }
}
