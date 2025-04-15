import { Component, Input, OnInit } from '@angular/core';
import { ItemDetails } from '../../../../core/models/item-detail/detail-row.model';
import { ItemDetailsComponent } from '../../../../core/components/item-details/item-details.component';
import { CommonModule, DatePipe } from '@angular/common';
import { OfferModel } from '../../../../models/offer/offer.model';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { OfferService } from '../../../../services/offer/offer.service';
import { faHome } from '@fortawesome/free-solid-svg-icons';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { ConfirmDialogComponent } from './cancel-offer-dialog/confirm-dialog.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../../services/auth/auth.service';
import { OfferStatus } from '../../../../core/enums/offer-status';
import { UserInformation } from '../../../../models/auth/user-information.model';

@Component({
  selector: 'app-offer-details',
  imports: [
    CommonModule,
    ItemDetailsComponent,
    FaIconComponent,
    RouterModule,
    MatDialogModule,
    MatSnackBarModule,
  ],
  templateUrl: './offer-details.component.html',
  styleUrl: './offer-details.component.css',
  standalone: true,
})
export class OfferDetailsComponent implements OnInit {
  @Input() offer!: OfferModel;
  details: ItemDetails[] = [];
  createdAt = '';
  lastUpdatedText = '';
  offerStatus = OfferStatus;
  currentUser: UserInformation | null = null;

  faHome = faHome;

  constructor(
    private readonly dialog: MatDialog,
    private readonly route: ActivatedRoute,
    private readonly offerService: OfferService,
    private readonly router: Router,
    private readonly snackBar: MatSnackBar,
    private readonly authService: AuthService
  ) { }

