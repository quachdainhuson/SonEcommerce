import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from '../shared/services/notification.service';
import { UtilityService } from '../shared/services/utility.service';
import { OrderDto, OrderItemDto, OrdersService } from '@proxy';
import { OrderStatus, PaymentMethod } from '@proxy/son-ecommerce/orders';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order.component.scss'],
})
export class OrderDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled = false;
  public form: FormGroup;

  //Dropdown
  dataTypes: any[] = [];
  selectedEntity = {} as OrderDto;
  selectedItem = {} as OrderItemDto;

  order: OrderDto;
  orderItems: OrderItemDto[];

  constructor(
    private orderService: OrdersService,
    private fb: FormBuilder,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef,
    private utilService: UtilityService,
    private notificationSerivce: NotificationService
  ) {}


  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    // this.initFormData();

    this.getOrderAndDetails();
  }


  getOrderAndDetails() {
    // Lấy ID đơn hàng từ route hoặc bất kỳ nguồn nào khác
    const orderId = this.config.data?.id;
    this.toggleBlockUI(true);

    this.orderService.getOrderAndDetails(orderId).subscribe(
      (orderDto) => {
        this.order = orderDto;
        this.orderItems = orderDto.orderItems;
        this.toggleBlockUI(false);
      },
      (error) => {
        this.toggleBlockUI(false);
      }
    );
  }

  // initFormData() {
  //   //Load edit data to form
  //   if (this.utilService.isEmpty(this.config.data?.id) == true) {
  //     this.toggleBlockUI(false);
  //   } else {
  //     this.loadFormDetails(this.config.data?.id);
  //   }
  // }

  // loadFormDetails(id: string) {
  //   this.toggleBlockUI(true);
  //   this.orderService
  //     .getOrderAndDetails(id)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response: any) => {
  //         this.selectedEntity = response;
  //         this.selectedItem = response;
  //         this.toggleBlockUI(false);
  //         console.log(response);
  //       },
  //       error: () => {
  //         this.toggleBlockUI(false);
  //       },
  //     });
  // }

  saveChange() {
    this.toggleBlockUI(true);
      this.orderService
        .update(this.config.data?.id, this.form.value)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.toggleBlockUI(false);
            this.ref.close(this.form.value);
          },
          error: err => {
            this.notificationSerivce.showError(err.error.error.message);
            this.toggleBlockUI(false);
          },
        });
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
      this.btnDisabled = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
        this.btnDisabled = false;
      }, 1000);
    }
  }


  getButtonTextClass(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Confirmed:
        return 'confirmed-text';
      case OrderStatus.Processing:
        return 'processing-text';
      case OrderStatus.Shipping:
        return 'shipping-text';
      case OrderStatus.Finished:
        return 'finished-text';
      case OrderStatus.Canceled:
        return 'canceled-text';
      default:
        return '';
    }
  }
  
  
}