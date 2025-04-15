import { InjectionToken } from '@angular/core';
import { IAuthService } from '../services/auth/auth-service.interface';
import { IPermissionService } from '../services/permission/permission-service.interface';
import { INotificationService } from '../services/notification/notification-service.interface';
import { ILoadingService } from '../services/loading/loading-service.interface';
import { ICandidateService } from '../services/candidate/candidate-service.interface';
import { IPositionService } from '../services/position/position-service.interface';
import { ISkillService } from '../services/skill/skill-service.interface';
import { IInterviewService } from '../services/interview/interview-service.interface';
import { IOfferService } from '../services/offer/offer-service.interface';
import { IUserService } from '../services/user/user-service.interface';
import { IJobService } from '../services/job/job-service.interface';
import { IBenefitService } from '../services/benefit/benefit-service.interface';
import { ILevelService } from '../services/level/level-service.interface';
import { IInterviewerService } from '../services/interviewer/interviewer-service.interface';
import { IDashboardService } from '../services/dashboard/dashboard-service.interface';

export const AUTH_SERVICE = new InjectionToken<IAuthService>('AUTH_SERVICE');
export const PERMISSION_SERVICE = new InjectionToken<IPermissionService>(
  'PERMISSION_SERVICE'
);
export const CANDIDATE_SERVICE = new InjectionToken<ICandidateService>(
  'CANDIDATE_SERVICE'
);
export const POSITION_SERVICE = new InjectionToken<IPositionService>(
  'POSITION_SERVICE'
);
export const SKILL_SERVICE = new InjectionToken<ISkillService>(
  'SKILL_SERVICE'
);
export const BENEFIT_SERVICE = new InjectionToken<IBenefitService>(
  'BENEFIT_SERVICE'
);
export const LEVEL_SERVICE = new InjectionToken<ILevelService>(
  'LEVEL_SERVICE'
);
export const INTERVIEW_SERVICE = new InjectionToken<IInterviewService>(
  'INTERVIEW_SERVICE'
);
export const NOTIFICATION_SERVICE = new InjectionToken<INotificationService>('NOTIFICATION_SERVICE');
export const LOADING_SERVICE = new InjectionToken<ILoadingService>('LOADING_SERVICE');


export const OFFER_SERVICE = new InjectionToken<IOfferService>(
  'OFFER_SERVICE'
);
export const USER_SERVICE = new InjectionToken<IUserService>(
  'USER_SERVICE'
);
export const JOB_SERVICE = new InjectionToken<IJobService>(
  'JOB_SERVICE'
);
export const INTERVIEWER_SERVICE = new InjectionToken<IInterviewerService>(
  'INTERVIEWER_SERVICE'
);
export const DASHBOARD_SERVICE = new InjectionToken<IDashboardService>(
  'DASHBOARD_SERVICE'
);
