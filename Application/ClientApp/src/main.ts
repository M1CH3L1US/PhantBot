import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  let baseHref: string;
  if (environment.production) {
    baseHref = document.getElementsByTagName('base')[0].href;
  } else {
    baseHref = 'https://localhost:7023';
  }

  return `${baseHref}/api`;
}

const providers = [{ provide: 'BASE_URL', useFactory: getBaseUrl }];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers)
  .bootstrapModule(AppModule)
  .catch((err) => console.log(err));
