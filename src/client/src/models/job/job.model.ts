import { JobStatus } from "../../core/enums/jobstatus";
import { BaseModel } from "../base.model";
import { BenefitModel } from "../benefit/benefit.model";
import { LevelModel } from "../level/level.model";
import { SkillModel } from "../skill/skill.model";

export class JobModel extends BaseModel {
  title!: string;
  startDate?: string | null;
  endDate?: string | null;
  minSalary!: number;
  maxSalary!: number;
  workingAddress?: string | null;
  description?: string | null;
  status!: string;
  jobSkills!: SkillModel[];
  jobLevels!: LevelModel[];
  jobBenefits!: BenefitModel[];
}