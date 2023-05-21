using Examine;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;
using UmbracoBoutique.Services.DTO;
using UmbracoBoutique.Services.Interfaces;
using static Umbraco.Cms.Core.Constants.HttpContext;

namespace UmbracoBoutique.Services
{
	public class CartService : ICartService
	{
		const string CartKey = "Cart";
		private List<AddToCartModel> cartItems;
		private readonly UmbracoHelper _umbracoHelper;
		private readonly IVariationContextAccessor _variationContextAccessor;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CartService(UmbracoHelper umbracoHelper,
							   IVariationContextAccessor variationContextAccessor,
							   IHttpContextAccessor httpContextAccessor
							)
		{
			_umbracoHelper = umbracoHelper;
			_variationContextAccessor = variationContextAccessor;
			_httpContextAccessor = httpContextAccessor;
		}
		private AddToCartModel PrePareModel(AddToCartModel model)
		{
			var getProduct = _umbracoHelper.Content(model.ProductId) as SanPham_base;

			//Check null
			if (getProduct == null) return default;

			var sizeItem = _umbracoHelper.Content(model.SizeId) as KichCoSanPham;
			var colorItem = _umbracoHelper.Content(model.ColorId) as MauSanPham;

            model.ProductName = getProduct.SanPham_tenSanPham;
			model.Price = getProduct.SanPham_giaTien;
			model.ImageSrc = getProduct.SanPham_anhSanPham?.FirstOrDefault()?.Url();
			model.SizeValue = sizeItem.KichCoSanPham_content;
			model.ColorValue = colorItem.MauSanPham_content;

			return model;
		}
		public object AddCart(AddToCartModel model)
		{
			if(model != null)
			{
				cartItems = Carts().ToList();

                var duplicateItem = cartItems.FirstOrDefault(x => x.ProductId == model.ProductId
                                                           && x.ColorId == model.ColorId
                                                           && x.SizeId == model.SizeId);
				if(duplicateItem == null)
				{
					cartItems.Add(PrePareModel(model));
				}
				else
				{
					var convertedDuplicateModel = PrePareModel(duplicateItem);
                    convertedDuplicateModel.Quantity += model.Quantity;
				}

                _httpContextAccessor.HttpContext.Session.SetString(CartKey, JsonConvert.SerializeObject(cartItems));
                return true;
			}
			return false;
		}

		public IEnumerable<AddToCartModel> Carts()
		{
			var cart = _httpContextAccessor.HttpContext.Session.GetString(CartKey);
			if (string.IsNullOrEmpty(cart))
			{
				return Enumerable.Empty<AddToCartModel>();
			}
			else
			{
				return JsonConvert.DeserializeObject<IEnumerable<AddToCartModel>>(cart);
			}
		}

		public object RemoveCart(AddToCartModel model)
		{
			cartItems = Carts().ToList();
			if (cartItems.Any())
			{
				var findItem = cartItems.FirstOrDefault(x => x.ProductId == model.ProductId
														&& x.SizeId == model.SizeId
														&& x.ColorId == model.ColorId);
				if (findItem != null)
				{
					cartItems.Remove(findItem);
					_httpContextAccessor.HttpContext.Session.SetString(CartKey, JsonConvert.SerializeObject(cartItems));
				}
				return true;
			}
			return false;
		}
    }
}
