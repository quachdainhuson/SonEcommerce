import { AuthService, PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ProductCategoriesService, ProductCategoryInListDto } from '@proxy/product-categories';
import { ProductDto, ProductInListDto, ProductsService } from '@proxy/products';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, max, take, takeUntil } from 'rxjs';
import { ProductDetailComponent } from './product-detail.component';
import { NotificationService } from '../shared/services/notification.service';

@Component({
  selector: 'app=product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
})
export class ProductComponent implements OnInit, OnDestroy{
  private ngUnsubscribe = new Subject<void>();
  blockPanel: boolean = false;
  items: ProductInListDto[] = [];
  // Bieens phan trang
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  productCategories: any[] = [];
  keyword: string = '';
  categoryId: string = '';

  constructor( 
    private prodcutService: ProductsService,
    private productCategoryService: ProductCategoriesService,
    private dialogService: DialogService,
    private notificationService: NotificationService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  ngOnInit(): void {
    this.loadProductCategories();
    this.loadData();
  }
  
  loadProductCategories(){
    this.productCategoryService.getListAll().subscribe((response: ProductCategoryInListDto[]) => {
      response.forEach(element => {
        this.productCategories.push({
          label: element.name, 
          value: element.id
        });
      })
    });
  };
  loadData(){
    this.toggleBlockUI(true);
    this.prodcutService.getListFilter({
      keyword: this.keyword,
      categoryId: this.categoryId,
      maxResultCount: this.maxResultCount,
      skipCount: this.skipCount  
  }).pipe(takeUntil(this.ngUnsubscribe))
  .subscribe({
    next: (response: PagedResultDto<ProductInListDto>) => {
      this.items = response.items;
      this.totalCount = response.totalCount;
      this.toggleBlockUI(false);
    },
    error: () => {
      this.toggleBlockUI(false);
    }
  });
  }

  pageChanged(event: any): void {
    this.skipCount = (event.page - 1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }

  private toggleBlockUI(enable: boolean){
    if(enable == true){
      this.blockPanel = true;
    }else{
      setTimeout(() => {
        this.blockPanel = false;
      },1000);
    }
  }

  showAddModal(){
    const ref = this.dialogService.open(ProductDetailComponent, {
      header: 'Thêm Mới Sản Phẩm',
      width: '70%',
      
    });

    ref.onClose.subscribe((data : ProductDto) => {
      if(data){
        this.loadData();
        this.notificationService.showSuccess('Thêm mới thành công');
      }else{
        this.notificationService.showError('Thêm mới thất bại');
      }
    });
  }
}
