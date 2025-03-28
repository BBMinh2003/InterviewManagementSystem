import { BaseModel } from "../base.model";

export class CandidateModel extends BaseModel {
    public name!: string;
    public email!: string;
    public dateOfBirth!: Date;
    public phone!: string;
    public address!: string;
    public gender!: string;
    public cvAttachment!: string;
    public note?: string;
    public status!: string;
    public yearOfExperience!: number;
    public highestLevel!: string;
    public positionId!: string;
    public positionName?: string;
    public recruiterOwnerId!: string;
    public recruiterOwnerName!: string;
    public candidateSkills!: string[];
}