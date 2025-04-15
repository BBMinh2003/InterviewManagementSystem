import { Observable } from 'rxjs';
import { UserInformation } from '../../models/auth/user-information.model';

export interface IPermissionService {
  canActivate(): Observable<boolean>;
  isUnauthenticated(): Observable<boolean>;
  getAccessToken(): string;
  canActivateAdmin(): Observable<boolean>;
  canActivateOffer(): Observable<boolean>;
  canActivateInterview(): Observable<boolean>;
  canEditInterview(): Observable<boolean>;
}
