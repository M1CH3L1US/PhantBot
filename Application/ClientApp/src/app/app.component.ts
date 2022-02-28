import { Component, forwardRef } from '@angular/core';
import { IRouterHost, ROUTER_HOST } from '@shared/di-token/router-host';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'pb-root',
  templateUrl: './app.component.html',
  styles: [
    `
      .view-wrapper {
        height: 100%;
        width: 100%;
        background-color: var(--background-color);
      }
    `,
  ],
  providers: [
    { provide: ROUTER_HOST, useExisting: forwardRef(() => AppComponent) },
  ],
})
export class AppComponent implements IRouterHost {
  shouldShowNavBar = new BehaviorSubject<boolean>(true);
}
