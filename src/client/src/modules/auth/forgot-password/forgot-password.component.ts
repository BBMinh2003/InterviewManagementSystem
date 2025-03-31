import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AUTH_SERVICE, NOTIFICATION_SERVICE } from '../../../constants/injection.constant';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { ForgotPasswordRequest } from '../../../models/auth/forgot-password-request.model';
import { INotificationService } from '../../../services/notification/notification-service.interface';

@Component({
  selector: 'app-forgot-password',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;

  @Output() close = new EventEmitter<void>(); // Để đóng modal

  constructor(private fb: FormBuilder, @Inject(AUTH_SERVICE) private authService: IAuthService,
  @Inject(NOTIFICATION_SERVICE) private notificationService: INotificationService
) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  submit() {
    if (this.forgotPasswordForm.valid) {
      this.authService
        .forgotPassword(new ForgotPasswordRequest(this.forgotPasswordForm.value.email, "http://localhost:4200/"))
        .subscribe({
          next: (response) => {
            this.notificationService.showMessage('Send email successfully','success');
            this.close.emit();
          },
          error: (error) => {
            alert("Failed to send reset password email. Please try again later.");
          }
        });
    }
  }

}
