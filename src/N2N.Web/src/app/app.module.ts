import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { N2NRoutingModule } from './n2n-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';

import { PAGES, MATERIAL } from './module.exports';

@NgModule({
  declarations: [
    AppComponent,
    PAGES,
  ],
  imports: [
    BrowserModule,
    N2NRoutingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MATERIAL
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
