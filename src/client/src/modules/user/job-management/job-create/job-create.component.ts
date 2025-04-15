import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, Validators, FormBuilder, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule, IconDefinition } from '@fortawesome/angular-fontawesome';
import { faHome, faTimes } from '@fortawesome/free-solid-svg-icons';
import { AUTH_SERVICE, BENEFIT_SERVICE, JOB_SERVICE, LEVEL_SERVICE, NOTIFICATION_SERVICE, SKILL_SERVICE } from '../../../../constants/injection.constant';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { IBenefitService } from '../../../../services/benefit/benefit-service.interface';
import { BenefitService } from '../../../../services/benefit/benefit.service';
import { SkillService } from '../../../../services/skill/skill.service';
import { ILevelService } from '../../../../services/level/level-service.interface';
import { LevelService } from '../../../../services/level/level.service';
import { IJobService } from '../../../../services/job/job-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-job-create',
  imports: [CommonModule, ReactiveFormsModule, FormsModule, FontAwesomeModule, NgSelectModule],
  templateUrl: './job-create.component.html',
  styleUrl: './job-create.component.css',
  providers: [
    { provide: BENEFIT_SERVICE, useClass: BenefitService },
    { provide: SKILL_SERVICE, useClass: SkillService },
    { provide: LEVEL_SERVICE, useClass: LevelService },
  ]
})
export class JobCreateComponent implements OnInit {
  public faHome: IconDefinition = faHome;
  public faTimes: IconDefinition = faTimes;

  jobForm !: FormGroup;
  public submitted = false;

  public skills: { value: string; label: string }[] = [];
  public benefits: { value: string; label: string }[] = [];
  public levels: { value: string; label: string }[] = [];

  constructor(
    @Inject(NOTIFICATION_SERVICE)
        private notificationService: INotificationService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(SKILL_SERVICE) private skillService: ISkillService,
    @Inject(LEVEL_SERVICE) private levelService: ILevelService,
    @Inject(BENEFIT_SERVICE) private benefitService: IBenefitService,
    @Inject(JOB_SERVICE) private jobService: IJobService,
    private fb: FormBuilder, private router: Router) {
    this.jobForm = this.fb.group({
      title: ['', [
        Validators.required,
        Validators.maxLength(255)
      ]],
      startDate: ['', [
        Validators.required,
      ]],
      endDate: ['', [Validators.required]],
      workingAddress: ['', Validators.required],
      skills: [[], Validators.required],
      benefits: [[], Validators.required],
      levels: [[], Validators.required],
      minSalary: ['', Validators.required],
      maxSalary: ['', Validators.required],
      description: [''],
    },{
      validators: [dateRangeValidator]
    });
  }

  ngOnInit(): void {
    this.loadBenefits();
    this.loadSkills();
    this.loadLevels();
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

  private loadBenefits(): void {
    this.benefitService.getAll().subscribe({
      next: (benefit) => {
        this.benefits = benefit.map(benefit => ({
          value: benefit.id,
          label: benefit.name
        }));
      },
      error: (err) => {
        console.error('Error loading skills:', err);
      }
    });
  }

  private loadLevels(): void {
    this.levelService.getAll().subscribe({
      next: (levels) => {
        this.levels = levels.map(level => ({
          value: level.id,
          label: level.name
        }));
      },
      error: (err) => {
        console.error('Error loading skills:', err);
      }
    });
  }

  goBack() {
    this.router.navigate(['job']);
  }

  onSubmit() {
    this.submitted = true;
    if (this.jobForm.invalid) {
      if (this.jobForm.hasError('dateRangeInvalid')) {
        this.notificationService.showMessage(
          'Start date must be before end date',
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

    const jobData = {
      ...this.jobForm.value,
      jobSkills: this.jobForm.value.skills,
      jobBenefits: this.jobForm.value.benefits,
      jobLevels: this.jobForm.value.levels,
    };

    console.log(jobData);

    this.jobService.create(jobData).subscribe(
      (response) => {
        console.log(response);

        this.notificationService.showMessage(
          'Interview created successfully',
          'success'
        );
        this.router.navigate(['job']);
      },
      (err) => {
        console.error('Error creating job:', err);
        this.notificationService.showMessage('Error! Try again.');
      },
    );
  }

}

export function dateRangeValidator(group: AbstractControl): ValidationErrors | null {
  const startDate = group.get('startDate')?.value;
  const endDate = group.get('endDate')?.value;

  if (!startDate || !endDate) return null;

  const start = new Date(startDate);
  const end = new Date(endDate);

  if (end <= start) {
    return { dateRangeInvalid: true };
  }

  return null;
}

