using Domain.Customer.Exceptions;
using Domain.Customer.Ports;

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
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        private void ValidateState()
        {
            if (this.Name == null)
            {
                throw new NameRequiredException();
            }

            if (this.Email == null)
            {
                throw new EmailRequiredException();
            }

            if (this.Cpf == null)
            {
                throw new CpfRequiredException();
            }

        }

        public async Task Save(ICustomerRepository customerRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                var result = await customerRepository.CreateBooking(this);
                this.Id = result.Id;
            }
        }

    }
}
