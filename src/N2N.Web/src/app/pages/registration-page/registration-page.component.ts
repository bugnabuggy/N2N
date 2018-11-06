import { Component, OnInit } from '@angular/core';
import { Endpoints } from 'src/app/enums/endpoints';
import { UserService, SecurityService, NotificationService } from 'src/app/services';
import { LoginContract } from 'src/app/models';
import { Router } from '@angular/router';

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
    debugger;
    if (this.password && this.password == this.rePassword) {
      this.userSvc.register(
        this.nickname,
        this.password
      ).subscribe(
        (resp: LoginContract ) => {
            debugger;
            this.securitySvc.setTokens(resp);
            this.router.navigate(['\\' + Endpoints.site.dashboard]);
        },
        (err) => {
          debugger;
          this.notifications.error(err.error);
          console.log(err);
        }
      );
    }
  }
}
