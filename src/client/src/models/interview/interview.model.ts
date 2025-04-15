import { BaseModel } from '../base.model';
import { UserModel } from '../user/user.model';

export class InterviewModel extends BaseModel {

  candidateName!: string;
  candidateId!: string;

  jobName!: string;
  jobId!: string;

  recruiterOwnerId!: string;
  recruiterOwnerName!: string;

  title!: string;
  note?: string;
  location?: string;
  meetingUrl?: string;

  result!: string;
  status!: string;

  startAt!: string;
  endAt!: string; 
  interviewDate!: string; 

  interviewers: UserModel[] = [];
}
