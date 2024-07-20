import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../services/notification.service';
import { Router } from '@angular/router';
import { DialogsService } from '../services/dialog.service';

@Injectable()
export class GlobalHttpInterceptorService implements HttpInterceptor {
  constructor(private notificationService: NotificationService, private router: Router, private customDialogService: DialogsService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        const currentUrl = this.router.url; // Lưu trữ URL hiện tại

        if (error.status === 500) {
          this.notificationService.showError('Hệ thống có lỗi xảy ra. Vui lòng liên hệ admin');
        } 
        if (error.status === 403 && error.error == null ) {
          this.notificationService.showError('Bạn không có quyền để thực hiện thao tác này.');
          this.customDialogService.closeDialog();
          // Điều hướng tạm thời tới trang nào đó, ví dụ như dashboard
          this.router.navigate(['/']).then(() => {
            // Điều hướng trở lại trang hiện tại sau khi hiển thị thông báo
            this.router.navigate([currentUrl]);
          });
          return of(error as any);
        }else
        {
          throw error;
        }

        // Trả về lỗi để tiếp tục xử lý
        return of(error as any);
      })
    );
  }
}
