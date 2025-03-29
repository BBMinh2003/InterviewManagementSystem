export class NotificationModel {
  public message!: string;
  status: 'success' | 'error' | 'warning' | 'info';

  constructor(
    message: string = '',
    status: 'success' | 'error' | 'warning' | 'info' = 'info'
  ) {
    this.message = message;
    this.status = status;
  }
}
