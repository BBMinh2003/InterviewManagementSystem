import { Component, Inject, OnInit } from '@angular/core';
import { UserModel } from '../../../../models/user/user.model';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {
  LOADING_SERVICE,
  NOTIFICATION_SERVICE,
} from '../../../../constants/injection.constant';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../../services/user/user.service';
import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import { faHome } from '@fortawesome/free-solid-svg-icons';
import { ILoadingService } from '../../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../../services/notification/notification-service.interface';

@Component({
  selector: 'app-user-details',
  imports: [
    CommonModule,
    FontAwesomeModule,
    RouterModule,
  ],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.css',
})
export class UserDetailsComponent implements OnInit {
  public user!: UserModel;
  public faHome: IconDefinition = faHome;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly userService: UserService,
    @Inject(LOADING_SERVICE) private readonly loadingService: ILoadingService,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadUser(id);
    }
  }

  private loadUser(id: string): void {
    this.loadingService.show();
    this.userService.getById(id).subscribe({
      next: (data) => {
        this.loadingService.hide();
        this.user = data;
      },
      error: (err) => {
        this.loadingService.hide();
        this.router.navigate(['/admin/user']);
      },
    });
  }
  goBack(): void {
    this.router.navigate(['/admin/user']);
  }
  edit(): void {
    this.router.navigate([
      '/admin/user/edit/' + this.route.snapshot.paramMap.get('id'),
    ]);
  }
  switch(): void {
    this.loadingService.show();

    this.userService.switchStatus(this.user.id, !this.user.isActive).subscribe(
      (response) => {
        if (response) {
          this.loadingService.hide();

          if (!this.user.isActive) {
            this.notificationService.showMessage('De-activated!', 'success');
          } else {
            this.notificationService.showMessage('Activated!', 'success');
          }
          this.router.navigate(['/admin/user']);
        }
      },
      (error) => {
        this.loadingService.hide();
        if (error.status === 401) {
          this.notificationService.showMessage('Request failed!', 'error');
          this.router.navigate(['/admin/user']);
        }
      }
    );
  }
}
