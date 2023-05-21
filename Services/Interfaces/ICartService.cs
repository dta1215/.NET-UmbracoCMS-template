using UmbracoBoutique.Services.DTO;

namespace UmbracoBoutique.Services.Interfaces
{
	public interface ICartService
	{
		IEnumerable<AddToCartModel> Carts();
		object AddCart(AddToCartModel model);
		object RemoveCart(AddToCartModel model);
	}
}
