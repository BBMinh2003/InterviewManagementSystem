import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NOTIFICATION_SERVICE } from '../../../constants/injection.constant';
import { INotificationService } from '../../../services/notification/notification-service.interface';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule], // ✅ Import CommonModule
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss'],
})
export class NotificationComponent implements OnInit {
  message: string | null = null;

  constructor(
    @Inject(NOTIFICATION_SERVICE)
    private notificationService: INotificationService
  ) {}

  ngOnInit() {
    this.notificationService.currentMessage.subscribe((msg) => {
      this.message = msg;
      console.log('Received message:', msg); 
    });
  }
}
