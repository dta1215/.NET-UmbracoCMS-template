using UmbracoBoutique.Services.DTO;

namespace UmbracoBoutique.Services.Interfaces
{
    public interface ICommonService
    {
        IEnumerable<dynamic> GetColorList(string culture = "en-US");
        IEnumerable<dynamic> GetSizeList(string culture = "en-US");
        IEnumerable<dynamic> GetProducts(List<string> Ids, string culture = "en-US");
        IEnumerable<dynamic> GetGalleryImages(ProductQueryParameterModel model, string culture = "en-US");

        DataSet<object> ProductByCategory(ProductQueryParameterModel model);

        SearchResultModel Search(string key = "");
    }
}
