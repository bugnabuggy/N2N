import { Component, OnInit } from '@angular/core';
import { Endpoints } from 'src/app/enums/endpoints';
import { UserService } from 'src/app/services';
import { NotificationService } from 'src/app/services/notification.service';

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
        (val) => {
          debugger;
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
