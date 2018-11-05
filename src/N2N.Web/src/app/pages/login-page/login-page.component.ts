import { Component, OnInit } from '@angular/core';
import { Endpoints } from 'src/app/enums/endpoints';
import { UserService } from 'src/app/services';

@Component({
  selector: 'n2n-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  Endpoints = Endpoints;

  username = '';
  password = '';

  constructor(
    public userSvc: UserService
  ) { }

  ngOnInit() {
  }

  onSubmit(f) {
    console.log(f);
    this.userSvc
      .login(this.username, this.password)
      .subscribe((val) => {
          debugger;
          console.log(val);
        },
        (err) => {
          debugger;
          console.log(err);
        });
  }
}
