using SonEcommerce.Admin.Orders;
using SonEcommerce.Orders;
using SonEcommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SonEcommerce.Admin
{
    public class OrdersAppService : CrudAppService<
        Order,
        OrderDto,
        Guid,
        PagedResultRequestDto, UpdateOrderDto>, IOrderAppService
    {
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly OrderCodeGenerator _orderCodeGenerator;
        private readonly IRepository<Product, Guid> _productRepository;
        public OrdersAppService(IRepository<Order, Guid> repository,
            OrderCodeGenerator orderCodeGenerator,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Product, Guid> productRepository)
            : base(repository)
        {
            _orderItemRepository = orderItemRepository;
            _orderCodeGenerator = orderCodeGenerator;
            _productRepository = productRepository;
        }

        public async Task ChangeStatusOrderAsync(Guid orderId, OrderStatus status)
        {
            // Lấy đơn hàng hiện có
            var order = await Repository.GetAsync(orderId);

            // Cập nhật trạng thái của đơn hàng
            order.Status = status;

            // Lưu các thay đổi vào cơ sở dữ liệu
             await Repository.UpdateAsync(order);
        }


        public async Task<List<OrderInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.Total > 0);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Order>, List<OrderInListDto>>(data);
        }

        public async Task<OrderDto> GetOrderAndDetailsAsync(Guid orderId)
        {
            var order = await Repository.GetAsync(orderId);
            var orderItems = await _orderItemRepository.GetListAsync(x => x.OrderId == orderId);

            var orderDto = ObjectMapper.Map<Order, OrderDto>(order);
            orderDto.OrderItems = ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(orderItems);

            return orderDto;
        }

        public override async Task<OrderDto> UpdateAsync(Guid orderId, UpdateOrderDto input)
        {
            var order = await Repository.GetAsync(orderId);
            order.Status = input.Status;
            await Repository.UpdateAsync(order);

            // Return the updated order DTO
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

    }
}
