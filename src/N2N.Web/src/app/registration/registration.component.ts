import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UserService } from '../userService'


@Component({
  selector: 'registration',
  templateUrl: 'registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

  constructor(
              public dialogRef: MatDialogRef<RegistrationComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private _userService: UserService
            ) {}

  sendRegistrationData(nickname: string, password: string, confiramtion: string, capcha: string): void {
    var nickname: string = nickname.trim();
    var password: string = password.trim();
    var confiramtion: string = confiramtion.trim();
    var capcha: string = capcha.trim();
    debugger
    if (nickname != "" && password != "" && capcha != "")
     {
        
      if (password == confiramtion) 
      {
        this._userService.sendUserDataForRegistration(nickname, password, capcha).then(data => {
          alert("Регистрация успешна");
        })
          .catch(data => { 
            alert(data._body);           
          });
      }
      else 
      {
        alert("Пароли не совподают");
      }
    }
    else
    {
      alert("Не все поля заполнены");
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}