import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IncentiveRegistrationComponent } from './incentive-registration.component';

describe('IncentiveRegistrationComponent', () => {
  let component: IncentiveRegistrationComponent;
  let fixture: ComponentFixture<IncentiveRegistrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IncentiveRegistrationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IncentiveRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
