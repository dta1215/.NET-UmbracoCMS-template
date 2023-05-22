using Examine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using UmbracoBoutique.Services.Interfaces;

namespace UmbracoBoutique.Controller
{
    public class CheckOutController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private readonly IExamineManager _examineManager;
        private readonly ICartService _cartService;

        public CheckOutController(UmbracoHelper umbracoHelper,
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

        [HttpGet("checkout")]
        public IActionResult Index()
        {

            return View("/Views/CheckOut.cshtml"); 
        }
    }
}
