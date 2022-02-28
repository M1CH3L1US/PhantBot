import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { IDonationIncentive } from '../dto/donation-incentive.interface';
import { StreamlabsRegistrationStatusDto } from '../dto/streamlabs-registration-status.dto';
import { IncentiveService } from '../services/incentive-service.service';

@Component({
  selector: 'pb-donation-incentive-view',
  templateUrl: './donation-incentive-view.component.html',
  styleUrls: ['./donation-incentive-view.component.scss'],
})
export class DonationIncentiveView implements OnInit {
  hasRegisteredStreamlabs: boolean = false;
  incentive$: Observable<IDonationIncentive | null>;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient,
    private incentiveService: IncentiveService
  ) {
    this.incentive$ = this.incentiveService.incentive$;
  }

  ngOnInit(): void {
    this.fetchRegistrationStatus();
  }

  onIncentiveUpdate(incentive: IDonationIncentive) {
    this.incentiveService.updateIncentive(incentive);
  }

  private fetchRegistrationStatus() {
    this.http
      .get<StreamlabsRegistrationStatusDto>(
        `${this.baseUrl}/registration/status?category=streamlabs`
      )
      .subscribe({
        next: ({ isRegistered }) => {
          this.hasRegisteredStreamlabs = isRegistered;
        },
      });
  }
}
