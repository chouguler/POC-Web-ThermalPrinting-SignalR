import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { SignalrthinclientComponent } from './signalrthinclient/signalrthinclient.component';


@NgModule({
  declarations: [
    AppComponent,
    SignalrthinclientComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule   
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
