import { Component, Inject, OnInit } from '@angular/core';
import { faHome, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { Status } from '../../../../core/enums/candidate-status';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HighestLevel } from '../../../../core/enums/highestLevel';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { SkillService } from '../../../../services/skill/skill.service';
import { AUTH_SERVICE, CANDIDATE_SERVICE, LOADING_SERVICE, NOTIFICATION_SERVICE, POSITION_SERVICE, SKILL_SERVICE, USER_SERVICE } from '../../../../constants/injection.constant';
import { IPositionService } from '../../../../services/position/position-service.interface';
import { PositionService } from '../../../../services/position/position.service';
import { UserInformation } from '../../../../models/auth/user-information.model';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { CandidateService } from '../../../../services/candidate/candidate.service';
import { IUserService } from '../../../../services/user/user-service.interface';
import { UserService } from '../../../../services/user/user.service';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../../core/components/notification/notification.component';
import { Observable, of, debounceTime, switchMap, map, catchError, first } from 'rxjs';
import { NgSelectModule } from '@ng-select/ng-select';
@Component({
  selector: 'app-create-candidate',
  imports: [
    ReactiveFormsModule,
    FontAwesomeModule,
    FormsModule,
    CommonModule,
    NotificationComponent,
    LoadingComponent,
    NgSelectModule
  ],
  templateUrl: './candidate-create.component.html',
  styleUrls: ['./candidate-create.component.css'],
  providers: [
    { provide: CANDIDATE_SERVICE, useClass: CandidateService },
    { provide: SKILL_SERVICE, useClass: SkillService },
    { provide: POSITION_SERVICE, useClass: PositionService },
    { provide: USER_SERVICE, useClass: UserService },
  ]
})
export class CandidateCreateComponent implements OnInit {

  public faTimes: IconDefinition = faTimes;

  public currentUserInfo: UserInformation | null = null;
  public selectedFile: File | null = null;
  //#region Font Awesome icons
  public faHome: IconDefinition = faHome;

  candidateForm: FormGroup;
  public submitted = false;
  public isExistEmail = false;

  public positions: { value: string; label: string }[] = [];
  public skills: { value: string; label: string }[] = [];
  public selectedSkills: { value: string; label: string }[] = [];
  public levels: { value: number; label: string }[] = [];
  public statuses: { value: number; label: string }[] = [];
  public recruiterOwners: { value: string; label: string }[] = [];

