import { BaseModel } from '../base.model';

export class UserModel extends BaseModel {
  fullName!: string;
  email!: string;
  phoneNumber!: string;
  isActive!: number | string | boolean;
  roles!: string[];
  dateOfBirth!: Date;
  address!: string;
  department!: string;
  gender!: number | string;
  note?: string;

}
