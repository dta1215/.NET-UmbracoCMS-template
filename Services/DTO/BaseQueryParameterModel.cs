namespace UmbracoBoutique.Services.DTO
{
    public abstract class BaseQueryParameterModel
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int SearchValue { get; set; }
    }
}
