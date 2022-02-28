import { InjectionToken } from '@angular/core';
import { Subject } from 'rxjs';

export interface IRouterHost {
  shouldShowNavBar: Subject<boolean>;
}

export const ROUTER_HOST = new InjectionToken<IRouterHost>('ROUTER_HOST');
