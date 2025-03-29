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
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
  PERMISSION_SERVICE,
} from '../constants/injection.constant';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { PermissionService } from '../services/permission/permission.service';
import { NotificationService } from '../services/notification/notification.service';
import { GlobalExceptionHandler } from '../core/exceptions/global-exception-handler';
import { ErrorInterceptor } from '../core/exceptions/interceptors/error.interceptor';
import { LoadingService } from '../services/loading/loading.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
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
    { provide: ErrorHandler, useClass: GlobalExceptionHandler },
    {provide:LOADING_SERVICE, useClass:LoadingService},
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
};
