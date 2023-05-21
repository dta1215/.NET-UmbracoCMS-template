namespace UmbracoBoutique.Services.DTO
{
	public class SearchResultModel
	{
		public object Items { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; } = 8;
		public int TotalRows { get; set; }
		public int TotalPages { get; set; }
		public string SearchValue { get; set; }
		public object Other { get; set; }
	}
}
