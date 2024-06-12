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

namespace SonEcommerce.Public.Orders
{
    public class OrdersAppService : CrudAppService<
        Order,
        OrderDto,
        Guid,
        PagedResultRequestDto, CreateOrderDto, CreateOrderDto>, IOrdersAppService
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

        public override async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            var subTotal = input.Items.Sum(x => x.Quantity * x.Price);
            var orderId = Guid.NewGuid();
            var order = new Order(orderId)
            {
                Code = await _orderCodeGenerator.GenerateAsync(),
                CustomerAddress = input.CustomerAddress,
                CustomerName = input.CustomerName,
                CustomerPhoneNumber = input.CustomerPhoneNumber,
                ShippingFee = 0,
                CustomerUserId = input.CustomerUserId,
                Tax = 0,
                Subtotal = subTotal,
                GrandTotal = subTotal,
                Discount = 0,
                PaymentMethod = PaymentMethod.COD,
                Total = subTotal,
                Status = OrderStatus.New

            };
            var items = new List<OrderItem>();
            foreach (var item in input.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                items.Add(new OrderItem()
                {
                    OrderId = orderId,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SKU = product.SKU,
                    Name = product.Name,
                });
            }
            await _orderItemRepository.InsertManyAsync(items);

            var result = await Repository.InsertAsync(order);


            return ObjectMapper.Map<Order, OrderDto>(result);
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

        public async Task<List<OrderInListDto>> GetListOrderByUserIdAsync(Guid userId)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.CustomerUserId == userId);
            var orders = await AsyncExecuter.ToListAsync(query);

            var orderDtos = new List<OrderInListDto>();

            foreach (var order in orders)
            {
                var orderDetailDto = await GetOrderAndDetailsAsync(order.Id);
                var orderInListDto = new OrderInListDto
                {
                    Id = order.Id,
                    Code = order.Code,
                    CustomerName = order.CustomerName,
                    CustomerPhoneNumber = order.CustomerPhoneNumber,
                    CustomerAddress = order.CustomerAddress,
                    Status = order.Status,
                    OrderItems = orderDetailDto.OrderItems,
                    CreationTime = orderDetailDto.CreationTime
                    

                };
                orderDtos.Add(orderInListDto);
            }

            return orderDtos;
        }
    }
}
