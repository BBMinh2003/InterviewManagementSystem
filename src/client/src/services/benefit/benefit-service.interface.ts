import { Observable } from 'rxjs';
import { BenefitModel } from '../../models/benefit/benefit.model';

export interface IBenefitService {
    getAll(): Observable<BenefitModel[]>;
}
