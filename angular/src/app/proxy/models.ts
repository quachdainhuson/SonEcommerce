import type { EntityDto, PagedResultRequestDto } from '@abp/ng.core';
import type { OrderStatus } from './son-ecommerce/orders/order-status.enum';
import type { PaymentMethod } from './son-ecommerce/orders/payment-method.enum';

export interface BaseListFilterDto extends PagedResultRequestDto {
  keyword?: string;
}

export interface CreateOrderDto {
  customerName?: string;
  customerPhoneNumber?: string;
  customerAddress?: string;
  customerUserId?: string;
  items: OrderItemDto[];
}

export interface OrderDto extends EntityDto<string> {
  code?: string;
  status: OrderStatus;
  paymentMethod: PaymentMethod;
  shippingFee: number;
  tax: number;
  total: number;
  subtotal: number;
  discount: number;
  grandTotal: number;
  customerName?: string;
  customerPhoneNumber?: string;
  customerAddress?: string;
  customerUserId?: string;
  id?: string;
  orderItems: OrderItemDto[];
  creationTime?: string;
}

export interface OrderInListDto extends EntityDto<string> {
  code?: string;
  status: OrderStatus;
  paymentMethod: PaymentMethod;
  total: number;
  customerName?: string;
  customerPhoneNumber?: string;
  customerUserId?: string;
  id?: string;
  items: OrderItemDto[];
  creationTime?: string;
}

export interface OrderItemDto extends EntityDto {
  orderId?: string;
  productId?: string;
  name?: string;
  sku?: string;
  quantity: number;
  price: number;
}
