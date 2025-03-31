import { Observable } from 'rxjs';
import { LoginRequest } from '../../models/auth/login-request.model';
import { LoginResponse } from '../../models/auth/login-response.model';
import { UserInformation } from '../../models/auth/user-information.model';
import { ForgotPasswordRequest } from '../../models/auth/forgot-password-request.model';
import { ResetPasswordRequest } from '../../models/auth/reset-password-request.model';

export interface IAuthService {
  login(loginRequest: LoginRequest): Observable<LoginResponse>;
  logout(): void;
  forgotPassword(forgotPasswordRequest: ForgotPasswordRequest): Observable<boolean>;
  resetPassword(resetPasswordRequest: ResetPasswordRequest): Observable<boolean>
  isAuthenticated(): Observable<boolean>;
  getUserInformation(): Observable<UserInformation | null>;
  getUserInformationFromAccessToken(): Observable<UserInformation | null>;
  getAccessToken(): string;

}
