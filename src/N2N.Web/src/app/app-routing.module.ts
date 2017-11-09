import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { MakeaPromiseComponent } from './Make-a-Promise/makeapromise.component';
import { SenfGiftComponent } from './Send-Gift/senfgift.component';
import {SendPostcardComponent} from './Send-Postcard/sendpostcard.component'


const routes: Routes = [
  { path: '', redirectTo: 'Make-a-Promise', pathMatch: 'full' },
  { path: 'Make-a-Promise', component: MakeaPromiseComponent },
  { path: 'Send-Gift', component: SenfGiftComponent },
  { path: 'Send-PostCard', component: SenfGiftComponent },
  { path: '**', redirectTo: 'Make-a-Promise' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
