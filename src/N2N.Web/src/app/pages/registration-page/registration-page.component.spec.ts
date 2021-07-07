import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationPageComponent } from './registration-page.component';
import { UserService } from 'src/app/services';
import { MATERIAL } from 'src/app/module.exports';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

describe('RegistrationPageComponent', () => {
  let component: RegistrationPageComponent;
  let fixture: ComponentFixture<RegistrationPageComponent>;

  let userSeeviceStub: Partial<UserService>;

  userSeeviceStub = {
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MATERIAL,
        FormsModule,
        ReactiveFormsModule,
        RouterModule
      ],
      declarations: [ RegistrationPageComponent ],
      providers: [{provide: UserService, useValue: userSeeviceStub} ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should check credentials before registration');

  it('should make a login call aftre registration');
});
