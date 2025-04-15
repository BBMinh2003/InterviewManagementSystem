import { DepartmentModel } from "../../models/department/department.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IDepartmentService extends IMasterDataService<DepartmentModel> {}
