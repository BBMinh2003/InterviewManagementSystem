import { Observable } from 'rxjs';
import { CandidateModel } from '../../models/candidate/candidate.model';
import { IMasterDataService } from '../master-data/master-data-service.interface';

export interface ICandidateService extends IMasterDataService<CandidateModel> {
    updateCandidate(id: string, formData: FormData): Observable<any>;
    createCandidate(formData: FormData): Observable<any>;
    banCandidate(id: string): Observable<boolean>;
}
