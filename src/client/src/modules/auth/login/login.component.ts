import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';
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


  constructor(private fb: FormBuilder, private router: Router, private dialog: MatDialog) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(7)]]
    });
  }

  login() {
    if (this.loginForm.valid) {
      console.log('Login success:', this.loginForm.value);
      console.log('Username:', this.loginForm.get('username')?.value);
      console.log('Password:', this.loginForm.get('password')?.value);
      // this.router.navigate(['/']);
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
