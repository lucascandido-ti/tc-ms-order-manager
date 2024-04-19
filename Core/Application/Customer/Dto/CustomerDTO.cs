using Entities = Domain.Entities;

namespace Application.Customer.Dto
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }

        public static Entities.Customer MapToEntity(CustomerDTO dto)
        {
            return new Entities.Customer
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf
            };
        }

        public static CustomerDTO MapToDTO(Entities.Customer customer)
        {
            return new CustomerDTO
            {
                Id = customer.Id,
                Email = customer.Email,
                Name = customer.Name,
                Cpf = customer.Cpf
            };
        }
    }
}
