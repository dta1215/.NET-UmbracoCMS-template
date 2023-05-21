using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace UmbracoBoutique.Controller.Components
{
	public class WidgetViewComponent : ViewComponent
	{
		public WidgetViewComponent()
		{
			
		}

		public IViewComponentResult Invoke(string title)
		{
			ViewBag.title = title;
			return View();
		}
	}
}
