import { Component, Input } from '@angular/core';

import { IDonationIncentive } from '../dto/donation-incentive.interface';

@Component({
  selector: 'pb-incentive-tracker',
  templateUrl: './incentive-tracker.component.html',
  styleUrls: ['./incentive-tracker.component.scss'],
})
export class IncentiveTrackerComponent {
  @Input()
  incentive!: IDonationIncentive | null;
}
