import { Component, Inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { CandidateService } from '../../../../services/candidate/candidate.service';
import { PositionService } from '../../../../services/position/position.service';
import { UserService } from '../../../../services/user/user.service';
import { DepartmentService } from '../../../../services/department/department.service';
import { InterviewService } from '../../../../services/interview/interview.service';
import { ContactTypeService } from '../../../../services/contact/contactType.service';
import { LevelService } from '../../../../services/level/level.service';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { faHome } from '@fortawesome/free-solid-svg-icons';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../../core/components/notification/notification.component';
import { OfferService } from '../../../../services/offer/offer.service';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import {
  AUTH_SERVICE,
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
} from '../../../../constants/injection.constant';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';

@Component({
  selector: 'app-offer-create',
  imports: [
    CommonModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    RouterModule,
    NotificationComponent,
    LoadingComponent,
  ],
  templateUrl: './offer-create.component.html',
})
export class OfferCreateComponent implements OnInit {
  public faHome: IconDefinition = faHome;
  offerForm!: FormGroup;
  candidates: any[] = [];
  contactTypes: any[] = [];
  positions: any[] = [];
  levels: any[] = [];
  approvers: any[] = [];
  departments: any[] = [];
  interviews: any[] = [];
  recruiters: any[] = [];
  public recruiterOwnerName!: string | undefined;

  isSubmitting = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly offerService: OfferService,
    private readonly candidateService: CandidateService,
    private readonly contactTypeService: ContactTypeService,
    private readonly positionService: PositionService,
    private readonly levelService: LevelService,
    private readonly userService: UserService,
    private readonly departmentService: DepartmentService,
    private readonly interviewService: InterviewService,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(LOADING_SERVICE) private readonly loadingService: ILoadingService,
    public router: Router
  ) {}

  private createForm(): void {
    const datePipe = new DatePipe('en-US');
    const today = datePipe.transform(new Date(), 'yyyy-MM-dd');

    this.offerForm = this.fb.group(
      {
        candidateId: ['', Validators.required],
        contactTypeId: ['', Validators.required],
        positionId: ['', Validators.required],
        levelId: ['', Validators.required],
        approverId: ['', Validators.required],
        departmentId: ['', Validators.required],
        interviewId: [''],
        recruiterOwnerId: ['', Validators.required],
        contactPeriodFrom: [
          today,
          [Validators.required, this.futureDateValidator],
        ],
        contactPeriodTo: ['', [Validators.required, this.futureDateValidator]],
        dueDate: [today, [Validators.required, this.futureDateValidator]],
        basicSalary: [
          '',
          [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)],
        ],
        interviewNotes: [''],
        note: ['', Validators.maxLength(500)],
      },
      { validators: this.dateRangeValidator() }
    );
  }

  ngOnInit(): void {
    this.createForm();
    this.loadDropdownData();
  }

  private loadDropdownData(): void {
    this.loadingService.show();

    this.candidateService
      .getAll()
      .subscribe((data) => (this.candidates = data));
    this.contactTypeService
      .getAll()
      .subscribe((data) => (this.contactTypes = data));
    this.positionService.getAll().subscribe((data) => (this.positions = data));
    this.levelService.getAll().subscribe((data) => (this.levels = data));
    this.userService.getAll().subscribe((users) => {
      if (users && Array.isArray(users)) {
        this.approvers = users.filter((user) =>
          user.roles?.some((role) => role === 'Manager' || role === 'Admin')
        );
      } else {
        console.error('User role is undefined or invalid data structure.');
      }
    });
    this.departmentService
      .getAll()
      .subscribe((data) => (this.departments = data));
    this.interviewService
      .getAll()
      .subscribe((data) => (this.interviews = data));
    this.userService.getAll().subscribe((users) => {
      if (users && Array.isArray(users)) {
        this.recruiters = users.filter((user) =>
          user.roles?.some((role) => role === 'Recruiter' || role === 'Admin')
        );
      } else {
        console.error('User role is undefined or invalid data structure.');
      }
    });

    this.loadingService.hide();
  }

  onInterviewSelect(event: any): void {
    const interviewId = event.target.value;
    const selectedInterview = this.interviews.find((i) => i.id === interviewId);

    if (selectedInterview) {
      this.offerForm.patchValue({
        interviewNotes: selectedInterview.note,
      });
    }
  }

  public assignCurrentUser(): void {
    this.authService.getUserInformation().subscribe((user) => {
      if (user) {
        this.recruiterOwnerName = user.displayName || user.id;
        this.offerForm.patchValue({ recruiterOwnerId: user.id });
      }
    });
  }

  updateContractPeriod(type: 'From' | 'To'): void {
    const value = this.offerForm.get(`contactPeriod${type}`)?.value;
    if (value) {
      this.offerForm.get(`contactPeriod${type}`)?.patchValue(value);
    }
  }

  updateDueDate(): void {
    const value = this.offerForm.get('dueDate')?.value;
    if (value) {
      this.offerForm.get('dueDate')?.patchValue(value);
    }
  }

  private dateRangeValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const from = control.get('contactPeriodFrom')?.value;
      const to = control.get('contactPeriodTo')?.value;

      if (from && to && new Date(to) <= new Date(from)) {
        return { dateRangeInvalid: true };
      }
      return null;
    };
  }

  private futureDateValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const selectedDate = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return selectedDate < today ? { pastDate: true } : null;
  }

  onSubmit(): void {
    if (this.offerForm.invalid) {
      this.markFormGroupTouched(this.offerForm);
      return;
    }
    this.loadingService.show();
    this.isSubmitting = true;
    const formData = this.offerForm.value;
    console.log(formData);

    this.offerService.create(formData).subscribe({
      next: (response) => {
        this.loadingService.hide();
        this.router.navigate(['/offer']);
        this.notificationService.showMessage(
          'Offer created successfully',
          'success'
        );
        this.isSubmitting = false;
      },
      error: (error) => {
        this.loadingService.hide();
        console.error('Error creating Offer:', error);
        this.notificationService.showMessage(
          error.error?.message || 'Failed to create offer',
          'error'
        );
        this.isSubmitting = false;
      },
    });
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach((control) => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/offer']);
  }

  get f() {
    return this.offerForm.controls;
  }
}
