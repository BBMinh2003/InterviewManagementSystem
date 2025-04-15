import { Inject, Injectable } from '@angular/core';
import { AUTH_SERVICE } from '../../constants/injection.constant';
import { IAuthService } from '../auth/auth-service.interface';
import { Router } from '@angular/router';
import { IPermissionService } from './permission-service.interface';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { UserInformation } from '../../models/auth/user-information.model';

@Injectable({
  providedIn: 'root',
})
export class PermissionService implements IPermissionService {
  constructor(
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    private router: Router
  ) {}
  canEditInterview(): Observable<boolean> {
    return this.authService.getUserInformation().pipe(
      map((userInfo) => {
        const role: string = userInfo?.role || 'none';
        const allowedRoles = ['Admin', 'Recruiter', 'Manager'];

        const isAuthorized = allowedRoles.includes(role);

        if (!isAuthorized) {
          this.router.navigate(['/home']);
        }

        return isAuthorized;
      })
    );
  }
  canActivateInterview(): Observable<boolean> {
    return this.authService.getUserInformation().pipe(
      map((userInfo) => {
        const role: string = userInfo?.role || 'none';

        const allowedRoles = ['Admin', 'Recruiter', 'Manager', 'Interviewer'];

        const isAuthorized = allowedRoles.includes(role);

        if (!isAuthorized) {
          this.router.navigate(['/home']);
        }

        return isAuthorized;
      })
    );
  }

  isUnauthenticated(): Observable<boolean> {
    return this.authService.isAuthenticated().pipe(
      tap((res) => {
        if (res) {
          this.router.navigate(['/home']);
        }
      }),
      map((res) => !res)
    );
  }

  canActivate(): Observable<boolean> {
    return this.authService.isAuthenticated().pipe(
      tap((authenticated) => {
        if (!authenticated) {
          this.router.navigate(['/login']);
        }
      }),
      map((authenticated) => authenticated)
    );
  }

  canActivateAdmin(): Observable<boolean> {
    return this.authService.getUserInformation().pipe(
      map((userInfor) => {
        let role: string = userInfor?.role || '';

        const isAdmin = role == 'Admin';

        if (!isAdmin) {
          this.router.navigate(['/home']);
        }
        return isAdmin;
      })
    );
  }

  canActivateOffer(): Observable<boolean> {
    return this.authService.getUserInformation().pipe(
      map((userInfor) => {
        let role: string = userInfor?.role || 'none';

        const isAuthorized = ['Admin', 'Recruiter', 'Manager'].includes(role);

        if (!isAuthorized) {
          this.router.navigate(['/home']);
        }
        return isAuthorized;
      })
    );
  }

  getAccessToken(): string {
    return this.authService.getAccessToken();
  }
}
