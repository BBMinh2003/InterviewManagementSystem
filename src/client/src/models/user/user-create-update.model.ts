import DepartmentView from "../common/department.model";
import RoleView from "../role/role.model";

export default class UserCreateUpdateView{
  roles: RoleView[] = [];
  departments: DepartmentView[] = [];
}
