import { AuthService } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderInListDto, OrdersService } from '@proxy';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  items: OrderInListDto[];
  totalCount: number;
  basicData: any;

  constructor(private authService: AuthService, private orderService: OrdersService) {}

  ngOnInit() {
    this.getData();
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getData() {
    this.orderService
      .getListAll()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: OrderInListDto[]) => {
          this.items = response;
          this.totalCount = response.length;
          this.processChartData(); // Call method to process chart data after items are fetched
        },
        error: (error) => {
          // Handle error if needed
        },
      });
  }

  processChartData() {
    const monthlyIncome: number[] = new Array(12).fill(0);
    this.items.forEach((order) => {
      const date = new Date(order.creationTime);
      const month = date.getMonth();
      monthlyIncome[month] += order.total;
    });

    this.basicData = {
      labels: ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'],
      datasets: [{
        label: "Thu nhập hàng tháng",
        data: monthlyIncome,
        backgroundColor: ['#f08080'],
        borderWidth: 1
      }]
    };
  }

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }


  login() {
    this.authService.navigateToLogin();
  }
}
