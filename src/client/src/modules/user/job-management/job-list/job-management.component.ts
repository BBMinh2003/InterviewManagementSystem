import { Component, Inject, OnInit } from '@angular/core';
import {
  AUTH_SERVICE,
  JOB_SERVICE,
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
} from '../../../../constants/injection.constant';
import { IJobService } from '../../../../services/job/job-service.interface';
import { Router } from '@angular/router';
import { MasterDataListComponent } from '../../master-data/master-data/master-data.component';
import { JobModel } from '../../../../models/job/job.model';
import { JobStatus } from '../../../../core/enums/jobstatus';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { TableComponent } from '../../../../core/components/table/table.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../../core/components/notification/notification.component';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { OrderDirection } from '../../../../models/search.model';
import { faFileImport, IconDefinition } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-job-management',
  templateUrl: './job-management.component.html',
  styleUrls: ['./job-management.component.css'],
  imports: [
    TableComponent,
    FontAwesomeModule,
    ReactiveFormsModule,
    CommonModule,
    NotificationComponent,
    LoadingComponent,
  ],
})
export class JobManagementComponent
  extends MasterDataListComponent<JobModel>
  implements OnInit {

  public faImport: IconDefinition = faFileImport;

  public override columns: TableColumn[] = [
    { name: 'Title', value: 'title', sortable: true },
    { name: 'Skills', value: 'skills', sortable: false },
    { name: 'Start Date', value: 'startDate', sortable: true },
    { name: 'End Date', value: 'endDate', sortable: true },
    { name: 'Level', value: 'level', sortable: false },
    { name: 'Status', value: 'status', sortable: true },
  ];
  jobs: JobModel[] = [];
  statuses: { value: number; label: string }[] = [];
  role: any;
  isShowDeleteAction: boolean = true;
  isShowEditAction: boolean = true;

  selectedFile: File | null = null;

  constructor(
    @Inject(JOB_SERVICE) private jobService: IJobService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private loadingService: ILoadingService,
    private router: Router
  ) {
    super();
  }

  override ngOnInit(): void {
    super.ngOnInit();

    this.authService.getUserInformation().subscribe((res) => {
      this.role = res?.role;
      if (this.role == 'Interviewer') {
        this.isShowDeleteAction = false;
        this.isShowEditAction = false;
      }
    });

    this.loadJobs();
    this.statuses = Object.keys(JobStatus)
      .filter((key) => !isNaN(Number(JobStatus[key as any])))
      .map((key) => ({
        value: JobStatus[key as keyof typeof JobStatus],
        label: key,
      }));
  }
  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      status: new FormControl<string | null>(null),
    });
  }
  private loadJobs(): void {
    this.loadingService.show();

    this.jobService.getAll().subscribe({
      next: (jobs) => {
        if (jobs && Array.isArray(jobs)) {
          this.jobs = jobs;
        } else {
          this.jobs = [];
        }
      },
      error: (err) => {
        console.error('Error loading jobs:', err);
        this.loadingService.hide();
      },
    });
  }

  public createJob(): void {
    this.router.navigate(['job/create']);
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      const file = fileInput.files[0];

      const validTypes = [
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 
        'application/vnd.ms-excel'
      ];

      const isExcelFile = validTypes.includes(file.type) || file.name.endsWith('.xlsx') || file.name.endsWith('.xls');

      if (!isExcelFile) {
        this.notificationService.showMessage(
          'Invalid file format. Please select an Excel file (.xlsx or .xls)',
          'error'
        );
        return;
      }

      this.selectedFile = file;
      this.importExcel();
    }
  }

  public importExcel(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('excelFile', this.selectedFile);

    this.jobService.import(formData).subscribe((res) => {
      if (res) {
        this.notificationService.showMessage(
          "Import successfully",
          "success"
        );
        this.loadingService.hide();
        this.ngOnInit();
      } else {
        this.notificationService.showMessage(
          "Import error",
          "error"
        );
      }
    });
  }

  public viewJobDetail(id: string): void {
    this.router.navigate(['/job', id]);
  }

  protected override searchData(): void {
    const filterRequest: any = { ...this.filter };

    if (!filterRequest.status || filterRequest.status === 'All') {
      delete filterRequest.status;
    }

    if (filterRequest.keyword) {
      filterRequest.keyword = filterRequest.keyword.trim().toLowerCase();
    }
    this.loadingService.show();
    this.jobService.search(filterRequest).subscribe((res) => {
      this.data = {
        ...res,
        items: res.items.map((item) => ({
          ...item,
          title: item.title || 'N/A',
          skills:
            item.jobSkills
              .map((skill: { id: string; name: string }) => skill.name)
              .join(', ') || 'N/A',
          startDate: item.startDate
            ? new Date(item.startDate).toLocaleDateString()
            : 'N/A',
          endDate: item.endDate
            ? new Date(item.endDate).toLocaleDateString()
            : 'N/A',
          level:
            item.jobLevels
              .map((level: { id: string; name: string }) => level.name)
              .join(', ') || 'N/A',
          status: JobStatus[item.status as keyof typeof JobStatus].toString(),
          minSalary: item.minSalary || 0,
          maxSalary: item.maxSalary || 0,
          workingAddress: item.workingAddress || 'N/A',
          description: item.description || 'N/A',
          isDelete: item.isDelete || false,
        })),
      };
      this.loadingService.hide();
    });
  }
  handleDelete(id: string): void {
    this.jobService.delete(id).subscribe({
      next: (response) => {
        if (response === true) {
          this.notificationService.showMessage(
            'Delete successfully',
            'success'
          );
        } else {
          this.notificationService.showMessage('Delete failed', 'error');
        }
      },
      error: (err) => {
        console.error('Error deleting job:', err);
      },
    });
  }

  public update(id: string): void {
    this.router.navigate(['job/update', id]);
  }

  onSort(event: { field: string; direction: OrderDirection }): void {
    this.filter.orderBy = event.field;
    this.filter.orderDirection = event.direction;
    console.log(this.filter);

    this.searchData(); // Gọi lại API với thông tin sắp xếp
  }

  public returnList(): void {
    this.router.navigate(['candidate/']);
  }
}
