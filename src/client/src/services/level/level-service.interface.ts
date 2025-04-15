import { LevelModel } from "../../models/level/level.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface ILevelService extends IMasterDataService<LevelModel> {}
