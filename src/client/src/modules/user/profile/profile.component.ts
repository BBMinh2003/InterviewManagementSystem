import { Component, Inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faHome,
  faUserEdit,
  IconDefinition,
} from '@fortawesome/free-solid-svg-icons';
import { NOTIFICATION_SERVICE, LOADING_SERVICE, USER_SERVICE } from '../../../constants/injection.constant';
import { ILoadingService } from '../../../services/loading/loading-service.interface';
import { INotificationService } from '../../../services/notification/notification-service.interface';
import { UserModel } from '../../../models/user/user.model';
import { IUserService } from '../../../services/user/user-service.interface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  imports: [RouterModule, FontAwesomeModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent implements OnInit {
  faHome: IconDefinition = faHome;
  faUserEdit: IconDefinition = faUserEdit;
  user!: UserModel;

  constructor(
    @Inject(USER_SERVICE) private readonly userService: IUserService,
    @Inject(NOTIFICATION_SERVICE)
    private readonly notificationService: INotificationService,
    @Inject(LOADING_SERVICE)
    private readonly loadingService: ILoadingService,
    private readonly router: Router
  ) { }

  ngOnInit(): void {
    this.loadingService.show();
    this.userService.getCurrentUserInformation().subscribe((response) => {
      if (response) {
        this.user = response;
      } else {
        this.notificationService.showMessage('User not found', 'error');
      }
      this.loadingService.hide()
    })
  }

  public editProfile() {
    this.router.navigate(['/profile/edit']);
  }
}
