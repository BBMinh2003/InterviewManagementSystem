import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import { faHome } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../../../../services/user/user.service';
import { Router, RouterModule } from '@angular/router';
import DepartmentView from '../../../../models/common/department.model';
import RoleView from '../../../../models/role/role.model';
import {
  NOTIFICATION_SERVICE,
  LOADING_SERVICE,
} from '../../../../constants/injection.constant';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../../core/components/notification/notification.component';

@Component({
  selector: 'app-user-create',
  imports: [
    CommonModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    RouterModule,
  ],
  templateUrl: './user-create.component.html',
  styleUrl: './user-create.component.css',
})
export class UserCreateComponent implements OnInit {
  public faHome: IconDefinition = faHome;
  public userForm: FormGroup = new FormGroup({});
  public roles: RoleView[] = [];
  public departments: DepartmentView[] = [];
  constructor(
    private readonly fb: FormBuilder,
    private readonly userService: UserService,
    private readonly router: Router,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private readonly loadingService: ILoadingService
  ) { }

  ngOnInit(): void {
    this.initializeForm();

    this.fetchRolesAndDepartments();
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      this.loadingService.show();
      const userData = this.userForm.value;
      this.userService.createUser(userData).subscribe((response) => {
        this.notificationService.showMessage(
          'User created successfully',
          'success'
        );
        this.loadingService.hide();
        this.router.navigate(['/admin/user']);
      },
      (error)=>{
        this.loadingService.hide();
      });
    } else
      this.markFormGroupTouched(this.userForm)
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/admin/user']);
  }

  fetchRolesAndDepartments(): void {
    this.loadingService.show();

    this.userService.getRolesAndDepartments().subscribe((response) => {
      this.roles = response.roles;
      this.departments = response.departments;
      this.loadingService.hide();
    });
  }
  initializeForm(): void {
    this.userForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(255)]],
      email: [
        '',
        [Validators.required, Validators.email, Validators.maxLength(255)],
      ],
      dateOfBirth: [new Date(), Validators.required],
      address: ['', Validators.maxLength(500)],
      phoneNumber: [
        '',
        [Validators.required, Validators.pattern(/^\+?[0-9]{10,15}$/)],
      ],
      gender: [0, Validators.required],
      roleId: ['', Validators.required],
      departmentId: ['', Validators.required],
      isActive: [true, Validators.required],
      note: [''],
    });
  }
}
