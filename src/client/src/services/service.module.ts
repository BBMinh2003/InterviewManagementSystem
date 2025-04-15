import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateService } from './candidate/candidate.service';
import { BENEFIT_SERVICE, CANDIDATE_SERVICE, POSITION_SERVICE, SKILL_SERVICE } from '../constants/injection.constant';
import { SkillService } from './skill/skill.service';
import { PositionService } from './position/position.service';
import { OfferService } from './offer/offer.service';
import { USER_SERVICE, OFFER_SERVICE } from '../constants/injection.constant';
import { UserService } from './user/user.service';
import { BenefitService } from './benefit/benefit.service';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [
    // Candidate service
    {
      provide: CANDIDATE_SERVICE,
      useClass: CandidateService,
    },
    // Skill service
    {
      provide: SKILL_SERVICE,
      useClass: SkillService,
    },
    // Benefit service
    {
      provide: BENEFIT_SERVICE,
      useClass: BenefitService,
    },
    // Position service
    {
      provide: POSITION_SERVICE,
      useClass: PositionService,
    },

    {
      provide: OFFER_SERVICE,
      useClass: OfferService
    },
    {
      provide: USER_SERVICE,
      useClass: UserService,
    },
  ],
})
export class ServicesModule {}
