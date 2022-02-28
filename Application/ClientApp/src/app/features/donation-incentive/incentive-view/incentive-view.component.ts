import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { IDonationIncentive } from '../dto/donation-incentive.interface';
import { IncentiveEditComponent } from '../incentive-edit/incentive-edit.component';

@Component({
  selector: 'pb-incentive-view',
  templateUrl: './incentive-view.component.html',
  styleUrls: ['./incentive-view.component.scss'],
})
export class IncentiveViewComponent {
  @Input()
  incentive!: IDonationIncentive | null;

  @Output()
  incentiveUpdate = new EventEmitter<IDonationIncentive>();

  constructor(private dialog: MatDialog) {}

  editIncentive() {
    const dialogRef = this.dialog.open(IncentiveEditComponent, {
      data: { incentive: this.incentive },
      width: '60%',
    });

    dialogRef
      .afterClosed()
      .subscribe((newIncentive: IDonationIncentive | null) => {
        if (newIncentive) {
          this.onIncentiveUpdate(newIncentive);
        }
      });
  }

  private onIncentiveUpdate(incentive: IDonationIncentive) {
    this.incentiveUpdate.emit(incentive);
  }
}
