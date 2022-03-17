import { Component, Input } from '@angular/core';

import { IDonationIncentive } from '../dto/donation-incentive.interface';
import { FontPreferenceService, IFontPreference } from '../services/font-preference.service';

@Component({
  selector: 'pb-incentive-tracker',
  templateUrl: './incentive-tracker.component.html',
  styleUrls: ['./incentive-tracker.component.scss'],
})
export class IncentiveTrackerComponent {
  @Input()
  incentive!: IDonationIncentive | null;

  private fontPreference!: IFontPreference;

  get font() {
    const { font, sizeInPx, style } = this.fontPreference;
    const weight =
      style === 'Regular' ? 'normal' : style === 'Light' ? 'lighter' : 'bold';

    return {
      'font-family': `${font}, sans-serif`,
      'font-weight': weight,
      'font-size': `${sizeInPx}px`,
    };
  }

  constructor(private fontPreferenceService: FontPreferenceService) {
    this.fontPreferenceService.fontPreferences$.subscribe((preference) => {
      this.fontPreference = preference;
    });
  }
}
