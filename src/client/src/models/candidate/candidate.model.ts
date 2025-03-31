import { Status } from "../../core/enums/candidate-status";
import { Gender } from "../../core/enums/gender";
import { BaseModel } from "../base.model";
import { SkillModel } from "../skill/skill.model";

export class CandidateModel extends BaseModel {
    public name!: string;
    public email!: string;
    public dateOfBirth!: Date;
    public address!: string;
    public phone!: string;
    public gender!: number | string;
    public cV_Attachment!: string;
    public note?: string;
    public status!: number | string;
    public yearOfExperience!: number;
    public highestLevel!: number;
    public positionId!: string;
    public positionName?: string;
    public recruiterOwnerId!: string;
    public recruiterOwnerName?: string;
    public candidateSkills!: SkillModel[];
}