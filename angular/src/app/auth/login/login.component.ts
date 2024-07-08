import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsersService } from '@proxy/users';
import { Subject, switchMap, takeUntil, throwError } from 'rxjs';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { LoginRequestDto } from 'src/app/shared/models/login-request.dto';
import { LoginResponseDto } from 'src/app/shared/models/login-response.dto';
import { AuthService } from 'src/app/shared/services/auth.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { TokenStorageService } from 'src/app/shared/services/token.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [
    `
      :host ::ng-deep .p-password input {
        width: 100%;
        padding: 1rem;
      }

      :host ::ng-deep .pi-eye {
        transform: scale(1.6);
        margin-right: 1rem;
        color: var(--primary-color) !important;
      }

      :host ::ng-deep .pi-eye-slash {
        transform: scale(1.6);
        margin-right: 1rem;
        color: var(--primary-color) !important;
      }
    `,
  ],
})
export class LoginComponent implements OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  valCheck: string[] = ['remember'];

  password!: string;

  loginForm: FormGroup;
  public blockedPanel: boolean = false;

  constructor(
    public layoutService: LayoutService,
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UsersService,
    private router: Router,
    private tokenService: TokenStorageService,
    private notificationService: NotificationService
  ) {
    this.loginForm = this.fb.group({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  login() {
    var username = this.loginForm.controls['username'].value;
    var password = this.loginForm.controls['password'].value;
    this.toggleBlockUI(true);
    var request: LoginRequestDto = {
      username: this.loginForm.controls['username'].value,
      password: this.loginForm.controls['password'].value,
    };
    //kiểm tra username và password có null không
    if (request.username == '' || request.password == '') {
      this.notificationService.showError('Vui lòng nhập tài khoản và mật khẩu.');
      this.toggleBlockUI(false);
      return;
    }
    

    this.authService.getUserIdByUsernameAsync(username).pipe(
      switchMap(userId => this.authService.checkPermission(userId)),
      switchMap(hasPermission => {
        if (hasPermission) {
          const request: LoginRequestDto = { username, password };
          return this.authService.login(request);
      } else {
        this.notificationService.showError('Bạn không có quyền đăng nhập.');
        return;
      }
      }),
      takeUntil(this.ngUnsubscribe)
    ).subscribe({next: (res: LoginResponseDto) => {
      this.tokenService.saveToken(res.access_token);
      this.tokenService.saveRefreshToken(res.refresh_token);
      this.toggleBlockUI(false);
      this.router.navigate(['']);
  },
  error: (ex) => {
      this.notificationService.showError('Đăng nhập không đúng.');
      this.toggleBlockUI(false);
  },
});
    
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

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
