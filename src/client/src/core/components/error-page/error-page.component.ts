import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorModel } from '../../models/error/error.model';
import { ForbiddenException, InternalServerErrorException, NotFoundException, UnauthorizedException } from '../../exceptions/custom/custom-exceptions';

@Component({
  selector: 'app-error-page',
  standalone: true,
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss'],
})
export class ErrorPageComponent {
  error: ErrorModel = new NotFoundException(); 

  constructor(private route: ActivatedRoute, private router: Router) {
    this.route.queryParams.subscribe((params) => {
      if (params['code']) {
        this.error = this.getErrorByCode(params['code']);
      }
    });
  }

  getErrorByCode(code: string): ErrorModel {
    const errorMap: { [key: string]: ErrorModel } = {
      '401': new UnauthorizedException(),
      '403': new ForbiddenException(),
      '404': new NotFoundException(),
      '500': new InternalServerErrorException(),
    };
    return errorMap[code] || new NotFoundException();
  }

  goHome() {
    this.router.navigate(['/']);
  }
}
