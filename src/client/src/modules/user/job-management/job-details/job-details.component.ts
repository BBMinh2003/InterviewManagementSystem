import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AUTH_SERVICE, JOB_SERVICE } from '../../../../constants/injection.constant';
import { IJobService } from '../../../../services/job/job-service.interface';
import { JobModel } from '../../../../models/job/job.model';
import { JobStatus } from '../../../../core/enums/jobstatus';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { FontAwesomeModule, IconDefinition } from '@fortawesome/angular-fontawesome';
import { faHome } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-job-details',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './job-details.component.html',
  styleUrl: './job-details.component.css',
})
export class JobDetailsComponent implements OnInit {

  public faHome: IconDefinition = faHome;

  jobId!: string;
  jobDetails!: JobModel | null;
  public JobStatus!: JobStatus;
  jobBenefitsText: string = '';
  jobSkillsText: string = '';
  jobLevelsText: string = '';
  role: any;

  constructor(
    private route: ActivatedRoute, private router: Router,
    @Inject(JOB_SERVICE) private jobService: IJobService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,

  ) { }

  ngOnInit(): void {
    this.authService.getUserInformation().subscribe((res) => {
      this.role = res?.role;
    });
    this.jobId = this.route.snapshot.paramMap.get('id')!;
    this.loadJobDetails();
  }

  private loadJobDetails(): void {
    if (!this.jobId) return;

    this.jobService.getById(this.jobId).subscribe(
      (job) => {
        this.jobDetails = job;
        console.log(this.jobDetails);
        this.jobBenefitsText = this.formatList(job?.jobBenefits);
        this.jobSkillsText = this.formatList(job?.jobSkills);
        this.jobLevelsText = this.formatList(job?.jobLevels);
      },
      (error) => {
        console.error('Failed to load job details:', error);
        this.router.navigate(['/job']);
      }
    );
  }

  getJobStatusLabel(status: number): string {
    return JobStatus[status] ?? 'Unknown';
  }

  private formatList(items?: { name: string }[]): string {
    return items && items.length ? items.map((i) => i.name).join(', ') : 'N/A';
  }

  public editJob(): void {
    this.router.navigate(['job/update', this.jobId!]);
  }

  public returnList(): void {
    this.router.navigate(['job/']);
  }
}
