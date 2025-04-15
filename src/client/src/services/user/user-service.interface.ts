import { Observable } from "rxjs";
import { UserModel } from "../../models/user/user.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";
import { UserCreateModel } from "../../models/user/user-create.model";
import UserCreateUpdateView from "../../models/user/user-create-update.model";
import { ProfileEditModel } from "../../models/user/profile-edit.model";

export interface IUserService extends IMasterDataService<UserModel> {
  switchStatus(id: string, isActive: boolean): Observable<UserModel>
  createUser(data: UserCreateModel): Observable<UserModel>
  getRolesAndDepartments(): Observable<UserCreateUpdateView>
  getCurrentUserInformation(): Observable<UserModel | null>;
  updateCurrentUserProfile(user: ProfileEditModel): Observable<UserModel | null>;

}
