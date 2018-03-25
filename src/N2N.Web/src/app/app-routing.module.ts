import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { MakeaPromiseCreatedComponent } from './Make-a-Promise/makeapromise.created.component';
import { SenfGiftComponent } from './Send-Gift/senfgift.component';
import {SendPostcardComponent} from './Send-Postcard/sendpostcard.component';
import {HomePageComponent} from './home-Page/homePage.component';
import { MakeaPromiseSuccessComponent } from './Make-a-Promise/makeapromise.Success.component';

const routes: Routes = [
  { path: '', redirectTo: 'homePage', pathMatch: 'full' },
  { path: 'homePage', component: HomePageComponent },
  { path: 'Make-a-Promise-created', component: MakeaPromiseCreatedComponent },
  { path: 'Make-a-Promise-Success', component: MakeaPromiseSuccessComponent },
  { path: 'Send-Gift', component: SenfGiftComponent },
  { path: 'Send-PostCard', component: SenfGiftComponent },
  { path: '**', redirectTo: 'homePage' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
