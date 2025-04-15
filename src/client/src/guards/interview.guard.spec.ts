import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { interviewGuardGuard } from './interview.guard.guard';

describe('interviewGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => interviewGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
