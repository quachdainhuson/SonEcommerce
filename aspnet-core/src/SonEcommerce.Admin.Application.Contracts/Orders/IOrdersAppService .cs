using SonEcommerce.Admin.Orders;
using SonEcommerce.Orders;
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
        UpdateOrderDto
        >
    {
        Task<List<OrderInListDto>> GetListAllAsync();
        Task<OrderDto> GetOrderAndDetailsAsync(Guid orderId);
        // change status order
        Task ChangeStatusOrderAsync(Guid orderId, OrderStatus status);
        Task<List<OrderInListDto>> GetListOrderByUserIdAsync(Guid userId);
        //getlistfilter
        Task<PagedResultDto<OrderInListDto>> GetListFilterAsync(OrderListFilterDto input);

    }
}

