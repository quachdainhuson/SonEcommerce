import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserDto } from '@proxy/system/users';
import { UsersService } from '@proxy/users';
import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { MessageConstants } from 'src/app/shared/constants/messages.constant';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ChangePasswordComponent } from './changePass.component';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;
  public form: FormGroup;
  public btnDisabled = false;
  public blockedPanelDetail: boolean = false;
  selectedEntity = {} as UserDto;
  userId: string;
  private fb: FormBuilder

  constructor(
    private userService: UsersService,
    public dialogService: DialogService,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService,
    public authService: AuthService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  validationMessages = {
    name: [{ type: 'required', message: 'Bạn phải nhập tên' }],
    surname: [{ type: 'required', message: 'Bạn phải nhập họ' }],
    email: [{ type: 'required', message: 'Bạn phải nhập email' }
    , { type: 'email', message: 'Email không đúng định dạng' }
    ],
    userName: [{ type: 'required', message: 'Bạn phải nhập tài khoản' }],
    phoneNumber: [{ type: 'required', message: 'Bạn phải nhập số điện thoại' }],
  };

  buildForm() {
    this.form = new FormGroup({
      email: new FormControl(this.selectedEntity.email || null, Validators.compose([Validators.required, Validators.email])),
      phoneNumber: new FormControl(this.selectedEntity.phoneNumber || null, Validators.required),
      name: new FormControl(this.selectedEntity.name || null, Validators.required),
      userName: new FormControl(this.selectedEntity.userName || null, Validators.required),
      surname: new FormControl(this.selectedEntity.surname || null, Validators.required),

    });
  }
  

  ngOnInit() {
    this.userId = this.authService.getUserIdFromToken();
    this.buildForm();
    this.loadUserProfile();
  }

  loadUserProfile() {
    if (this.userId) {
      this.userService.get(this.userId)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (user: UserDto) => {
            console.log(user);
            this.selectedEntity = user;
            console.log(this.selectedEntity);
            this.buildForm();
          },
          error: (error) => {
            this.notificationService.showError('Cập nhật thông tin bị lỗi');
          },
        });
    }
  }

  saveChange() {
    this.toggleBlockUI(true);
    this.saveData();
  }

  private saveData() {
    this.userId = this.authService.getUserIdFromToken();
    this.userService
      .updateProfile(this.userId, this.form.value)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.notificationService.showSuccess(MessageConstants.UPDATED_OK_MSG);
          this.toggleBlockUI(false);
        },
        error: (error) => {
          this.notificationService.showError(MessageConstants.UPDATED_FAIL_MSG);
          this.toggleBlockUI(false);
        },
      });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled) {
      this.btnDisabled = true;
      this.blockedPanelDetail = true;
    } else {
      setTimeout(() => {
        this.btnDisabled = false;
        this.blockedPanelDetail = false;
      }, 1000);
    }
  }

  changePassword() {
    const ref = this.dialogService.open(ChangePasswordComponent, {
      header: 'Đổi mật khẩu',
      width: '30%',
    });
    
    ref.onClose.subscribe((result) => {
      if (result) {
        this.notificationService.showSuccess(MessageConstants.CHANGE_PASSWORD_SUCCCESS_MSG);
      }
    });
  }
}
