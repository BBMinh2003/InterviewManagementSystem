export interface UserCreateModel {
  fullName: string;
  email: string;
  dateOfBirth: Date;
  address?: string;
  phoneNumber: string;
  gender: string;
  roleId: string;
  departmentId: string;
  isActive: boolean;
  note?: string;
}
