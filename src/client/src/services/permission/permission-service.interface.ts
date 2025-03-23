import { Observable } from "rxjs";

export interface IPermissionService {
  canActivate(): Observable<boolean>;
  isUnauthenticated(): Observable<boolean>;
}
