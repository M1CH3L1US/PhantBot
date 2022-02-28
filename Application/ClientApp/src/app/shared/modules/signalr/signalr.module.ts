import { ModuleWithProviders, NgModule, Optional } from '@angular/core';

import { SignalRClient } from './signalr-client.service';
import { SIGNALR_HUB_URL } from './signalr-hub-url.token';

@NgModule()
export class SignalRModule {
  public static forUrl(hubUrl: string): ModuleWithProviders<SignalRModule> {
    return {
      ngModule: SignalRModule,
      providers: [
        {
          provide: SIGNALR_HUB_URL,
          useValue: hubUrl,
        },
        {
          provide: SignalRClient,
          useClass: SignalRClient,
          deps: [[new Optional(), 'BASE_URL'], SIGNALR_HUB_URL],
        },
      ],
    };
  }
}
