import type { CreateUpdateManufacturerDto, ManufacturerDto, ManufacturerInListDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseListFilterDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class ManufacturerService {
  apiName = 'Default';
  

  create = (input: CreateUpdateManufacturerDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufacturerDto>({
      method: 'POST',
      url: '/api/app/manufacturer',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/manufacturer/${id}`,
    },
    { apiName: this.apiName,...config });
  

  deleteMultiple = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/manufacturer/multiple',
      params: { ids },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufacturerDto>({
      method: 'GET',
      url: `/api/app/manufacturer/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ManufacturerDto>>({
      method: 'GET',
      url: '/api/app/manufacturer',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufacturerInListDto[]>({
      method: 'GET',
      url: '/api/app/manufacturer/all',
    },
    { apiName: this.apiName,...config });
  

  getListWithFilter = (input: BaseListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ManufacturerInListDto>>({
      method: 'GET',
      url: '/api/app/manufacturer/with-filter',
      params: { keyword: input.keyword, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateManufacturerDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufacturerDto>({
      method: 'PUT',
      url: `/api/app/manufacturer/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
