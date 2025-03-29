import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorModel } from '../../models/error/error.model';

@Component({
  selector: 'app-error-page',
  standalone: true,
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss'],
})
export class ErrorPageComponent {
  error: ErrorModel = new ErrorModel('404', 'Page not found!');

  constructor(private route: ActivatedRoute, private router: Router) {
    this.route.queryParams.subscribe((params) => {
      if (params['code']) {
        this.error = this.getErrorByCode(params['code']);
      }
    });
  }

  getErrorByCode(code: string): ErrorModel {
    const errorMap: { [key: string]: ErrorModel } = {
      '403': new ErrorModel('403', 'You do not have permission to access!'),
      '404': new ErrorModel('404', 'Page not found'),
      '500': new ErrorModel('500', 'Server error, please try again later!'),
    };
    return errorMap[code] || new ErrorModel('404', 'Page not found');
  }

  goHome() {
    this.router.navigate(['/']);
  }
}
