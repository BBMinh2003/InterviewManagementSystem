import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faHome, faSave, faTimes, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';

import { NOTIFICATION_SERVICE, LOADING_SERVICE, USER_SERVICE } from '../../../constants/injection.constant'; 
import { ILoadingService } from '../../../services/loading/loading-service.interface'; 
import { INotificationService } from '../../../services/notification/notification-service.interface'; 
import { UserModel } from '../../../models/user/user.model';
import { IUserService } from '../../../services/user/user-service.interface'; 
import { ProfileEditModel } from '../../../models/user/profile-edit.model';

interface SelectOption {
  id: string | number;
  name: string;
}

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ],
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css']
})
export class ProfileEditComponent implements OnInit, OnDestroy {
  faHome: IconDefinition = faHome;
  faSave: IconDefinition = faSave;
  faCancel: IconDefinition = faTimes;

  userForm!: FormGroup;
  currentUser: UserModel | null = null;
  isLoading: boolean = false;

  roles: SelectOption[] = [];
  departments: SelectOption[] = [];

  private userSub: Subscription | undefined;
  private updateSub: Subscription | undefined;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    @Inject(USER_SERVICE) private readonly userService: IUserService,
    @Inject(NOTIFICATION_SERVICE) private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE) private readonly loadingService: ILoadingService
  ) { }

  ngOnInit(): void {
    this.loadingService.show()
    this.initForm();
    this.loadCurrentUser();
  }

  ngOnDestroy(): void {
    this.userSub?.unsubscribe();
    this.updateSub?.unsubscribe();
  }

  private initForm(): void {
    this.userForm = this.fb.group({
      id: [null],
      fullName: ['', [Validators.required, Validators.maxLength(255)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9\\-\\+\\s\\(\\)]*$')]],
      dateOfBirth: [null],
      address: [''],
      gender: [null, Validators.required],
      note: [''],
    });
  }

  private loadCurrentUser(): void {
    this.loadingService.show();
    this.isLoading = true;
    this.userSub = this.userService.getCurrentUserInformation().subscribe({
      next: (user) => {
        if (user) {
          this.currentUser = user;
          this.userForm.patchValue({
            ...user,
            dateOfBirth: user.dateOfBirth
          });
        } else {
          this.notificationService.showMessage('Could not load user profile.', 'error');
          this.router.navigate(['/home']); 
        }
        this.loadingService.hide();
        this.isLoading = false;
      },
      error: (err) => {
        console.error("Error loading user profile:", err);
        this.notificationService.showMessage('Error loading profile. Please try again.', 'error');
        this.loadingService.hide();
        this.isLoading = false;
        this.router.navigate(['/profile']); 
      }
    });
  }

  private formatDateForInput(date: Date): string | null {
    if (!date || !(date instanceof Date) || isNaN(date.getTime())) {
      return null;
    }
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');

    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }


  onSubmit(): void {
    this.userForm.markAllAsTouched(); 
    if (this.userForm.invalid) {
      this.notificationService.showMessage('Please correct the errors in the form.', 'error');
      const firstInvalidControl = Object.keys(this.userForm.controls).find(key => {
        const control = this.userForm.get(key);
        return control && control.invalid;
      });
      if (firstInvalidControl) {
        document.getElementById(firstInvalidControl)?.focus();
      }
      return;
    }

    this.loadingService.show();
    this.isLoading = true;

    const formValue = { ...this.userForm.value };

    console.log("Form Value:", formValue);
    
    const payload: ProfileEditModel = {
      ...formValue, 
      dateOfBirth: formValue.dateOfBirth ? new Date(formValue.dateOfBirth) : null,
    };

    this.updateSub = this.userService.updateCurrentUserProfile(payload) 
      .subscribe({
        next: (updatedUser) => {
          this.notificationService.showMessage('Profile updated successfully!', 'success');
          this.loadingService.hide();
          this.isLoading = false;
          this.router.navigate(['/profile']); 
        },
        error: (err) => {
          console.error("Error updating profile:", err);
          this.notificationService.showMessage('Failed to update profile. Please try again.', 'error');
          this.loadingService.hide();
          this.isLoading = false;
        }
      });
  }

  cancel(): void {
    this.router.navigate(['/profile']); 
  }

  get f(): { [key: string]: AbstractControl } {
    return this.userForm.controls;
  }
}