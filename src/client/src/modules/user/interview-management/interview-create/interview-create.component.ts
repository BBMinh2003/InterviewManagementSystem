import { Component, Inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import {
  AUTH_SERVICE,
  CANDIDATE_SERVICE,
  INTERVIEW_SERVICE,
  INTERVIEWER_SERVICE,
  JOB_SERVICE,
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
  USER_SERVICE,
} from '../../../../constants/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faHome,
  faTimes,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { Result } from '../../../../core/enums/result';
import { InterviewStatus } from '../../../../core/enums/interview-status';
import { Router, RouterModule } from '@angular/router';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { IJobService } from '../../../../services/job/job-service.interface';
import { IUserService } from '../../../../services/user/user-service.interface';
import { JobModel } from '../../../../models/job/job.model';
import { UserModel } from '../../../../models/user/user.model';
import { NgSelectModule } from '@ng-select/ng-select';
import { IInterviewerService } from '../../../../services/interviewer/interviewer-service.interface';
import { InterviewerModel } from '../../../../models/user/interview.model';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';

@Component({
  selector: 'app-interview-create',
  templateUrl: './interview-create.component.html',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
    FontAwesomeModule,
    RouterModule,
    LoadingComponent,
    NgSelectModule,
  ],
  styleUrls: ['./interview-create.component.css'],
})
export class InterviewCreateComponent implements OnInit {
  interviewForm!: FormGroup;

  resultEnum = Result;
  statusEnum = InterviewStatus;
  public faHome: IconDefinition = faHome;

  resultKeys: number[] = Object.keys(Result)
    .filter((key) => !isNaN(Number(key)))
    .map(Number);
  statusKeys: number[] = Object.keys(InterviewStatus)
    .filter((key) => !isNaN(Number(key)))
    .map(Number);

  interviewers!: InterviewerModel[];
  recruiters!: InterviewerModel[];
  formSubmitted: boolean = false;
  jobs!: JobModel[];
  candidates: CandidateModel[] = [];
  public interviewerTouched = false;
  isSubmitting = false;
  public faTimes: IconDefinition = faTimes;
  public recruiterOwnerName!: string | undefined;
  selectedInterviewers: { id: string; fullName: string }[] = [];
  constructor(
    private fb: FormBuilder,
    @Inject(INTERVIEW_SERVICE) private interviewService: IInterviewService,
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService,
    @Inject(JOB_SERVICE) private jobService: IJobService,
    @Inject(LOADING_SERVICE) private loadingService: ILoadingService,

    @Inject(USER_SERVICE) private userService: IUserService,
    @Inject(INTERVIEWER_SERVICE)
    private interviewerService: IInterviewerService,

    public router: Router
  ) {}

  ngOnInit(): void {
    this.createForm();
    this.loadCandidates();
    this.loadInterviewer();
    this.loadJob();
  }

  public loadCandidates(): void {
    this.candidateService.getAll().subscribe((data) => {
      this.candidates = data;
    });
  }
  private loadInterviewer(): void {
    this.interviewerService.getInterviewers().subscribe((users) => {
      if (users && Array.isArray(users)) {
        this.interviewers = users;
      } else {
        console.error('User role is undefined or invalid data structure.');
      }
    });
    this.interviewerService.getRecruitersAndManagers().subscribe((users) => {
      if (users && Array.isArray(users)) {
        this.recruiters = users;
      } else {
        console.error('User role is undefined or invalid data structure.');
      }
    });
  }
  private loadJob(): void {
    this.jobService.getAll().subscribe(
      (data) => {
        this.jobs = data;
      },
      (error) => {
        console.error('Error fetching candidates:', error);
      }
    );
  }
  futureDateValidator: ValidatorFn = (
    control: AbstractControl
  ): ValidationErrors | null => {
    if (!control.value) return null;
    const today = new Date().setHours(0, 0, 0, 0);
    const selectedDate = new Date(control.value).setHours(0, 0, 0, 0);
    return selectedDate >= today ? null : { futureDate: true };
  };

