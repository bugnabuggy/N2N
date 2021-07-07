import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginPageComponent } from './login-page.component';
import { MATERIAL } from 'src/app/module.exports';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserService } from 'src/app/services';

describe('LoginPageComponent', () => {
  let component: LoginPageComponent;
  let fixture: ComponentFixture<LoginPageComponent>;

  let userSeeviceStub: Partial<UserService>;

  userSeeviceStub = {
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MATERIAL,
        FormsModule,
        RouterModule
      ],
      declarations: [ LoginPageComponent ],
      providers: [{provide: UserService, useValue: userSeeviceStub} ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
