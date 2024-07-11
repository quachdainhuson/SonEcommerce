import type { BaseListFilterDto } from '../models';
import type { OrderStatus } from '../son-ecommerce/orders/order-status.enum';

export interface OrderListFilterDto extends BaseListFilterDto {
  status?: OrderStatus;
}

export interface UpdateOrderDto {
  status: OrderStatus;
}
