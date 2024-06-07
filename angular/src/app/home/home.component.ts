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
      { creationTime: '2023-01-15T00:00:00Z', total: 100, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2023-02-20T00:00:00Z', total: 150, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2023-03-10T00:00:00Z', total: 200, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2023-04-05T00:00:00Z', total: 250, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2023-06-05T00:00:00Z', total: 250, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2024-01-12T00:00:00Z', total: 300, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2024-02-25T00:00:00Z', total: 350, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2024-03-18T00:00:00Z', total: 400, status: 1, paymentMethod: 1, items: [] },
      { creationTime: '2024-04-22T00:00:00Z', total: 450, status: 1, paymentMethod: 1, items: [] }
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
