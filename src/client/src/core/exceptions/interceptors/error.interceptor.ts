import { Inject, Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ErrorModel } from '../../models/error/error.model';
import {
  ForbiddenException,
  InternalServerErrorException,
  NotFoundException,
  UnauthorizedException,
} from '../custom/custom-exceptions';
import { NOTIFICATION_SERVICE } from '../../../constants/injection.constant';
import { INotificationService } from '../../../services/notification/notification-service.interface';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService
  ) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let customError;

        switch (error.status) {
          case 403:
            customError = new ForbiddenException();
            this.router.navigate(['/error'], {
              queryParams: {
                code: customError.code,
                message: customError.message,
              },
            });
            break;
          case 401:
            customError = new UnauthorizedException();

            if (req.url.includes('/login')) {
              this.notificationService.showMessage(
                'Wrong username or password!',
                'error'
              );
              break;
            }
            if (req.url.includes('/forgotPassword')) {
              this.notificationService.showMessage(
                'Email not found!',
                'error'
              );
              break;
            }
            this.router.navigate(['/error'], {
              queryParams: {
                code: customError.code,
                message: customError.message,
              },
            });

            break;
          case 404:
            customError = new NotFoundException();
            this.router.navigate(['/error'], {
              queryParams: {
                code: customError.code,
                message: customError.message,
              },
            });
            break;
          case 500:
            customError = new InternalServerErrorException();
            this.router.navigate(['/error'], {
              queryParams: {
                code: customError.code,
                message: customError.message,
              },
            });
            break;
          default:
            customError = new ErrorModel(
              error.status.toString(),
              error.message
            );
            this.router.navigate(['/error'], {
              queryParams: {
                code: customError.code,
                message: customError.message,
              },
            });
            break;
        }

        return throwError(() => customError);
      })
    );
  }
}
