using Domain.Customer.Exceptions;
using Domain.Customer.Ports;
using Domain.Utils;

namespace Domain.Entities
{
    public class Customer
    {
        public Customer() { 
            this.CreatedAt = DateTime.UtcNow;
            this.LastUpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public List<Order> Orders { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        private void ValidateState()
        {
            if (this.Name == null)
            {
                throw new NameRequiredException();
            }

            if (Utils.Utils.ValidateEmail(Email) == false)
            {
                throw new EmailRequiredException();
            }

            if (Utils.Utils.ValidateCPF(Cpf) == false)
            {
                throw new CpfRequiredException();
            }

        }

        public async Task Save(ICustomerRepository customerRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                var result = await customerRepository.CreateCustomer(this);
                this.Id = result.Id;
            }
        }

    }
}
