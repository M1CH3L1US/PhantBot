import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IncentiveTrackerComponent } from './incentive-tracker.component';

describe('IncentiveTrackerComponent', () => {
  let component: IncentiveTrackerComponent;
  let fixture: ComponentFixture<IncentiveTrackerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IncentiveTrackerComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IncentiveTrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
