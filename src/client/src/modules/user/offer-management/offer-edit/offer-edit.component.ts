import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  FormGroup,
  FormBuilder,
  Validators,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import { faHome } from '@fortawesome/free-solid-svg-icons';
import {
  NOTIFICATION_SERVICE,
  AUTH_SERVICE,
  LOADING_SERVICE,
} from '../../../../constants/injection.constant';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../../core/components/notification/notification.component';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { CandidateService } from '../../../../services/candidate/candidate.service';
import { ContactTypeService } from '../../../../services/contact/contactType.service';
import { DepartmentService } from '../../../../services/department/department.service';
import { InterviewService } from '../../../../services/interview/interview.service';
import { LevelService } from '../../../../services/level/level.service';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { OfferService } from '../../../../services/offer/offer.service';
import { PositionService } from '../../../../services/position/position.service';
import { UserService } from '../../../../services/user/user.service';
import { OfferModel } from '../../../../models/offer/offer.model';
import { forkJoin, tap, finalize } from 'rxjs';

@Component({
  selector: 'app-offer-edit',
  standalone: true,
  imports: [
    CommonModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    RouterModule,
    NotificationComponent,
    LoadingComponent,
  ],
  templateUrl: './offer-edit.component.html',
  styleUrl: './offer-edit.component.css',
})
export class OfferEditComponent implements OnInit {
  public faHome: IconDefinition = faHome;

  public offer!: OfferModel;

  offerForm!: FormGroup;
  offerId!: string;
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
    private readonly route: ActivatedRoute,
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
        contactPeriodFrom: ['', [Validators.required]],
        contactPeriodTo: ['', [Validators.required]],
        dueDate: ['', [Validators.required, this.futureDateValidator]],
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
    this.offerId = this.route.snapshot.params['id'];
    this.createForm();
    this.loadDropdownData();
    this.loadOfferData();
  }

  private loadDropdownData(): void {
    this.loadingService.show();

    const requests = {
      candidates: this.candidateService.getAll(),
      contactTypes: this.contactTypeService.getAll(),
      positions: this.positionService.getAll(),
      levels: this.levelService.getAll(),
      departments: this.departmentService.getAll(),
      interviews: this.interviewService.getAll(),
      users: this.userService.getAll(),
    };

    forkJoin(requests)
      .pipe(
        tap(({ users }) => {
          this.approvers = users.filter((user) =>
            user.roles?.some((role) => role === 'Manager' || role === 'Admin')
          );
          this.recruiters = users.filter((user) =>
            user.roles?.some((role) => role === 'Recruiter' || role === 'Admin')
          );
        }),
        finalize(() => this.loadingService.hide())
      )
      .subscribe({
        next: (results) => {
          this.candidates = results.candidates;
          this.contactTypes = results.contactTypes;
          this.positions = results.positions;
          this.levels = results.levels;
          this.departments = results.departments;
          this.interviews = results.interviews;

          this.loadOfferData();
        },
        error: (error) => {
          console.error('Error loading dropdown data:', error);
          this.notificationService.showMessage(
            'Failed to load dropdown data',
            'error'
          );
        },
      });
  }

  private loadOfferData(): void {
    this.offerService.getById(this.offerId).subscribe({
      next: (offer) => {
        const recruiterMap = this.createRecruiterMap();
        const approverMap = this.createApproverMap();

        const idMaps = {
          candidate: this.createNameToIdMap(this.candidates),
          contactType: this.createNameToIdMap(this.contactTypes, 'name'),
          position: this.createNameToIdMap(this.positions),
          level: this.createNameToIdMap(this.levels),
          department: this.createNameToIdMap(this.departments),
          approver: this.createNameToIdMap(this.approvers, 'Name'),
          recruiterOwner: this.createNameToIdMap(this.recruiters, 'Name'),
          interview: this.createNameToIdMap(this.interviews, 'title'),
        };

        const formData = {
          candidateId: this.findIdByName(offer.candidateName, idMaps.candidate),
          contactTypeId: this.findIdByName(
            offer.contactType,
            idMaps.contactType
          ),
          positionId: this.findIdByName(offer.positionName, idMaps.position),
          levelId: this.findIdByName(offer.level, idMaps.level),
          departmentId: this.findIdByName(
            offer.departmentName,
            idMaps.department
          ),
          recruiterOwnerId: this.findRecruiterId(
            offer.recruiterOwnerName,
            recruiterMap
          ),
          approverId: this.handleApprover(offer.approverName, approverMap),
          interviewId: this.findIdByName(
            offer.interviewInfo!,
            idMaps.interview
          ),

          contactPeriodFrom: this.convertToDateInput(offer.contactPeriodFrom),
          contactPeriodTo: this.convertToDateInput(offer.contactPeriodTo),
          dueDate: this.convertToDateInput(offer.dueDate),
          basicSalary: offer.basicSalary,
          note: offer.note,
          interviewNotes: offer.interviewNote,
        };

        console.log('Mapped Form Data:', formData);
        this.offerForm.patchValue(formData);
      },
      error: (error) => {
        console.error('Error loading offer:', error);
      },
    });
  }
  private createRecruiterMap(): Map<string, string> {
    return this.recruiters.reduce((map, recruiter) => {
      const key = recruiter.fullName?.trim().toLowerCase() || '';
      return map.set(key, recruiter.id);
    }, new Map<string, string>());
  }

  private createApproverMap(): Map<string, string> {
    return this.approvers.reduce((map, approver) => {
      const key = approver.fullName?.trim().toLowerCase() || '';
      return map.set(key, approver.id);
    }, new Map<string, string>());
  }

  private findRecruiterId(
    name: string | null,
    map: Map<string, string>
  ): string | null {
    if (!name) {
      console.warn('Recruiter Owner Name is null');
      return null;
    }

    const normalized = name.trim().toLowerCase();
    const id = map.get(normalized);

    if (!id) {
      console.error(`Recruiter không tồn tại: ${name}`, {
        recruiters: this.recruiters,
        mapEntries: Array.from(map.entries()),
      });
    }

    return id ?? null;
  }

  private handleApprover(
    name: string | null,
    map: Map<string, string>
  ): string | null {
    if (!name) {
      console.warn('Approver là null, để trống hoặc gán giá trị mặc định');
      return this.offerForm.value.approverId || null; // Giữ nguyên giá trị nếu có
    }

    const normalized = name.trim().toLowerCase();
    return map.get(normalized) ?? null;
  }
  private createNameToIdMap(
    items: any[],
    nameField: string = 'name'
  ): Map<string, string> {
    const map = new Map<string, string>();
    items?.forEach((item) => {
      map.set(item[nameField]?.trim().toLowerCase(), item.id);
    });
    return map;
  }

  private findIdByName(
    name: string | null,
    map: Map<string, string>
  ): string | null {
    if (!name) return null;
    return map.get(name.trim().toLowerCase()) ?? null;
  }

  private convertToDateInput(date: string | Date): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0];
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
    console.log(this.offerId);

    this.offerService.update(this.offerId, formData).subscribe({
      next: (response) => {
        this.loadingService.hide();
        this.router.navigate(['/offer']);
        this.notificationService.showMessage(
          'Offer updated successfully',
          'success'
        );
        this.isSubmitting = false;
      },
      error: (error) => {
        this.loadingService.hide();
        console.error('Error update Offer:', error);
        this.notificationService.showMessage(
          error.error?.message || 'Failed to update offer',
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
