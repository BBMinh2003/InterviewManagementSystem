import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { editInterviewGuard } from './edit-interview.guard';

describe('editInterviewGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => editInterviewGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
