import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UsersService } from '@proxy/users';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/shared/services/auth.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MessageConstants } from 'src/app/shared/constants/messages.constant';

@Component({
  selector: 'app-changePass',
  templateUrl: './changePass.component.html',
})
export class ChangePasswordComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;
  public form: FormGroup;
  public btnDisabled = false;
  public userId: string;
  noSpecial: RegExp = /^[^<>*!_~]+$/;

  constructor(
    private userService: UsersService,
    private notificationService: NotificationService,
    public authService: AuthService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  buildForm() {
    this.form = new FormGroup({
      currentPassword: new FormControl(null, Validators.required),
      newPassword: new FormControl(null, [
        Validators.required,
        Validators.pattern(/^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/),
      ]),
      confirmPassword: new FormControl(null, Validators.required),
    });
  }

  ngOnInit() {
    this.buildForm();
    this.toggleBlockUI(false);
  }

  validationMessages = {
    currentPassword: [{ type: 'required', message: 'Bạn phải nhập mật khẩu hiện tại' }],
    newPassword: [
      { type: 'required', message: 'Bạn phải nhập mật khẩu mới' },
      { type: 'pattern', message: 'Mật khẩu ít nhất 8 ký tự, ít nhất 1 số, 1 ký tự đặc biệt, và một chữ hoa' },
    ],
    confirmPassword: [{ type: 'required', message: 'Bạn phải xác nhận mật khẩu mới' }],
  };

  private toggleBlockUI(enabled: boolean) {
    if (enabled) {
      this.btnDisabled = true;
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.btnDisabled = false;
        this.blockedPanel = false;
      }, 1000);
    }
  }

  public saveChange() {
    this.toggleBlockUI(true);

    if (this.form.invalid) {
      return;
    }

    const { newPassword, confirmPassword } = this.form.value;
    if (newPassword !== confirmPassword) {
      this.notificationService.showError('Mật khẩu mới và xác nhận mật khẩu không khớp');
      return;
    }

    this.saveData();
  }

  private saveData() {
    this.userId = this.authService.getUserIdFromToken();
    this.userService
      .changePassword(this.userId, this.form.value)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.toggleBlockUI(false);
          this.notificationService.showSuccess(MessageConstants.CHANGE_PASSWORD_SUCCCESS_MSG);
        },
        error: () => {
          this.toggleBlockUI(false);
          this.notificationService.showError('Đã xảy ra lỗi khi đổi mật khẩu');
        },
      });
  }
}