  constructor(
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private readonly loadingService: ILoadingService,
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(USER_SERVICE) private userService: IUserService,
    @Inject(SKILL_SERVICE) private skillService: ISkillService,
    @Inject(POSITION_SERVICE) private positionService: IPositionService,
    private fb: FormBuilder, private router: Router) {
    this.candidateForm = this.fb.group({
      fullname: ['', [
        Validators.required,
        Validators.maxLength(255)
      ]],
      DOB: ['', [
        Validators.required,
        dobValidator
      ]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      gender: ['', Validators.required],
      cV_Attachment: [''],  
      position: ['', Validators.required],
      skills: [[], Validators.required],
      recruiterOwner: ['', Validators.required],
      note: [''],
      status: ['', Validators.required],
      yearOfExperience: ['', [Validators.required, Validators.min(0)]],
      highestLevel: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.genEnum();
    this.loadSkills();
    this.loadPositions();
    this.loadRecruiters();
  }

  private loadRecruiters(): void {
    this.userService.getAll().subscribe({
      next: (users) => {
        this.recruiterOwners = users.map(user => ({
          value: user.id,
          label: user.fullName
        }));
      },
      error: (err) => {
        console.error('Error loading skills:', err);
      }
    });
  }

  private loadSkills(): void {
    this.skillService.getAll().subscribe({
      next: (skills) => {
        this.skills = skills.map(skill => ({
          value: skill.id,
          label: skill.name
        }));
      },
      error: (err) => {
        console.error('Error loading skills:', err);
      }
    });
  }

  private loadPositions(): void {
    this.positionService.getAll().subscribe({
      next: (positions) => {
        this.positions = positions.map(position => ({
          value: position.id,
          label: position.name
        }));
      },
      error: (err) => {
        console.error('Error loading skills:', err);
      }
    });
  }

  public genEnum(): void {
    this.statuses = Object.keys(Status)
      .filter(key => !isNaN(Number(Status[key as any])))
      .map(key => ({
        value: Status[key as keyof typeof Status],
        label: key.replace(/([A-Z])/g, ' $1').trim()
      }));

    this.levels = Object.keys(HighestLevel)
      .filter(key => !isNaN(Number(HighestLevel[key as any])))
      .map(key => ({
        value: HighestLevel[key as keyof typeof HighestLevel],
        label: key.replace(/([A-Z])/g, ' $1').trim()
      }));
  }

  onSubmit() {
    this.submitted = true;

    // Check xem email đã tồn tại hay chưa
    this.candidateService.getAll().subscribe((candidates) => {
      const emailExists = candidates.some(candidate => candidate.email === this.candidateForm.value.email);
      if (emailExists) {
        this.isExistEmail = true;
        this.notificationService.showMessage(
          'Email already exists',
          'error'
        );
        return;
      } else {
        if (this.candidateForm.valid) {
          this.loadingService.show();
          const formData = new FormData();
          formData.append('CvFile', this.selectedFile!); 
          formData.append('CV_Attachment', this.selectedFile!.name); 
          formData.append('Name', this.candidateForm.value.fullname);
          formData.append('Note', this.candidateForm.value.note);
          formData.append('Status', this.candidateForm.value.status);
          formData.append('YearOfExperience', this.candidateForm.value.yearOfExperience);
          formData.append('Email', this.candidateForm.value.email);
          formData.append('DateOfBirth', this.candidateForm.value.DOB);
          formData.append('Address', this.candidateForm.value.address);
          formData.append('Phone', this.candidateForm.value.phoneNumber);
          formData.append('Gender', this.candidateForm.value.gender);
          
          this.candidateForm.value.skills.forEach((skillId: string) => {
            formData.append('CandidateSkillIds', skillId);
          });

          formData.append('HighestLevel', this.candidateForm.value.highestLevel);
          formData.append('PositionId', this.candidateForm.value.position);
          formData.append('RecruiterOwnerId', this.candidateForm.value.recruiterOwner);
          formData.forEach((value, key) => {
            console.log(`${key}:`, value);
          });

          
          this.candidateService.createCandidate(formData).subscribe((res) => {
            if (res) {
              this.notificationService.showMessage(
                'Candidate created successfully',
                'success'
              );
              this.loadingService.hide();
              this.router.navigate(['/Candidate']);
            } else {
              console.log('Create failed');
            }
          });

        } else {
          console.log('Form is invalid');
          console.log(this.candidateForm.value);
        }
      }
    });


  }

  assignToMe(event: Event) {
    event.preventDefault();
    this.authService.getUserInformation().subscribe({
      next: (user) => {
        this.currentUserInfo = user;
        this.candidateForm.patchValue({
          recruiterOwner: user?.id,
        });
      },
      error: (err) => {
        console.error('Error fetching user info:', err);
      }
    });
    const currentUserRecruiterId = this.currentUserInfo!.id;
    if (currentUserRecruiterId) {
      this.candidateForm.get('recruiterOwner')?.setValue(currentUserRecruiterId);
    }
  }

  public clearAdminAssignment(): void {
    this.currentUserInfo = null;
    this.candidateForm.patchValue({
      recruiterOwner: '',
    });
  }

  onFileChange(event: any) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.submitted = false; 
      const file = input.files[0]; 
      this.selectedFile = input.files[0];
    }
  }

  public returnList(): void {
    this.router.navigate(['candidate/']);
  }
}

export function dobValidator(control: AbstractControl): ValidationErrors | null {
  const dob = new Date(control.value);
  const today = new Date();

  if (!control.value || isNaN(dob.getTime())) {
    return { invalidDOB: true };
  }

  if (dob >= today) {
    return { invalidDOB: true };
  }

  return null;
}