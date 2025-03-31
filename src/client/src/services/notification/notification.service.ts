import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NotificationModel } from '../../core/models/notification/notification.model';
import { INotificationService } from './notification-service.interface';

@Injectable({
  providedIn: 'root',
})
export class NotificationService implements INotificationService {
  private messagesSource = new BehaviorSubject<NotificationModel[]>([]);
  messages$ = this.messagesSource.asObservable();

  showMessage(
    message: string,
    status: 'success' | 'error' | 'warning' | 'info' = 'info'
  ): void {
    const notification = new NotificationModel(message, status);

    // Đẩy thông báo mới vào đầu danh sách
    const currentMessages = this.messagesSource.getValue();
    this.messagesSource.next([notification, ...currentMessages]);

    // Xóa thông báo sau 3 giây
    setTimeout(() => this.removeMessage(notification), 3000);
  }

  private removeMessage(notification: NotificationModel) {
    const updatedMessages = this.messagesSource
      .getValue()
      .filter((n) => n !== notification);
    this.messagesSource.next(updatedMessages);
  }
}
