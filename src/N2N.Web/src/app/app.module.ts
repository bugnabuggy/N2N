import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { MakeaPromiseComponent } from './Make-a-Promise/makeapromise.component';
import { SenfGiftComponent } from './Send-Gift/senfgift.component';
import {SendPostcardComponent} from './Send-Postcard/sendpostcard.component'
import {LogInComponent} from './login/login.component';
import {RegistrationComponent} from './registration/registration.component';

import {UserService} from './userService';

import { AppRoutingModule } from './app-routing.module';

import {MatDialogModule,MatButtonModule,MatInputModule,MatCardModule,MatCheckboxModule} from '@angular/material';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    MakeaPromiseComponent,
    SenfGiftComponent,
    SendPostcardComponent,
    LogInComponent,
    RegistrationComponent
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
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
