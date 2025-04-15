import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MasterDataListComponent } from '../../master-data/master-data/master-data.component';
import { OfferModel } from '../../../../models/offer/offer.model';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { IOfferService } from '../../../../services/offer/offer-service.interface';
import { Router } from '@angular/router';
import {
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
  OFFER_SERVICE,
} from '../../../../constants/injection.constant';
import { OfferStatus } from '../../../../core/enums/offer-status';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { OfferSearch } from '../../../../models/offer/offer-search.model';
import { CommonModule } from '@angular/common';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import { TableComponent } from '../../../../core/components/table/table.component';
import { ServicesModule } from '../../../../services/service.module';
import { DepartmentService } from '../../../../services/department/department.service';
import { OrderDirection } from '../../../../models/search.model';
import { LoadingComponent } from '../../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../../core/components/notification/notification.component';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { faFile } from '@fortawesome/free-solid-svg-icons';
import saveAs from 'file-saver';
import { ExportOfferDialogComponent } from '../export-offer-dialog/export-offer-dialog.component';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-offer-management',
  imports: [
    CommonModule,
    ServicesModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    FormsModule,
    TableComponent,
    NotificationComponent,
    LoadingComponent,
    ExportOfferDialogComponent,
  ],
  templateUrl: './offer-management.component.html',
  styleUrl: './offer-management.component.css',
})
export class OfferManagementComponent
  extends MasterDataListComponent<OfferModel>
  implements OnInit
{
  public faFile: IconDefinition = faFile;

  fromDate!: Date;
  toDate!: Date;
  isExporting = false;
  exportError?: string;

  @ViewChild(ExportOfferDialogComponent)
  exportDialog!: ExportOfferDialogComponent;

  public override columns: TableColumn[] = [
    { name: 'Candidate Name', value: 'candidateName'},
    { name: 'Email', value: 'candidateEmail'},
    { name: 'Approver', value: 'approverName'},
    { name: 'Department', value: 'departmentName'},
    { name: 'Notes', value: 'note'},
    { name: 'Status', value: 'statusText'},
  ];
  public statuses: { value: number; label: string }[] = [];
  public departments: { id: string; name: string }[] = [];

  constructor(
    @Inject(OFFER_SERVICE) private readonly offerService: IOfferService,
    private readonly departmentService: DepartmentService,
    private readonly router: Router,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private readonly loadingService: ILoadingService
  ) {
    super();
  }

  override ngOnInit(): void {
    super.ngOnInit();
    this.loadDepartments();
    this.initializeStatuses();
  }

  private loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (departments) => {
        this.departments = departments;
      },
      error: (err) => {
        console.error('Failed to load departments', err);
      },
    });
  }

  private initializeStatuses(): void {
    this.statuses = Object.keys(OfferStatus)
      .filter((key) => !isNaN(Number(OfferStatus[key as any])))
      .map((key) => {
        const spacedLabel = key.replace(/([A-Z])/g, ' $1').trim();
  
        return {
          value: OfferStatus[key as keyof typeof OfferStatus],
          label: spacedLabel,
        };
      });
  }

  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      departmentId: new FormControl<string | null>(null),
      status: new FormControl<number | null>(null),
    });
  }

  public override filter: OfferSearch = {
    keyword: '',
    pageNumber: 1,
    pageSize: this.currentPageSize,
    orderBy: 'createdAt',
    orderDirection: OrderDirection.ASC,
    departmentId: null,
    status: null,
  };

  protected override searchData(): void {
    this.loadingService.show();
    this.offerService.search(this.filter).subscribe((res) => {
      this.data = {
        ...res,
        items: res.items.map((item) => ({
          ...item,
          statusText: this.getStatusString(item.status),
        })),
      };
      this.loadingService.hide();
    });
  }

  onSort(event: { field: string; direction: OrderDirection }): void {
    this.filter.orderBy = event.field;
    this.filter.orderDirection = event.direction;
    console.log(this.filter);

    this.searchData(); 
  }

  private getStatusString(statusCode: number): string {
    const statusName = Object.entries(OfferStatus).find(
      ([key, val]) => val === statusCode
    )?.[0];
  
    if (!statusName) {
      return 'Unknown';
    }
  
    const spacedName = statusName.replace(/([A-Z])/g, ' $1').trim();
  
    return spacedName;
  }

  handleExport(dates: { from: Date; to: Date }) {
    this.loadingService.show();

    this.offerService
      .exportOffers(dates.from, dates.to)
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: (blob) => {
          if (!(blob instanceof Blob)) {
            this.notificationService.showMessage(
              'Invalid file format received',
              'error'
            );
            return;
          }

          const fileName = `OffersList_${this.formatDate(
            dates.from
          )}_${this.formatDate(dates.to)}.xlsx`;
          saveAs(blob, fileName);
          this.notificationService.showMessage(
            'File exported successfully',
            'success'
          );
        },
        error: (err) => {
          const errorMessage = this.getErrorMessage(err);
          this.notificationService.showMessage(
            `Export failed: ${errorMessage}`,
            'error'
          );
          console.error('Export error:', err);
        },
      });
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  private getErrorMessage(error: any): string {
    if (error?.error?.message) {
      return error.error.message;
    }
    if (error?.status === 0) {
      return 'Unable to connect to server';
    }
    return error.message || 'Unknown error occurred';
  }

  viewOfferDetail(id: string): void {
    this.router.navigate(['/offer', id]);
  }

  editOffer(id: string): void {
    console.log('ID đang điều hướng:', id);
    this.router.navigate(['/offer/edit', id]);
  }

  createNewOffer(): void {
    this.router.navigate(['/offer/create']);
  }
}
