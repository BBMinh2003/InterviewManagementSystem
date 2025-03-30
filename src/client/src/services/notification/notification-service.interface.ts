import { Observable } from 'rxjs';
import { NotificationModel } from '../../core/models/notification/notification.model';

export interface INotificationService {
  currentMessage: Observable<NotificationModel | null>;
  showMessage(
    message: string,
    status?: 'success' | 'error' | 'warning' | 'info'
  ): void;
}
