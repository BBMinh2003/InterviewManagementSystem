import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import { faHome } from '@fortawesome/free-solid-svg-icons';
import DepartmentView from '../../../../models/common/department.model';
import RoleView from '../../../../models/role/role.model';
import { UserService } from '../../../../services/user/user.service';
import { UserModel } from '../../../../models/user/user.model';
import {
  NOTIFICATION_SERVICE,
  LOADING_SERVICE,
} from '../../../../constants/injection.constant';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { tap, concatMap, finalize, takeUntil, Subject } from 'rxjs';

@Component({
  selector: 'app-user-edit',
  imports: [
    CommonModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    RouterModule,
  ],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.css',
})
export class UserEditComponent implements OnInit {
  public faHome: IconDefinition = faHome;
  public userForm: FormGroup = new FormGroup({});
  public roles: RoleView[] = [];
  public departments: DepartmentView[] = [];
  public currentUser!: UserModel;
  private destroy$ = new Subject<void>();
  constructor(
    private readonly fb: FormBuilder,
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private readonly loadingService: ILoadingService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.initializeForm(); 

    if (!id) {
      this.notificationService.showMessage('User ID not found in route.', 'error');
      this.router.navigate(['/admin/user']);
      return;
    }

    this.loadingService.show(); 

    this.userService.getRolesAndDepartments().pipe(
      tap(response => {
        this.roles = response.roles;
        this.departments = response.departments;
      }),
      concatMap(() => {
        return this.userService.getById(id);
      }),
      tap(userResponse => {
        this.currentUser = userResponse;
        console.log('Fetched User Data:', this.currentUser);

        this.userForm.patchValue({
          id: id, 
          fullName: this.currentUser.fullName,
          email: this.currentUser.email,
          dateOfBirth: this.currentUser.dateOfBirth, 
          address: this.currentUser.address,
          phoneNumber: this.currentUser.phoneNumber,
          gender: this.currentUser.gender,
          roleId: this.roles.find(
            (role) => role.name === this.currentUser.roles?.[0] 
          )?.id || '', 
          departmentId: this.departments.find(
            (dept) => dept.name === this.currentUser.department
          )?.id || '', 
          isActive:
            this.currentUser.isActive === 1 ||
            this.currentUser.isActive === true,
          note: this.currentUser.note,
        });
      }),
      finalize(() => {
        this.loadingService.hide();
      }),
      takeUntil(this.destroy$)
    )
    .subscribe({
      next: () => {
        console.log('Successfully fetched all required data and patched form.');
      },
      error: (err) => {
        console.error('Error fetching initial data:', err);
        this.notificationService.showMessage('Failed to load user details.', 'error');
        this.router.navigate(['/admin/user']);
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      this.loadingService.show();
      const userData = this.userForm.value;
      this.userService.editUser(userData).subscribe(
        (response) => {
          this.currentUser = response;
          this.notificationService.showMessage(
            'Change has been successfully updated',
            'success'
          );
          this.loadingService.hide();
          this.router.navigate(["/admin/user"])
        }, (error) => {
          this.notificationService.showMessage('Failed to updated change');
          this.router.navigate(["/admin/user"])
        });
    } else
      this.notificationService.showMessage(
        'Please enter all of the required fields in a correct format.',
        'error'
      );
  }

  goBack(): void {
    this.router.navigate(['/admin/user']);
  }

  initializeForm(): void {
    this.userForm = this.fb.group({
      id: ['', [Validators.required]],
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
