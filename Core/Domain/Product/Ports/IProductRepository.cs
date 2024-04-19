namespace Domain.Product.Ports
{
    public interface IProductRepository
    {
        Task<Entities.Product> Get(int id);
        Task<List<Entities.Product>> List();
    }
}
