import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { InterviewModel } from '../../../../models/interview/interview.model';
import { IInterviewService } from '../../../../services/interview/interview-service.interface';
import {
  AUTH_SERVICE,
  INTERVIEW_SERVICE,
  NOTIFICATION_SERVICE,
  LOADING_SERVICE
} from '../../../../constants/injection.constant';
import { CommonModule } from '@angular/common';
import { InterviewStatus } from '../../../../core/enums/interview-status';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../../../../core/components/confirmation-dialog/confirmation-dialog.component';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { FormsModule } from '@angular/forms';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { Result } from '../../../../core/enums/result';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faHome, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { LoadingComponent } from "../../../../core/components/loading/loading.component";

@Component({
  selector: 'app-interview-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, FontAwesomeModule, RouterModule, LoadingComponent],
  templateUrl: './interview-details.component.html',
  styleUrl: './interview-details.component.css',
})
export class InterviewDetailsComponent implements OnInit {
  public interview!: InterviewModel;
  interviewId: string | null = null;
  isEditingResult = false;
  selectedResult: number = 0;
  userRole: string = "";
  isEditing = false;
  editableInterview!: InterviewModel;
  public Result = Result;
  public faHome: IconDefinition = faHome;
  private loadingTimeout: any;

  constructor(
    private route: ActivatedRoute,
    @Inject(INTERVIEW_SERVICE) private interviewService: IInterviewService,
    @Inject(NOTIFICATION_SERVICE) private notificationService: INotificationService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(LOADING_SERVICE) private loadingService: ILoadingService,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.showLoading();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.interviewId = id;
      this.loadInterview(id);
    }

    this.authService.getUserInformation().subscribe({
      next: (user) => {
        if (user && user.role) {
          this.userRole = user.role;
        }
      },
      error: (err) => console.error('Error loading user info:', err),
      complete: () => this.hideLoading()
    });
  }

  private showLoading(): void {
    this.loadingService.show();
    this.loadingTimeout = setTimeout(() => {
      this.hideLoading();
    }, 10000);
  }

  private hideLoading(): void {
    if (this.loadingTimeout) {
      clearTimeout(this.loadingTimeout);
    }
    this.loadingService.hide();
  }


  ngOnDestroy(): void {
    this.hideLoading();
  }

  public sendReminder(): void {
    this.showLoading();
    this.interviewService.sendReminder(this.interviewId!).subscribe({
      next: (response) => {
        this.notificationService.showMessage(
          'Reminder sent successfully!',
          'success'
        );
      },
      error: (error) => {
        this.notificationService.showMessage(
          'Error sending reminder.',
          'error'
        );
        console.error('Error sending reminder:', error);
      },
      complete: () => this.hideLoading()
    });
  }

  private loadInterview(id: string): void {
    const startTime = Date.now(); 
    const MIN_LOADING_TIME = 1000; 
    
    this.showLoading();
  
    this.interviewService.getById(id).subscribe({
      next: (data) => {
        const elapsedTime = Date.now() - startTime;
        const remainingTime = Math.max(0, MIN_LOADING_TIME - elapsedTime);
  
        setTimeout(() => {
          this.interview = data;
          
          const resultMap: Record<string, Result> = {
            passed: Result.Passed,
            failed: Result.Failed,
            na: Result.NA,
          };
          this.selectedResult = resultMap[this.interview.result.toLowerCase()] ?? Result.NA;
          this.hideLoading();
        }, remainingTime);
      },
      error: (err) => {
        const elapsedTime = Date.now() - startTime;
        const remainingTime = Math.max(0, MIN_LOADING_TIME - elapsedTime);
  
        setTimeout(() => {
          console.error('Error fetching interview details', err);
          this.hideLoading();
        }, remainingTime);
      }
    });
  }

  public submitResult(): void {
    if (!this.interviewId) return;

    this.showLoading();
    const requestPayload = {
      id: this.interviewId,
      result: Number(this.selectedResult),
    };

    this.interviewService.submitResult(this.interviewId, requestPayload.result).subscribe({
      next: (response) => {
        if (response == true) {
          this.notificationService.showMessage(
            'Result submitted successfully!',
            'success'
          );
          this.isEditingResult = false;
          this.interview.result = requestPayload.result.toString();
        } else {
          this.notificationService.showMessage(
            'Result already submitted!',
            'warning'
          );
        }
      },
      error: (error) => {
        this.notificationService.showMessage(
          'Failed to submit result.',
          'error'
        );
        console.error('Error submitting result:', error);
      },
      complete: () => this.hideLoading()
    });
  }

  public get interviewersDisplay(): string {
    return (
      this.interview?.interviewers?.map((i) => i.fullName).join(', ') || 'N/A'
    );
  }

  public goBack(): void {
    this.router.navigate(['/interview']);
  }

  public get statusDisplay(): string {
    return String(
      InterviewStatus[this.interview?.status as keyof typeof InterviewStatus]
    );
  }
  get hasResult(): boolean {
    return !!this.interview?.result; 
  }

  public toggleEditResult(): void {
    this.isEditingResult = true;
    this.selectedResult = 0;
  }

  public onEdit(id: string): void {
    this.router.navigate(['/interview/interview-edit', id]);
  }
}