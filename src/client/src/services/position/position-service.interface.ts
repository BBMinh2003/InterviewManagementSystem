import { PositionModel } from "../../models/position/position.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IPositionService extends IMasterDataService<PositionModel> {}
