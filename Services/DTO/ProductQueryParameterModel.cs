namespace UmbracoBoutique.Services.DTO
{
    public class ProductQueryParameterModel : BaseQueryParameterModel
    {
        public int CategoryId { get; set; }
        public string SortBy { get; set; }
        public string Culture { get; set; }
        public string ProductSize { get; set; }
        public string ProductColor { get; set; }
        public string OrderBy { get; set; }

    }
}
