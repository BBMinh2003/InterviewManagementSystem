import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateService } from './candidate/candidate.service';
import { CANDIDATE_SERVICE } from '../constants/injection.constant';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [
    // Candidate service
    {
      provide: CANDIDATE_SERVICE,
      useClass: CandidateService,
    },
  ],
})
export class ServicesModule {}
