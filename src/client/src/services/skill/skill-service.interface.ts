import { Observable } from 'rxjs';
import { SkillModel } from '../../models/skill/skill.model';

export interface ISkillService {
    getAll(): Observable<SkillModel[]>;
}
