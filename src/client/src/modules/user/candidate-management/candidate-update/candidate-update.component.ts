import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { AUTH_SERVICE, CANDIDATE_SERVICE, NOTIFICATION_SERVICE, POSITION_SERVICE, SKILL_SERVICE, USER_SERVICE } from '../../../../constants/injection.constant';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { FontAwesomeModule, IconDefinition } from '@fortawesome/angular-fontawesome';
import { CandidateService } from '../../../../services/candidate/candidate.service';
import { PositionService } from '../../../../services/position/position.service';
import { SkillService } from '../../../../services/skill/skill.service';
import { UserService } from '../../../../services/user/user.service';
import { faHome, faTimes } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute, Router } from '@angular/router';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { IPositionService } from '../../../../services/position/position-service.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { IUserService } from '../../../../services/user/user-service.interface';
import { Status } from '../../../../core/enums/candidate-status';
import { HighestLevel } from '../../../../core/enums/highestLevel';
import { UserInformation } from '../../../../models/auth/user-information.model';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-candidate-update',
  imports: [CommonModule, ReactiveFormsModule, FontAwesomeModule, FormsModule, CommonModule, NgSelectModule],
  templateUrl: './candidate-update.component.html',
  styleUrl: './candidate-update.component.css',
  providers: [
    { provide: CANDIDATE_SERVICE, useClass: CandidateService },
    { provide: SKILL_SERVICE, useClass: SkillService },
    { provide: POSITION_SERVICE, useClass: PositionService },
    { provide: USER_SERVICE, useClass: UserService },
  ]
})
export class CandidateUpdateComponent implements AfterViewInit{
  public faHome: IconDefinition = faHome;
  public faTimes: IconDefinition = faTimes;

  candidateForm: FormGroup;
  submitted = false;
  isFileInputVisible: boolean = false;
  currentCV_path!: string;

  @ViewChild('fileInput') fileInput!: ElementRef;
  ngAfterViewInit() {
    if (this.fileInput) {
      console.log('File input đã sẵn sàng:', this.fileInput);
    }
    else {
      console.log('Error');

    }
  }

  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }

  public currentUserInfo: UserInformation | null = null;
  public currentCandidateInfo: CandidateModel | null = null;
  public currentCandidateId!: string;
  public selectedFile!: File;

  public positions: { value: string; label: string }[] = [];
  public skills: { value: string; label: string }[] = [];
  public selectedSkills: { value: string; label: string }[] = [];
  public levels: { value: number; label: string }[] = [];
  public statuses: { value: number; label: string }[] = [];
  public recruiterOwners: { value: string; label: string }[] = [];

  constructor(
    @Inject(NOTIFICATION_SERVICE)
        private notificationService: INotificationService,
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(USER_SERVICE) private userService: IUserService,
    @Inject(SKILL_SERVICE) private skillService: ISkillService,
    @Inject(POSITION_SERVICE) private positionService: IPositionService,
    private fb: FormBuilder, private router: Router, private route: ActivatedRoute,) {
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
    this.loadCandidateInformation();
    this.isFileInputVisible = false;
  }

  public loadCandidateInformation(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.currentCandidateId = id;
    }
    this.candidateService.getById(this.currentCandidateId).subscribe({
      next: (candidate) => {
        this.currentCandidateInfo = candidate;
        console.log(this.currentCandidateInfo);
        this.currentCV_path = this.currentCandidateInfo.cV_Attachment.slice(37);
        this.selectedSkills = this.currentCandidateInfo.candidateSkills.map(skill => ({
          value: skill.id,
          label: skill.name
        }));

        const dobDate = new Date(this.currentCandidateInfo.dateOfBirth);
        const dob = `${dobDate.getFullYear()}-${(dobDate.getMonth() + 1).toString().padStart(2, '0')}-${dobDate.getDate().toString().padStart(2, '0')}`;
        this.candidateForm.patchValue({
          fullname: this.currentCandidateInfo.name,
          DOB: dob,  
          phoneNumber: this.currentCandidateInfo.phone,
          email: this.currentCandidateInfo.email,
          address: this.currentCandidateInfo.address,
          gender: this.currentCandidateInfo.gender,
          cV_Attachment: this.currentCandidateInfo.cV_Attachment,
          position: this.currentCandidateInfo.positionId,
          skills: this.currentCandidateInfo.candidateSkills.map((i) => i.id),
          recruiterOwner: this.currentCandidateInfo.recruiterOwnerId,
          note: this.currentCandidateInfo.note,
          status: this.currentCandidateInfo.status,
          yearOfExperience: this.currentCandidateInfo.yearOfExperience,
          highestLevel: this.currentCandidateInfo.highestLevel
        });
      },
      error: (err) => {
        console.error('Error fetching user info:', err);
      }
    });
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

    if (this.candidateForm.valid) {
      console.log('Form Data:', this.candidateForm.value);
      const formData = new FormData();
      formData.append('CvFile', this.selectedFile!); 
      formData.append('CV_Attachment', this.selectedFile?.name ?? this.currentCandidateInfo!.cV_Attachment); 
      formData.append('Name', this.candidateForm.value.fullname);
      formData.append('Note', this.candidateForm.value.note ?? "");
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

      console.log('Candidate Skills Ids:', this.candidateForm.value.skills);
      
      this.candidateService.updateCandidate(this.currentCandidateInfo!.id, formData).subscribe((res) => {
        if (res) {
          this.notificationService.showMessage(
            'Candidate updated successfully',
            'success'
          );
          this.router.navigate(['/candidate']);
        } else {
          console.log('update failed');
        }
      });

    } else {
      console.log('Form is invalid');
    }
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
      const file = input.files[0]; // Lấy file đầu tiên
      this.selectedFile = input.files[0];
      this.isFileInputVisible = true;
    }
  }

  public returnList(): void {
    this.router.navigate(['/candidate']);
  }
}

export function dobValidator(control: FormControl) {
  const selectedDate = new Date(control.value);
  const today = new Date();

  return selectedDate >= today ? { invalidDOB: true } : null;
}

