import type { OrderStatus } from '../son-ecommerce/orders/order-status.enum';

export interface UpdateOrderDto {
  status: OrderStatus;
}
