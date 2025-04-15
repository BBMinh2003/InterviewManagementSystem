import { DashboardService } from './../../../services/dashboard/dashboard.service';
import {
  AfterViewInit,
  Component,
  ElementRef,
  Inject,
  ViewChild,
} from '@angular/core';
import { Chart } from 'chart.js/auto';
import {
  AUTH_SERVICE,
  DASHBOARD_SERVICE,
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
} from '../../../constants/injection.constant';
import { IAuthService } from '../../../services/auth/auth-service.interface';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import {
  faBriefcase,
  faComments,
  faHandshake,
  faUsers,
} from '@fortawesome/free-solid-svg-icons';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from '../../../core/components/loading/loading.component';
import { NotificationComponent } from '../../../core/components/notification/notification.component';
import { ILoadingService } from '../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../services/notification/notification-service.interface';
import { RouterModule } from '@angular/router';
import { IDashboardService } from '../../../services/dashboard/dashboard-service.interface';
import DashboardView from '../../../models/common/dashboard.model';

@Component({
  selector: 'app-home',
  imports: [
    FontAwesomeModule,
    CommonModule,
    RouterModule,
    NotificationComponent,
    LoadingComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements AfterViewInit {
  @ViewChild('analyticsChart', { static: false }) chartRef!: ElementRef;
  public role!: string;
  constructor(
    @Inject(AUTH_SERVICE) private readonly authService: IAuthService,
    @Inject(DASHBOARD_SERVICE)
    private readonly dashboardService: IDashboardService,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private readonly loadingService: ILoadingService
  ) {}
  dashboardView: DashboardView = new DashboardView();
  public faPeople: IconDefinition = faUsers;
  public faBriefcase: IconDefinition = faBriefcase;
  public faComments: IconDefinition = faComments;
  public faHandshake: IconDefinition = faHandshake;

  ngAfterViewInit() {
    this.loadingService.show();
    this.authService.getUserInformation().subscribe((response) => {
      this.role = response?.role || 'none';
      this.loadingService.hide();
    });
    this.dashboardService.getData().subscribe((response) => {
      this.dashboardView = response

      this.initChart()
      this.loadingService.hide();
    });

  }

  private initChart(){
    new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: {
        labels: !this.role.includes('Interviewer')
          ? ['Candidates', 'Jobs', 'Interviews', 'Offers']
          : ['Candidates', 'Jobs', 'Interviews'],
        datasets: [
          {
            label: 'Statistics',
            data: [
              this.dashboardView.candidateCount,
              this.dashboardView.jobCount,
              this.dashboardView.interviewCount,
              this.dashboardView.offerCount,
            ],
            backgroundColor: ['#3b82f6', '#10b981', '#facc15', '#ef4444'],
          },
        ],
      },
      options: {
        responsive: true,
        scales: {
          y: { beginAtZero: true },
        },
      },
    });
  }
}
