import { Component, Inject } from '@angular/core';
import { AUTH_SERVICE, BENEFIT_SERVICE, JOB_SERVICE, LEVEL_SERVICE, NOTIFICATION_SERVICE, SKILL_SERVICE } from '../../../../constants/injection.constant';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FontAwesomeModule, IconDefinition } from '@fortawesome/angular-fontawesome';
import { faHome, faTimes } from '@fortawesome/free-solid-svg-icons';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { IBenefitService } from '../../../../services/benefit/benefit-service.interface';
import { BenefitService } from '../../../../services/benefit/benefit.service';
import { IJobService } from '../../../../services/job/job-service.interface';
import { ILevelService } from '../../../../services/level/level-service.interface';
import { LevelService } from '../../../../services/level/level.service';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { ISkillService } from '../../../../services/skill/skill-service.interface';
import { SkillService } from '../../../../services/skill/skill.service';
import { CommonModule, formatDate } from '@angular/common';
import { JobModel } from '../../../../models/job/job.model';
import { dateRangeValidator } from '../job-create/job-create.component';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-job-update',
  imports: [CommonModule, ReactiveFormsModule, FormsModule, FontAwesomeModule, NgSelectModule],
  templateUrl: './job-update.component.html',
  styleUrl: './job-update.component.css',
  providers: [
    { provide: BENEFIT_SERVICE, useClass: BenefitService },
    { provide: SKILL_SERVICE, useClass: SkillService },
    { provide: LEVEL_SERVICE, useClass: LevelService },
  ]
})
export class JobUpdateComponent {
  public faHome: IconDefinition = faHome;
  public faTimes: IconDefinition = faTimes;

  jobForm !: FormGroup;
  public submitted = false;

  public currentJobInfo!: JobModel;
  public currentJobId!: string;

  public skills: { value: string; label: string }[] = [];
  public selectedSkills: { value: string; label: string }[] = [];
  public benefits: { value: string; label: string }[] = [];
  public selectedBenefits: { value: string; label: string }[] = [];
  public levels: { value: string; label: string }[] = [];
  public selectedLevels: { value: string; label: string }[] = [];

  constructor(
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(SKILL_SERVICE) private skillService: ISkillService,
    @Inject(LEVEL_SERVICE) private levelService: ILevelService,
    @Inject(BENEFIT_SERVICE) private benefitService: IBenefitService,
    @Inject(JOB_SERVICE) private jobService: IJobService,
    private fb: FormBuilder, private router: Router, private route: ActivatedRoute) {
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
    this.loadJobInfor();
    console.log(this.selectedSkills);

  }

  public loadJobInfor(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.currentJobId = id;
      console.log(this.currentJobId);

    }
    this.jobService.getById(this.currentJobId).subscribe({
      next: (job) => {
        this.currentJobInfo = job;

        this.selectedSkills = job.jobSkills.map(skill => ({
          value: skill.id,
          label: skill.name
        }));
        this.selectedLevels = job.jobLevels.map(level => ({
          value: level.id,
          label: level.name
        }));
        this.selectedBenefits = job.jobBenefits.map(benefit => ({
          value: benefit.id,
          label: benefit.name
        }));

        console.log(this.currentJobInfo);

        this.jobForm.patchValue({
          title: this.currentJobInfo.title,
          startDate: formatDate(this.currentJobInfo.startDate!, 'yyyy-MM-dd', 'en'),
          endDate: formatDate(this.currentJobInfo.endDate!, 'yyyy-MM-dd', 'en'),
          minSalary: this.currentJobInfo.minSalary,
          maxSalary: this.currentJobInfo.maxSalary,
          workingAddress: this.currentJobInfo.workingAddress,
          description: this.currentJobInfo.description,
          skills: this.currentJobInfo.jobSkills.map((i) => i.id),
          benefits: this.currentJobInfo.jobBenefits.map((i) => i.id),
          levels: this.currentJobInfo.jobLevels.map((i) => i.id),
        });
      },
      error: (err) => {
        console.error('Error fetching user info:', err);
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

  isSelected(skillId: string) {
    return this.selectedSkills.some((i) => i.value === skillId);
  }

  handleSelection(event: Event, list: { value: string; label: string }[], selectedList: { value: string; label: string }[], formControlName: string) {
    const target = event.target as HTMLSelectElement;
    if (!target || !target.value) return;

    const selectedItem = list.find(item => item.value === target.value);
    if (selectedItem && !selectedList.some(i => i.value === selectedItem.value)) {
      selectedList.push(selectedItem);
      const currentValues = this.jobForm.get(formControlName)?.value || [];
      this.jobForm.patchValue({
        [formControlName]: [...currentValues, selectedItem.value],
      });
    }

    target.value = '';
  }

  removeSelection(item: { value: string; label: string }, selectedList: { value: string; label: string }[], formControlName: string) {
    this.jobForm.patchValue({
      [formControlName]: selectedList.filter(i => i.value !== item.value).map(i => i.value),
    });

    const index = selectedList.findIndex(i => i.value === item.value);
    if (index !== -1) {
      selectedList.splice(index, 1);
    }
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

    this.jobService.update(this.currentJobId, jobData).subscribe(
      (response) => {

        this.notificationService.showMessage(
          'Interview updated successfully',
          'success'
        );
        this.router.navigate(['job']);
      },
      (err) => {
        console.error('Error updating job:', err);
        this.notificationService.showMessage('Error! Try again.');
      },
    );
  }
}
