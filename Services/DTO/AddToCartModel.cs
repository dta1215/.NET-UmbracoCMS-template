namespace UmbracoBoutique.Services.DTO
{
	public class AddToCartModel
	{
		public int ProductId { get; set; }
		public int SizeId { get; set; }
		public int ColorId  { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }
		public string ProductName { get; set; }
		public string ImageSrc { get; set; }

		public string SizeValue { get; set; }
		public string ColorValue { get; set; }
	}
}
