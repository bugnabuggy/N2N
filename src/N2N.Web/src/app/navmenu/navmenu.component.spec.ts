import { async, ComponentFixture, fakeAsync, inject, TestBed, tick} from '@angular/core/testing';
import { NavMenuComponent } from './navmenu.component';
import { RouterTestingModule } from '@angular/router/testing'
import { MatDialogModule } from '@angular/material';

import { HttpModule } from '@angular/http';

import {StoreHeaders} from '../storeHeaders';
import {StoreLinks} from '../storeLinks';
import {UserService} from '../services/userService';

import { By }           from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

describe('NavMenuComponent', () => {
    let comp: NavMenuComponent;
   
    let fixture: ComponentFixture<NavMenuComponent>;
    let heroEl: DebugElement;
  beforeEach(async(() => {
    fixture = TestBed.createComponent(NavMenuComponent);
    comp    = fixture.componentInstance;
    heroEl  = fixture.debugElement.query(By.css('.main-nav'));
    TestBed.configureTestingModule({
      declarations: [

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


  // it('should create the app', async(() => {
  //   const fixture = TestBed.createComponent(NavMenuComponent);
  //   const app = fixture.debugElement.componentInstance;
  //   expect(app).toBeTruthy();
  // }));
});