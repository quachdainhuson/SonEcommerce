import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UsersService } from '@proxy/users';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/shared/services/auth.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MessageConstants } from 'src/app/shared/constants/messages.constant';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { PrimeNGConfig } from 'primeng/api';

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
    public authService: AuthService,
    private fb: FormBuilder,
    private ref: DynamicDialogRef,
    private primengConfig: PrimeNGConfig

  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  buildForm() {
    this.form = this.fb.group({
      currentPassword: new FormControl(null, Validators.required),
      newPassword: new FormControl(null, [
        Validators.required,
        Validators.pattern(/^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/),
      ]),
      confirmPassword: new FormControl(null, Validators.required),
    },
    passwordMatchingValidatior
  );
  }

  ngOnInit() {
    this.primengConfig.zIndex = {
      modal: 700,    // dialog, sidebar
      overlay: 1000,  // dropdown, overlaypanel
      menu: 1000,     // overlay menus
      tooltip: 1100
    };
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
          this.ref.close(this.form.value);
        },
        error: () => {
          this.toggleBlockUI(false);
          this.notificationService.showError('Đã xảy ra lỗi khi đổi mật khẩu');
        },
      });
  }
  
}
export const passwordMatchingValidatior: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const password = control.get('newPassword');
  const confirmPassword = control.get('confirmNewPassword');
  return password?.value === confirmPassword?.value ? null : { notmatched: true };
};