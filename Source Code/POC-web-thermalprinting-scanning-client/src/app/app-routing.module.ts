import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SignalrthinclientComponent } from './signalrthinclient/signalrthinclient.component';

const routes: Routes = [
  { path: '', component: SignalrthinclientComponent, pathMatch: 'full' },
  { path: 'signalrthinclient', component: SignalrthinclientComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes),
    CommonModule
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
