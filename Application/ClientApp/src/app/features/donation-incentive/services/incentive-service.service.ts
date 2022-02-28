import { HttpClient } from '@angular/common/http';
import { forwardRef, Inject, Injectable } from '@angular/core';
import { SignalRClient } from '@shared/modules/signalr/signalr-client.service';
import { Observable } from 'rxjs';

import { DonationIncentiveModule } from '../donation-incentive.module';
import { IDonationIncentive } from '../dto/donation-incentive.interface';

@Injectable({
  providedIn: forwardRef(() => DonationIncentiveModule),
})
export class IncentiveService {
  public incentive$!: Observable<IDonationIncentive | null>;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private incentiveClient: SignalRClient,
    private http: HttpClient
  ) {
    this.incentive$ = this.incentiveClient.on('update');
  }

  public updateIncentive(incentive: IDonationIncentive): Promise<void> {
    const apiUrl = `${this.baseUrl}/incentive`;
    return this.http.post<void>(apiUrl, incentive).toPromise();
  }
}
