using Entities = Domain.Entities;

namespace Application.Category.Dto
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static Entities.Category MapToEntity(CategoryDTO dto)
        {
            return new Entities.Category
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description
            };
        }

        public static CategoryDTO MapToDTO(Entities.Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
