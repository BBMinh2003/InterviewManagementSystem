import { Injectable } from '@angular/core';
import { IUserService } from './user-service.interface';
import { UserModel } from '../../models/user/user.model';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { map, Observable } from 'rxjs';
import { UserCreateModel } from '../../models/user/user-create.model';
import UserCreateUpdateView from '../../models/user/user-create-update.model';
import { ProfileEditModel } from '../../models/user/profile-edit.model';

@Injectable({
  providedIn: 'root',
})
export class UserService
  extends MasterDataService<UserModel>
  implements IUserService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'User');
  }

  switchStatus(id: string, isActive: boolean): Observable<UserModel> {
    return this.httpClient.put<UserModel>(`${this.baseUrl}/switch-status`, {
      id,
      isActive,
    });
  }

  createUser(data: UserCreateModel): Observable<UserModel> {
    return this.httpClient.post<UserModel>(this.baseUrl + '/create', data);
  }

  editUser(data: UserCreateModel): Observable<UserModel> {
    return this.httpClient.put<UserModel>(this.baseUrl + '/update', data);
  }

  getRolesAndDepartments(): Observable<UserCreateUpdateView> {
    return this.httpClient.get<UserCreateUpdateView>(
      this.baseUrl + '/createUpdate'
    );
  }

  getCurrentUserInformation(): Observable<UserModel | null> {
    return this.httpClient.get<UserModel>(this.baseUrl + '/info').pipe(
      map((user) => {
        if (user) {
          return user;
        } else {
          return null;
        }
      })
    );
  }

  updateCurrentUserProfile(user: ProfileEditModel): Observable<UserModel | null> {
    return this.httpClient.put<UserModel>(this.baseUrl + '/info-update', user).pipe(
      map((user) => {
        if (user) {
          return user;
        } else {
          return null;
        }
      })
    );
  }


}
