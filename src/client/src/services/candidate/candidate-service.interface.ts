import { CandidateModel } from '../../models/candidate/candidate.model';
import { IMasterDataService } from '../master-data/master-data-service.interface';

export interface ICandidateService extends IMasterDataService<CandidateModel> {}
