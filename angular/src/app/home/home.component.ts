import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { OrderInListDto, OrdersService } from '@proxy';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  private ngUnsubscribe = new Subject<void>();
  items: OrderInListDto[];
  totalCount: number;

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService,private orderService: OrdersService,) {}

  basicData: any

  ngOnInit(){
    this.getData();
    this.basicData = {
      labels: ['Th1','Th2','Th3','Th4','Th5','Th6','Th7','Th8','Th9','Th10','Th11','Th12'],
      datasets:[{
        label: "Thu nhập hàng tháng",
        data: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120],
        backgroundColor: ['#f08080'],
        borderWidth: 1
      }]
    }
  }
  getData(){
      this.orderService
        .getListAll()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: OrderInListDto[]) => {
            this.items = response;
            this.totalCount = response.length;
            console.log(this.items);
          },
        });
  }

  login() {
    this.authService.navigateToLogin();
  }
}
