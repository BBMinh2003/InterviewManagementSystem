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
  notification: NotificationModel = new NotificationModel();

  constructor(
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService
  ) {}

  ngOnInit() {
    this.notificationService.currentMessage.subscribe((msg) => {
      this.notification.message = msg || ''; 
      console.log('Received message:', this.notification.message);
    });
  }
}
