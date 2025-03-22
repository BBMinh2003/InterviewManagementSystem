import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import { AUTH_SERVICE } from '../../../constants/injection.constant';
import { ResetPasswordRequest } from '../../../models/auth/reset-password-request.model';

@Component({
  selector: 'app-reset-password',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  email: string = "";
  token: string = "";



  constructor(private fb: FormBuilder, private router: Router, private route: ActivatedRoute, @Inject(AUTH_SERVICE) private authService: IAuthService) {
    this.resetPasswordForm = this.fb.group({
      password: ['', [
        Validators.required,
        Validators.minLength(7),
        Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d).{7,}$/)
      ]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordsMatch });
  }

  passwordsMatch(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }
  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.email = params.get('email') ?? "";
      this.route.queryParamMap.subscribe(params => {
        let rawToken = params.get('token');

        if (rawToken) {
          rawToken = rawToken.replace(/\s/g, '+');
          this.token = decodeURIComponent(rawToken);
        }
      });
    });
  }
  onSubmit() {
    if (this.resetPasswordForm.valid) {
      const request = new ResetPasswordRequest(
        this.resetPasswordForm.value.password,
        this.resetPasswordForm.value.confirmPassword,
        this.email,
        this.token
      );

      this.authService.resetPassword(request).subscribe({
        next: (success) => {
          if (success) {
            alert('Password has been reset successfully!');
            this.router.navigate(['/login']);
          } else {
            alert('Failed to reset password. Please try again.');
          }
        },
        error: (err) => {
          alert(`Reset password failed: ${err.message}`);
        }
      });
    } else {
      alert('Please check the form and try again.');
    }
  }
}
