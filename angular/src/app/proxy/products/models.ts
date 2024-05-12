import type { ProductType } from '../son-ecommerce/products/product-type.enum';
import type { EntityDto } from '@abp/ng.core';
import type { BaseListFilterDto } from '../models';

export interface CreateUpdateProductDto {
  manufacturerId?: string;
  name?: string;
  code?: string;
  productType: ProductType;
  sku?: string;
  slug?: string;
  sortOrder: number;
  visiblity: boolean;
  isActive: boolean;
  sellPrice: number;
  categoryId?: string;
  seoMetaDescription?: string;
  description?: string;
  thumbnailPicture?: string;
}

export interface ProductDto {
  manufacturerId?: string;
  name?: string;
  code?: string;
  productType: ProductType;
  sku?: string;
  slug?: string;
  sortOrder: number;
  visiblity: boolean;
  isActive: boolean;
  sellPrice: number;
  categoryId?: string;
  seoMetaDescription?: string;
  description?: string;
  thumbnailPicture?: string;
  id?: string;
}

export interface ProductInListDto extends EntityDto<string> {
  name?: string;
  code?: string;
  sortOrder: number;
  coverPicture?: string;
  visibility: boolean;
  isActive: boolean;
}

export interface ProductListFilterDto extends BaseListFilterDto {
  categoryId?: string;
}
