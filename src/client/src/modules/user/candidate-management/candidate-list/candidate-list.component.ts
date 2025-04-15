import { CommonModule } from '@angular/common';
import { Component, OnInit, Inject } from '@angular/core';
import {
  ReactiveFormsModule,
  FormsModule,
  FormGroup,
  FormControl,
} from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  AUTH_SERVICE,
  CANDIDATE_SERVICE,
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
} from '../../../../constants/injection.constant';
import { TableComponent } from '../../../../core/components/table/table.component';
import { Status } from '../../../../core/enums/candidate-status';
import { Gender } from '../../../../core/enums/gender';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { ServicesModule } from '../../../../services/service.module';
import { MasterDataListComponent } from '../../master-data/master-data/master-data.component';
import { Router } from '@angular/router';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../../core/components/notification/notification.component';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { OrderDirection } from '../../../../models/search.model';

@Component({
  selector: 'app-candidate-list',
  imports: [
    CommonModule,
    ServicesModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    FormsModule,
    TableComponent,
    NotificationComponent,
    LoadingComponent,
  ],
  templateUrl: './candidate-list.component.html',
  styleUrl: './candidate-list.component.css',
})
export class CandidateListComponent
  extends MasterDataListComponent<CandidateModel>
  implements OnInit
{
  public override columns: TableColumn[] = [
    { name: 'Name', value: 'name', sortable: true },
    { name: 'Email', value: 'email', sortable: true },
    { name: 'Phone No.', value: 'phone', sortable: true },
    { name: 'Current Position', value: 'positionName', sortable: false },
    { name: 'Owner HR', value: 'recruiterOwnerName', sortable: false },
    { name: 'Status', value: 'status', sortable: true },
  ];

  role: any;
  isShowDeleteAction: boolean = true;
  isShowEditAction: boolean = true;

  constructor(
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    private router: Router,
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private loadingService: ILoadingService
  ) {
    super();
  }

  public statuses: { value: number; label: string }[] = [];

  override ngOnInit(): void {
    super.ngOnInit();

    this.authService.getUserInformation().subscribe((res) => {
      this.role = res?.role;
      if (this.role == 'Interviewer') {
        this.isShowDeleteAction = false;
        this.isShowEditAction = false;
      }
    });
    this.statuses = Object.keys(Status)
      .filter((key) => !isNaN(Number(Status[key as any])))
      .map((key) => ({
        value: Status[key as keyof typeof Status],
        label: key.replace(/([A-Z])/g, ' $1').trim(),
      }));
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      status: new FormControl<number | null>(null),
    });
  }

  protected override searchData(): void {
    this.loadingService.show();
    this.candidateService.search(this.filter).subscribe((res) => {
      this.data = {
        ...res,
        items: res.items.map((item) => ({
          ...item,
          gender: Gender[item.gender as keyof typeof Gender],
          status: Status[item.status as keyof typeof Status]
            .toString()
            .replace(/([A-Z])/g, ' $1')
            .trim(),
        })),
      };
      this.loadingService.hide();
    });
  }

  public create(): void {
    this.router.navigate(['/candidate/create']);
  }

  public delete(id: string): void {
    this.candidateService.delete(id).subscribe((data) => {
      if (data) {
        this.searchData();
      }
    });
  }

  public viewCandidateDetail(id: string): void {
    this.router.navigate(['/candidate', id]);
  }

  public update(id: string): void {
    this.router.navigate(['candidate/update', id]);
  }

  onSort(event: { field: string; direction: OrderDirection}): void {
    this.filter.orderBy = event.field;
    this.filter.orderDirection = event.direction;
    console.log(this.filter);
    
    this.searchData(); // Gọi lại API với thông tin sắp xếp
  }

  public returnList(): void {
    this.router.navigate(['candidate/']);
  }
}
