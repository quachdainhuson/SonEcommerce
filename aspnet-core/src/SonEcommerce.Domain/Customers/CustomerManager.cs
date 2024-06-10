using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace SonEcommerce.Customers
{
    public class CustomerManager : DomainService
    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        public CustomerManager(IRepository<Customer, Guid> productCategoryRepository)
        {
            _customerRepository = productCategoryRepository;
        }

        public async Task<Customer> CreateAsync(
            string name,
            string email,
            string phone,
            string address,
            string username,
            string password
        )
        {
            if (await _customerRepository.AnyAsync(x => x.Username == username) && username != null)
            {
                throw new UserFriendlyException("Tên đăng nhập đã tồn tại", SonEcommerceDomainErrorCodes.CustomerUserNameAlreadyExists);
            }
            if (await _customerRepository.AnyAsync(x => x.Email == email ) && email != null)
            {
                throw new UserFriendlyException("Email đã tồn tại", SonEcommerceDomainErrorCodes.CustomerEmailAlreadyExists);
            }
            if (await _customerRepository.AnyAsync(x => x.Phone == phone) && phone != null)
            {
                throw new UserFriendlyException("Số điện thoại đã tồn tại", SonEcommerceDomainErrorCodes.CustomerPhoneAlreadyExists);
            }
            return new Customer(Guid.NewGuid(), name, email, phone, address, username, password);
        }
    }
}
