import type { EntityDto, PagedResultRequestDto } from '@abp/ng.core';
import type { OrderStatus } from './son-ecommerce/orders/order-status.enum';
import type { PaymentMethod } from './son-ecommerce/orders/payment-method.enum';
import type { IdentityUserDto } from './volo/abp/identity/models';
import type { ProductDto } from './products/models';

export interface BaseListFilterDto extends PagedResultRequestDto {
  keyword?: string;
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
  userCity?: string;
  userDistrict?: string;
  userWard?: string;
  creationTime?: string;
  customerUserId?: string;
  id?: string;
  orderItems: OrderItemDto[];
}

export interface OrderInListDto extends EntityDto<string> {
  code?: string;
  status: OrderStatus;
  paymentMethod: PaymentMethod;
  total: number;
  customerName?: string;
  customerPhoneNumber?: string;
  customerUserId?: string;
  customerAddress?: string;
  userCity?: string;
  userDistrict?: string;
  userWard?: string;
  creationTime?: string;
  id?: string;
  orderItems: OrderItemDto[];
  user: IdentityUserDto;
}

export interface OrderItemDto extends EntityDto {
  orderId?: string;
  productId?: string;
  quantity: number;
  name?: string;
  product: ProductDto;
  price: number;
}
