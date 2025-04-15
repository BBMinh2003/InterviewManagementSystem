import { Observable } from 'rxjs';
import { IMasterDataService } from '../master-data/master-data-service.interface';
import { JobModel } from '../../models/job/job.model';

export interface IJobService extends IMasterDataService<JobModel> {
    import(formData: FormData): Observable<any>;
}
