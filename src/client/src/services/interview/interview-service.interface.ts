import { Observable } from 'rxjs';
import { InterviewModel } from '../../models/interview/interview.model';
import { IMasterDataService } from '../master-data/master-data-service.interface';

export interface IInterviewService extends IMasterDataService<InterviewModel> {
    cancelInterview(id: string): Observable<boolean>;
    sendReminder(interviewId: string): Observable<any>;
    submitResult(id: string, result: number): Observable<any>;
}
