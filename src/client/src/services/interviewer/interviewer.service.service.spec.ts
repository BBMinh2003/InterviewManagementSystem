import { TestBed } from '@angular/core/testing';

import { InterviewerServiceService } from './interviewer.service.service';

describe('InterviewerServiceService', () => {
  let service: InterviewerServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InterviewerServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
