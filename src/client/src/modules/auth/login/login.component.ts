import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { AUTH_SERVICE } from '../../../constants/injection.constant';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, RouterModule, ForgotPasswordComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;

  isForgotPasswordOpen = false; // Trạng thái mở/đóng modal


  constructor(private fb: FormBuilder, @Inject(AUTH_SERVICE) private authService: IAuthService,
    private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(7)]]
    });
  }

  login() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe((response) => {
        if (response) {
          this.router.navigate(['/home']);
        }
      });
    }

  }

  // openForgotPassword() {
  //   this.dialog.open(ForgotPasswordComponent);
  // }


  openForgotPassword() {
    this.isForgotPasswordOpen = true;
  }

  closeForgotPassword() {
    this.isForgotPasswordOpen = false;
  }

  // closeForgotPassword() {
  //   this.isForgotPasswordOpen = false;
  // }
}
