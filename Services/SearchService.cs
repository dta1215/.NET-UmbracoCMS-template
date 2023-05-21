using UmbracoBoutique.Services.DTO;

namespace UmbracoBoutique.Services
{
    public interface ISearchService
    {
        IEnumerable<QueryResultModel> Search(ProductQueryParameterModel productQueryParameterModel);
    }
    public class SearchService : ISearchService
    {
        public IEnumerable<QueryResultModel> Search(ProductQueryParameterModel productQueryParameterModel)
        {


            return default;
        }
    }
}
