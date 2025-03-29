import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NotificationModel } from '../../core/models/notification/notification.model';
import { INotificationService } from './notification-service.interface';

@Injectable({
  providedIn: 'root',
})
export class NotificationService implements INotificationService {
  private messageSource = new BehaviorSubject<NotificationModel | null>(null);
  currentMessage = this.messageSource.asObservable();
  private timeoutId: any;

  showMessage(
    message: string,
    status: 'success' | 'error' | 'warning' | 'info' = 'info'
  ): void {
    const notification = new NotificationModel(message, status);
    this.messageSource.next(notification);

    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }

    this.timeoutId = setTimeout(() => {
      if (this.messageSource.getValue() === notification) {
        this.messageSource.next(null);
      }
    }, 3000);
  }
}
