import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { INotificationService } from './notification-service.interface';

@Injectable({
  providedIn: 'root',
})
export class NotificationService implements INotificationService {
  private messageSource = new BehaviorSubject<string | null>(null);
  currentMessage: Observable<string | null> = this.messageSource.asObservable(); // Thêm dòng này

  showMessage(message: string): void {
    this.messageSource.next(message);
    setTimeout(() => {
      this.messageSource.next(null);
    }, 3000);
  }
}
