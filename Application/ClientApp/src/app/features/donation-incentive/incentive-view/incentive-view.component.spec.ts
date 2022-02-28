import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IncentiveViewComponent } from './incentive-view.component';

describe('IncentiveViewComponent', () => {
  let component: IncentiveViewComponent;
  let fixture: ComponentFixture<IncentiveViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IncentiveViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IncentiveViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
