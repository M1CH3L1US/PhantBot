import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DonationIncentiveView } from './donation-incentive-view/donation-incentive-view.component';
import { IncentiveTrackerComponent } from './incentive-tracker/incentive-tracker.component';

const routes: Routes = [
  {
    path: 'incentive',
    component: DonationIncentiveView,
  },
  {
    path: 'incentive/tracker',
    component: IncentiveTrackerComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DonationIncentiveRouterModule {}
