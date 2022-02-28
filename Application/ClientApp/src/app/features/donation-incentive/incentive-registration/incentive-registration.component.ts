import { DOCUMENT } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

import { StreamlabsOAuthUrlDto } from '../dto/streamlabs-oauth-url.dto';

@Component({
  selector: 'pb-incentive-registration',
  templateUrl: './incentive-registration.component.html',
  styleUrls: ['./incentive-registration.component.scss'],
})
export class IncentiveRegistrationComponent implements OnInit {
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient,
    @Inject(DOCUMENT) private _document: Document
  ) {}

  ngOnInit(): void {}

  async register(): Promise<void> {
    const url = `${this.baseUrl}/streamlabs/codeurl`;
    const { url: redirectionUrl } = await this.http
      .get<StreamlabsOAuthUrlDto>(url)
      .toPromise();

    this._document.location.href = redirectionUrl;
  }
}
