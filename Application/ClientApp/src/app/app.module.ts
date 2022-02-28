import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { DonationIncentiveModule } from '@features/donation-incentive/donation-incentive.module';
import { HomeModule } from '@features/home/home.module';
import { NavigationModule } from '@features/navigation/navigation.module';
import { APP_ROUTES } from 'src/app/app-router.module';

import { AppComponent } from './app.component';

@NgModule({
  declarations: [AppComponent],
  imports: [
    RouterModule.forRoot(APP_ROUTES),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    NavigationModule,
    HomeModule,
    DonationIncentiveModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
