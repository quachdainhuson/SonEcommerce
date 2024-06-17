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
using Volo.Abp.Identity;
using Volo.Abp.Users;
using SonEcommerce.Admin.Products;
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
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        public OrdersAppService(IRepository<Order, Guid> repository,
            OrderCodeGenerator orderCodeGenerator,
            IRepository<OrderItem> orderItemRepository,
            IRepository<Product, Guid> productRepository,
            IRepository<IdentityUser, Guid> userRepository)
            : base(repository)
        {
            _orderItemRepository = orderItemRepository;
            _orderCodeGenerator = orderCodeGenerator;
            _productRepository = productRepository;
            _userRepository = userRepository;
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

        public async Task<List<OrderInListDto>> GetListOrderByUserIdAsync(Guid userId)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.CustomerUserId == userId);
            var orders = await AsyncExecuter.ToListAsync(query);
            var orderDtos = new List<OrderInListDto>();
            foreach (var order in orders)
            {
                var orderDetailDto = await GetOrderAndDetailsAsync(order.Id);

                // Lấy thông tin người dùng
                var user = await _userRepository.GetAsync(userId);
                var userDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);

                var orderInListDto = new OrderInListDto
                {
                    Id = order.Id,
                    Code = order.Code,
                    CustomerName = order.CustomerName,
                    CustomerPhoneNumber = order.CustomerPhoneNumber,
                    CustomerAddress = order.CustomerAddress,
                    UserCity = order.UserCity,
                    UserDistrict = order.UserDistrict,
                    UserWard = order.UserWard,
                    Status = order.Status,
                    PaymentMethod = order.PaymentMethod,
                    Total = (double)order.Total,
                    CustomerUserId = userId,
                    CreationTime = orderDetailDto.CreationTime,
                    OrderItems = orderDetailDto.OrderItems
                };

                orderDtos.Add(orderInListDto);
            }

            return orderDtos;
        }

        public async Task<OrderDto> GetOrderAndDetailsAsync(Guid orderId)
        {
            // Lấy thông tin order
            var order = await Repository.GetAsync(orderId);

            // Lấy danh sách order items
            var orderItems = await _orderItemRepository.GetListAsync(x => x.OrderId == orderId);

            // Lấy thông tin sản phẩm cho từng OrderItem
            var productIds = orderItems.Select(item => item.ProductId).ToList();
            var products = await _productRepository.GetListAsync(p => productIds.Contains(p.Id));

            // Map dữ liệu vào DTO
            var orderDto = ObjectMapper.Map<Order, OrderDto>(order);

            var orderItemDtos = orderItems.Select(item =>
            {
                var orderItemDto = ObjectMapper.Map<OrderItem, OrderItemDto>(item);
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                orderItemDto.Product = ObjectMapper.Map<Product, ProductDto>(product);
                return orderItemDto;
            }).ToList();

            orderDto.OrderItems = orderItemDtos;

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
