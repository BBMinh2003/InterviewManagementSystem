import { Observable } from 'rxjs';
import { NotificationModel } from '../../core/models/notification/notification.model';

export interface INotificationService {
  messages$: Observable<NotificationModel[]>; 
  showMessage(
    message: string,
    status?: 'success' | 'error' | 'warning' | 'info'
  ): void;
}
