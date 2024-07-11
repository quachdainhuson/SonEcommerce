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
  OrderStatus: any[] = [];
  keyword: string = '';
  categoryId: string = '';
  Status: number;
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
    this.loadOrderStatus();
    this.loadData();
  }
  loadOrderStatus() {
    this.OrderStatus = [
      { label: 'Tất cả', value: 0 },
      { label: 'Mới', value: OrderStatus.New },
      { label: 'Đã xác nhận', value: OrderStatus.Confirmed },
      { label: 'Đang xử lý', value: OrderStatus.Processing },
      { label: 'Đang giao hàng', value: OrderStatus.Shipping },
      { label: 'Hoàn thành', value: OrderStatus.Finished },
      { label: 'Đã hủy', value: OrderStatus.Canceled
      },
    ];
  }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);
      this.orderService
        .getListFilter({
          keyword: this.keyword,
          maxResultCount: this.maxResultCount,
          skipCount: this.skipCount,
          status: this.Status,
        }
        )
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: PagedResultDto<OrderInListDto>) => {
            this.items = response.items;
            this.totalCount = response.totalCount;
            this.toggleBlockUI(false);
          },
          
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    } 
    translateStatusToVietnamese(status: string) {
      switch (status) {
        case 'New':
          return 'Moi';
        case 'Confirmed':
          return 'DaXacNhan';
        case 'Processing':
          return 'DangXuLy';
        case 'Shipping':
          return 'DangGiaoHang';
        case 'Finished':
          return 'HoanThanh';
        case 'Canceled':
          return 'DaHuy';
        default:
          return status;
      }
    }
    pageChanged(event: any): void {
      this.maxResultCount = event.rows;
      this.skipCount = (event.first / this.maxResultCount) * this.maxResultCount;
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


  changeOrderStatus(order: OrderInListDto) {
    let newStatus;
    switch (order.status) {
      case OrderStatus.New:
        newStatus = OrderStatus.Confirmed;
        break;
      case OrderStatus.Confirmed:
        newStatus = OrderStatus.Processing;
        break;
      case OrderStatus.Processing:
        newStatus = OrderStatus.Shipping;
        break;
      case OrderStatus.Shipping:
        newStatus = OrderStatus.Finished;
        break;
      default:
        newStatus = OrderStatus.Finished;
    }
  
    this.orderService.changeStatusOrder(order.id, newStatus)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.notificationService.showSuccess('Cập nhật trạng thái đơn hàng thành công');
          this.loadData();
        },
        error: () => {
          this.notificationService.showError('Có lỗi xảy ra khi cập nhật trạng thái đơn hàng');
        }
      });
  }
  
  getButtonSeverity(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Confirmed:
        return 'help'; // Màu tím
      case OrderStatus.Processing:
        return 'warning'; // Màu vàng
      case OrderStatus.Shipping:
        return 'info'; // Màu xanh dương
      case OrderStatus.Finished:
        return 'success'; // Màu xanh lá cây
      case OrderStatus.Canceled:
        return 'danger'; // Màu đỏ
      default:
        return 'basic'; // Màu mặc định
    }
  }
  
}