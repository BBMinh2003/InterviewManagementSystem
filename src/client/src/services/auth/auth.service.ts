import { Injectable } from '@angular/core';
import { IAuthService } from './auth-service.interface';
import { LoginRequest } from '../../models/auth/login-request.model';
import { LoginResponse } from '../../models/auth/login-response.model';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, of, tap } from 'rxjs';
import { RegisterRequest } from '../../models/auth/register-request.model';
import { UserInformation } from '../../models/auth/user-information.model';
import { ForgotPasswordRequest } from '../../models/auth/forgot-password-request.model';
import { BaseResponse } from '../../models/base-response.model';
import { ResetPasswordRequest } from '../../models/auth/reset-password-request.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService implements IAuthService {
  private apiUrl: string = 'http://localhost:5108/api/auth';
  private _isAuthenticated: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);

  private _isAuthenticated$: Observable<boolean> =
    this._isAuthenticated.asObservable();

  private _userInformation: BehaviorSubject<UserInformation | null> =
    new BehaviorSubject<UserInformation | null>(null);

  private _userInformation$: Observable<UserInformation | null> =
    this._userInformation.asObservable();

  private refreshInProgress = false;


  constructor(private httpClient: HttpClient, private router: Router) {
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) {
      this._isAuthenticated.next(true);
    }
    const userInformation = localStorage.getItem('userInformation');
    if (userInformation) {
      this._userInformation.next(JSON.parse(userInformation));
    }
  }

  public isAuthenticated(): Observable<boolean> {
    return this._isAuthenticated$;
  }


  public getUserInformation(): Observable<UserInformation | null> {
    return this._userInformation$;
  }

  public getUserInformationFromAccessToken(): Observable<UserInformation | null> {
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) {
      const payload = JSON.parse(atob(accessToken.split('.')[1]));
      const userInformation: UserInformation = {
        id: payload.nameid,
        email: payload.email,
        displayName: payload.fullName,
        username: payload.unique_name,
        role:
          payload[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ],
      };
      this._userInformation.next(userInformation);
    }
    return this._userInformation$;
  }

  public logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userInformation');
    localStorage.removeItem('userRole');

    this._isAuthenticated.next(false);
    this._userInformation.next(null);
  }

  public login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(this.apiUrl + '/login', loginRequest)
      .pipe(
        tap((response: LoginResponse) => {
          this.handleLogin(response);
        })
      );
  }

  private handleLogin(response: LoginResponse) {
    this.storeTokens(response.accessToken, response.refreshToken);
    const userInformation = this.extractUserInformation(response.accessToken);
    localStorage.setItem('userInformation', JSON.stringify(userInformation));

    this._userInformation.next(userInformation);
    this._isAuthenticated.next(true);
  }

  public forgotPassword(
    forgotPasswordRequest: ForgotPasswordRequest
  ): Observable<boolean> {
    return this.httpClient
      .post<BaseResponse>(
        this.apiUrl + '/forgotPassword',
        forgotPasswordRequest
      )
      .pipe(
        tap((response: BaseResponse) => {}),
        map((response: BaseResponse) => response.success),
        catchError((error) => {
          console.error('Forgot Password Error:', error);
          return of(false);
        })
      );
  }

  public resetPassword(
    resetPasswordRequest: ResetPasswordRequest
  ): Observable<boolean> {
    return this.httpClient
      .post<BaseResponse>(this.apiUrl + '/resetPassword', resetPasswordRequest)
      .pipe(
        tap((response: BaseResponse) => {}),
        map((response: BaseResponse) => response.success),
        catchError((error) => {
          return of(false);
        })
      );
  }

  private extractUserInformation(token: string): UserInformation {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));

      return {
        id: payload.nameid || '',
        email: payload.email || '',
        displayName: payload.fullName || payload.name || '',
        username: payload.unique_name || payload.preferred_username || '',
        role:
          payload[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ] || [],
      };
    } catch (error) {
      console.error('Invalid token format', error);
      return {
        id: '',
        email: '',
        displayName: '',
        username: '',
        role: '',
      };
    }
  }

  private storeTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
  }

  getAccessToken(): string {
    return localStorage.getItem('accessToken') || '';
  }

  getRefreshToken(): string {
    return localStorage.getItem('refreshToken') || '';
  }

  private getTokenExpiration(token: string): number | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp ? payload.exp * 1000 : null;
    } catch (e) {
      return null;
    }
  }

  public isAccessTokenExpired(): boolean {
    const token = this.getAccessToken();
    if (!token) return true;
    const expiration = this.getTokenExpiration(token);
    return expiration ? expiration < Date.now() : true;
  }

  public isRefreshTokenExpired(): boolean {
    const token = this.getRefreshToken();
    if (!token) {
      if (!this.router.url.includes('login')) this.router.navigate(['/login']);
      return true;
    }
    return false;
  }

  public refreshToken(): Observable<string> {
    if (this.refreshInProgress || this.isRefreshTokenExpired()) {
      this.logout();
      return of('');
    }

    this.refreshInProgress = true;

    return this.httpClient
      .post<LoginResponse>(`${this.apiUrl}/refreshAccessToken`, {
        refreshToken: this.getRefreshToken(),
      })
      .pipe(
        tap((response) => {
          this.handleLogin(response);
          this.refreshInProgress = false;
        }),
        map((response) => response.accessToken),
        catchError(() => {
          this.refreshInProgress = false;
          this._isAuthenticated.next(false);
          alert('Logging out');
          this.logout();
          return of('');
        })
      );
  }
}