  startBeforeEndValidator: ValidatorFn = (
    group: AbstractControl
  ): ValidationErrors | null => {
    const startAt = group.get('startAt')?.value;
    const endAt = group.get('endAt')?.value;

    if (!startAt || !endAt) return null;
    return startAt < endAt ? null : { invalidTimeRange: true };
  };

  private createForm(): void {
    this.interviewForm = this.fb.group(
      {
        title: ['', [Validators.required, Validators.maxLength(100)]],
        candidateId: ['', Validators.required],
        interviewDate: ['', [Validators.required, this.futureDateValidator]],
        startAt: ['', Validators.required],
        endAt: ['', Validators.required],
        notes: ['', [Validators.maxLength(500)]],
        jobId: ['', Validators.required],
        interviewerIds: [[], Validators.required],
        location: ['', [Validators.maxLength(255)]],
        recruiterOwnerId: ['', Validators.required],
        meetingID: [''],
        result: [{ value: Result.NA, disabled: true }],
        status: [{ value: InterviewStatus.New, disabled: true }],
      },
      {
        validators: this.startBeforeEndValidator,
      }
    );
  }

  public currentAdmin: { id: string; fullName: string } | null = null;
  public clearAdminAssignment(): void {
    this.currentAdmin = null;
    this.interviewForm.patchValue({
      recruiterOwnerId: '',
    });
  }

  public assignCurrentUser(): void {
    this.authService.getUserInformation().subscribe((user) => {
      if (user) {
        this.currentAdmin = {
          id: user.id,
          fullName: user.displayName || 'Admin',
        };

        this.interviewForm.patchValue({
          recruiterOwnerId: user.id,
        });
      }
    });
  }
  public goBack(): void {
    this.router.navigate(['/interview']);
  }
  public onSubmit(): void {
    this.isSubmitting = true;

    if (!this.interviewForm || this.interviewForm.invalid) {
      this.interviewForm.markAllAsTouched(); 
      this.isSubmitting = false;
      return;
    }

    this.loadingService.show();

    const formData = this.interviewForm.value;
    const startTime = Date.now();
    const MIN_LOADING_TIME = 2000;

    this.interviewService.create(formData).subscribe({
      next: (response) => {
        const elapsed = Date.now() - startTime;
        const remainingTime = Math.max(0, MIN_LOADING_TIME - elapsed);

        setTimeout(() => {
          this.loadingService.hide();
          this.router.navigate(['/interview']);
          this.notificationService.showMessage(
            'Interview created successfully',
            'success'
          );
          this.isSubmitting = false;
        }, remainingTime);
      },
      error: (error) => {
        const elapsed = Date.now() - startTime;
        const remainingTime = Math.max(0, MIN_LOADING_TIME - elapsed);

        setTimeout(() => {
          this.loadingService.hide();
          console.error('Error creating interview:', error);
          this.notificationService.showMessage(
            'Error creating interview',
            'error'
          );
          this.isSubmitting = false;
        }, remainingTime);
      },
    });
  }
  // getFirstError(controlName: string): string | null {
  //   const control = this.interviewForm.get(controlName);
  //   if (
  //     !control ||
  //     !control.errors ||
  //     (!control.touched && !this.formSubmitted)
  //   ) {
  //     return null;
  //   }

  //   if (control.errors['required']) {
  //     return 'This field is required';
  //   }

  //   if (control.errors['maxlength']) {
  //     return `Max ${control.errors['maxlength'].requiredLength} character`;
  //   }

  //   if (control.errors['futureDate']) {
  //     return 'The date must be in the future.';
  //   }

  //   return null;
  // }
  showInterviewerError(): boolean {
    return this.interviewerTouched && this.selectedInterviewers.length === 0;
  }
}
