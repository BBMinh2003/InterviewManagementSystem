import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-error-page',
  standalone: true,
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss'],
})
export class ErrorPageComponent {
  errorCode: number = 404;
  errorMessage: string = 'Trang bạn tìm không tồn tại!';

  constructor(private route: ActivatedRoute, private router: Router) {
    this.route.queryParams.subscribe((params) => {
      if (params['code'] === '500') {
        this.errorCode = 500;
        this.errorMessage = 'Lỗi máy chủ, vui lòng thử lại sau!';
      }
    });
  }

  goHome() {
    this.router.navigate(['/']);
  }
}
