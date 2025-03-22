import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { MatIconModule } from '@angular/material/icon';
import { provideAnimations } from '@angular/platform-browser/animations';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AuthService } from '../services/auth/auth.service';
import { AUTH_SERVICE, PERMISSION_SERVICE } from '../constants/injection.constant';
import { HttpClientModule } from '@angular/common/http';
import { PermissionService } from '../services/permission/permission.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    importProvidersFrom(
      MatIconModule,
      FontAwesomeModule,
      HttpClientModule
    ),
    {
      provide: AUTH_SERVICE,
      useClass: AuthService,
    },
    {
      provide: PERMISSION_SERVICE,
      useClass: PermissionService,
    },
  ]
};