import { Injectable } from '@angular/core';
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

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

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
