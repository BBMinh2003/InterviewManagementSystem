import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { ServicesModule } from '../../../services/service.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { TableComponent } from '../../../core/components/table/table.component';
import { MasterDataListComponent } from '../../user/master-data/master-data/master-data.component';
import { UserModel } from '../../../models/user/user.model';
import { TableColumn } from '../../../core/models/table/table-column.model';
import { LOADING_SERVICE, NOTIFICATION_SERVICE, USER_SERVICE } from '../../../constants/injection.constant';
import { IUserService } from '../../../services/user/user-service.interface';
import { Role } from '../../../core/enums/role';
import { OrderDirection } from '../../../models/search.model';
import { UserSearchModel } from '../../../models/user/user-search.model';
import { Router, RouterModule } from '@angular/router';
import { ILoadingService } from '../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../services/notification/notification-service.interface';

@Component({
  selector: 'app-user-management',
  imports: [
    CommonModule,
    ServicesModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    FormsModule,
    TableComponent,
    RouterModule
  ],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css',
})
export class UserManagementComponent
  extends MasterDataListComponent<UserModel>
  implements OnInit {
  public override columns: TableColumn[] = [
    { name: 'Username', value: 'fullName', sortable: true },
    { name: 'Email', value: 'email', sortable: true },
    { name: 'Phone No.', value: 'phoneNumber', sortable: true },
    { name: 'Role', value: 'roles' },
    { name: 'Status', value: 'isActive', sortable: true },
  ];
  constructor(@Inject(USER_SERVICE) private readonly userService: IUserService, private readonly router: Router,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private readonly loadingService: ILoadingService,) {
    super();
  }
  public override filter: UserSearchModel = {
    keyword: '',
    pageNumber: 1,
    pageSize: this.currentPageSize,
    orderBy: 'createdAt',
    orderDirection: OrderDirection.ASC,
    role: '',
  };

  public roles: string[] = [];

  override ngOnInit(): void {
    super.ngOnInit();
    this.roles = Object.keys(Role).filter((key) => isNaN(Number(key)));
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      role: new FormControl<string | null>(null),
    });
  }

  protected override searchData(): void {
    this.loadingService.show();
    this.userService.search(this.filter).subscribe((res) => {
      this.data = {
        ...res,
        items: res.items.map((item) => ({
          ...item,
          phoneNumber: item.phoneNumber || 'N/A',
          roles: item.roles || 'None',
          isActive: item.isActive ? 'Activated' : 'De-activated',
        })),
      };
      console.log(this.data);
      this.loadingService.hide();
    });
  }

  viewUserDetails(id: string): void {
    this.router.navigate(['/admin/user', id]);
  }

  createUser(): void {
    this.router.navigate(['/admin/user/create']);
  }

  editUser(id: string): void {
    this.router.navigate(['/admin/user/edit', id]);
  }

  onSort(event: { field: string; direction: OrderDirection }): void {
    this.filter.orderBy = event.field;
    this.filter.orderDirection = event.direction;
    console.log(this.filter);

    this.searchData();
  }

  public returnList(): void {
    this.router.navigate(['candidate/']);
  }
}
