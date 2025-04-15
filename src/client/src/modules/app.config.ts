import {
  ApplicationConfig,
  ErrorHandler,
  importProvidersFrom,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { MatIconModule } from '@angular/material/icon';
import { provideAnimations } from '@angular/platform-browser/animations';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AuthService } from '../services/auth/auth.service';
import {
  AUTH_SERVICE,
  CANDIDATE_SERVICE,
  DASHBOARD_SERVICE,
  INTERVIEW_SERVICE,
  INTERVIEWER_SERVICE,
  JOB_SERVICE,
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
  PERMISSION_SERVICE,
  USER_SERVICE,
} from '../constants/injection.constant';
import {
  HTTP_INTERCEPTORS,
  HttpClientModule,
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { PermissionService } from '../services/permission/permission.service';
import { NotificationService } from '../services/notification/notification.service';
import { GlobalExceptionHandler } from '../core/exceptions/global-exception-handler';
import { ErrorInterceptor } from '../core/exceptions/interceptors/error.interceptor';
import { LoadingService } from '../services/loading/loading.service';
import { InterviewService } from '../services/interview/interview.service';
import { authInterceptor } from '../interceptors/auth.interceptor';
import { CandidateService } from '../services/candidate/candidate.service';
import { JobService } from '../services/job/job.service.service';
import { UserService } from '../services/user/user.service';
import { InterviewerService } from '../services/interviewer/interviewer.service.service';
import { DashboardService } from '../services/dashboard/dashboard.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor]), withFetch()),
    provideAnimations(),
    importProvidersFrom(MatIconModule, FontAwesomeModule, HttpClientModule),
    {
      provide: AUTH_SERVICE,
      useClass: AuthService,
    },
    {
      provide: PERMISSION_SERVICE,
      useClass: PermissionService,
    },
    {
      provide: NOTIFICATION_SERVICE,
      useClass: NotificationService,
    },
    {
      provide: INTERVIEW_SERVICE,
      useClass: InterviewService,
    },
    {
      provide: JOB_SERVICE,
      useClass: JobService,
    },
    {
      provide: CANDIDATE_SERVICE,
      useClass: CandidateService,
    },
    {
      provide: USER_SERVICE,
      useClass: UserService,
    },
    {
      provide: INTERVIEWER_SERVICE,
      useClass: InterviewerService,
    },
    {
      provide: DASHBOARD_SERVICE,
      useClass: DashboardService,
    },
    { provide: ErrorHandler, useClass: GlobalExceptionHandler },
    { provide: LOADING_SERVICE, useClass: LoadingService },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
};