  ngOnInit() {
    if(this.offer==null){
      this.router.navigate(['/offer']);
    }
    this.loadUserInfo();
    console.log('OfferDetailsComponent ngOnInit called');
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      console.log('ParamMap ID:', id);
      if (id) {
        this.transformData(id);
      }
    });
  }

  private loadUserInfo(): void {
    this.authService.getUserInformation().subscribe({
      next: (user) => {
        this.currentUser = user;
        console.log('Current user roles:', user?.role);
      },
      error: (err) => {
        console.error('Failed to load user info:', err);

      }
    });
  }

  isManagerOrAdmin(): boolean {
    return (this.currentUser?.role === 'Manager' || this.currentUser?.role === 'Admin') ?? false;
  }

  canCancelOffer(): boolean {
    const allowedStatuses = [
      OfferStatus.WaitingForApproval,
      OfferStatus.Approved,
      OfferStatus.WaitingForResponse
    ];
    return allowedStatuses.includes(this.offer?.status) &&
      (this.isManagerOrAdmin() ||
        this.currentUser?.id === this.offer?.recruiterOwnerId);
  }

  confirmStatusChange(newStatus: OfferStatus): void {
    const dialogConfig = this.getDialogConfig(newStatus);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: dialogConfig.title,
        message: dialogConfig.message,
        confirmText: dialogConfig.confirmText,
        confirmColor: dialogConfig.confirmColor,
        cancelText: dialogConfig.cancelText
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) this.updateOfferStatus(newStatus);
    });
  }

  private getDialogConfig(status: OfferStatus): {
    title: string,
    message: string,
    confirmText: string,
    confirmColor: string,
    cancelText: string
  } {
    const baseConfig = {
      cancelText: 'Cancel',
      cancelColor: 'bg-gray-100 hover:bg-gray-200 text-gray-700'
    };

    // Sử dụng enum làm key thay vì số
    const statusConfigs: Record<OfferStatus, any> = {
      [OfferStatus.WaitingForApproval]: {
        title: 'Approve Offer',
        message: this.getStatusActionMessage(status),
        confirmText: 'Approve',
        confirmColor: 'bg-green-600 hover:bg-green-700'
      },
      [OfferStatus.Approved]: {
        title: 'Approve Offer',
        message: this.getStatusActionMessage(status),
        confirmText: 'Approve',
        confirmColor: 'bg-green-600 hover:bg-green-700'
      },
      [OfferStatus.Rejected]: {
        title: 'Reject Offer',
        message: this.getStatusActionMessage(status),
        confirmText: 'Reject',
        confirmColor: 'bg-red-600 hover:bg-red-700'
      },
      [OfferStatus.WaitingForResponse]: {
        title: 'Mark as Sent to candidate',
        message: this.getStatusActionMessage(status),
        confirmText: 'Mark Sent',
        confirmColor: 'bg-blue-600 hover:bg-blue-700'
      },
      [OfferStatus.Accepted]: {
        title: 'Accept Offer',
        message: this.getStatusActionMessage(status),
        confirmText: 'Accept',
        confirmColor: 'bg-green-600 hover:bg-green-700'
      },
      [OfferStatus.Declined]: {
        title: 'Decline Offer',
        message: this.getStatusActionMessage(status),
        confirmText: 'Decline',
        confirmColor: 'bg-red-600 hover:bg-red-700'
      },
      [OfferStatus.Cancelled]: {
        title: 'Cancel Offer',
        message: this.getStatusActionMessage(status),
        confirmText: 'Confirm Cancel',
        confirmColor: 'bg-red-600 hover:bg-red-700'
      }
    };

    return { ...baseConfig, ...statusConfigs[status] };
  }



  private getStatusText(statusCode: OfferStatus): string {
    const statusMap: Record<OfferStatus, string> = {
      [OfferStatus.WaitingForApproval]: 'Waiting For Approval',
      [OfferStatus.Approved]: 'Approved',
      [OfferStatus.Rejected]: 'Rejected',
      [OfferStatus.WaitingForResponse]: 'Waiting For Response',
      [OfferStatus.Accepted]: 'Accepted',
      [OfferStatus.Declined]: 'Declined',
      [OfferStatus.Cancelled]: 'Cancelled'
    };
    return statusMap[statusCode] || 'Unknown Status';
  }

  private getStatusClass(status: OfferStatus): string {
    const statusClasses: Record<OfferStatus, string> = {
      [OfferStatus.WaitingForApproval]: 'bg-gray-100 text-gray-800',
      [OfferStatus.Approved]: 'bg-yellow-100 text-yellow-800',
      [OfferStatus.Rejected]: 'bg-red-100 text-red-800',
      [OfferStatus.WaitingForResponse]: 'bg-red-100 text-red-800',
      [OfferStatus.Accepted]: 'bg-green-100 text-green-800',
      [OfferStatus.Declined]: 'bg-gray-200 text-gray-600',
      [OfferStatus.Cancelled]: 'bg-red-100 text-red-800'
    };
    return `px-3 py-1 rounded-full text-sm ${statusClasses[status] || 'bg-gray-100'}`;
  }

  private getStatusActionMessage(status: OfferStatus): string {
    const messages: Record<OfferStatus, string> = {
      [OfferStatus.WaitingForApproval]: '', // Thêm giá trị mặc định
      [OfferStatus.Approved]: 'Approve this offer',
      [OfferStatus.Rejected]: 'Reject this offer',
      [OfferStatus.WaitingForResponse]: 'Mark as sent to candidate',
      [OfferStatus.Accepted]: 'Accept this offer',
      [OfferStatus.Declined]: 'Decline this offer',
      [OfferStatus.Cancelled]: 'Cancel this offer'
    };
    return messages[status];
  }

  private showStatusMessage(status: OfferStatus): void {
    const messages: Record<OfferStatus, string> = {
      [OfferStatus.WaitingForApproval]: 'Waiting for approval', // Thêm trường hợp mới
      [OfferStatus.Approved]: 'Offer approved successfully!',
      [OfferStatus.Rejected]: 'Offer rejected successfully!',
      [OfferStatus.WaitingForResponse]: 'Offer marked as sent!',
      [OfferStatus.Accepted]: 'Offer accepted!',
      [OfferStatus.Declined]: 'Offer declined!',
      [OfferStatus.Cancelled]: 'Offer cancelled successfully!'
    };

    // Thêm fallback cho các trường hợp không xác định
    const message = messages[status] || 'Status updated successfully!';

    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: ['bg-green-100', 'text-green-800']
    });
  }

  private handleStatusError(error: any): void {
    console.error('Status update failed:', error);
    this.snackBar.open('Failed to update status!', 'Close', {
      duration: 3000,
      panelClass: ['bg-red-100', 'text-red-800']
    });
  }


  private transformData(id: string): void {
    console.log('transformData called with ID:', id);
    this.offerService.getById(id).subscribe({
      next: (data) => {
        console.log('offerService.getById success:', data);
        this.offer = data;
        this.details = [
          // Row 1
          {
            label: 'Candidate',
            value: [this.offer.candidateName],
          },
          {
            label: 'Contract Type',
            value: [this.offer.contactType],
          },

          // Row 2
          {
            label: 'Position',
            value: [this.offer.positionName],
          },
          {
            label: 'Level',
            value: [this.offer.level],
          },

          // Row 3
          {
            label: 'Approver',
            value: [this.offer.approverName || 'N/A'],
          },
          {
            label: 'Department',
            value: [this.offer.departmentName],
          },

          // Row 4
          {
            label: 'Interview Info',
            value: this.getInterviewInfo(),
          },
          {
            label: 'Recruiter Owner',
            value: [this.offer.recruiterOwnerName],
          },

          // Row 5
          {
            label: 'Contract Period',
            value: [
              `From ${this.formatDateString(this.offer.contactPeriodFrom)}`,
              `To ${this.formatDateString(this.offer.contactPeriodTo)}`,
            ],
          },
          {
            label: 'Due Date',
            value: [this.formatDateString(this.offer.dueDate)],
          },

          // Row 6
          {
            label: 'Interview Notes',
            value: this.getInterviewNotes(),
          },
          {
            label: 'Basic Salary',
            value: [this.formatCurrency(this.offer.basicSalary)],
          },

          // Row 7
          {
            label: 'Status',
            value: [this.getStatusText(this.offer.status)],
            customClass: this.getStatusClass(this.offer.status),
          },
          {
            label: 'Note',
            value: [this.offer.note ?? 'N/A'],
          },
        ];
        console.log('Details array:', this.details);
      },
      error: (err) => {
        console.error('Error fetching offer details', err);
      },
    });
  }

  private extractLevelName(level: string): string {
    return (
      level
        .split('.')
        .pop()
        ?.replace(/([A-Z])/g, ' $1')
        .trim() ?? 'N/A'
    );
  }

  private formatDateString(dateString: string): string {
    return new DatePipe('en-US').transform(dateString, 'dd/MM/yyyy') ?? 'N/A';
  }

  public formatDate(date: Date, showTime = false): string {
    try {
      const datePipe = new DatePipe('en-US');
      const format = showTime ? 'dd/MM/yyyy HH:mm' : 'dd/MM/yyyy';
      return datePipe.transform(date, format) ?? 'N/A';
    } catch (e) {
      console.error('Invalid date format:', date);
      return 'N/A';
    }
  }

  private formatCurrency(amount: number): string {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND',
      minimumFractionDigits: 0,
    }).format(amount);
  }

  private getInterviewInfo(): string[] {
    const info = [];
    if (this.offer.interviewInfo) {
      info.push(this.offer.interviewInfo);
    }
    if (this.offer.recruiterOwnerName) {
      info.push(`Interviewer: ${this.offer.recruiterOwnerName}`);
    }
    return info.length > 0 ? info : ['N/A'];
  }

  private getInterviewNotes(): string[] {
    if (!this.offer.interviewNote) return ['N/A'];
    return this.offer.interviewNote.split('\n').filter((n) => n.trim());
  }

  private updateOfferStatus(newStatus: OfferStatus): void {
    const originalId = this.offer.id; // Lưu trữ ID trước khi gọi API

    this.offerService.updateOfferStatus(originalId, newStatus).subscribe({
      next: (updatedOffer) => {
        this.offer = { ...updatedOffer, id: originalId }; // Đảm bảo giữ lại ID
        this.showStatusMessage(newStatus);
        this.transformData(originalId); // Sử dụng ID đã lưu

        if (newStatus === OfferStatus.Cancelled) {
          setTimeout(() => this.router.navigate(['/offer']), 1500);
        }
      },
      error: (err) => this.handleStatusError(err)
    });
  }

  goBack(): void {
    this.router.navigate(['/offer']);
  }

  editOffer(): void {
    this.router.navigate(['/offer/edit', this.route.snapshot.params['id']]);
  }
}
