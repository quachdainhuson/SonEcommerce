using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SonEcommerce.Admin
{
    public interface IOrderAppService : ICrudAppService
        <OrderDto,
        Guid,
        PagedResultRequestDto,
        CreateOrderDto,
        CreateOrderDto
        >
    {
        Task<List<OrderInListDto>> GetListAllAsync();
        Task<OrderDto> GetOrderAndDetailsAsync(Guid orderId);
        // change status order
        Task<OrderDto> ChangeStatusOrderAsync(Guid orderId, int status);
    }
}

