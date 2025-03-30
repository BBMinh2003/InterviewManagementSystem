import { InjectionToken } from '@angular/core';
import { IAuthService } from '../services/auth/auth-service.interface';
import { IPermissionService } from '../services/permission/permission-service.interface';
import { ICandidateService } from '../services/candidate/candidate-service.interface';

export const AUTH_SERVICE = new InjectionToken<IAuthService>('AUTH_SERVICE');
export const PERMISSION_SERVICE = new InjectionToken<IPermissionService>(
  'PERMISSION_SERVICE'
);

export const CANDIDATE_SERVICE = new InjectionToken<ICandidateService>(
  'CANDIDATE_SERVICE'
);
