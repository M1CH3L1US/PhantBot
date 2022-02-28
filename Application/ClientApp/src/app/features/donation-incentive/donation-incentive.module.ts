import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { DonationIncentiveRouterModule } from '@features/donation-incentive/donation-incentive-router.module';
import { SignalRModule } from '@shared/modules/signalr/signalr.module';

import { DonationIncentiveView } from './donation-incentive-view/donation-incentive-view.component';
import { IncentiveEditComponent } from './incentive-edit/incentive-edit.component';
import { IncentiveRegistrationComponent } from './incentive-registration/incentive-registration.component';
import { IncentiveTrackerComponent } from './incentive-tracker/incentive-tracker.component';
import { IncentiveViewComponent } from './incentive-view/incentive-view.component';

@NgModule({
  declarations: [
    DonationIncentiveView,
    IncentiveRegistrationComponent,
    IncentiveViewComponent,
    IncentiveEditComponent,
    IncentiveTrackerComponent,
  ],
  imports: [
    DonationIncentiveRouterModule,
    CommonModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    FormsModule,
    SignalRModule.forUrl('/hub/donation-incentive'),
  ],
})
export class DonationIncentiveModule {}
