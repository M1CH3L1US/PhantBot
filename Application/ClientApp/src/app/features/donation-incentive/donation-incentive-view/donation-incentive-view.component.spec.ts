import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonationIncentiveViewComponent } from './donation-incentive-view.component';

describe('DonationIncentiveViewComponent', () => {
  let component: DonationIncentiveViewComponent;
  let fixture: ComponentFixture<DonationIncentiveViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DonationIncentiveViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DonationIncentiveViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
