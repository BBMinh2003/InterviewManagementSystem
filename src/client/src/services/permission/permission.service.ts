import { Inject, Injectable } from '@angular/core';
import { AUTH_SERVICE } from '../../constants/injection.constant';
import { IAuthService } from '../auth/auth-service.interface';
import { Router } from '@angular/router';
import { IPermissionService } from './permission-service.interface';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class PermissionService implements IPermissionService {
  constructor(
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    private router: Router
  ) {}

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
}
