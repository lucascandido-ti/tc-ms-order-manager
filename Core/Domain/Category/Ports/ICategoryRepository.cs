namespace Domain.Category.Ports
{
    public interface ICategoryRepository
    {
        Task<Entities.Category> Get(int id);
        Task<List<Entities.Category>> List();
    }
}
