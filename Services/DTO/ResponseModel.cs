namespace UmbracoBoutique.Services.DTO
{
	public class ResponseModel
	{
		public int StatusCode { get; set; }
		public bool Success { get; set; }
		public object Message { get; set; }
		public object Data { get; set; }
	}
}
