import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'pb-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    // console.log(baseUrl);
    // http.get(baseUrl + 'weatherforecast').subscribe(
    //   (result) => {},
    //   (error) => console.error(error)
    // );
  }

  ngOnInit(): void {}
}
