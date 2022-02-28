import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'pb-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}

  openTracker() {
    window.open('/incentive/tracker', 'Donation Tracker', 'popupWindow');
  }
}
