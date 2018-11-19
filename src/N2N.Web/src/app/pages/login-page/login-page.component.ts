import { Component, OnInit } from '@angular/core';
import { Endpoints } from 'src/app/enums/endpoints';
import { UserService, SecurityService, NotificationService } from 'src/app/services';
import { LoginContract } from 'src/app/models';
import { Router } from '@angular/router';
import { ErrorParsingHelper } from 'src/app/utilities/error-parsing-helper';

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
    private userSvc: UserService,
    private secSvc: SecurityService,
    private notificationSvc: NotificationService,
    private router: Router
  ) { }

  ngOnInit() {
  }

  onSubmit(f) {
    console.log(f);
    this.userSvc
      .login(this.username, this.password)
      .subscribe((val: LoginContract) => {
          this.secSvc.setTokens(val);
          this.router.navigate([Endpoints.site.dashboard]);
        },
        (err) => {
          this.notificationSvc.error(ErrorParsingHelper.getErrorMessage(err));
          console.log(err);
        });
  }
}
