import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { INotificationService } from './notification-service.interface';

@Injectable({
  providedIn: 'root',
})
export class NotificationService implements INotificationService {
  private messageSource = new BehaviorSubject<string | null>(null);
  currentMessage: Observable<string | null> = this.messageSource.asObservable();
  private timeoutId: any; // Lưu timeout hiện tại

  showMessage(message: string): void {
    console.log('Showing message:', message);
    this.messageSource.next(message);

    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }

    this.timeoutId = setTimeout(() => {
      if (this.messageSource.getValue() === message) {
        this.messageSource.next(null);
        console.log('⏳ Hiding message after 3s');
      }
    }, 3000);
  }
}
