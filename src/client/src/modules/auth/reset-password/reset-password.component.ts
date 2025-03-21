import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  resetPasswordForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router) {
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

  onSubmit() {
    if (this.resetPasswordForm.valid) {
      console.log("Password:", this.resetPasswordForm.get('password')?.value);
      console.log("Confirm Password:", this.resetPasswordForm.get('confirmPassword')?.value);
      alert('Password reset successfully!');
      this.router.navigate(['/login']);
    } else {
      alert('Please check the form and try again.');
    }
  }
}
