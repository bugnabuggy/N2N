import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { MakeaPromiseCreatedComponent } from './Make-a-Promise/makeapromise.created.component';
import { MakeaPromiseSuccessComponent } from './Make-a-Promise/makeapromise.Success.component';
import { SenfGiftComponent } from './Send-Gift/senfgift.component';
import {SendPostcardComponent} from './Send-Postcard/sendpostcard.component'
import {LogInComponent} from './login/login.component';
import {RegistrationComponent} from './registration/registration.component';
import {HomePageComponent} from './home-Page/homePage.component';

import {UserService} from './userService';
import {StoreHeaders} from './storeHeaders';
import {StoreLinks} from './storeLinks';
import {Web3Service} from './Web3Service';
import {PromiseService} from './Make-a-Promise/promiseService';

import { AppRoutingModule } from './app-routing.module';

import {MatDialogModule,MatButtonModule,MatInputModule,MatCardModule,MatCheckboxModule} from '@angular/material';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    MakeaPromiseCreatedComponent,
    MakeaPromiseSuccessComponent,
    SenfGiftComponent,
    SendPostcardComponent,
    LogInComponent,
    RegistrationComponent,
    HomePageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatCheckboxModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    ReactiveFormsModule
  ],
  entryComponents: [LogInComponent,RegistrationComponent],
  providers: [UserService,StoreHeaders,StoreLinks,Web3Service,PromiseService],
  bootstrap: [AppComponent]
})
export class AppModule { }
