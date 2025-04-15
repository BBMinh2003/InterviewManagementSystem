import {
  HttpEvent,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { PERMISSION_SERVICE, AUTH_SERVICE } from '../constants/injection.constant';
import { inject } from '@angular/core';
import { Observable, switchMap, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const permissionsService = inject(PERMISSION_SERVICE);
  const authService = inject(AUTH_SERVICE);

  let accessToken = permissionsService.getAccessToken();

  // Check if the access token is expired
  if (!accessToken || authService.isAccessTokenExpired()) {
    return authService.refreshToken().pipe(
      switchMap((newToken) => {
        if (newToken) {
          req = req.clone({
            setHeaders: { Authorization: `Bearer ${newToken}` },
          });
        }
        return next(req);
      }),
      catchError((error) => {
        authService.logout();
        return throwError(() => error);
      })
    );
  }

  // If access token is valid, attach it to the request
  req = req.clone({
    setHeaders: { Authorization: `Bearer ${accessToken}` },
  });

  return next(req);
};
