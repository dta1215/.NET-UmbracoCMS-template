using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UmbracoBoutique.Services
{
    public interface IWidgetService
    {
        Task Render(dynamic HTMLContent);
    }

    public interface IWidgetRenderStrategyService
    {
        void Render();
    }

    public class WidgetBanner : IWidgetRenderStrategyService
    {
        public WidgetBanner()
        {
        }

        public void Render()
        {
        }
    }
}
