import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';



import { AppComponent } from './app.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { MakeaPromiseComponent } from './Make-a-Promise/makeapromise.component';
import { SenfGiftComponent } from './Send-Gift/senfgift.component';
import {SendPostcardComponent} from './Send-Postcard/sendpostcard.component'

import { AppRoutingModule } from './app-routing.module';



@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    MakeaPromiseComponent,
    SenfGiftComponent,
    SendPostcardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
