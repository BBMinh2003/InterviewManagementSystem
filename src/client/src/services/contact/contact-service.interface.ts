import { ContactTypeModel } from "../../models/contactType/contactType.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IContactTypeService extends IMasterDataService<ContactTypeModel> {}
