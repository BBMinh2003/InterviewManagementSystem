import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  imports: [],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  constructor(
    private readonly router: Router
) {}
  email: string = '';

  submitEmail() {
    console.log('Reset password for:', this.email);
    alert('Password reset link has been sent to ' + this.email);
  }

  handleForgotPassword() {
    console.log('Navigate to forgot password');
    this.router.navigate(['/forgot-password']);
  }
}
