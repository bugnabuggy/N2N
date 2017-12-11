import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './navmenu/navmenu.component';
import { RouterTestingModule } from '@angular/router/testing'
import { MatDialogModule } from '@angular/material';

import { HttpModule } from '@angular/http';

import {StoreHeaders} from './storeHeaders';
import {StoreLinks} from './storeLinks';
import {UserService} from './userService';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        NavMenuComponent
      ],
      imports: [
        RouterTestingModule,
        MatDialogModule,
        HttpModule 
      ],
      providers:[ 
        UserService,
        StoreHeaders,
        StoreLinks
      ]
    }).compileComponents();
  }));


  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
});
