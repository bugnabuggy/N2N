import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UserService } from '../userService'
import { StoreHeaders } from '../storeHeaders'
import { RegistrationComponent } from '../registration/registration.component';
@Component({
  selector: 'login',
  templateUrl: 'login.component.html',
  styleUrls: ['./login.component.css']
})
export class LogInComponent {

  constructor(
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<LogInComponent>,
    private _userService: UserService,
    private _storeHeaders: StoreHeaders,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  sendDataLogIn(nickname: string, password: string, captcha: string): void {
    var nickname: string = nickname.trim();
    var password: string = password.trim();
    var captcha: string = captcha.trim();

    if (nickname != "" && password != "" && captcha != "") {

      this._userService.logIn(nickname, password, captcha)
        .then(data => {
          localStorage.setItem("Token", JSON.parse(data._body).access_token);
          this._storeHeaders.refrechJsonAndTokenHeaders();
          alert(JSON.parse(data._body).access_token);
        })
        .catch(data => {
          alert(data._body);
        });
    }
    else {
      alert("Не все поля заполнены");
    }
  }


  onNoClick(): void {
    this.dialogRef.close();
  }

  openDialog(): void {
    this.dialogRef.close();
    let dialogRef = this.dialog.open(RegistrationComponent, {
      width: '350px',

    });

  }
}
