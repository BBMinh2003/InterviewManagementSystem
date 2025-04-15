import { Observable } from "rxjs";
import { InterviewerModel } from "../../models/user/interview.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IInterviewerService extends IMasterDataService<InterviewerModel> {
    getInterviewers(): Observable<InterviewerModel[]>;
    getRecruitersAndManagers(): Observable<InterviewerModel[]>;
    getAdmins(): Observable<InterviewerModel[]>;

}

