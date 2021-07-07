import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { HttpClientModule } from '@angular/common/http';

import { N2NRoutingModule } from './n2n-routing.module';
import { AppComponent } from './app.component';
import { PAGES, MATERIAL, SERVICES, COMPONENTS } from './module.exports';
import { N2NHttpInterceptor } from './interceptors/n2n-http.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    PAGES,
    COMPONENTS,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    N2NRoutingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MATERIAL,
    HttpClientModule
  ],
  providers: [
    SERVICES,
    { provide: HTTP_INTERCEPTORS, useClass: N2NHttpInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
