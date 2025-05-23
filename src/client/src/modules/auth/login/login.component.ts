import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import {
  AUTH_SERVICE,
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
} from '../../../constants/injection.constant';
import { INotificationService } from '../../../services/notification/notification-service.interface';
import { NotificationComponent } from '../../../core/components/notification/notification.component';
import { ILoadingService } from '../../../services/loading/loading-service.interface';
import { LoadingComponent } from '../../../core/components/loading/loading.component';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    RouterModule,
    ForgotPasswordComponent,
    NotificationComponent,
    LoadingComponent,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginForm: FormGroup;

  isForgotPasswordOpen = false;

  constructor(
    private fb: FormBuilder,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private loadingService: ILoadingService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
    });
  }

  login() {
    if (this.loginForm.valid) {
      this.loadingService.show();
      this.authService.login(this.loginForm.value).subscribe(
        (response) => {
          if (response) {
            this.loadingService.hide();

            this.notificationService.showMessage(
              ' ✅ Login successfully',
              'success'
            );
            this.router.navigate(['/home']);
          }
        },
        (error) => {
          this.loadingService.hide();
          if (error.status === 401) {
            this.notificationService.showMessage(
              'Wrong username or password',
              'error'
            );
          }
        }
      );
    }
  }

  openForgotPassword() {
    this.isForgotPasswordOpen = true;
  }

  closeForgotPassword() {
    this.isForgotPasswordOpen = false;
  }
}
