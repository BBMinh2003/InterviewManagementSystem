import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class GlobalExceptionHandler implements ErrorHandler {
  constructor(private router: Router, private ngZone: NgZone) {}

  handleError(error: any): void {
    console.error('Global Exception:', error);

    this.ngZone.run(() => {
      if (error.status === 403) {
        this.router.navigate(['/error'], { queryParams: { code: '403' } });
      } else if (error.status === 404) {
        this.router.navigate(['/error'], { queryParams: { code: '404' } });
      } else if (error.status === 500) {
        this.router.navigate(['/error'], { queryParams: { code: '500' } });
      }
    });
  }
}
