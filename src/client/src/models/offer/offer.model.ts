import { BaseModel } from "../base.model";

export class OfferModel extends BaseModel{
    candidateId! : string;
    candidateName! : string;
    candidateEmail! : string;
    departmentId! : string;
    departmentName! : string;
    recruiterOwnerId! : string;
    recruiterOwnerName!: string;
    approverId! : string;
    approverName!: string;
    positionId! : string;
    positionName!: string;
    interviewId! : string;
    interviewInfo?: string;
    interviewNote?: string;
    contactTypeId! : string;
    contactType!: string;
    levelId! : string;
    level!: string;
    basicSalary!: number;
    note?: string;
    dueDate!: string;
    contactPeriodFrom!: string;
    contactPeriodTo!: string;
    status!: number;
    statusText!: string;
}