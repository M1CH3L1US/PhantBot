import { Component } from '@angular/core';

@Component({
  selector: 'pb-root',
  templateUrl: './app.component.html',
  styles: [
    `
      :host {
        height: 100%;
        width: 100%;
      }
    `,
  ],
})
export class AppComponent {}
