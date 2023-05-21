using Examine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;
using UmbracoBoutique.Services.DTO;
using UmbracoBoutique.Services.Interfaces;

namespace UmbracoBoutique.Controller
{
	public class CartController : Microsoft.AspNetCore.Mvc.Controller
	{
		private readonly UmbracoHelper _umbracoHelper;
		private readonly IVariationContextAccessor _variationContextAccessor;
		private readonly IExamineManager _examineManager;
		private readonly ICartService _cartService;

		public CartController(UmbracoHelper umbracoHelper,
							   IVariationContextAccessor variationContextAccessor,
							   IExamineManager examineManager,
							   ICartService cartService
							)
		{
			_umbracoHelper = umbracoHelper;
			_variationContextAccessor = variationContextAccessor;
			_examineManager = examineManager;
			_cartService = cartService;
		}

		[HttpPost("addtocart")]
		public IActionResult AddToCart(AddToCartModel data)
		{
			_cartService.AddCart(data);
			return Ok();
		}

        [HttpPost("removeitem")]
        public IActionResult RemoveCartItem(AddToCartModel data)
        {
            _cartService.RemoveCart(data);
            return Ok();
        }

        [HttpGet("carts")]
		public IActionResult Carts()
		{
			var items = _cartService.Carts();
			return Ok(items);
		}

		[HttpGet("cartcount")]
		public IActionResult CartCount()
		{
			var count = _cartService.Carts()?.Count() ?? 0;
			return Ok(count);
		}
	}
}
