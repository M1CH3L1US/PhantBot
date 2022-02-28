import { TestBed } from '@angular/core/testing';

import { FontPreferenceService } from './font-preference.service';

describe('FontPreferenceService', () => {
  let service: FontPreferenceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FontPreferenceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
