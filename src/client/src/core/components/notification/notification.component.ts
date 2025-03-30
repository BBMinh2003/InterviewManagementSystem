import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NOTIFICATION_SERVICE } from '../../../constants/injection.constant';
import { INotificationService } from '../../../services/notification/notification-service.interface';
import { NotificationModel } from '../../models/notification/notification.model';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss'],
})
export class NotificationComponent implements OnInit {
  notification: NotificationModel | null = null;

  constructor(
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService
  ) {}

  ngOnInit() {
    this.notificationService.currentMessage.subscribe((msg) => {
      this.notification = msg; 
    });
  }

  getNotificationClass() {
    if (!this.notification) return '';

    switch (this.notification.status) {
      case 'success':
        return 'bg-green-400 text-white';
      case 'error':
        return 'bg-red-500 text-white';
      case 'warning':
        return 'bg-yellow-400 text-black';
      case 'info':
        return 'bg-blue-400 text-white';
      default:
        return 'bg-gray-400 text-white';
    }
  }
}
