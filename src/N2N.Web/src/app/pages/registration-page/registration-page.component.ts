import { Component, OnInit } from '@angular/core';
import { Endpoints } from 'src/app/enums/endpoints';
import { UserService, SecurityService, NotificationService } from 'src/app/services';
import { LoginContract } from 'src/app/models';
import { Router } from '@angular/router';

import { flatMap } from 'rxjs/operators';
import { ErrorParsingHelper } from 'src/app/utilities/error-parsing-helper';

@Component({
  selector: 'n2n-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.scss']
})
export class RegistrationPageComponent implements OnInit {
  Endpoints = Endpoints;

  nickname = '';
  password = '';
  rePassword = '';

  constructor(
    public userSvc: UserService,
    private notifications: NotificationService,
    private securitySvc: SecurityService,
    private router: Router
  ) { }

  ngOnInit() {
  }


  onSubmit($event) {
    if (  this.nickname &&
          this.password &&
          this.password === this.rePassword) {
      this.userSvc.register(
        this.nickname,
        this.password
      ).pipe(
        flatMap( (value, i) => {
          return this.userSvc.login(this.nickname, this.password);
        }
      )
        // .subscribe(
        //   (resp: LoginContract ) => {
        //       this.userSvc.login(this.nickname, this.password)
        //       .subscribe(

        //       )

        //       this.securitySvc.setTokens(resp);
        //       this.router.navigate(['\\' + Endpoints.site.dashboard]);
        //   },
      //   (err) => {
      //   this.notifications.error(err.error);
      //   console.log(err);
      // }
      ).subscribe(
        (resp: LoginContract) => {
          this.securitySvc.setTokens(resp);
          this.router.navigate(['\\' + Endpoints.site.dashboard]);
        },
        (err) => {
            const errMsg = ErrorParsingHelper.getErrorMessage(err);
            this.notifications.error(errMsg);
            console.log(err);
        }
      );
    } else {
      this.notifications.error('Fill all necessary fields');
    }
  }
}
