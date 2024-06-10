import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateCustomerDto {
  name?: string;
  email?: string;
  phone?: string;
  address?: string;
  username?: string;
  password?: string;
}

export interface CustomerDto {
  name?: string;
  email?: string;
  phone?: string;
  address?: string;
  username?: string;
  password?: string;
  id?: string;
}

export interface CustomerInListDto extends EntityDto<string> {
  name?: string;
  email?: string;
  phone?: string;
  address?: string;
  username?: string;
  password?: string;
}
