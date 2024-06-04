import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { OrderStatus, PaymentMethod } from '@proxy/son-ecommerce/orders';
import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, take, takeUntil } from 'rxjs';
import { NotificationService } from '../shared/services/notification.service';
import { OrderDetailComponent } from './order-detail.component';
import { OrderDto, OrderInListDto, OrdersService } from '@proxy';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',

})
export class OrderComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  items: OrderInListDto[] = [];
  public selectedItems: OrderInListDto[] = [];

  //Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  //Filter
  AttributeCategories: any[] = [];
  keyword: string = '';
  categoryId: string = '';

  constructor(
    private orderService: OrdersService,
    private dialogService: DialogService,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.loadData();
  }


  loadData(selectionId = null) {
    this.toggleBlockUI(true);
      this.orderService
        .getListAll()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: OrderInListDto[]) => {
            this.items = response;
            this.totalCount = response.length; // Since there's no pagination, totalCount is the length of the response array
            if (selectionId != null && this.items.length > 0) {
              this.selectedItems = this.items.filter(x => x.id == selectionId);
            }
            this.toggleBlockUI(false);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    } 

  pageChanged(event: any): void {
    this.skipCount = (event.page - 1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }
  showAddModal() {
  }

  
  getPaymentTypeName(value: number){
    return PaymentMethod[value];
  }

  getStatusTypeName(value: number){
    return OrderStatus[value];
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }

  showDetailOrder(id: string){
    const ref = this.dialogService.open(OrderDetailComponent, {
      header: 'Chi tiết đơn hàng',
      width: '70%',
      data: {
        id: id,
      },
    });

    ref.onClose.subscribe((data: OrderDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Cập nhật đơn hàng thành công');
        this.selectedItems = [];
      }
    });
  }
}