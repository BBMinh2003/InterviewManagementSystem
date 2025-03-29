import { Observable } from "rxjs";

export interface INotificationService {
  currentMessage: Observable<string | null>;
  showMessage(message:string) : void;
}
