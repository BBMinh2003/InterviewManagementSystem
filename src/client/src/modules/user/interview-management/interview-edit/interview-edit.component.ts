import { Component, Inject, OnInit, SimpleChanges } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { Result } from '../../../../core/enums/result';
import { InterviewStatus } from '../../../../core/enums/interview-status';
import {
  faHome,
  faTimes,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';
import {
  INTERVIEW_SERVICE,
  CANDIDATE_SERVICE,
  AUTH_SERVICE,
  NOTIFICATION_SERVICE,
  USER_SERVICE,
  LOADING_SERVICE,
  JOB_SERVICE,
  INTERVIEWER_SERVICE,
} from '../../../../constants/injection.constant';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../../../../core/components/confirmation-dialog/confirmation-dialog.component';
import { IUserService } from '../../../../services/user/user-service.interface';
import { UserModel } from '../../../../models/user/user.model';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { IJobService } from '../../../../services/job/job-service.interface';
import { JobModel } from '../../../../models/job/job.model';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { InterviewerModel } from '../../../../models/user/interview.model';
import { IInterviewerService } from '../../../../services/interviewer/interviewer-service.interface';
import {
  catchError,
  delay,
  finalize,
  forkJoin,
  Observable,
  of,
  switchMap,
  tap,
  throwError,
} from 'rxjs';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-interview-edit',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
    FontAwesomeModule,
    LoadingComponent,
    RouterModule,
    NgSelectModule,
  ],
  templateUrl: './interview-edit.component.html',
  styleUrls: ['./interview-edit.component.css'],
})
export class InterviewEditComponent implements OnInit {
  interviewerss!: InterviewerModel[];
  interviewForm!: FormGroup;
  public interview!: InterviewModel;
  interviewId: string | null = null;
  userRole: string = '';
  public faHome: IconDefinition = faHome;
  jobs!: JobModel[];
  isCancel!: boolean;
  formSubmitted: boolean = false;
  candidates: CandidateModel[] = [];
  public submitted = false;
  public faTimes: IconDefinition = faTimes;
  public recruiterOwnerId: string = '';
  public interviewerTouched = false;
  public recruiters!: InterviewerModel[];
  private admin!: InterviewerModel[];
  public isAdmin = false;
  selectedInterviewers: { id: string; fullName: string }[] = [];
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    @Inject(INTERVIEW_SERVICE)
    private readonly interviewService: IInterviewService,
    @Inject(CANDIDATE_SERVICE)
    private readonly candidateService: ICandidateService,
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(JOB_SERVICE) private readonly jobService: IJobService,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE) private readonly loadingService: ILoadingService,
    @Inject(INTERVIEWER_SERVICE)
    private readonly interviewerService: IInterviewerService,
    public dialog: MatDialog,
    public router: Router
  ) {}

  private createForm(): void {
    this.interviewForm = this.fb.group(
      {
        title: ['', [Validators.required, Validators.maxLength(100)]],
        candidateId: ['', Validators.required],
        interviewDate: ['', [Validators.required, this.futureDateValidator]],
        startAt: ['', Validators.required],
        endAt: ['', Validators.required],
        note: ['', [Validators.maxLength(500)]],
        jobId: ['', Validators.required],
        interviewerIds: [[], Validators.required],
        location: ['', [Validators.maxLength(255)]],
        recruiterOwnerId: ['', Validators.required],
        meetingUrl: [''],
        result: [{ value: Result.NA, disabled: true }],
        status: [{ value: InterviewStatus.New, disabled: true }],
      },
      {
        validators: this.startBeforeEndValidator,
      }
    );
  }
  patchInterviewForm(interview: any): void {
    if (!this.interviewForm) {
      this.createForm();
    }

    this.interviewForm.patchValue({
      title: interview?.title || '',
      candidateId: interview?.candidateId || '',
      interviewDate: interview?.interviewDate || '',
      startAt: interview?.startAt || '',
      endAt: interview?.endAt || '',
      note: interview?.note || '',
      jobId: interview?.jobId || '',
      interviewerIds:
        this.selectedInterviewers.map((interviewer) => interviewer.id) || [],
      location: interview?.location || '',
      recruiterOwnerId: interview.recruiterOwnerId || '',
      meetingUrl: interview?.meetingUrl || '',
      result: interview?.result || Result.NA,
      status: interview?.status || InterviewStatus.New,
    });
  }
  ngOnInit(): void {
    this.loadingService.show();
    this.createForm();

    forkJoin([this.loadCandidates(), this.loadInterviewer(), this.loadJob()])
      .pipe(
        switchMap(() => {
          const id = this.route.snapshot.paramMap.get('id');
          if (id) {
            this.interviewId = id;
            return this.loadInterview(id);
          }
          return of(null);
        }),
        finalize(() => {
          this.loadingService.hide();
        })
      )
      .subscribe({
        error: (err) => console.error('Error loading data', err),
      });

    this.loadUserInfo();
  }

  private loadUserInfo(): void {
    this.authService.getUserInformation().subscribe({
      next: (user) => {
        if (user && user.role) {
          this.userRole = user.role;
        }
      },
    });
  }
  private loadInterviewer(): Observable<any> {
    return forkJoin([
      this.interviewerService.getInterviewers(),
      this.interviewerService.getRecruitersAndManagers(),
      this.interviewerService.getAdmins(),
    ]).pipe(
      tap(([interviewers, recruiters, admins]) => {
        this.interviewerss = interviewers;
        this.recruiters = recruiters;
        this.admin = admins;
      }),
      catchError((error) => {
        console.error('Error loading interviewers', error);
        return of(null);
      })
    );
  }

  private loadJob(): Observable<any> {
    return this.jobService.getAll().pipe(
      tap((data) => (this.jobs = data)),
      catchError((error) => {
        console.error('Error fetching jobs:', error);
        return of(null);
      })
    );
  }

  private loadInterview(id: string): Observable<any> {
    this.loadingService.show();
    return this.interviewService.getById(id).pipe(
      tap(),
      delay(2000),
      tap((data) => {
        this.processInterviewData(data);

        this.isAdmin = data.recruiterOwnerName === 'Admin';

        const recruiterOwner = this.recruiters.find(
          (recruiter) => recruiter.fullName === data.recruiterOwnerName
        );

        if (recruiterOwner) {
          this.interviewForm.patchValue({
            recruiterOwnerId: recruiterOwner.id,
          });
        }
        if (data.interviewers) {
          this.selectedInterviewers = (data.interviewers || []).map(
            (interviewer: any) => {
              const existingInterviewer = this.interviewerss.find(
                (i) => i.id === interviewer.userId
              );

              return existingInterviewer
                ? {
                    id: existingInterviewer.id,
                    fullName: existingInterviewer.fullName,
                  }
                : {
                    id: interviewer.userId,
                    fullName: interviewer.fullName || 'Unknown',
                  };
            }
          );
          this.interviewForm.patchValue({
            interviewerIds: this.selectedInterviewers.map((i) => i.id),
          });
        }
      }),
      catchError((err) => {
        return throwError(err);
      }),
      finalize(() => {
        this.loadingService.hide();
      })
    );
  }

  private loadCandidates(): Observable<any> {
    return this.candidateService.getAll().pipe(
      tap((data) => (this.candidates = data)),
      catchError((error) => {
        console.error('Error fetching candidates:', error);
        return of(null);
      })
    );
  }
  private processInterviewData(data: any): void {
    this.interview = data;
    console.log(this.interview);

    if (data) {
      const job = this.jobs.find((j) => j.title === data.jobName);
      const candidate = this.candidates.find(
        (c) => c.name === data.candidateName
      );
      const recruiter = this.admin.find(
        (a) => a.fullName === data.recruiterOwnerName
      );
      const patchData: any = {
        ...data,
        jobId: job ? job.id : '',
        candidateId: candidate ? candidate.id : '',
      };

      if (recruiter) {
        patchData.recruiterOwnerId = recruiter.id;
      }
      this.patchInterviewForm(patchData);
    }
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

  public get statusDisplay(): string {
    return String(
      InterviewStatus[this.interview?.status as keyof typeof InterviewStatus]
    );
  }
  public get resultDisplay(): string {
    const resultEnumValue = this.interview?.result;

    switch (resultEnumValue) {
      case 'Passed':
        return 'Passed';
      case 'Failed':
        return 'Failed';
      case 'NotApplicable':
        return 'N/A';
      default:
        return 'Unknown';
    }
  }

  public currentAdmin: { id: string; fullName: string } | null = null;
  public clearAdminAssignment(): void {
    this.interviewForm.patchValue({
      recruiterOwnerId: '',
    });
    this.isAdmin = false;

    this.currentAdmin = null;
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
    this.router.navigate(['/interview/interview-detail', this.interviewId]);
  }
  public onSubmit(): void {
    this.submitted = true;

    if (this.interviewForm.invalid) {
      if (this.interviewForm.hasError('invalidTimeRange')) {
        this.notificationService.showMessage(
          'The end time must be after the start time',
          'error'
        );
      } else {
        this.notificationService.showMessage(
          'Please fill in all required fields correctly.',
          'error'
        );
      }

      return;
    }

    const formData = {
      ...this.interviewForm.value,
      interviewerIds: this.selectedInterviewers.map((i) => i.id),
    };

    this.interviewService.update(this.interviewId!, formData).subscribe(
      (response) => {
        this.notificationService.showMessage(
          'Interview updated successfully',
          'success'
        );
        this.router.navigate(['/interview']);
      },
      (error) => {
        console.error('Error updating interview:', error);
        this.notificationService.showMessage('Error! Try again.', 'error');
      }
    );
  }

  public cancelSchedule(): void {
    if (this.interviewId) {
      const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        data: {
          title: 'Cancel Interview',
          message: 'Are you sure you want to cancel this interview?',
          confirmText: 'Yes',
          cancelText: 'No',
          confirmColor: 'bg-red-600 hover:bg-red-700',
          cancelColor: 'bg-gray-100 hover:bg-gray-200 text-gray-700',
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.interviewService.cancelInterview(this.interviewId!).subscribe({
            next: (response) => {
              if (response === true) {
                this.notificationService.showMessage(
                  'Interview cancelled successfully',
                  'success'
                );
                this.router.navigate([
                  '/interview/interview-detail',
                  this.interviewId,
                ]);
              } else {
                this.notificationService.showMessage(
                  'Interview already cancelled or failed to cancel',
                  'warning'
                );
              }
            },
            error: (error) => {
              this.notificationService.showMessage('Error', 'error');
            },
          });
        }
      });
    }
  }

  getFirstError(controlName: string): string | null {
    const control = this.interviewForm.get(controlName);
    if (
      !control ||
      !control.errors ||
      (!control.touched && !this.formSubmitted)
    ) {
      return null;
    }

    if (control.errors['required']) {
      return 'This field is required';
    }

    if (control.errors['maxlength']) {
      return `Max ${control.errors['maxlength'].requiredLength} character`;
    }

    if (control.errors['futureDate']) {
      return 'The date must be in the future.';
    }

    return null;
  }
  showInterviewerError(): boolean {
    return this.interviewerTouched && this.selectedInterviewers.length === 0;
  }
}
