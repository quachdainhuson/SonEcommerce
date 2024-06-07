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
  yearlyIncome: { [year: number]: number[] } = {};
  yearlyTotalIncome: { [year: number]: number } = {};
  selectedYear: number;
  yearOptions: { label: string, value: number }[] = [];

  constructor(private authService: AuthService, private orderService: OrdersService) {}

  ngOnInit() {
    this.getData();
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getData() {
    // this.orderService
    //   .getListAll()
    //   .pipe(takeUntil(this.ngUnsubscribe))
    //   .subscribe({
    //     next: (response: OrderInListDto[]) => {
    //       this.items = response;
    //       this.totalCount = response.length;
    //       this.processChartData(); // Call method to process chart data after items are fetched
    //     },
    //     error: (error) => {
    //       // Handle error if needed
    //     },
    //   });

    this.items = [
      {"creationTime": "2022-03-25T00:00:00Z", "total": 888, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2022-01-14T00:00:00Z", "total": 460, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2022-04-08T00:00:00Z", "total": 478, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2022-03-09T00:00:00Z", "total": 370, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2022-12-13T00:00:00Z", "total": 287, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2022-05-23T00:00:00Z", "total": 698, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2022-07-22T00:00:00Z", "total": 126, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2022-02-12T00:00:00Z", "total": 743, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2022-05-13T00:00:00Z", "total": 296, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2022-08-19T00:00:00Z", "total": 686, "status": 1, "paymentMethod": 2, "items": []},
      
      {"creationTime": "2023-08-12T00:00:00Z", "total": 127, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2023-05-21T00:00:00Z", "total": 244, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2023-01-03T00:00:00Z", "total": 801, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2023-10-14T00:00:00Z", "total": 114, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2023-06-21T00:00:00Z", "total": 641, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2023-09-21T00:00:00Z", "total": 214, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2023-08-24T00:00:00Z", "total": 235, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2023-05-23T00:00:00Z", "total": 915, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2023-08-18T00:00:00Z", "total": 269, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2023-07-13T00:00:00Z", "total": 910, "status": 1, "paymentMethod": 2, "items": []},
      
      {"creationTime": "2024-03-14T00:00:00Z", "total": 324, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2024-08-18T00:00:00Z", "total": 877, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2024-06-08T00:00:00Z", "total": 300, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2024-06-10T00:00:00Z", "total": 854, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2024-06-11T00:00:00Z", "total": 981, "status": 1, "paymentMethod": 3, "items": []},
      {"creationTime": "2024-02-04T00:00:00Z", "total": 743, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2024-11-23T00:00:00Z", "total": 224, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2024-10-07T00:00:00Z", "total": 227, "status": 1, "paymentMethod": 1, "items": []},
      {"creationTime": "2024-12-19T00:00:00Z", "total": 864, "status": 1, "paymentMethod": 2, "items": []},
      {"creationTime": "2024-08-28T00:00:00Z", "total": 713, "status": 1, "paymentMethod": 1, "items": []}
    ];
    this.totalCount = this.items.length;
    this.processChartData();
  }

  processChartData() {
    this.yearlyIncome = {};
    this.yearlyTotalIncome = {};

    this.items.forEach((order) => {
      const date = new Date(order.creationTime);
      const year = date.getFullYear();
      const month = date.getMonth();

      if (!this.yearlyIncome[year]) {
        this.yearlyIncome[year] = new Array(12).fill(0);
        this.yearlyTotalIncome[year] = 0;
      }
      this.yearlyIncome[year][month] += order.total;
      this.yearlyTotalIncome[year] += order.total;
    });

    const currentYear = new Date().getFullYear();
    this.selectedYear = currentYear; // Default to the current year
    this.updateChartData(currentYear);
    this.yearOptions = this.getYears().map(year => ({ label: year.toString(), value: year }));
  }

  updateChartData(year: number) {
    const monthlyIncome = this.yearlyIncome[year] || new Array(12).fill(0);

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

  onYearChange(event: any) {
    const year = event.value;
    this.selectedYear = year;
    this.updateChartData(year);
  }

  getYears(): number[] {
    return Object.keys(this.yearlyIncome).map(Number);
  }

  getTotalIncomeForYear(year: number): number {
    return this.yearlyTotalIncome[year] || 0;
  }
}
