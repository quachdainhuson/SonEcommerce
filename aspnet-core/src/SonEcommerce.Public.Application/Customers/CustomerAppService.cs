using SonEcommerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SonEcommerce.Public
{
    public class CustomerAppService : CrudAppService<
        Customer,
        CustomerDto,
        Guid,
        PagedResultRequestDto, CreateUpdateCustomerDto, CreateUpdateCustomerDto>, ICustomerAppService

    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly CustomerManager _customerManager;
        public CustomerAppService(IRepository<Customer, Guid> repository, IRepository<Customer, Guid> customerRepository, CustomerManager customerManager)
            : base(repository)
        {
            _customerRepository = customerRepository;
            _customerManager = customerManager;
        }
        public async Task<List<CustomerInListDto>> GetListAllAsync()
        {
            var customers = await Repository.GetListAsync();
            return ObjectMapper.Map<List<Customer>, List<CustomerInListDto>>(customers);
        }

        public Task<PagedResultDto<CustomerInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            throw new NotImplementedException();
        }

        public override async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            var customer = await _customerManager.CreateAsync(
                input.Name,
                input.Email,
                input.Phone,
                input.Address,
                input.Username,
                input.Password 
            );
            await Repository.InsertAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        public async Task<bool> IsUsernameExistAsync(string username)
        {
            var customer = await Repository.FirstOrDefaultAsync(x => x.Username == username);
            if (customer != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            var customer = await Repository.FirstOrDefaultAsync(x => x.Email == email);
            if (customer != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsPhoneExistAsync(string phone)
        {
            var customer = await Repository.FirstOrDefaultAsync(x => x.Phone == phone);
            if (customer != null)
            {
                return true;
            }
            return false;
        }

        public async Task<CustomerDto> LoginAsync(string username, string password)
        {
            var customer = await Repository.FirstOrDefaultAsync(x =>
            (x.Username == username || x.Phone == username || x.Email == username) && x.Password == password);
            if (customer == null)
            {
                throw new UserFriendlyException("Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return ObjectMapper.Map<Customer, CustomerDto>(customer);

        }
    }
}
