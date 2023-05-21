namespace UmbracoBoutique.Services.Interfaces
{
    public interface IProductOrderStrategy<T>
    {
        IEnumerable<T> Order(IEnumerable<T> query);
    }
}
