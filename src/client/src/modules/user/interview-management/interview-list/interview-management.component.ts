import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MasterDataListComponent } from '../../master-data/master-data/master-data.component';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import {
  AUTH_SERVICE,
  INTERVIEW_SERVICE,
  INTERVIEWER_SERVICE,
  LOADING_SERVICE,
  USER_SERVICE,
} from '../../../../constants/injection.constant';
import { ServicesModule } from '../../../../services/service.module';
import { InterviewStatus } from '../../../../core/enums/interview-status';
import { Router } from '@angular/router';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import { TableComponent } from '../../../../core/components/table/table.component';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { IUserService } from '../../../../services/user/user-service.interface';
import { UserModel } from '../../../../models/user/user.model';
import { IInterviewerService } from '../../../../services/interviewer/interviewer-service.interface';
import { InterviewerModel } from '../../../../models/user/interview.model';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { OrderDirection } from '../../../../models/search.model';
import { UserSearchModel } from '../../../../models/user/user-search.model';

@Component({
  selector: 'app-interview-management',
  imports: [
    CommonModule,
    ServicesModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    FormsModule,
    TableComponent,
    LoadingComponent,
  ],
  templateUrl: './interview-management.component.html',
  styleUrl: './interview-management.component.css',
})
export class InterviewManagementComponent
  extends MasterDataListComponent<InterviewModel>
  implements OnInit
{
  public override columns: TableColumn[] = [
    { name: 'Title', value: 'title', sortable: true },
    { name: 'Candidate', value: 'candidateName' },
    { name: 'Interviewer', value: 'interviewerName' },
    { name: 'Schedule', value: 'schedule' },
    { name: 'Result', value: 'result', sortable: true },
    { name: 'Status', value: 'status', sortable: true },
    { name: 'Job', value: 'jobName' },
  ];
  interviewerss!: InterviewerModel[];
  userInfo: any;
  isInterviewer: boolean = false;
  userRole: string = '';
  isEdit: boolean = false;
  public override filter: UserSearchModel = {
    keyword: '',
    pageNumber: 1,
    pageSize: 5,
    orderBy: 'createdAt',
    orderDirection: OrderDirection.ASC,
  };

  public statuses: { value: number; label: string }[] = [];
  constructor(
    @Inject(INTERVIEW_SERVICE) private interviewService: IInterviewService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(USER_SERVICE) private userService: IUserService,
    @Inject(INTERVIEWER_SERVICE)
    private interviewerService: IInterviewerService,
    @Inject(LOADING_SERVICE) private loadingService: ILoadingService,

    private router: Router
  ) {
    super();
  }

  override ngOnInit(): void {
    super.ngOnInit();

    this.statuses = Object.keys(InterviewStatus)
      .filter((key) => !isNaN(Number(InterviewStatus[key as any])))
      .map((key) => ({
        value: InterviewStatus[key as keyof typeof InterviewStatus],
        label: key,
      }));

    this.authService.getUserInformation().subscribe((user) => {
      if (user && user.role) {
        this.userRole = user.role;
        this.isEdit = this.userRole !== 'Interviewer';
      }
    });
    this.loadInterviewer();
  }
  private loadInterviewer(): void {
    this.interviewerService.getInterviewers().subscribe((users) => {
      if (users && Array.isArray(users)) {
        this.interviewerss = users;
      } else {
        console.error('User role is undefined or invalid data structure.');
      }
    });
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      interviewerId: new FormControl<string | null>(null),
      status: new FormControl<string | null>(null),
    });
  }
  protected override searchData(): void {
    this.loadingService.show();

    const filterRequest: any = { ...this.filter };

    if (!filterRequest.status || filterRequest.status === 'All')
      delete filterRequest.status;
    if (!filterRequest.interviewerId || filterRequest.interviewerId === 'All')
      delete filterRequest.interviewerId;

    const loadingStartTime = Date.now();
    const MIN_LOADING_TIME = 1000;

    this.interviewService.search(filterRequest).subscribe({
      next: (res) => {
        const elapsed = Date.now() - loadingStartTime;
        const remainingTime = Math.max(0, MIN_LOADING_TIME - elapsed);

        setTimeout(() => {
          this.loadingService.hide();
          this.processData(res);
        }, remainingTime);
      },
      error: (err) => {
        const elapsed = Date.now() - loadingStartTime;
        const remainingTime = Math.max(0, MIN_LOADING_TIME - elapsed);

        setTimeout(() => {
          this.loadingService.hide();
          console.error('Error:', err);
        }, remainingTime);
      },
    });
  }

  private processData(res: any): void {
    this.data = {
      ...res,
      items: res.items.map((item: InterviewModel) => ({
        ...item,
        candidateName: item.candidateName || 'N/A',
        interviewerName: item.interviewers.length
          ? item.interviewers[0].fullName
          : 'N/A',
        schedule: `${item.interviewDate} (${item.startAt} - ${item.endAt})`,
        jobName: item.jobName || 'N/A',
        status:
          InterviewStatus[
            item.status as keyof typeof InterviewStatus
          ].toString(),
        result: item.result === 'NotApplicable' ? 'N/A' : item.result,
      })),
    };
  }

  public create(): void {
    this.router.navigate(['/interview/create']);
  }

  public viewInterviewDetail(id: string): void {
    this.router.navigate(['/interview/interview-detail', id]);
  }
  public editInterview(id: string): void {
    this.router.navigate(['/interview/interview-edit', id]);
  }
  onSort(event: { field: string; direction: OrderDirection }): void {
    this.filter.orderBy = event.field;
    this.filter.orderDirection = event.direction;

    this.searchData();
  }
}
